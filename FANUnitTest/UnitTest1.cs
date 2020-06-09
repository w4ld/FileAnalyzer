using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileAnalyzer;
namespace FANUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestParsePhoneNumbers1()
        {
            List<string> numbers = new List<string>();
            numbers.Add("123-456-7890");
            numbers.Add("123456-7890");
            numbers.Add("123-4567890");
            numbers.Add("1234567890");

            List<Match> matches = FoundStrings.ParsePhoneNumbers(numbers);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in numbers)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParsePhoneNumbers2()
        {
            List<string> numbers = new List<string>();
            numbers.Add("+1123-456-7890");
            numbers.Add("+11234567890");
            numbers.Add("+1123-4567890");
            numbers.Add("+1123456-7890");

            List<Match> matches = FoundStrings.ParsePhoneNumbers(numbers);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in numbers)
                Assert.Contains(s, strMatches);


        }
        [Test]
        public void TestParsePhoneNumbers3()
        {
            List<string> numbers = new List<string>();
            numbers.Add("+1ij123-456-7890");//letters interspered
            numbers.Add("+1234567890");//has +1, but is only 9 digits 
            numbers.Add("+11999923-4567890");//too many digits
            numbers.Add("+1123456-789");
            numbers.Add("+21123456-789");

            List<Match> matches = FoundStrings.ParsePhoneNumbers(numbers);
            foreach (Match m in matches)
            {
                System.Console.WriteLine("Fail: {0}", m.Value);
            }
            Assert.AreEqual(0, matches.Count);


        }
        [Test]
        public void TestParseWebsites1()
        {
            List<string> websites = new List<string>();
            websites.Add("https://oreilly.com");
            websites.Add("https://google.com");
            List<Match> matches = FoundStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseWebsites2()
        {
            List<string> websites = new List<string>();
            websites.Add("http://oreilly.com");
            websites.Add("http://google.com");
            List<Match> matches = FoundStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseWebsites3()
        {
            List<string> websites = new List<string>();
            websites.Add(@"https://learning.oreilly.com/library/view/program");
            websites.Add(@"https://learning.oreilly.com");
           
            websites.Add(@"http://google.com/flights");
            List<Match> matches = FoundStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseWebsites4()
        {
            //the realistic hard ones... maybe split these  up
            List<string> websites = new List<string>();
            websites.Add(@"https://learning.oreilly.com/library/view/programming-for-the/9781509302154/ch03.html#ch03");
            websites.Add(@"https://github.com/MicrosoftLearning/20483-Programming-in-C-Sharp/blob/master/Instructions/20483C_MOD01_LAB_MANUAL.md");

            websites.Add(@"https://www.amazon.com/kuman-3O-IUX5-O0TZ-Digital-Oscilloscope-pre-soldered/dp/B0195ZIURK/ref=lp_393269011_1_9?s=industrial&ie=UTF8&qid=1590246868&sr=1-9");
            List<Match> matches = FoundStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseDLLs1()
        {
            List<string> dlls = new List<string>();
            dlls.Add("win32.dll");
            dlls.Add("kernel.dll");
            dlls.Add("win32api.dll");
            dlls.Add("system32.dll");
            List<Match> matches = FoundStrings.ParseDLLs(dlls);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in dlls)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseDLLs2()
        {
            //Not dlls
            List<string> dlls = new List<string>();
            dlls.Add("win32.dllo");
            dlls.Add("kernelw.oejf.dll");
            dlls.Add("win32api.dl");
            dlls.Add("system32");
            List<Match> matches = FoundStrings.ParseDLLs(dlls);           
            Assert.AreEqual(0, matches.Count);
        }
    }
}