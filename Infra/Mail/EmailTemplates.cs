namespace Ilmanar.Infra.Mail;

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
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700;900&display=swap');
        
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        body {{
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            background-color: #082540;
            padding: 60px 20px;
            color: #ffffff;
            line-height: 1.6;
        }}
        .wrapper {{
            width: 100%;
            background-color: #082540;
            padding-bottom: 60px;
        }}
        .container {{
            max-width: 500px;
            margin: 0 auto;
            background-color: #0B3152;
            border-radius: 32px;
            border: 1px solid rgba(255, 255, 255, 0.1);
            overflow: hidden;
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
        }}
        .header {{
            padding: 50px 40px 30px;
            text-align: center;
        }}
        .logo-img {{
            width: 80px;
            height: 80px;
            margin-bottom: 24px;
        }}
        .platform-name {{
            color: #ffffff;
            font-size: 32px;
            font-weight: 900;
            letter-spacing: -1px;
            text-transform: uppercase;
            margin-bottom: 4px;
        }}
        .platform-subtitle {{
            color: #C39712;
            font-size: 12px;
            font-weight: 700;
            letter-spacing: 2px;
            text-transform: uppercase;
            opacity: 0.8;
        }}
        .content {{
            padding: 0 40px 50px;
            text-align: center;
        }}
        .greeting {{
            font-size: 24px;
            color: #ffffff;
            margin-bottom: 20px;
            font-weight: 800;
            letter-spacing: -0.5px;
        }}
        .text {{
            font-size: 16px;
            color: rgba(255, 255, 255, 0.7);
            margin-bottom: 30px;
        }}
        .button-wrapper {{
            margin: 40px 0;
        }}
        .button {{
            display: inline-block;
            background-color: #ffffff;
            color: #082540 !important;
            padding: 16px 40px;
            text-decoration: none;
            border-radius: 16px;
            font-size: 16px;
            font-weight: 700;
            transition: all 0.2s ease;
            box-shadow: 0 10px 15px -3px rgba(255, 255, 255, 0.1);
        }}
        .footer {{
            padding: 40px;
            background-color: rgba(0, 0, 0, 0.1);
            text-align: center;
            border-top: 1px solid rgba(255, 255, 255, 0.05);
        }}
        .footer-text {{
            color: rgba(255, 255, 255, 0.4);
            font-size: 13px;
        }}
        .divider {{
            height: 1px;
            background-color: rgba(255, 255, 255, 0.1);
            margin: 30px 0;
        }}
        .highlight {{
            color: #C39712;
            font-weight: 700;
        }}
        .alt-text {{
            font-size: 11px;
            color: rgba(255, 255, 255, 0.3);
            word-break: break-all;
            margin-top: 20px;
            display: block;
        }}
    </style>
</head>
<body>
    <div class='wrapper'>
        <div class='container'>
            <div class='header'>
                <img src='{2}/Ilmanar.svg' alt='Ilmanar Logo' class='logo-img' />
                <div class='platform-name'>ILMANAR</div>
                <div class='platform-subtitle'>RÉINVENTE VOTRE APPRENTISSAGE</div>
            </div>
            <div class='content'>
                {1}
            </div>
            <div class='footer'>
                <p class='footer-text'>© 2025 Ilmanar. Tous droits réservés.</p>
                <p class='footer-text' style='margin-top: 10px; font-size: 11px;'>
                    Si vous n'êtes pas à l'origine de cet email, vous pouvez l'ignorer en toute sécurité.
                </p>
            </div>
        </div>
    </div>
</body>
</html>";

    public static string GetEmailConfirmationTemplate(string userName, string confirmationLink, string frontendUrl)
    {
        var content = $@"
            <h1 class='greeting'>Assalamou alaikoum, {userName} !</h1>
            <p class='text'>
                Nous sommes ravis de vous compter parmi les membres d'<span class='highlight'>Ilmanar</span>. 
                Une dernière étape pour activer votre accès aux connaissances :
            </p>
            
            <div class='button-wrapper'>
                <a href='{confirmationLink}' class='button'>
                    ACTIVER MON COMPTE
                </a>
            </div>

            <span class='alt-text'>
                Si le bouton ne fonctionne pas, utilisez ce lien :<br/>
                {confirmationLink}
            </span>

            <div class='divider'></div>
            
            <p class='text' style='font-size: 13px; color: rgba(255,255,255,0.5); margin-bottom: 0;'>
                ⚠️ Ce lien expirera dans 24 heures pour votre sécurité.
            </p>";

        return string.Format(BaseTemplate, "Activez votre compte Ilmanar", content, frontendUrl);
    }

    public static string GetPasswordResetTemplate(string userName, string resetLink, string frontendUrl)
    {
        var content = $@"
            <h1 class='greeting'>Réinitialisation de mot de passe</h1>
            <p class='text'>
                Bonjour {userName}, vous avez demandé à réinitialiser votre mot de passe sur Ilmanar. 
                Cliquez sur le bouton ci-dessous pour choisir votre nouveau secret :
            </p>
            
            <div class='button-wrapper'>
                <a href='{resetLink}' class='button'>
                    CHANGER MON MOT DE PASSE
                </a>
            </div>

            <span class='alt-text'>
                Si le bouton ne fonctionne pas, utilisez ce lien :<br/>
                {resetLink}
            </span>

            <div class='divider'></div>
            
            <p class='text' style='font-size: 13px; color: rgba(255,255,255,0.5); margin-bottom: 0;'>
                🕒 Ce lien est valable pendant 1 heure. Si vous n'êtes pas l'auteur de cette demande, 
                votre mot de passe actuel ne sera pas modifié.
            </p>";

        return string.Format(BaseTemplate, "Récupération de compte Ilmanar", content, frontendUrl);
    }

    public static string GetWelcomeTemplate(string userName, string frontendUrl)
    {
        var content = $@"
            <h1 class='greeting'>Bienvenue à bord !</h1>
            <p class='text'>
                Félicitations {userName}, votre compte est activé. 
                Tout est prêt pour commencer votre aventure sur Ilmanar.
            </p>
            
            <div class='button-wrapper'>
                <a href='{frontendUrl}' class='button'>
                    ACCÉDER À LA PLATEFORME
                </a>
            </div>";

        return string.Format(BaseTemplate, "Bienvenue sur Ilmanar", content, frontendUrl);
    }
}
