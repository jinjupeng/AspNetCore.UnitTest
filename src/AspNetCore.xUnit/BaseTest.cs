using AspNetCore.UnitTest.Api.Helpers;
using AspNetCore.UnitTest.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AspNetCore.xUnit
{
    /// <summary>
    /// https://github.com/xunit/samples.xunit
    /// </summary>
    public class BaseTest
    {

        #region BaseTest

        /// <summary>
        /// 正则表达式测试
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
        /// 使用 Fact 标记的测试方法不能有方法参数，只有标记 Theory 的方法可以有方法参数
        /// </summary>
        [Fact]
        public void AddTest()
        {
            // 使用 Assert 来断言结果是否符合预期
            Assert.Equal(4, Helper.Add(2, 2));
            Assert.NotEqual(3, Helper.Add(2, 2));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void AddTestWithTestData(int num1, int num2)
        {
            Assert.Equal(num1 + num2, Helper.Add(num1, num2));
        }

        #endregion

        #region Exception Assert

        /// <summary>
        /// Assert.Throw(exceptionType, action) 和 Assert.Throw<TExceptionType>(action) 这样的 exception 类型只能是这个类型，继承于这个类型的不算，会 fail，
        /// 而 Assert.ThrowAny<TExceptionType>(action) 则更包容一点，是这个类型或者是继承于这个类型的都可以。
        /// </summary>
        [Fact]
        public void ExceptionTest()
        {
            var exceptionType = typeof(ArgumentException);
            Assert.Throws(exceptionType, Helper.ArgumentExceptionTest);
            Assert.Throws<ArgumentException>(testCode: Helper.ArgumentExceptionTest);
        }

        [Fact]
        public void ExceptionAnyTest()
        {
            Assert.Throws<ArgumentNullException>(Helper.ArgumentNullExceptionTest);
            Assert.ThrowsAny<ArgumentNullException>(Helper.ArgumentNullExceptionTest);
            Assert.ThrowsAny<ArgumentException>(Helper.ArgumentNullExceptionTest);
        }

        #endregion

        #region InlineData

        /// <summary>
        /// 最基本数据驱动的方式当属 InlineData，添加多个 InlineData 即可使用不同的测试数据进行测试
        /// </summary>
        /// <param name="num"></param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InlineDataTest(int num)
        {
            Assert.True(num > 0);
        }

        #endregion

        #region MemberData

        /*
         * MemberData 可以一定程度上解决 InlineData 存在的问题，MemberData 支持字段、属性或方法，且需要满足下面两个条件：
         * 1、需要是 public 的
         * 2、需要是 static 的
         * 3、可以隐式转换为 IEnumerable<object[]> 或者方法返回值可以隐式转换为 IEnumerable<object[]>
         * 
         */

        [Theory]
        [MemberData(nameof(TestMemberData))]
        public void MemberDataPropertyTest(int num)
        {
            Assert.True(num > 0);
        }

        public static IEnumerable<object[]> TestMemberData =>
            Enumerable.Range(1, 10)
                .Select(x => new object[] { x })
                .ToArray();

        [Theory]
        [MemberData(nameof(TestMemberDataField))]
        public void MemberDataFieldTest(int num)
        {
            Assert.True(num > 0);
        }

        public static readonly IList<object[]> TestMemberDataField = Enumerable.Range(1, 10).Select(x => new object[] { x }).ToArray();

        [Theory]
        [MemberData(nameof(TestMemberDataMethod), 10)]
        public void MemberDataMethodTest(int num)
        {
            Assert.True(num > 0);
        }

        public static IEnumerable<object[]> TestMemberDataMethod(int count)
        {
            return Enumerable.Range(1, count).Select(i => new object[] { i });
        }

        #endregion

        #region Custom Data Source

        /*
         * MemberData 相比之下提供了更大的便利和可自定义程度，只能在当前测试类中使用，想要跨测试类还是不行，xunit 还提供了 DataAttribute ，使得我们可以通过自定义方式实现测试方法数据源，甚至也可以从数据库里动态查询出数据，写了一个简单的示例，可以参考下面的示例：
         */

        public class NullOrEmptyStringDataAttribute : DataAttribute
        {
            public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            {
                yield return new object[] { null };
                yield return new object[] { string.Empty };
            }
        }

        [Theory]
        [NullOrEmptyStringData]
        public void CustomDataAttributeTest(string value)
        {
            Assert.True(string.IsNullOrEmpty(value));
        }

        #endregion


    }

    #region Output

    /*
     * 在测试方法中如果想要输出一些测试信息，直接是用 Console.Write/Console.WriteLine 是没有效果的，在测试方法中输出需要使用 ITestoutputHelper 来输出，来看下面的示例：
     */
    public class OutputTest
    {
        private readonly ITestOutputHelper _outputHelper;

        public OutputTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void ConsoleWriteTest()
        {
            Console.WriteLine("Console");
        }

        [Fact]
        public void OutputHelperTest()
        {
            _outputHelper.WriteLine("Output");
        }
    }

    #endregion

}
