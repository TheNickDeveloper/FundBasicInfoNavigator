using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WpfDataGrid
{
    public class HttpHandler
    {
        public List<string> LogList { get; set; } = new List<string>();

        public void StoreCurrentFundInfo(string bpndCode, ref List<BasicInfo> listObj)
        {
            var url = $"http://fundgz.1234567.com.cn/js/{bpndCode}.js?rt=1463558676006";
            var contents = GetStreamReaderContents(url);
            var objResult = new BasicInfo();

            if ( !string.IsNullOrEmpty(contents))
            {
                objResult = JsonConvert.DeserializeObject<BasicInfo>(ModifyJsonFormat(contents));
                LogList.Add($"Bond Code: {bpndCode} success.");
            }
            else
            {
                objResult.FundCode = bpndCode;
                objResult.Name = "No Found";
                LogList.Add($"Bond Code: {bpndCode} no found.");
            }

            listObj.Add(objResult);
        }

        public string GetStreamReaderContents(string url)
        {
            var requestObjGet = WebRequest.Create(url);

            try
            {
                using (HttpWebResponse webReponseObjGet = (HttpWebResponse)requestObjGet.GetResponse())
                using (Stream stream = webReponseObjGet.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader?.ReadToEnd();
                }
            }
            catch (System.Exception)
            {

                return string.Empty;
            }
        }

        private string ModifyJsonFormat(string strJason)
        {
            var startIndex = strJason.IndexOf('(');
            var endIndex = strJason.IndexOf(')');
            return strJason.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

    }
}