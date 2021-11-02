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
        /// ������ʽ����
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
        /// ʹ�� Fact ��ǵĲ��Է��������з���������ֻ�б�� Theory �ķ��������з�������
        /// </summary>
        [Fact]
        public void AddTest()
        {
            // ʹ�� Assert �����Խ���Ƿ����Ԥ��
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
        /// Assert.Throw(exceptionType, action) �� Assert.Throw<TExceptionType>(action) ������ exception ����ֻ����������ͣ��̳���������͵Ĳ��㣬�� fail��
        /// �� Assert.ThrowAny<TExceptionType>(action) �������һ�㣬��������ͻ����Ǽ̳���������͵Ķ����ԡ�
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
        /// ��������������ķ�ʽ���� InlineData����Ӷ�� InlineData ����ʹ�ò�ͬ�Ĳ������ݽ��в���
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
         * MemberData ����һ���̶��Ͻ�� InlineData ���ڵ����⣬MemberData ֧���ֶΡ����Ի򷽷�������Ҫ������������������
         * 1����Ҫ�� public ��
         * 2����Ҫ�� static ��
         * 3��������ʽת��Ϊ IEnumerable<object[]> ���߷�������ֵ������ʽת��Ϊ IEnumerable<object[]>
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
         * MemberData ���֮���ṩ�˸���ı����Ϳ��Զ���̶ȣ�ֻ���ڵ�ǰ��������ʹ�ã���Ҫ������໹�ǲ��У�xunit ���ṩ�� DataAttribute ��ʹ�����ǿ���ͨ���Զ��巽ʽʵ�ֲ��Է�������Դ������Ҳ���Դ����ݿ��ﶯ̬��ѯ�����ݣ�д��һ���򵥵�ʾ�������Բο������ʾ����
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
     * �ڲ��Է����������Ҫ���һЩ������Ϣ��ֱ������ Console.Write/Console.WriteLine ��û��Ч���ģ��ڲ��Է����������Ҫʹ�� ITestoutputHelper ����������������ʾ����
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
