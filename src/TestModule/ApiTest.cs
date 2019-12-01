using Xunit;
using FundBasicInfoNavigator;

namespace TestModule
{
    public class ApiTest
    {
        [Theory]
        [InlineData("001186",false)]
        [InlineData("x", true)]

        public void GetStreamReaderContents_Test(string bpndCode,bool expectedResult)
        {
            var httpHelper = new FundApiHandler();
            var url = $"http://fundgz.1234567.com.cn/js/{bpndCode}.js?rt=1463558676006";
            var fact = string.IsNullOrEmpty(httpHelper.GetStreamReaderContents(url));
            Assert.Equal(expectedResult, fact);
        }
    }
}
