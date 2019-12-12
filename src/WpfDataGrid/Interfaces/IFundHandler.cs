using System.Collections.Generic;

namespace FundBasicInfoNavigator.Interfaces
{
    public interface IFundHandler<T>
    {
        string ApiUrl { get; }
        List<string> LogList { get; set; }
        List<T> ResultList { get; set; }

        void StoreCurrentFundInfo(string fundCode);
    }
}