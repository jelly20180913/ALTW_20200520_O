using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using NLog;
using System.Configuration;
namespace TestWebApi.Common
{
    public class FtpFile
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static List<string> ListError = new List<string>();
        private static string account = ConfigurationManager.AppSettings["Account"];
        private static string password = ConfigurationManager.AppSettings["Password"];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePath">purpose path</param>
        /// <param name="childPath">partner name path</param>
        /// <param name="ftpPath"></param>
        public static string Download(string basePath, string childPath, string ftpPath, string fileName)
        {
            string _Log = "";
            ListError.Clear();
            string _FilePath = System.Environment.CurrentDirectory;
            string _DownloadPath = _FilePath + "//" + basePath + "//" + childPath + "//" + fileName;
            createDirectory(basePath, childPath);
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(_DownloadPath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpPath + "//" + childPath + "//" + fileName));

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(account, password);
                reqFTP.KeepAlive = true;
                reqFTP.UsePassive = true;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;

                int bufferSize = 204800;

                int readCount;

                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                response.Close();
                outputStream.Close();


            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " FtpFile:Download:\r\n" + ex.Message;
                logger.Error(_Log);
                _DownloadPath = "";
                ListError.Add(_Log);
            }
            return _DownloadPath;
        }
        private static void createDirectory(string basePath, string childPath)
        {
            string _Log = "";
            ListError.Clear();
            try
            {
                string[] child = childPath.Split('/');
                string _FilePath = System.Environment.CurrentDirectory;
                string pathString = _FilePath + "/" + basePath;
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                string _CustomerFilePath = pathString + "/" + child[0];
                if (!Directory.Exists(_CustomerFilePath))
                {
                    Directory.CreateDirectory(_CustomerFilePath);
                }
                string _CustomerFileTablePath = _CustomerFilePath + "/" + child[1];
                if (!Directory.Exists(_CustomerFileTablePath))
                {
                    Directory.CreateDirectory(_CustomerFileTablePath);
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " FtpFile:createDirectory:\r\n" + ex.Message;
                logger.Error(_Log);
                ListError.Add(_Log);
            }
        }
        public static List<string> GetList(string childPath, string ftpPath)
        {
            string _Log = "";
            ListError.Clear();
            List<string> _ListFile = new List<string>();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpPath + "//" + childPath));

                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(account, password);
                reqFTP.KeepAlive = true;
                reqFTP.UsePassive = true;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                StreamReader ftpStream = new StreamReader(response.GetResponseStream());

                string str = ftpStream.ReadLine();
                while (str != null)
                {
                    _ListFile.Add(str);
                    str = ftpStream.ReadLine();
                }


                ftpStream.Close();

                response.Close();


            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " FtpFile:GetList:\r\n" + ex.Message;
                logger.Error(_Log);
                ListError.Add(_Log);
            }
            return _ListFile;
        }
        public static void RemoveFile(string childPath, string ftpPath, string fileName)
        {
            string _Log = "";
            ListError.Clear();
            List<string> _ListFile = new List<string>();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpPath + "//" + childPath + "//" + fileName));

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.Credentials = new NetworkCredential(account, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " FtpFile:RemoveFile:\r\n" + ex.Message;
                logger.Error(_Log);
                ListError.Add(_Log);
            }
        }
        /// <summary>
        ///  copy edi data to bin 
        /// </summary>
        /// <param name="basePath">Edi</param>
        /// <param name="childPath">AS2_Mouser/Send</param>
        /// <param name="fileName"></param>
        /// <param name="edi">edi data</param>
        public static void CopyTo(string basePath, string childPath, string fileName, string edi)
        {
            string _FilePath = System.Environment.CurrentDirectory;
            string _DownloadPath = _FilePath + "//" + basePath + "//" + childPath + "//" + fileName;
            createDirectory(basePath, childPath);
            File.WriteAllText(_DownloadPath, edi);
        }
        /// <summary>
        /// upload file to ftp server
        /// </summary>
        /// <param name="basePath">Edi</param>
        /// <param name="childPath">EX:AS2_Mounser </param>
        /// <param name="ftpPath"></param>
        /// <param name="fileName">EX:test.edi</param>
        public static void Upload(string basePath, string childPath, string ftpPath, string fileName)
        {
            string _Log = "";
            string _FilePath = System.Environment.CurrentDirectory;
            string _DownloadPath = _FilePath + "//" + basePath + "//" + childPath + "//" + fileName;
            try
            {
                // ***Uri need fileName parameter
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpPath + childPath + "//" + fileName));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(account, password);
                // Copy the contents of the file to the request stream.
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(_DownloadPath))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " FtpFile:Upload:\r\n" + ex.Message;
                logger.Error(_Log);
                ListError.Add(_Log);
            }
        }
    }
}
