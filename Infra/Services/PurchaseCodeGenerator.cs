namespace KitabStock.Infra.Services;

using System.Security.Cryptography;
using System.Text;

public static class PurchaseCodeGenerator
{
    private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Sans caractères ambigus (I, O, 0, 1)
    
    /// <summary>
    /// Génère un code d'achat unique de 12 caractères
    /// Format: XXXX-XXXX-XXXX (3 groupes de 4 caractères)
    /// </summary>
    public static string GenerateCode()
    {
        var random = RandomNumberGenerator.Create();
        var bytes = new byte[12];
        random.GetBytes(bytes);
        
        var code = new StringBuilder(12);
        for (int i = 0; i < 12; i++)
        {
            code.Append(AllowedChars[bytes[i] % AllowedChars.Length]);
        }
        
        // Formater avec des tirets pour meilleure lisibilité: XXXX-XXXX-XXXX
        return $"{code.ToString(0, 4)}-{code.ToString(4, 4)}-{code.ToString(8, 4)}";
    }
    
    /// <summary>
    /// Valide le format d'un code d'achat
    /// </summary>
    public static bool IsValidCodeFormat(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;
        
        // Enlever les tirets pour la validation
        var cleanCode = code.Replace("-", "");
        
        // Doit faire 12 caractères
        if (cleanCode.Length != 12)
            return false;
        
        // Tous les caractères doivent être dans la liste autorisée
        return cleanCode.All(c => AllowedChars.Contains(c));
    }
    
    /// <summary>
    /// Normalise un code d'achat (enlève les espaces, met en majuscules, ajoute les tirets)
    /// </summary>
    public static string NormalizeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return string.Empty;
        
        // Enlever espaces et tirets, mettre en majuscules
        var cleanCode = code.Replace("-", "").Replace(" ", "").ToUpper();
        
        if (cleanCode.Length != 12)
            return cleanCode;
        
        // Reformater avec tirets
        return $"{cleanCode.Substring(0, 4)}-{cleanCode.Substring(4, 4)}-{cleanCode.Substring(8, 4)}";
    }
}

