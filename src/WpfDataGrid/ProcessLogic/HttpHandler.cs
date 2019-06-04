using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WpfDataGrid
{
    public class HttpHandler
    {
        public void StoreCurrentFundInfo(string bpndCode, ref List<BasicInfo> listObj)
        {
            var url = $"http://fundgz.1234567.com.cn/js/{bpndCode}.js?rt=1463558676006";
            var objResult = GetRequestObject<BasicInfo>(url);
            listObj.Add(objResult);
        }

        private T GetRequestObject<T>(string url)
        {
            var jsonString = string.Empty;
            var requestObjGet = WebRequest.Create(url);

            using (HttpWebResponse webReponseObjGet = (HttpWebResponse)requestObjGet.GetResponse())
            using (Stream stream = webReponseObjGet.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(stream))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(ModifyJsonFormat(jsonString));
        }

        private string ModifyJsonFormat(string strJason)
        {
            var startIndex = strJason.IndexOf('(');
            var endIndex = strJason.IndexOf(')');
            return strJason.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

    }
}