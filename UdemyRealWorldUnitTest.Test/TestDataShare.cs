using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyRealWorldUnitTest.Test
{
    public static class TestDataShare
    {
        public static IEnumerable<object[]> IsOddOrEvenData
        {
            get
            {
                yield return new object[] { 1, true };
                yield return new object[] { 2, false };
            }
        }

        public static IEnumerable<object[]> AddTwoNumbersData
        {
            get
            {
                yield return new object[] { 2,5, 7 };
                yield return new object[] { 5,6, 11 };
            }
        }
    }
}
