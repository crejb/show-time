using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShowTime.Services;

namespace ShowTimeTests
{
    [TestClass]
    public class DateTimeToHumanReadableFormatConverterTests
    {
        [TestMethod]
        public void TestSeconds()
        {
            Test(TimeSpan.FromSeconds(5), "Just now");
            Test(TimeSpan.FromSeconds(59), "Just now");
        }

        [TestMethod]
        public void TestMinutes()
        {
            Test(TimeSpan.FromSeconds(60), "1 minute ago");
            Test(TimeSpan.FromSeconds(119), "1 minute ago");
            Test(TimeSpan.FromMinutes(2), "2 minutes ago");
            Test(TimeSpan.FromMinutes(2.5), "2 minutes ago");
            Test(TimeSpan.FromMinutes(2.8), "2 minutes ago");
            Test(TimeSpan.FromMinutes(3), "3 minutes ago");
            Test(TimeSpan.FromMinutes(59.8), "59 minutes ago");
        }

        [TestMethod]
        public void TestHours()
        {
            Test(TimeSpan.FromMinutes(60), "1 hour ago");
            Test(TimeSpan.FromMinutes(119), "1 hour ago");
            Test(TimeSpan.FromHours(2), "2 hours ago");
            Test(TimeSpan.FromHours(2.5), "2 hours ago");
            Test(TimeSpan.FromHours(2.8), "2 hours ago");
            Test(TimeSpan.FromHours(3), "3 hours ago");
            Test(TimeSpan.FromHours(23.5), "23 hours ago");
        }

        [TestMethod]
        public void TestDays()
        {
            Test(TimeSpan.FromHours(24), "1 day ago");
            Test(TimeSpan.FromHours(25), "1 day ago");
            Test(TimeSpan.FromHours(48), "2 days ago");
            Test(TimeSpan.FromDays(6), "6 days ago");
        }

        [TestMethod]
        public void TestWeeks()
        {
            Test(TimeSpan.FromDays(7), "1 week ago");
            Test(TimeSpan.FromDays(14), "2 weeks ago");
            Test(TimeSpan.FromDays(15), "2 weeks ago");
            Test(TimeSpan.FromDays(20.6), "2 weeks ago");
            Test(TimeSpan.FromDays(21), "3 weeks ago");
            Test(TimeSpan.FromDays(364), "52 weeks ago");
        }

        [TestMethod]
        public void TestYears()
        {
            Test(TimeSpan.FromDays(365), "1 year ago");
            Test(TimeSpan.FromDays(700), "1 year ago");
            Test(TimeSpan.FromDays(730), "2 years ago");
        }

        private void Test(TimeSpan howLongAgo, string expectedResult)
        {
            DateTimeToHumanReadableFormatConverter converter = new DateTimeToHumanReadableFormatConverter();
            string result = converter.ConvertDateTimeToHumanReadableFormat(DateTime.Now.Subtract(howLongAgo));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedResult, result);
        }
    }
}
