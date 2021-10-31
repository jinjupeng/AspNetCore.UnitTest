using AspNetCore.UnitTest.Service;
using Xunit;

namespace AspNetCore.xUnit
{
    public class UnitTest1
    {
        /// <summary>
        /// 测试方法实现
        /// </summary>
        [Fact]
        public void TestMoneyRegex()
        {
            var moneyRegex = new MoneyRegex();
            Assert.True(moneyRegex.IsPrize("99999"));
            Assert.False(moneyRegex.IsPrize("-99999"));
            Assert.True(moneyRegex.IsPrize("0"));
            Assert.False(moneyRegex.IsPrize("000"));
            Assert.True(moneyRegex.IsPrize("9.00"));
            Assert.True(moneyRegex.IsPrize("99999.99"));
            Assert.False(moneyRegex.IsPrize("999999"));
            Assert.False(moneyRegex.IsPrize("99999.999"));
            Assert.False(moneyRegex.IsPrize("+999"));
            Assert.True(moneyRegex.IsPrize("99999.0"));
            Assert.True(moneyRegex.IsPrize("0.00"));
            Assert.True(moneyRegex.IsPrize("0.0"));
            Assert.False(moneyRegex.IsPrize("99,999"));
        }

        /// <summary>
        /// 测试Controller
        /// </summary>
        [Fact]
        public void TestApi()
        {
            // 会涉及到moq
        }


        // 测试接口IService和实现Service
    }
}
