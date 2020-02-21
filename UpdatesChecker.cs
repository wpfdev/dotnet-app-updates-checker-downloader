using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppUpdater
{
    public class UpdatesChecker
    {
        public static bool CheckUpdates(string appName, string curVersion)
        {
            try
            {
                var wReq = GetRequest(string.Format(AppSettings.UPDATES_SERVER_URL, appName));
                string response;
                using (Stream str = (wReq.GetResponse() as HttpWebResponse).GetResponseStream())
                {
                    StreamReader sr = new StreamReader(str);
                    response = sr.ReadToEnd().Trim();
                }
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(response);
                return !xdoc.DocumentElement.SelectSingleNode("ProgramInfo/LastVersion").InnerText.Equals(curVersion);
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Update app
        /// </summary>
        /// <param name="appName">name</param>
        /// <param name="fileName">file name, default name get from xml</param>
        /// <param name="openFileAfterUpdate">run app after download</param>
        public static void DownloadUpdates(string appName, string fileName = "", bool openFileAfterUpdate = true)
        {
            //Определяем ссылку на файл обновления
            var wReq = GetRequest(string.Format(AppSettings.UPDATES_SERVER_URL, appName));
            string response;
            using (Stream str = (wReq.GetResponse() as HttpWebResponse).GetResponseStream())
            {
                StreamReader sr = new StreamReader(str);
                response = sr.ReadToEnd().Trim();
            }
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(response);
            string fileurl = xdoc.DocumentElement.SelectSingleNode("ProgramInfo/DownloadURLs/URL").InnerText;
            //скачиваем файл
            WebClient Client = new WebClient();
            Client.Headers[HttpRequestHeader.Accept] = "text/html, application/xhtml+xml, */*";
            Client.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU";
            Client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            Client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            Client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            string fileNameCalc = string.IsNullOrEmpty(fileName) ? fileurl.Split('/').Last() : fileName;

            Client.DownloadFile(fileurl, fileNameCalc);
            Client.Dispose();

            if (openFileAfterUpdate)
                Process.Start(fileNameCalc);
        }

        internal static HttpWebRequest GetRequest(string url, string method = "GET")
        {
            HttpWebRequest wReq = (HttpWebRequest)HttpWebRequest.Create(url);
            wReq.Method = method;

            wReq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:34.0) Gecko/20100101 Firefox/34.0";
            wReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wReq.Headers.Add("Accept-Language: ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
            wReq.Headers.Add("Accept-Encoding: gzip, deflate");
            wReq.Headers.Add("Accept-Charset: windows-1251,utf-8;q=0.7,*;q=0.7");
            wReq.ContentType = "application/x-www-form-urlencoded; charset=utf-8;";
            return wReq;
        }
    }
}
