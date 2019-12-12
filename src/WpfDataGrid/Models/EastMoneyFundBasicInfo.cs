using FundBasicInfoNavigator.Interfaces;

namespace FundBasicInfoNavigator.Models
{
    public class EastMoneyFundBasicInfo : IFundBasicInfo
    {
        public string FundCode { get; set; }
        public string Name { get; set; } = "No Found";
        public string JZRQ { get; set; } = "No Found";
        public string DWJZ { get; set; } = "No Found";
        public string GSZ { get; set; } = "No Found";
        public string GSZZL { get; set; } = "No Found";
        public string GzTime { get; set; } = "No Found";

        public override string ToString()
        {
            return $"{FundCode.ToString()}, {Name.ToString()}, {JZRQ.ToString()}" +
                $", {DWJZ.ToString()}, {GSZ.ToString()}, {GSZZL.ToString()}, {GzTime.ToString()}";
        }

    }
}