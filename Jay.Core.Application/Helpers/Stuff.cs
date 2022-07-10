using Jay.Core.Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Helpers
{
    public static class Stuff
    {
        public static string URL = "https://localhost:44342";

        public static string EncryptSHA256(string pass)
        {
            //Create a SHA256
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));

                //Convert To String
                StringBuilder builder = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string SetDate(DateTime fecha)
        {
            long time = (fecha.Second) + (fecha.Minute * 60) + (fecha.Hour * 60 * 60)
            + (fecha.Day * 24 * 60 * 60) + (fecha.Month * 30 * 24 * 60 * 60)
            + (fecha.Year * 365 * 24 * 60 * 60);

            DateTime now = DateTime.Now;
            long present = (now.Second) + (now.Minute * 60) + (now.Hour * 60 * 60)
            + (now.Day * 24 * 60 * 60) + (now.Month * 30 * 24 * 60 * 60)
            + (now.Year * 365 * 24 * 60 * 60);

            long plazo = present - time;

            if (plazo < 59)
            {
                return plazo == 1 ? "Hace 1 segundo" : "Hace " + plazo + " segundos";
            }
            else if (plazo < (60 * 60))
            {
                return plazo < 120 ? "Hace 1 minuto" : "Hace " + (int)(plazo / 60) + " minutos";
            }
            else if (plazo < (24 * 60 * 60))
            {
                return plazo < (2 * 60 * 60) ? "Hace 1 hora" : "Hace " + (int)(plazo / (60 * 60)) + " horas";
            }
            else if (plazo < (7 * 24 * 60 * 60))
            {
                return plazo < (2 * 24 * 60 * 60) ? "Hace 1 dia" : "Hace " + (int)(plazo / (24 * 60 * 60)) + " dias";
            }
            else if (plazo < (30 * 24 * 60 * 60))
            {
                return plazo < (2 * 7 * 24 * 60 * 60) ? "Hace 1 semana" : "Hace " + (int)(plazo / (7 * 24 * 60 * 60)) + " semanas";
            }
            else if (plazo < (365 * 24 * 60 * 60))
            {
                return plazo < (2 * 30 * 24 * 60 * 60) ? "Hace 1 mes" : "Hace " + (int)(plazo / (30 * 24 * 60 * 60)) + " meses";
            }
            else
            {
                return plazo < (2 * 365 * 24 * 60 * 60) ? "Hace 1 año" : "Hace " + (int)(plazo / (365 * 24 * 60 * 60)) + " años";
            }
        }

        public static string SetVerifyEmail(string secLogoLink, string verUrl)
        {
            return """
                <!DOCTYPE html>
                <html>
                <head>

                    <meta charset="utf-8">
                    <meta http-equiv="x-ua-compatible" content="ie=edge">
                    <title>Verificacion de Email</title>
                    <meta name="viewport" content="width=device-width, initial-scale=1">
                    <style type="text/css">
                    /**
                    * Google webfonts. Recommended to include the .woff version for cross-client compatibility.
                    */
                    @media screen {
                    @font-face {
                        font-family: 'Source Sans Pro';
                        font-style: normal;
                        font-weight: 400;
                        src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                    }

                    @font-face {
                        font-family: 'Source Sans Pro';
                        font-style: normal;
                        font-weight: 700;
                        src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                    }
                    }

                    /**
                    * Avoid browser level font resizing.
                    * 1. Windows Mobile
                    * 2. iOS / OSX
                    */
                    body,
                    table,
                    td,
                    a {
                    -ms-text-size-adjust: 100%; /* 1 */
                    -webkit-text-size-adjust: 100%; /* 2 */
                    }

                    /**
                    * Remove extra space added to tables and cells in Outlook.
                    */
                    table,
                    td {
                    mso-table-rspace: 0pt;
                    mso-table-lspace: 0pt;
                    }

                    /**
                    * Better fluid images in Internet Explorer.
                    */
                    img {
                    -ms-interpolation-mode: bicubic;
                    }

                    /**
                    * Remove blue links for iOS devices.
                    */
                    a[x-apple-data-detectors] {
                    font-family: inherit !important;
                    font-size: inherit !important;
                    font-weight: inherit !important;
                    line-height: inherit !important;
                    color: inherit !important;
                    text-decoration: none !important;
                    }

                    /**
                    * Fix centering issues in Android 4.4.
                    */
                    div[style*="margin: 16px 0;"] {
                    margin: 0 !important;
                    }

                    body {
                    width: 100% !important;
                    height: 100% !important;
                    padding: 0 !important;
                    margin: 0 !important;
                    }

                    /**
                    * Collapse table borders to avoid space between cells.
                    */
                    table {
                    border-collapse: collapse !important;
                    }

                    a {
                    color: #1a82e2;
                    }

                    img {
                    height: auto;
                    line-height: 100%;
                    text-decoration: none;
                    border: 0;
                    outline: none;
                    }
                    </style>

                </head>
                <body style="background-color: #e9ecef;">

                    <!-- start preheader -->
                    <div class="preheader" style="display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;">
                    Verifica tu cuenta para continuar
                    </div>
                    <!-- end preheader -->

                    <!-- start body -->
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                    <!-- start logo -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                            <tr>
                            <td align="center" valign="top" style="padding: 36px 24px;">
                                
                            </td>
                            </tr>
                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end logo -->

                    <!-- start hero -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;">
                                <h1 style="margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;">Casi has terminado!</h1>
                            </td>
                            </tr>
                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end hero -->

                    <!-- start copy block -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">

                            <!-- start copy -->
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;">
                                <p style="margin: 0;">Presiona el boton debajo para verifcar tu cuenta. Si no creaste una cuenta en <a href="
                """+
                URL
                +"""
                ">Jay</a>, puedes ignorar y eliminar este email</p>
                            </td>
                            </tr>
                            <!-- end copy -->

                            <!-- start button -->
                            <tr>
                            <td align="left" bgcolor="#ffffff">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" bgcolor="#ffffff" style="padding: 12px;">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                        <td align="center" bgcolor="#1a82e2" style="border-radius: 6px;">
                                            <a href="
                """+
                URL+verUrl
                +"""
                " target="_blank" style="display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;">Verificar Email</a>
                                        </td>
                                        </tr>
                                    </table>
                                    </td>
                                </tr>
                                </table>
                            </td>
                            </tr>
                            <!-- end button -->

                            <!-- start copy -->
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;">
                                <p style="margin: 0;">Si el boton no funciona, puedes copiar y pegar el siguiente link en tu navegador:</p>
                                <p style="margin: 0;"><a href="
                """+
                URL+verUrl
                +"""
                " target="_blank">
                """+
                URL+verUrl
                +"""
                </a></p>
                            </td>
                            </tr>
                            <!-- end copy -->

                            <!-- start copy -->
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf">
                                <p style="margin: 0;">Se despide,<br>Equipo de soporte de Jay</p>
                            </td>
                            </tr>
                            <!-- end copy -->

                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end copy block -->

                    <!-- start footer -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef" style="padding: 24px;">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">

                            <!-- start permission -->
                            <tr>
                            <td align="center" bgcolor="#e9ecef" style="padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;">
                                <p style="margin: 0;">Recibiste este email debido a que recibimos una peticion de verificacion para tu cuenta. Si no pediste una verificacion de cuenta, puedes ignorar y eliminar este email.</p>
                            </td>
                            </tr>
                            <!-- end permission -->


                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end footer -->

                    </table>
                    <!-- end body -->

                </body>
                </html>
                """;
        }

        public static string SetResetEmail(string name, string password)
        {
            return """
                <!DOCTYPE html>
                <html>
                <head>

                    <meta charset="utf-8">
                    <meta http-equiv="x-ua-compatible" content="ie=edge">
                    <title>Contraseña reiniciada</title>
                    <meta name="viewport" content="width=device-width, initial-scale=1">
                    <style type="text/css">
                    /**
                    * Google webfonts. Recommended to include the .woff version for cross-client compatibility.
                    */
                    @media screen {
                    @font-face {
                        font-family: 'Source Sans Pro';
                        font-style: normal;
                        font-weight: 400;
                        src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                    }

                    @font-face {
                        font-family: 'Source Sans Pro';
                        font-style: normal;
                        font-weight: 700;
                        src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                    }
                    }

                    /**
                    * Avoid browser level font resizing.
                    * 1. Windows Mobile
                    * 2. iOS / OSX
                    */
                    body,
                    table,
                    td,
                    a {
                    -ms-text-size-adjust: 100%; /* 1 */
                    -webkit-text-size-adjust: 100%; /* 2 */
                    }

                    /**
                    * Remove extra space added to tables and cells in Outlook.
                    */
                    table,
                    td {
                    mso-table-rspace: 0pt;
                    mso-table-lspace: 0pt;
                    }

                    /**
                    * Better fluid images in Internet Explorer.
                    */
                    img {
                    -ms-interpolation-mode: bicubic;
                    }

                    /**
                    * Remove blue links for iOS devices.
                    */
                    a[x-apple-data-detectors] {
                    font-family: inherit !important;
                    font-size: inherit !important;
                    font-weight: inherit !important;
                    line-height: inherit !important;
                    color: inherit !important;
                    text-decoration: none !important;
                    }

                    /**
                    * Fix centering issues in Android 4.4.
                    */
                    div[style*="margin: 16px 0;"] {
                    margin: 0 !important;
                    }

                    body {
                    width: 100% !important;
                    height: 100% !important;
                    padding: 0 !important;
                    margin: 0 !important;
                    }

                    /**
                    * Collapse table borders to avoid space between cells.
                    */
                    table {
                    border-collapse: collapse !important;
                    }

                    a {
                    color: #1a82e2;
                    }

                    img {
                    height: auto;
                    line-height: 100%;
                    text-decoration: none;
                    border: 0;
                    outline: none;
                    }
                    </style>

                </head>
                <body style="background-color: #e9ecef;">

                    <!-- start preheader -->
                    <div class="preheader" style="display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;">
                    Contraseña Reiniciada
                    </div>
                    <!-- end preheader -->

                    <!-- start body -->
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                    <!-- start logo -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                            <tr>
                            <td align="center" valign="top" style="padding: 36px 24px;">
                                
                            </td>
                            </tr>
                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end logo -->

                    <!-- start hero -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;">
                                <h1 style="margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;">Contraseña reiniciada</h1>
                            </td>
                            </tr>
                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end hero -->

                    <!-- start copy block -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">

                            <!-- start copy -->
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;">
                                <p style="margin: 0;">
                """
                +name+
                """
                , tu contraseña ha sido reiniciada correctamente, esta es tu nueva contraseña: <br><br><b>
                """ +
                password
                + """
                </b><br><br>Ya puedes iniciar sesion con ella</p>
                            </td>
                            </tr>
                            <!-- end copy -->

                            <!-- start button -->
                            <tr>
                            <td align="left" bgcolor="#ffffff">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" bgcolor="#ffffff" style="padding: 12px;">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                        <td align="center" bgcolor="#1a82e2" style="border-radius: 6px;">
                                            <a href="
                """ +
                URL
                + """
                " target="_blank" style="display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;">Ir a Jay</a>
                                        </td>
                                        </tr>
                                    </table>
                                    </td>
                                </tr>
                                </table>
                            </td>
                            </tr>
                            <!-- end button -->


                            <!-- start copy -->
                            <tr>
                            <td align="left" bgcolor="#ffffff" style="padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf">
                                <p style="margin: 0;">Se despide,<br>Equipo de soporte de Jay</p>
                            </td>
                            </tr>
                            <!-- end copy -->

                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end copy block -->

                    <!-- start footer -->
                    <tr>
                        <td align="center" bgcolor="#e9ecef" style="padding: 24px;">
                        <!--[if (gte mso 9)|(IE)]>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                        <td align="center" valign="top" width="600">
                        <![endif]-->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">

                            <!-- start permission -->
                            <tr>
                            <td align="center" bgcolor="#e9ecef" style="padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;">
                                
                            </td>
                            </tr>
                            <!-- end permission -->


                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        </td>
                    </tr>
                    <!-- end footer -->

                    </table>
                    <!-- end body -->

                </body>
                </html>
                """;
        }
    }
}
