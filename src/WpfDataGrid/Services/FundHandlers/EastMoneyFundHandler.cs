using Newtonsoft.Json;
using System.Collections.Generic;
using FundBasicInfoNavigator.Interfaces;
using FundBasicInfoNavigator.Models;
using FundBasicInfoNavigator.Data;

namespace FundBasicInfoNavigator.Services.FundHandlers
{
    public class EastMoneyFundHandler : IFundHandler<EastMoneyFundBasicInfo>
    {
        private readonly IApiDataExtractor _apiDataExtractor;
        private readonly FundApiSource _fundApiSource;

        public List<string> LogList { get; set; } = new List<string>();
        public List<EastMoneyFundBasicInfo> ResultList { get; set; }
        public string ApiUrl
        {
            get => _fundApiSource.EastMoneyFundUrl;
        }

        public EastMoneyFundHandler(IApiDataExtractor apiDataExtractor, FundApiSource fundApiSource)
        {
            _apiDataExtractor = apiDataExtractor;
            _fundApiSource = fundApiSource;
            ResultList = new List<EastMoneyFundBasicInfo>();
        }

        public void StoreCurrentFundInfo(string fundCode)
        {
            var url = ApiUrl.Replace("targetFundCode", fundCode);
            var contents = _apiDataExtractor.GetStreamReaderContents(url);
            var objResult = new EastMoneyFundBasicInfo();
            bool searchResult = false;

            if (!string.IsNullOrEmpty(contents))
            {
                var convertResult = JsonConvert.DeserializeObject<EastMoneyFundBasicInfo>(ModifyJsonFormat(contents));

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
                LogList.Add($"Bond Code: {fundCode} no found.");
            }

            ResultList.Add(objResult);
        }

        private string ModifyJsonFormat(string strJason)
        {
            var startIndex = strJason.IndexOf('(');
            var endIndex = strJason.IndexOf(')');
            return strJason.Substring(startIndex + 1, endIndex - startIndex - 1);
        }
    }
}