using FundBasicInfoNavigator.Interfaces;
using System.IO;
using System.Net;

namespace FundBasicInfoNavigator.Data
{
    public class ApiDataExtractor : IApiDataExtractor
    {
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
    }
}
