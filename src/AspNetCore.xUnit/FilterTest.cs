using AspNetCore.UnitTest.Api.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace AspNetCore.xUnit
{
    #region Test Filter

    /*
     * xunit 提供了 BeforeAfterTestAttribute 来让我们实现一些自定义的逻辑来在测试运行前和运行后执行，和 mvc 里的 action filter 很像，所以这里我把他称为 test filter，来看下面的一个示例，改编自 xunit 的示例：
     * 这里实现了一个设置测试用例运行过程中 Thread.CurrentThread.Culture 的属性，测试结束后恢复原始的属性值，可以用作于 Class 也可以用在测试方法中，使用示例如下：
     */


    [UseCulture("en-US", "zh-CN")]
    public class FilterTest
    {
        [Fact]
        [UseCulture("en-US")]
        public void CultureTest()
        {
            Assert.Equal("en-US", Thread.CurrentThread.CurrentCulture.Name);
        }

        [Fact]
        [UseCulture("zh-CN")]
        public void CultureTest2()
        {
            Assert.Equal("zh-CN", Thread.CurrentThread.CurrentCulture.Name);
        }

        [Fact]
        public void CultureTest3()
        {
            Assert.Equal("en-US", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("zh-CN", Thread.CurrentThread.CurrentUICulture.Name);
        }
    }

    #endregion
}
