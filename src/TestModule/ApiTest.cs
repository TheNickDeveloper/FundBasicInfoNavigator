using Xunit;
using FundBasicInfoNavigator.Data;

namespace TestModule
{
    public class ApiTest
    {
        [Theory]
        [InlineData("001186", false)]
        [InlineData("x", true)]

        public void GetStreamReaderContents_Test(string bpndCode, bool expectedResult)
        {
            var apiDataExtractor = new ApiDataExtractor();
            var url = $"http://fundgz.1234567.com.cn/js/{bpndCode}.js?rt=1463558676006";
            var fact = string.IsNullOrEmpty(apiDataExtractor.GetStreamReaderContents(url));
            Assert.Equal(expectedResult, fact);
        }
    }
}
