using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FundBasicInfoNavigator
{
    public class FundApiHandler
    {
        public List<string> LogList { get; set; } = new List<string>();

        public string GetFundApiType(string fundApiType)
        {
            switch (fundApiType)
            {
                case "EastMoneyFund":
                    return $"http://fundgz.1234567.com.cn/js/targetFundCode.js?rt=1463558676006";
                default:
                    return $"http://fundgz.1234567.com.cn/js/targetFundCode.js?rt=1463558676006";
            }
        }

        public void StoreCurrentFundInfo(string fundApiType, string fundCode, ref List<BasicInfo> listObj)
        {
            var url = GetFundApiType(fundApiType).Replace("targetFundCode", fundCode);
            var contents = GetStreamReaderContents(url);
            var objResult = new BasicInfo();
            bool searchResult = false;

            if (!string.IsNullOrEmpty(contents))
            {
                //todo, could tyr auto mappter from one class to another 
                var convertResult = JsonConvert.DeserializeObject<BasicInfo>(ModifyJsonFormat(contents));

                if (convertResult != null)
                {
                    objResult = convertResult;
                    searchResult = true;
                }
            }

            if (searchResult)
            {
                LogList.Add($"Bond Code: {fundCode} success.");
            }
            else
            {
                objResult.FundCode = fundCode;
                objResult.Name = "No Found";
                objResult.JZRQ = "No Found";
                objResult.DWJZ = "No Found";
                objResult.GSZ = "No Found";
                objResult.GSZZL = "No Found";
                objResult.GzTime = "No Found";

                LogList.Add($"Bond Code: {fundCode} no found.");
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
                    return streamReader.ReadToEnd();
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