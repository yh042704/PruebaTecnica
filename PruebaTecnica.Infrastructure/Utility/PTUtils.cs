using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Xml.Linq;

namespace PruebaTecnica.Infrastructure.Utility
{
    public class PTUtils
    {
        public static string JsonToXmlWithExplicitRoot(string json, string rootName, string item)
        {
            XDocument xmlDoc = new();
            XElement root = new(rootName)
            {
                Name = rootName
            };

            var dataTable = JsonConvert.DeserializeObject<DataTable>(json);
            root.Add(
                     from row in dataTable!.AsEnumerable()
                     select new XElement(item,
                                         from column in dataTable!.Columns.Cast<DataColumn>()
                                         select new XElement(column.ColumnName, row[column])
                                        )
                   );


            xmlDoc.Add(root);

            return xmlDoc.ToString();
        }

        public static string GetMessageError(Exception e)
        {
            string aggregateExcMessage = string.Empty;

            if (e is AggregateException)
            {
                var aggrEx = e as AggregateException;
                aggregateExcMessage = e.Message + $" Possible reasons: {string.Join(", ", aggrEx!.InnerExceptions.Select(s => s.Message))}";
            }
            else
            {
                aggregateExcMessage = e.Message;
            }

            return aggregateExcMessage;
        }

        public async static Task SendEmail(string mailFrom, string mailNameFrom, string subject, string Body, string SendTo = "gab.merino@gmail.com")
        {
            SmtpClient SMTP = new("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(SendTo, "hiexosbnienmuwpj")
            };

            if (!string.IsNullOrEmpty(SendTo))
            {
                MailMessage Message = new();
                Message.ReplyToList.Add(new MailAddress(mailFrom));
                Message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;

                foreach (string emailTo in SendTo.Split(';'))
                {
                    Message.To.Add(emailTo.Trim());
                }

                Message.CC.Add(new MailAddress("yh042704@gmail.com"));
                Message.From = new MailAddress("gmerino@gmail.com", "Notificaciones Prueba Técnica", System.Text.Encoding.UTF8);
                Message.Subject = subject;
                Message.SubjectEncoding = System.Text.Encoding.UTF8;
                Message.Priority = MailPriority.High;
                Message.IsBodyHtml = true;
                Message.AlternateViews.Add(GetBody("Mensaje Plataforma  Prueba Técnica", $"Correo: {mailFrom} <br/>Nombre: {mailNameFrom} <br/> Mensaje: {Body}"));

                try
                {
                    await SMTP.SendMailAsync(Message);
                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Message.Dispose();
                    SMTP.Dispose();
                }
            }
        }

        private static AlternateView GetBody(string titulo, string detalle)
        {
            string detalleDefault = string.Concat("<div style='margin-left:0px;margin-top:0px;margin-right:0px;margin-bottom:0px;background-color:#f8f8f8;font-family:'Open Sans',Arial,sans-serif'><div class='adM'>",
                                                "</div><table width='100%' border='0' cellspacing='0' cellpadding='0'>",
                                                "<tbody><tr><td align='center'><table border='0' cellpadding='0' cellspacing='0' style='height:100%;width:660px;background-color:#f8f8f8;padding:10px auto'>",
                                                "<tbody><tr style='height:0'><td>",
                                                "<table border='0' cellpadding='0' cellspacing='0' style='height:100%;width:660px;background-color:#f8f8f8;padding:10px auto'>",
                                                "<tbody><tr style='height:0'><td>",
                                                "<table border='0' cellpadding='0' cellspacing='0' style='background-color:#fff;padding-top:10px;padding-bottom:10px;width:100%'>",
                                                "<tbody><tr><td align='left'>",
                                                "<img src=cid:MailImage id='img' width='100px' height='100px' />",
                                                "</td><td colspan='2' align='right'></td></tr></tbody>",
                                                "</table></td></tr><tr><td valign='top'>",
                                                "<table border='0' cellpadding='2' cellspacing='0' style='height:auto;width:660px;border:none;background-color:#ffffff;padding-left:40px;padding-right:20px'>",
                                                "<tbody><tr><td style='font-family:'Open Sans',Arial,sans-serif;font-size:15px;font-weight:bold;color:#2f576d;text-align:left;padding-bottom:15px;padding-top:25px;border-top:1px solid #e3e2e2'><hr/><b>",
                                                string.IsNullOrEmpty(titulo) ? "" : titulo,
                                                "</b><br/><hr/></td></tr><tr>",
                                                "<td style='font-family:'Open Sans',Arial,sans-serif;font-size:13px;color:#333333'>",
                                                "<span>", string.IsNullOrEmpty(detalle) ? "" : detalle, "</span>",
                                                "</td></tr><tr><td>&nbsp;</td></tr><tr>",
                                                "<td style='font-family:'Open Sans',Arial,sans-serif;font-size:13px;color:#333333'>Atentamente,",
                                                "</td></tr><tr><td style='font-family:'Open Sans',Arial,sans-serif;font-size:12px;color:#333333;border-bottom-width:1px;border-bottom-style:solid;border-bottom-color:#e6e5e5'>",
                                                "Equipo Tecnico</td></tr></tbody></table></td></tr><tr><td valign='top'>&nbsp;</td></tr>",
                                                "</tbody></table></td></tr></tbody></table></div>");


            MemoryStream ms = new(File.ReadAllBytes("Img/img-01-1.png"));
            LinkedResource Img = new(ms, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "MailImage"
            };

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(detalleDefault, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(Img);

            return alternateView;
        }

        public static DateTime ConvertDateTimeSQL(string fecha, string[]? vdateFormats = null)
        {
            string[] dateFormats = vdateFormats ?? ["yyyyMMdd", "dd-MM-yyyy", "dd/MM/yyyy", "M/d/yyyy", "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", "yyyy-MM-dd'T'HH:mm:ss'Z'"];
            DateTime Fecha = DateTime.ParseExact(fecha, dateFormats, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.AdjustToUniversal);

            return Fecha;
        }
    }
}
