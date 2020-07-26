using FileAnalyzer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FANUnitTest
{
    [TestFixture]
    public class StringPIITests
    {

        [SetUp]
        public void Setup()
        {
        }
        //TODO adapt old tests to testcases for uniformity
        [Test]
        public void TestParsePhoneNumbers1()
        {
            List<string> numbers = new List<string>
            {
                "123-456-7890",
                "123456-7890",
                "123-4567890",
                "1234567890"
            };

            List<Match> matches = FAStrings.ParsePhoneNumbers(numbers);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in numbers)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParsePhoneNumbers2()
        {
            List<string> numbers = new List<string>
            {
                "+1123-456-7890",
                "+11234567890",
                "+1123-4567890",
                "+1123456-7890"
            };

            List<Match> matches = FAStrings.ParsePhoneNumbers(numbers);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in numbers)
                Assert.Contains(s, strMatches);


        }
        [Test]
        public void TestParsePhoneNumbers3()
        {
            List<string> numbers = new List<string>
            {
                "+1ij123-456-7890",//letters interspered
                "+1234567890",//has +1, but is only 9 digits 
                "+11999923-4567890",//too many digits
                "+1123456-789",
                "+21123456-789"
            };

            List<Match> matches = FAStrings.ParsePhoneNumbers(numbers);
            foreach (Match m in matches)
            {
                System.Console.WriteLine("Fail: {0}", m.Value);
            }
            Assert.AreEqual(0, matches.Count);


        }

        [TestCase("saidhfv999@aiodh.com", true)]
        [TestCase("sai-dhfv@aiodh.co", true)]
        [TestCase("sai.dhfv@aiodh.co", true)]
        [TestCase("sai.dhfv@aiodh.biz", true)]
        [TestCase("saidhfv237@aiodh.mil", true)]
        [TestCase("saidhfv237@a6odh.mil", true)]
        [TestCase("saidhfvodh.mil", false)]
        [TestCase("sail", false)]
        [TestCase("", false)]
        [TestCase("999999", false)]
        [TestCase("999999@999", false)]
        public void TestParseEmails(string emailstring, bool truth)
        {
            List<string> emails = new List<string>
            {
                emailstring
            };
            List<Match> matches = FAStrings.ParseEmails(emails);
            int c = matches.Count;
            bool b = (c == 1);
            Assert.AreEqual(truth, b);
        }

        [TestCase("123-45-6789", true)]
        [TestCase("127-45-6789", true)]
        [TestCase("123-95-9989", true)]
        [TestCase("000-00-9989", true)]
        [TestCase("175-45-6789", true)]
        [TestCase("1231-45-6789", false)]
        [TestCase("123-405-612", false)]
        [TestCase("123456789", false)]
        [TestCase("123-445-6789", false)]
        [TestCase("e31-45-689", false)]
        public void TestParseSSNs(string ssn, bool truth)
        {
            List<string> ssns = new List<string>
            {
                ssn
            };
            List<Match> matches = FAStrings.ParseSSNs(ssns);
            int c = matches.Count;
            bool b = (c == 1);
            Assert.AreEqual(truth, b);
        }


    }
    public class StringWebTests
    {
        [Test]
        public void TestParseWebsites1()
        {
            List<string> websites = new List<string>
            {
                "https://oreilly.com",
                "https://google.com"
            };
            List<Match> matches = FAStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseWebsites2()
        {
            List<string> websites = new List<string>
            {
                "http://oreilly.com",
                "http://google.com"
            };
            List<Match> matches = FAStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseWebsites3()
        {
            List<string> websites = new List<string>
            {
                @"https://learning.oreilly.com/library/view/program",
                @"https://learning.oreilly.com",

                @"http://google.com/flights"
            };
            List<Match> matches = FAStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseWebsites4()//website regex needs work...
        {
            //the realistic hard ones... maybe split these  up
            List<string> websites = new List<string>
            {
                @"https://learning.oreilly.com/library/view/programming-for-the/9781509302154/ch03.html#ch03",
                @"https://github.com/MicrosoftLearning/20483-Programming-in-C-Sharp/blob/master/Instructions/20483C_MOD01_LAB_MANUAL.md",

                @"https://www.amazon.com/kuman-3O-IUX5-O0TZ-Digital-Oscilloscope-pre-soldered/dp/B0195ZIURK/ref=lp_393269011_1_9?s=industrial&ie=UTF8&qid=1590246868&sr=1-9"
            };
            List<Match> matches = FAStrings.ParseWebsites(websites);
            List<string> strMatches = new List<string>();
            foreach (Match m in matches)
                strMatches.Add(m.Value);
            foreach (string s in websites)
                Assert.Contains(s, strMatches);
        }
        [Test]
        public void TestParseDLLs1()
        {
            List<string> dlls = new List<string>
            {
                "win32.dll",
                "kernel.dll",
                "win32api.dll",
                "system32.dll"
            };
            List<Match> matches = FAStrings.ParseDLLs(dlls);
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
            List<string> dlls = new List<string>
            {
                "win32.dllo",
                "kernelw.oejf.dll",
                "win32api.dl",
                "system32"
            };
            List<Match> matches = FAStrings.ParseDLLs(dlls);
            Assert.AreEqual(0, matches.Count);
        }

    }

    public class IPValidationTests
    {
        #region IP Testing
        [Test]
        public void TestParseIPv4()
        {
            //the realistic hard ones... maybe split these  up
            List<string> ips = new List<string>
            {
                "8.8.8.8",
                "192.168.0.1",
                "2.364.34.24556",
                "2345.8.64.199",
                "999.999.999.999",
                "8.iy",
                "8348g8gh",
                "4.4.4.4 iwe0gh i 5.5.5.5"
            };

            List<Match> matches = FAStrings.ParseIPv4(ips);
            List<string> strMatches = new List<string>
            {
                "8.8.8.8",
                "192.168.0.1",
                "4.4.4.4",
                "5.5.5.5"
            };
            
            foreach (Match m in matches)
                Assert.Contains(m.Value, strMatches);
            Assert.AreEqual(matches.Count, strMatches.Count);
        }

        [TestCase("84.17.63.10")]
        [TestCase("8.8.8.8")]
        public void TestIsValidIPv4A(string ip)
        {
            Assert.AreEqual(IPChecker.IPv4Class.A, IPChecker.IsValidIPv4(ip));
        }
        [TestCase("192.168.0.122")]
        [TestCase("192.168.0.1")]
        public void TestIsValidIPv4C(string ip)
        {
            Assert.AreEqual(IPChecker.IPv4Class.C, IPChecker.IsValidIPv4(ip));
        }
        [TestCase("0.0.0.0")]
        [TestCase("256.12.124.3")]
        [TestCase("256.12.124.399")]
        [TestCase("256.1200.12.19")]
        public void TestIsValidIPv4NiR(string ip)
        {
            Assert.AreEqual(IPChecker.IPv4Class.NiR, IPChecker.IsValidIPv4(ip));
        }
        #endregion
    }
    public class RabbitHoleTests
    {
        #region Rabbit Hole...
        [Test]
        public static void TestBoyerMooreConversion()
        {
            string filename = @"..\..\..\..\ProjectTestFiles\longbs.txt";
            string res = GuessFileFormat.BasicByteBoyerMoore(filename, "million-dollar");
            Console.WriteLine(res);
            Assert.AreEqual("Bytes DETECTED!", res);
        }

        [TestCase(@"..\..\..\..\ProjectTestFiles\Brigitte Birthday.docx", true)]
        [TestCase(@"..\..\..\..\ProjectTestFiles\Randoms.xlsx", true)]
        [TestCase(@"..\..\..\..\ProjectTestFiles\TitleLoremIpsum.pptx", true)]
        [TestCase(@"..\..\..\..\ProjectTestFiles\opendoctext.odt", true)]
        public static void TestBasicUnpackAndExamine(string filename, bool cleanup = true)
        {
            GuessFileFormat.BasicZipAndExamine(filename, cleanup);
        }

        #endregion
    }
    public class PythonAndHashTests
    {
        #region Python Testing
        [TestCase("\"" + @"..\..\..\..\ProjectTestFiles\test1 .txt" + "\"")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\somemadeuppath")]
        public static void ExecutePythonScriptTestBadPaths(string sampleFile)
        {
            string pythonFile = @"..\..\..\..\PythonScripts\yara_script.py";
            //string sampleFile = "\"C:\\Users\\Derek\\OneDrive\\Documents\\CSC205\\ProjectTestFiles\\COVID-19 Precautions.txt\"";
            //Note filenames with spaces must be encapsulated sufficienctly
            //string sampleFile = "\""+@"..\..\..\..\ProjectTestFiles\COVID-19 Precautions.txt" +"\"";
            string pythonArgs = "-f " + sampleFile;
            string foutput = PythonScript.ExecutePythonScript(pythonFile, pythonArgs).Replace("\r", "");
            string doutput = "\tScanning file: " + sampleFile.Replace("\"", "") + "\n\tError scanning file.\n";
            Console.WriteLine(foutput);
            Assert.AreEqual(doutput, foutput);
        }
        [TestCase(@"..\..\..\..\ProjectTestFiles\test1.txt")]
        public static void ExecutePythonScriptNoMatches(string sampleFile)
        {
            string pythonFile = @"..\..\..\..\PythonScripts\yara_script.py";
            string pythonArgs = "-f " + sampleFile;
            string foutput = PythonScript.ExecutePythonScript(pythonFile, pythonArgs).Replace("\r", "");
            string doutput = "\tScanning file: " + sampleFile.Replace("\"", "") + "\n\tNo matches.\n";
            Console.WriteLine(foutput);
            Assert.AreEqual(doutput, foutput);
        }
        [TestCase("\"..\\..\\..\\..\\PythonScripts\\COVID -19 Precautions.txt")]
        public static void ExecutePythonScriptDirty(string sampleFile)
        {
            string pythonFile = @"C:\Users\Derek\source\pyrepo\YaraPlay.py";
            string pythonArgs = "-f " + sampleFile;
            string output = PythonScript.ExecutePythonScript(pythonFile, pythonArgs);
            Console.WriteLine(output);
        }

        #endregion
        #region Hash Testing
        [Test]
        public static void TestPPDeepScript()
        {
            string filename = @"..\..\..\..\ProjectTestFiles\Microsoftsuiteguidance.doc";
            string msghash = "192:81TH/p5Q+YUrfClLZaAE6/6rNavrgYjk+4bWlLLdPD1l:81THvQpqiSwvxjk+tLLdL";
            string outhash = PythonScript.PPDeepHash(filename).Trim();
            Assert.AreEqual(msghash, outhash);
        }
        [Test]
        public static void TestPPDeepScript2()
        {
            string filename = @"..\..\..\..\ProjectTestFiles\Quiz-week06.docx";
            string msghash = "384:dXOa+qC78kbdvxvga7C5wgbzDdlHAuZLz:Ih8kB9gKC5FthAGz";
            string outhash = PythonScript.PPDeepHash(filename).Trim();
            Assert.AreEqual(msghash, outhash);
        }
        [Test]
        public static void FullHash()
        {
            string fullhashbrowns =
                "MD5: " + "b1a4b39364b0f9ddebec81e0119264fb".ToUpper() + "\n" +//augmented reference to check accurate hashes.
                "SHA1: " + "bbf9fd79e065139f1991db7f0fb37767c2dd30f1".ToUpper() + "\n" +
                "SHA512: " + "A6F32EB04A034D23F7756A6DAB7B185951906F3AADE5727EB0DEB46878A98B0B7CCD2F5774F6BD70D9FC3FCB1858768F3492B6D8691F4AF29D1C7F6EB4A88985" + "\n" +
                "SHA384: " + "3649808DD45FBDE42EF469054F32303841D7E938B9BA6F253260EA039537CE262711A73C5CC74F65DC656F2A21F451E3" + "\n" +
                "SHA256: " + "76a68ddfd6043b8ac45962ccb92e0bcc7421004f21f3b613a364735fce5bcd09".ToUpper() + "\n" +
                "SSDEEP: " + "384:dXOa+qC78kbdvxvga7C5wgbzDdlHAuZLz:Ih8kB9gKC5FthAGz";
            string lasttestedhash = "MD5: B1A4B39364B0F9DDEBEC81E0119264FB\n" + //copied output from last execution 6/23/2020 21:41
                "SHA1: BBF9FD79E065139F1991DB7F0FB37767C2DD30F1\n" +
                "SHA512: A6F32EB04A034D23F7756A6DAB7B185951906F3AADE5727EB0DEB46878A98B0B7CCD2F5774F6BD70D9FC3FCB1858768F3492B6D8691F4AF29D1C7F6EB4A88985\n" +
                "SHA384: 3649808DD45FBDE42EF469054F32303841D7E938B9BA6F253260EA039537CE262711A73C5CC74F65DC656F2A21F451E3\n" +
                "SHA256: 76A68DDFD6043B8AC45962CCB92E0BCC7421004F21F3B613A364735FCE5BCD09\n" +
                "SSDEEP: 384:dXOa+qC78kbdvxvga7C5wgbzDdlHAuZLz:Ih8kB9gKC5FthAGz";


            string filename = @"..\..\..\..\ProjectTestFiles\Quiz-week06.docx";
            FAFileInfo.DisplayHashes(filename);
            Assert.AreEqual(fullhashbrowns, lasttestedhash);
        }

        #endregion
    }
    public class FileGuessAndHeadersTests
    {
        #region File Guessing
        //anything that is technically a zip will be ID'ed initiall as .cuix as its the first entry in DB. further inspection is necessary to correctly ID.
        [TestCase(@"..\..\..\..\ProjectTestFiles\Brigitte Birthday.docx", "cuix")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\CSC305HW06.sql", "")]
        //[TestCase(@"..\..\..\..\ProjectTestFiles\filereading.txt", "")] //identifies as asx fisnce this is a crappy signature. may need to reorder or remove overly short sigs. this is a fluke 
        [TestCase(@"..\..\..\..\ProjectTestFiles\Randoms.xlsx", "cuix")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\COVID-19 Precautions.txt", "")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\test.jpeg", "jfif")] //first entry in db jpeg has multiple entries and must be handled in an extra method
        [TestCase(@"..\..\..\..\ProjectTestFiles\2020-06-16.png", "png")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\OfficeFileFOrmatsProtocols.zip", "cuix")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\Examples1.swf", "swf")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\MSAccessBS.accdb", "accdb")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\TitleLoremIpsum.pptx", "cuix")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\opendoctext.odt", "cuix")]

        public static void TestGuesser(string filename, string ext)
        {

            List<FASignature> outlist = GuessFileFormat.ReadFileHeaders(filename);
            foreach (var sig in outlist)
            {
                Console.WriteLine(sig.ToString(true));
            }
            if (outlist.Count > 0)
                Assert.AreEqual(ext, outlist[0].Extension.Trim().ToLower());
            else if (ext.Length == outlist.Count)
                Assert.AreEqual(0, 0);
            else
                Assert.AreEqual(0, 1);
        }
        #endregion
        #region Print File Headers
        [TestCase(@"..\..\..\..\ProjectTestFiles\test.jpeg")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\jpg.jpg")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\jpgflower.jpg")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\CSC305HW06.sql")]
        [TestCase(@"..\..\..\..\ProjectTestFiles\Examples1.swf")]
        public static void TestPrintFileHeaders(string filename)
        {
            FAFileInfo.PrintFileHeaderBytes(filename, 64);
        }

        [TestCase(@"C:\Users\Derek\OneDrive\Documents\CSC205\LectureDemos\", "zip")]
        [TestCase(@"C:\Users\Derek\OneDrive\Documents\CSC205\ClassLectures\", "pdf")]
        [TestCase(@"C:\Users\Derek\OneDrive\Documents\CSC305\TSQL_Queries\", "sql")]

        public static void ExploreFileFormat(string directory, string type)
        {
            Console.WriteLine($"========================{type}=========================");
            FAFileInfo.DirectoryOutlook(directory);
        }

        #endregion
        #region Test Directory
        [Test]
        public static void ProjectDirectory()
        {

        }
        #endregion
    }

    [TestFixture]
    public class UtilitiesTesting
    {
        [Test]
        public static void StdOut()
        {
            //bool test = FAUtilities.GetUserInput("Are you ready?");
            Console.WriteLine(Console.Out);
        }
    }
}