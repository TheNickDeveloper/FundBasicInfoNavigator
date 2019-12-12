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
    }
}
