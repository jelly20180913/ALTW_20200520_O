using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Configuration;
using NLog;
using System.Data;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace WebApi.Common
{
    public class Function
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static string  FeedbackEmail = ConfigurationManager.AppSettings["FeedbackEmail"];
        static string FeedbackEmailCC = ConfigurationManager.AppSettings["FeedbackEmailCC"];
        static string MailServerHost= ConfigurationManager.AppSettings["MailServerHost"];
        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <returns>Server name</returns>
        public static string GetServerName()
        {
            return Environment.MachineName;

            // Or can use
            // return System.Net.Dns.GetHostName();
            // return System.Windows.Forms.SystemInformation.ComputerName;
            // return System.Environment.GetEnvironmentVariable("COMPUTERNAME"); 
        }

        /// <summary>
        /// Gets the login account.
        /// </summary>
        /// <returns>Login account</returns>
        public static string GetLoginAccount()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <returns>ip addresses</returns>
        public static string[] GetIpAddresses()
        {
            string hostName = GetServerName();
            return System.Net.Dns.GetHostAddresses(hostName).Select(i => i.ToString()).ToArray();
        }
        /// <summary>
        /// water mark
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="textColor"></param>
        /// <param name="backColor"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static System.Drawing.Image DrawText(String text, System.Drawing.Font font, Color textColor, Color backColor, double height, double width)
        {
            //create a bitmap image with specified width and height
            Image img = new Bitmap((int)width, (int)height);
            Graphics drawing = Graphics.FromImage(img);
            //get the size of text
            SizeF textSize = drawing.MeasureString(text, font);
            //set rotation point
            drawing.TranslateTransform(((int)width - textSize.Width) / 2, ((int)height - textSize.Height) / 2);
            //rotate text
            drawing.RotateTransform(-45);
            //reset translate transform
            drawing.TranslateTransform(-((int)width - textSize.Width) / 2, -((int)height - textSize.Height) / 2);
            //paint the background
            drawing.Clear(backColor);
            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);
            //draw text on the image at center position
            drawing.DrawString(text, font, textBrush, ((int)width - textSize.Width) / 2, ((int)height - textSize.Height) / 2);
            drawing.Save();
            return img;
        }
        /// <summary>
        /// altw mailserver setting for email 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        public static bool SendEmail(string subject,string body,string from ,string email)
        {
            bool _Success = false;
            System.Net.Mail.MailMessage _MyMail = new System.Net.Mail.MailMessage();
            _MyMail.From = new System.Net.Mail.MailAddress(from);
            _MyMail.To.Add(email); //設定收件者Email
            _MyMail.Bcc.Add(FeedbackEmailCC); //加入密件副本的Mail   
            _MyMail.Bcc.Add(FeedbackEmail);
            _MyMail.Subject = subject;
            _MyMail.Body = body; //設定信件內容
            _MyMail.IsBodyHtml = true; //是否使用html格式 
            System.Net.Mail.SmtpClient _MySMTP = new System.Net.Mail.SmtpClient(MailServerHost, 25); 
            try
            {
                _MySMTP.Send(_MyMail);
                _MyMail.Dispose(); //釋放資源
                _Success = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
            }
            return _Success;
        }
        public static string SetFontColor(string columnName, string color)
        {
            string _Font = "<font color = '" + color + "'>" + columnName + "</font>";
            return _Font;
        }
        /// <summary>
        /// render html datatable 
        /// </summary>
        /// <param name="dtInfo"></param>
        /// <returns></returns>
        public static string RenderDataTableToHtml(DataTable dtInfo)
        {
            StringBuilder tableStr = new StringBuilder();

            if (dtInfo.Rows != null && dtInfo.Rows.Count > 0)
            {
                int columnsQty = dtInfo.Columns.Count;
                int rowsQty = dtInfo.Rows.Count;

                tableStr.Append("<TABLE BORDER=\"1\">");
                tableStr.Append("<TR>");
                for (int j = 0; j < columnsQty; j++)
                {
                    tableStr.Append("<TH>" + dtInfo.Columns[j].ColumnName + "</TH>");
                }
                tableStr.Append("</TR>");

                for (int i = 0; i < rowsQty; i++)
                {
                    tableStr.Append("<TR>");
                    for (int k = 0; k < columnsQty; k++)
                    {
                        tableStr.Append("<TD>");
                        tableStr.Append(dtInfo.Rows[i][k].ToString());
                        tableStr.Append("</TD>");
                    }
                    tableStr.Append("</TR>");
                }

                tableStr.Append("</TABLE>");
            }

            return tableStr.ToString();
        }
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            try
            {  
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    DescriptionAttribute description = property.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                    dt.Columns.Add(new DataColumn(description.Description, property.PropertyType));
                }

                foreach (var v in list)
                {
                    DataRow newRow = dt.NewRow();
                    int conlumnIndex = 0;
                    foreach (PropertyInfo property in v.GetType().GetProperties())
                    {
                        //newRow[property.Name] = vehicle.GetType().GetProperty(property.Name).GetValue(vehicle, null);
                        newRow[conlumnIndex] = property.GetValue(v, null);
                        conlumnIndex++;
                    }
                    dt.Rows.Add(newRow);
                } 
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
            }
            return dt;
        }

    }
}
