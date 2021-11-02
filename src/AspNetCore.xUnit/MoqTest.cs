using AspNetCore.UnitTest.Api.Models;
using AspNetCore.UnitTest.Api.Persistence;
using AspNetCore.UnitTest.Api.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.xUnit
{
    /// <summary>
    /// https://mp.weixin.qq.com/s/bch5kMj6aJ3I8Jy6QbUnDg
    /// https://github.com/moq/moq4/wiki/Quickstart
    /// https://github.com/moq/moq4
    /// https://github.com/WeihanLi/SamplesInPractice/blob/master/XunitSample/MoqTest.cs
    /// https://www.cnblogs.com/tylerzhou/p/11410337.html
    /// https://www.cnblogs.com/cgzl/p/9304567.html
    /// https://www.cnblogs.com/haogj/archive/2011/07/22/2113496.html
    /// </summary>
    public class MoqTest
    {
        /*
         * Moq 是 .NET 中一个很流行的 Mock 框架，使用 Mock 框架我们可以只针对我们关注的代码进行测试，对于依赖项使用 Mock 对象配置预期的依赖服务的行为。
         * Moq 是基于 Castle 的动态代理来实现的，基于动态代理技术动态生成满足指定行为的类型
         */

        [Fact]
        public void BasicTest()
        {
            var userIdProviderMock = new Mock<IUserIdProvider>();
            userIdProviderMock.Setup(x => x.GetUserId())
                .Returns("mock");

            Assert.Equal("mock", userIdProviderMock.Object.GetUserId());
        }

        #region Match Arguments

        /*
         * 通常我们的方法很多是带有参数的，在使用 Moq 的时候我们可以通过设置参数匹配为不同的参数返回不同的结果，来看下面的这个例子：
         */

        [Fact]
        public void MethodParameterMatch()
        {
            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(true);
            repositoryMock.Setup(x => x.GetById(It.Is<int>(_ => _ > 0)))
                .Returns((int id) => new TestModel()
                {
                    Id = id
                });

            var service = new TestService(repositoryMock.Object);
            var deleted = service.Delete(new TestModel());
            Assert.True(deleted);

            var result = service.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            result = service.GetById(-1);
            Assert.Null(result);

            repositoryMock.Setup(x => x.GetById(It.Is<int>(_ => _ <= 0)))
                .Returns(new TestModel()
                {
                    Id = -1
                });
            result = service.GetById(0);
            Assert.NotNull(result);
            Assert.Equal(-1, result.Id);
        }

        /*
         * 通过 It.IsAny<T> 来表示匹配这个类型的所有值，通过 It.Is<T>(Expression<Func<bool>>) 来设置一个表达式来断言这个类型的值

         * 通过上面的例子，我们可以看的出来，设置返回值的时候，可以直接设置一个固定的返回值，也可以设置一个委托来返回一个值，也可以根据方法的参数来动态配置返回结果
         */

        #endregion

        #region Async Method

        /// <summary>
        /// 现在很多地方都是在用异步方法，Moq 设置异步方法有三种方式，一起来看一下示例：
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AsyncMethod()
        {
            var repositoryMock = new Mock<IRepository>();

            // Async method mock
            repositoryMock.Setup(x => x.GetCountAsync())
                .Returns(Task.FromResult(10));
            repositoryMock.Setup(x => x.GetCountAsync())
                .ReturnsAsync(10);
            // start from 4.16
            repositoryMock.Setup(x => x.GetCountAsync().Result)
                .Returns(10);

            var service = new TestService(repositoryMock.Object);
            var result = await service.GetCountAsync();
            Assert.True(result > 0);
        }

        #endregion

        #region Mock Property

        /*
         * Moq 也可以 mock 属性，property 的本质是方法加一个字段，所以也可以用 Mock 方法的方式来 Mock 属性，只是使用 Mock 方法的方式进行 Mock 属性的话，后续修改属性值就不会引起属性值的变化了，如果修改属性，则要使用 SetupProperty 的方式来 Mock 属性，具体可以参考下面的这个示例：
         */

        [Fact]
        public void Property()
        {
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);
            repositoryMock.Setup(x => x.Version).Returns(1);
            Assert.Equal(1, service.Version);

            service.Version = 2;
            Assert.Equal(1, service.Version);
        }

        [Fact]
        public void PropertyTracking()
        {
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);
            repositoryMock.SetupProperty(x => x.Version, 1);
            Assert.Equal(1, service.Version);

            service.Version = 2;
            Assert.Equal(2, service.Version);
        }

        #endregion

        #region Callback

        /*
         * 我们在设置 Mock 行为的时候可以设置 callback 来模拟方法执行时的逻辑，来看一下下面的示例：
         */

        [Fact]
        public void Callback()
        {
            var deletedIds = new List<int>();
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);
            repositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    deletedIds.Add(id);
                })
                .Returns(true);

            for (var i = 0; i < 10; i++)
            {
                service.Delete(new TestModel() { Id = i });
            }
            Assert.Equal(10, deletedIds.Count);
            for (var i = 0; i < 10; i++)
            {
                Assert.Equal(i, deletedIds[i]);
            }
        }

        #endregion

        #region Verification

        /*
         * 有时候我们会验证某个方法是否执行，并不需要关注是否方法的返回值，这时我们可以使用 Verification 验证某个方法是否被调用，示例如下：
         * 如果方法没有被调用，就会引发一个 MockException 异常：
         */

        [Fact]
        public void Verification()
        {
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);

            service.Delete(new TestModel()
            {
                Id = 1
            });

            repositoryMock.Verify(x => x.Delete(1));
            repositoryMock.Verify(x => x.Version, Times.Never());
            Assert.Throws<MockException>(() => repositoryMock.Verify(x => x.Delete(2)));
        }

        #endregion

        #region Generic Type

        /*
         * 如果要 Mock 指定类型的数据，可以直接指定泛型类型，如上面的第一个测试用例，如果要不同类型设置不同的结果一种是直接设置类型，如果要指定某个类型或者某个类型的子类，可以用 It.IsSubtype<T>，如果要指定值类型可以用 It.IsValueType，如果要匹配所有类型则可以用 It.IsAnyType
         * 有些方法会是泛型方法，对于泛型方法，我们来看下面的示例：
         */

        [Fact]
        public void GenericType()
        {
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);

            repositoryMock.Setup(x => x.GetResult<int>(It.IsAny<string>()))
                .Returns(1);
            Assert.Equal(1, service.GetResult(""));

            repositoryMock.Setup(x => x.GetResult<string>(It.IsAny<string>()))
                .Returns("test");
            Assert.Equal("test", service.GetResult<string>(""));
        }

        [Fact]
        public void GenericTypeMatch()
        {
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);

            repositoryMock.Setup(m => m.GetNum<It.IsAnyType>())
                .Returns(-1);
            repositoryMock.Setup(m => m.GetNum<It.IsSubtype<TestModel>>())
                .Returns(0);
            repositoryMock.Setup(m => m.GetNum<string>())
                .Returns(1);
            repositoryMock.Setup(m => m.GetNum<int>())
                .Returns(2);

            Assert.Equal(0, service.GetNum<TestModel>());
            Assert.Equal(1, service.GetNum<string>());
            Assert.Equal(2, service.GetNum<int>());
            Assert.Equal(-1, service.GetNum<byte>());
        }

        #endregion

        #region Sequence

        /*
         * 我们可以通过 Sequence 来指定一个方法执行多次返回不同结果的效果，看一下示例就明白了：
         * 第一次调用返回值是1，第二次是2，第三次是3，第四次是抛了一个 InvalidOperationException
         */

        [Fact]
        public void Sequence()
        {
            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);

            repositoryMock.SetupSequence(x => x.GetCount())
                .Returns(1)
                .Returns(2)
                .Returns(3)
                .Throws(new InvalidOperationException());

            Assert.Equal(1, service.GetCount());
            Assert.Equal(2, service.GetCount());
            Assert.Equal(3, service.GetCount());
            Assert.Throws<InvalidOperationException>(() => service.GetCount());
        }

        #endregion

        #region Mock Behavior

        /*
         * 默认的 Mock Behavior 是 Loose，默认没有设置预期行为的时候不会抛异常，会返回方法返回值类型的默认值或者空数组或者空枚举，
         * 在声明 Mock 对象的时候可以指定 Behavior 为 Strict，这样就是一个**"真正"**的 mock 对象，没有设置预期行为的时候就会抛出异常，示例如下：
         * 使用 Strict 模式不设置预期行为的时候就会报异常
         */

        [Fact]
        public void MockBehaviorTest()
        {
            // Make mock behave like a "true Mock",
            // raising exceptions for anything that doesn't have a corresponding expectation: in Moq slang a "Strict" mock;
            // default behavior is "Loose" mock,
            // which never throws and returns default values or empty arrays, enumerable, etc

            var repositoryMock = new Mock<IRepository>();
            var service = new TestService(repositoryMock.Object);
            Assert.Equal(0, service.GetCount());
            Assert.Null(service.GetList());

            var arrayResult = repositoryMock.Object.GetArray();
            Assert.NotNull(arrayResult);
            Assert.Empty(arrayResult);

            repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            Assert.Throws<MockException>(() => new TestService(repositoryMock.Object).GetCount());
        }

        #endregion
    }
}
