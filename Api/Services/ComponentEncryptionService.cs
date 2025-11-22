using System.Security.Cryptography;
using System.Text;

namespace Ilmanar.Api.Services;

/// <summary>
/// Service pour chiffrer/déchiffrer les composants interactifs protégés
/// Utilise AES-256 pour la protection du code des composants
/// </summary>
public class ComponentEncryptionService
{
    private readonly string _encryptionKey;
    private readonly ILogger<ComponentEncryptionService> _logger;
    
    public ComponentEncryptionService(IConfiguration configuration, ILogger<ComponentEncryptionService> logger)
    {
        _logger = logger;
        _encryptionKey = configuration["ComponentEncryption:Key"] 
            ?? throw new InvalidOperationException("Clé de chiffrement manquante dans la configuration. Ajoutez 'ComponentEncryption:Key' dans appsettings.json");
    }

    /// <summary>
    /// Chiffre un composant Vue.js pour le protéger
    /// </summary>
    /// <param name="componentCode">Code source du composant à chiffrer</param>
    /// <returns>Composant chiffré en Base64</returns>
    public async Task<string> EncryptComponentAsync(string componentCode)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_encryptionKey);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            
            // Écrire l'IV au début du flux
            await msEncrypt.WriteAsync(aes.IV, 0, aes.IV.Length);
            
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                await swEncrypt.WriteAsync(componentCode);
            }

            var encrypted = Convert.ToBase64String(msEncrypt.ToArray());
            _logger.LogInformation("Composant chiffré avec succès ({size} bytes)", encrypted.Length);
            
            return encrypted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du chiffrement du composant");
            throw;
        }
    }

    /// <summary>
    /// Déchiffre un composant pour un utilisateur autorisé
    /// </summary>
    /// <param name="encryptedComponent">Composant chiffré en Base64</param>
    /// <returns>Code source déchiffré du composant</returns>
    public async Task<string> DecryptComponentAsync(string encryptedComponent)
    {
        try
        {
            var fullCipher = Convert.FromBase64String(encryptedComponent);

            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(_encryptionKey);

            // Extraire l'IV (les 16 premiers bytes)
            var iv = new byte[aes.IV.Length];
            var cipher = new byte[fullCipher.Length - iv.Length];
            
            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            var decrypted = await srDecrypt.ReadToEndAsync();
            _logger.LogInformation("Composant déchiffré avec succès ({size} bytes)", decrypted.Length);
            
            return decrypted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du déchiffrement du composant");
            throw new InvalidOperationException("Impossible de déchiffrer le composant. Il peut être corrompu.", ex);
        }
    }

    /// <summary>
    /// Calcule un hash SHA256 pour vérifier l'intégrité du composant
    /// </summary>
    /// <param name="content">Contenu à hasher</param>
    /// <returns>Hash en Base64</returns>
    public string ComputeHash(string content)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Vérifie l'intégrité d'un composant
    /// </summary>
    /// <param name="content">Contenu à vérifier</param>
    /// <param name="expectedHash">Hash attendu</param>
    /// <returns>True si le hash correspond</returns>
    public bool VerifyHash(string content, string expectedHash)
    {
        var computedHash = ComputeHash(content);
        return computedHash == expectedHash;
    }

    /// <summary>
    /// Sauvegarde un composant chiffré sur disque
    /// </summary>
    /// <param name="componentCode">Code source du composant</param>
    /// <param name="componentName">Nom du composant (sans extension)</param>
    /// <param name="environment">Environnement web host</param>
    /// <returns>Chemin relatif du composant sauvegardé</returns>
    public async Task<(string Path, string Hash)> SaveEncryptedComponentAsync(
        string componentCode, 
        string componentName,
        IWebHostEnvironment environment)
    {
        try
        {
            // Créer le dossier protégé s'il n'existe pas
            var protectedFolder = Path.Combine(
                environment.ContentRootPath, 
                "wwwroot", 
                "protected", 
                "components"
            );
            
            Directory.CreateDirectory(protectedFolder);

            // Chiffrer le composant
            var encrypted = await EncryptComponentAsync(componentCode);
            
            // Calculer le hash du code original
            var hash = ComputeHash(componentCode);
            
            // Générer un nom de fichier sécurisé
            var safeFileName = SanitizeFileName(componentName);
            var fileName = $"{safeFileName}_{Guid.NewGuid().ToString("N").Substring(0, 8)}.enc";
            var filePath = Path.Combine(protectedFolder, fileName);

            // Sauvegarder le fichier chiffré
            await File.WriteAllTextAsync(filePath, encrypted);

            var relativePath = $"/protected/components/{fileName}";
            
            _logger.LogInformation("Composant protégé sauvegardé: {path}", relativePath);

            return (relativePath, hash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la sauvegarde du composant chiffré");
            throw;
        }
    }

    /// <summary>
    /// Lit et déchiffre un composant depuis le disque
    /// </summary>
    /// <param name="componentPath">Chemin relatif du composant (ex: /protected/components/quiz.enc)</param>
    /// <param name="environment">Environnement web host</param>
    /// <returns>Code source déchiffré</returns>
    public async Task<string> LoadEncryptedComponentAsync(
        string componentPath,
        IWebHostEnvironment environment)
    {
        try
        {
            var filePath = Path.Combine(
                environment.ContentRootPath, 
                "wwwroot", 
                componentPath.TrimStart('/')
            );
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Composant introuvable: {componentPath}");
            }

            var encryptedContent = await File.ReadAllTextAsync(filePath);
            return await DecryptComponentAsync(encryptedContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du chargement du composant: {path}", componentPath);
            throw;
        }
    }

    /// <summary>
    /// Nettoie un nom de fichier pour le rendre sécurisé
    /// </summary>
    private string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(fileName
            .Where(c => !invalidChars.Contains(c))
            .ToArray());
        
        return string.IsNullOrWhiteSpace(sanitized) ? "component" : sanitized;
    }
}

