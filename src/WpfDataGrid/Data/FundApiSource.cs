using System.Collections.Generic;

namespace FundBasicInfoNavigator.Data
{
    public class FundApiSource
    {
        public List<string> FundApiType
        {
            get
            {
                return new List<string>
                {
                    "EastMoneyFund",
                    "Test1",
                    "Test2"
                };
            }
        }

        public string EastMoneyFundUrl
        {
            get => $"http://fundgz.1234567.com.cn/js/targetFundCode.js?rt=1463558676006";
        }
    }
}
