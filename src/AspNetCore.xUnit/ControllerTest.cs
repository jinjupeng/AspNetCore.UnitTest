using AspNetCore.UnitTest.Api.Models.Request;
using AspNetCore.UnitTest.Api.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AspNetCore.xUnit
{
    public class ControllerTest
    {
        /// <summary>
        /// https://codepiecesorg.wordpress.com/2018/09/28/unit-test-for-custom-validation-attribute/
        /// https://stackoverflow.com/questions/4666678/how-can-i-unit-test-my-custom-validation-attribute/42206031
        /// </summary>
        [Fact]
        public void TestValidationAttribute()
        {
            var model = new UnitTestRequest() { UserName = "1", UserName2 = "2" };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);
            Assert.True(isValid);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/45017295/assert-an-exception-using-xunit
        /// </summary>
        [Fact]
        public void TestException()
        {
            //arrange
            UnitTestService profiles = new UnitTestService();
            // act & assert
            Assert.Throws<ArgumentException>(() => profiles.GetUserList(""));

            //// arrange
            //UnitTestService service = new UnitTestService();
            ////act
            //Action act = () => service.GetUserList("");
            ////assert
            //ArgumentException exception = Assert.Throws<ArgumentException>(act);
            ////The thrown exception can be used for even more detailed assertions.
            //Assert.Equal("expected error message here", exception.Message);
        }


    }
}
