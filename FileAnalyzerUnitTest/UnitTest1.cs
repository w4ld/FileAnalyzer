using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileAnalyzer;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileAnalyzerUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSizeToProperPrefix()
        {

            Assert.AreEqual("1MB", SizeToProperPrefix(1000000));
        }

        [TestMethod]
        public void TestParsePhoneNumbers()
        {
            List<string> numbers = new List<string>();
            List<string> numbersOut = new List<string>();
            numbers.Add("+1123-456-7890");
            numbers.Add("+11234567890");
            numbers.Add("(123)-456-7890");
            numbers.Add("+34561123-456-7890");
            numbers.Add("+34561123-4567890");
            numbers.Add("+1123-4567890");
            numbers.Add("+1123456-7890");
            numbers.Add("123-4567890");
            numbers.Add("3251123456-7890");

            numbersOut.Add("+1123-456-7890");
            numbersOut.Add("+11234567890");
            numbersOut.Add("(123)-456-7890");
            numbersOut.Add("+1123-4567890");
            numbersOut.Add("+1123456-7890");
            numbersOut.Add("123-4567890");
           

            List<Match> matches = FoundStrings.ParsePhoneNumbers(numbers);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            Assert.AreEqual(numbersOut, strMatches);
        }
    }
}
