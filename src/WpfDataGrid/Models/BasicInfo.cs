namespace FundBasicInfoNavigator
{
    public class BasicInfo
    {
        public string FundCode { get; set; }
        public string Name { get; set; }
        public string JZRQ { get; set; }
        public string DWJZ { get; set; }
        public string GSZ { get; set; }
        public string GSZZL { get; set; }
        public string GzTime { get; set; }

        public override string ToString()
        {
            return $"{FundCode.ToString()}, {Name.ToString()}, {JZRQ.ToString()}" +
                $", {DWJZ.ToString()}, {GSZ.ToString()}, {GSZZL.ToString()}, {GzTime.ToString()}";
        }

    }
}