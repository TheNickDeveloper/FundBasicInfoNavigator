using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WpfDataGrid
{
    public class HttpHandler
    {
        public T GetRequestObject<T>(string url)
        {
            var jsonString = string.Empty;
            var requestObjGet = WebRequest.Create(url);

            using (HttpWebResponse webReponseObjGet = (HttpWebResponse)requestObjGet.GetResponse())
            using (var stream = webReponseObjGet.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(stream))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return  JsonConvert.DeserializeObject<T>(ModifyJsonFormat(jsonString));
        }

        private string ModifyJsonFormat(string strJason)
        {
            var startIndex = strJason.IndexOf('(');
            var endIndex = strJason.IndexOf(')');
            return strJason.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        public List<BasicInfo> SimulateExampleList( List<string> listSource)
        {
            var httpHandler = new HttpHandler();
            var listTarget = new List<BasicInfo>();

            foreach (var item in listSource)
            {
                var url = $"http://fundgz.1234567.com.cn/js/{item}.js?rt=1463558676006";
                var resultList = httpHandler.GetRequestObject<BasicInfo>(url);
                listTarget.Add(resultList);
            }

            return listTarget;
        }
    }
}