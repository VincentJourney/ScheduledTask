using System.IO;
using System.Net;
using System.Text;

namespace AS.OCR.Commom.Http
{
    public static class HttpHelper
    {

        public static string HttpPost(string url, string parama, string contentType = "application/json")
        {
            var result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = contentType;
            req.KeepAlive = false;
            byte[] data = Encoding.UTF8.GetBytes(parama);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static string HttpGet(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容 
                using (StreamReader reader = new StreamReader(stream))
                    result = reader.ReadToEnd();
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
    }
}
