namespace KitabStock.Infra.Mail;

public static class EmailTemplates
{
    private const string BaseTemplate = @"
<!DOCTYPE html>
<html lang='fr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{0}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            background: linear-gradient(135deg, #1e3a5f 0%, #2c5282 100%);
            padding: 40px 20px;
            min-height: 100vh;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background: rgba(255, 255, 255, 0.95);
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
        }}
        .header {{
            background: linear-gradient(135deg, #2d3748 0%, #1a202c 100%);
            padding: 50px 40px;
            text-align: center;
        }}
        .header h1 {{
            color: #d4af7a;
            font-size: 48px;
            font-weight: 300;
            letter-spacing: 2px;
            margin-bottom: 10px;
        }}
        .header p {{
            color: #e2e8f0;
            font-size: 16px;
            margin-top: 10px;
        }}
        .content {{
            padding: 50px 40px;
            color: #2d3748;
        }}
        .greeting {{
            font-size: 24px;
            color: #1a202c;
            margin-bottom: 20px;
            font-weight: 600;
        }}
        .message {{
            font-size: 16px;
            line-height: 1.8;
            color: #4a5568;
            margin-bottom: 30px;
        }}
        .button-container {{
            text-align: center;
            margin: 40px 0;
        }}
        .button {{
            display: inline-block;
            background: linear-gradient(135deg, #d4af7a 0%, #c9a366 100%);
            color: #1a202c !important;
            padding: 16px 48px;
            text-decoration: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: 600;
            transition: transform 0.2s, box-shadow 0.2s;
            box-shadow: 0 4px 12px rgba(212, 175, 122, 0.3);
        }}
        .button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(212, 175, 122, 0.4);
        }}
        .alternative-link {{
            margin-top: 30px;
            padding: 20px;
            background: #f7fafc;
            border-radius: 8px;
            border-left: 4px solid #d4af7a;
        }}
        .alternative-link p {{
            font-size: 14px;
            color: #4a5568;
            margin-bottom: 10px;
        }}
        .alternative-link a {{
            color: #2d3748;
            word-break: break-all;
            font-size: 13px;
        }}
        .footer {{
            background: #f7fafc;
            padding: 30px 40px;
            text-align: center;
            border-top: 1px solid #e2e8f0;
        }}
        .footer p {{
            color: #718096;
            font-size: 14px;
            line-height: 1.6;
        }}
        .divider {{
            height: 1px;
            background: linear-gradient(90deg, transparent, #e2e8f0, transparent);
            margin: 30px 0;
        }}
        .highlight {{
            color: #d4af7a;
            font-weight: 600;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Kitab</h1>
            <p>Rejoignez Kitab et accédez à des contenus authentiques sur l'Islam</p>
        </div>
        <div class='content'>
            {1}
        </div>
        <div class='footer'>
            <p>© 2025 Kitab. Tous droits réservés.</p>
            <p style='margin-top: 10px; font-size: 12px;'>
                Si vous n'avez pas demandé cet email, vous pouvez l'ignorer en toute sécurité.
            </p>
        </div>
    </div>
</body>
</html>";

    public static string GetEmailConfirmationTemplate(string userName, string confirmationLink)
    {
        var content = $@"
            <div class='greeting'>Bienvenue {userName} !</div>
            <div class='message'>
                <p>Merci de vous être inscrit sur <span class='highlight'>Kitab</span>.</p>
                <p style='margin-top: 15px;'>
                    Pour commencer à profiter de nos contenus authentiques sur l'Islam, 
                    veuillez confirmer votre adresse email en cliquant sur le bouton ci-dessous :
                </p>
            </div>
            
            <div class='button-container'>
                <a href='{confirmationLink}' class='button'>
                    → Confirmer mon email
                </a>
            </div>

            <div class='alternative-link'>
                <p><strong>Le bouton ne fonctionne pas ?</strong></p>
                <p>Copiez et collez ce lien dans votre navigateur :</p>
                <a href='{confirmationLink}'>{confirmationLink}</a>
            </div>

            <div class='divider'></div>

            <div class='message'>
                <p style='font-size: 14px; color: #718096;'>
                    Ce lien de confirmation expirera dans 24 heures pour des raisons de sécurité.
                </p>
            </div>";

        return string.Format(BaseTemplate, "Confirmez votre email - Kitab", content);
    }

    public static string GetPasswordResetTemplate(string userName, string resetLink)
    {
        var content = $@"
            <div class='greeting'>Bonjour {userName},</div>
            <div class='message'>
                <p>Vous avez demandé à réinitialiser votre mot de passe sur <span class='highlight'>Kitab</span>.</p>
                <p style='margin-top: 15px;'>
                    Cliquez sur le bouton ci-dessous pour créer un nouveau mot de passe :
                </p>
            </div>
            
            <div class='button-container'>
                <a href='{resetLink}' class='button'>
                    → Réinitialiser mon mot de passe
                </a>
            </div>

            <div class='alternative-link'>
                <p><strong>Le bouton ne fonctionne pas ?</strong></p>
                <p>Copiez et collez ce lien dans votre navigateur :</p>
                <a href='{resetLink}'>{resetLink}</a>
            </div>

            <div class='divider'></div>

            <div class='message'>
                <p style='font-size: 14px; color: #718096;'>
                    Ce lien de réinitialisation expirera dans 1 heure pour des raisons de sécurité.
                </p>
                <p style='font-size: 14px; color: #718096; margin-top: 10px;'>
                    Si vous n'avez pas demandé cette réinitialisation, veuillez ignorer cet email. 
                    Votre mot de passe actuel restera inchangé.
                </p>
            </div>";

        return string.Format(BaseTemplate, "Réinitialisation de mot de passe - Kitab", content);
    }

    public static string GetWelcomeTemplate(string userName)
    {
        var content = $@"
            <div class='greeting'>Bienvenue sur Kitab, {userName} !</div>
            <div class='message'>
                <p>Votre compte a été confirmé avec succès ! 🎉</p>
                <p style='margin-top: 15px;'>
                    Vous pouvez maintenant accéder à tous nos contenus authentiques sur l'Islam.
                </p>
            </div>
            
            <div class='button-container'>
                <a href='{{frontendUrl}}' class='button'>
                    → Découvrir Kitab
                </a>
            </div>

            <div class='divider'></div>

            <div class='message'>
                <p style='font-size: 15px;'>
                    <strong>Que pouvez-vous faire maintenant ?</strong>
                </p>
                <ul style='margin-top: 10px; padding-left: 20px; color: #4a5568;'>
                    <li style='margin: 8px 0;'>Parcourir notre bibliothèque de contenus</li>
                    <li style='margin: 8px 0;'>Accéder à des vidéos authentiques</li>
                    <li style='margin: 8px 0;'>Rejoindre notre communauté</li>
                </ul>
            </div>";

        return string.Format(BaseTemplate, "Bienvenue sur Kitab", content);
    }
}

