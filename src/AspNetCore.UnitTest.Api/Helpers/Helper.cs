using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Helpers
{
    public class Helper
    {
        public static int Add(int x, int y)
        {
            return x + y;
        }

        public static void ArgumentExceptionTest() => throw new ArgumentException();

        public static void ArgumentNullExceptionTest() => throw new ArgumentNullException();
    }
}
