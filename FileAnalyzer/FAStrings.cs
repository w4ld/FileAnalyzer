using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileAnalyzer
{
    public class FAStrings
    {
        //todo maybe refactor this into a display method with a title parameter and not found string
        public static void DisplayIPv4s(List<string> foundStrings)
        {
            List<Match> foundIPs = FAStrings.ParseIPv4(foundStrings);

            Console.WriteLine("\n\n++++++++Found IPs++++++++");
            foreach (var s in foundIPs)
            {
                //Console.WriteLine("\t" + s.Value);
                Console.WriteLine(PythonScript.GeoipCheck(s.Value));
            }
            if (foundIPs.Count == 0)
                Console.WriteLine("\tNo IPv4 addresses found.");
        }
        public static void DisplayWebsites(List<string> foundStrings)
        {
            List<Match> foundWebsites = FAStrings.ParseWebsites(foundStrings);

            Console.WriteLine("\n\n++++++++Found Websites++++++++");
            foreach (var s in foundWebsites)
            {
                Console.WriteLine("\t" + s.Value);

            }
            if (foundWebsites.Count == 0)
                Console.WriteLine("\tNo websites found.");
        }
        public static void DisplayDLLs(List<string> foundStrings)
        {
            List<Match> foundDLLs = FAStrings.ParseDLLs(foundStrings);

            Console.WriteLine("\n\n++++++++Found DLLs++++++++");
            foreach (var s in foundDLLs)
            {
                Console.WriteLine("\t" + s.Value);

            }
            if (foundDLLs.Count == 0)
                Console.WriteLine("\tNo DLLs found.");
        }
        public static void DisplayErrors(List<string> foundStrings)
        {
            List<Match> foundErrors = FAStrings.ParseErrors(foundStrings);

            Console.WriteLine("\n\n++++++++Found Error Strings++++++++");
            foreach (var s in foundErrors)
            {
                Console.WriteLine("\t" + s.Value);

            }
            if (foundErrors.Count == 0)
                Console.WriteLine("\tNo Error strings found.");
        }
        public static void DisplayPhoneNumbers(List<string> foundStrings)
        {
            List<Match> foundNumbers = FAStrings.ParsePhoneNumbers(foundStrings);

            Console.WriteLine("\n\n++++++++Found Phone Numbers++++++++");
            foreach (var s in foundNumbers)
            {
                Console.WriteLine("\t" + s.Value);

            }
            if (foundNumbers.Count == 0)
                Console.WriteLine("\tNo Phone Numbers found.");
        }

        public static void DisplaySSNs(List<string> foundStrings)
        {
            List<Match> foundSSNs = FAStrings.ParsePhoneNumbers(foundStrings);

            Console.WriteLine("\n\n++++++++Found Social Security Numbers++++++++");
            foreach (var s in foundSSNs)
            {
                Console.WriteLine("\t" + s.Value);

            }
            if (foundSSNs.Count == 0)
                Console.WriteLine("\tNo SSNs found.");
        }

        public static void DisplayEmails(List<string> foundStrings)
        {
            List<Match> foundEmails = FAStrings.ParseEmails(foundStrings);
            Console.WriteLine("\n\n++++++++Found Email Addresses++++++++");
            foreach (var s in foundEmails)
            {
                Console.WriteLine("\t" + s.Value);

            }
            if (foundEmails.Count == 0)
                Console.WriteLine("\tNo email addresses found.");

        }


        #region String Work
        public static bool CheckByteASCII(byte b)
        {
            if (b > 0x1F && b < 0x7F)
            {

                //hex range for printable characters in ASCII
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Parse file for human readable strings
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="length">minimum string length</param>
        /// <param name="stringList">out string list</param>
        public static void GetStrings(string fileName, int length, ref List<string> stringList)
        {
            //Adapt this for a wider range of character sets
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                try
                {
                    FileInfo info = new FileInfo(fileName);
                    long size = info.Length;
                    byte[] buffer = new byte[size];
                    //fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, (int)size);
                    //go byte by byte
                    bool inString = false;
                    StringBuilder currentString = new StringBuilder();
                    for (int i = 0; i < size; i++)
                    {
                        if (CheckByteASCII(buffer[i]))
                        {// is human readable in ASCII.  
                            //Console.WriteLine("Found readable character: {0}", Convert.ToChar(buffer[i]));
                            inString = true;
                            currentString.Append(Convert.ToChar(buffer[i]));
                        }
                        else
                        {
                            if (inString)
                            {
                                if (currentString.Length >= length)
                                {
                                    //Console.WriteLine($"Adding String: {currentString.ToString()}");
                                    stringList.Add(currentString.ToString());
                                }
                                inString = false;
                                currentString.Clear();
                            }
                        }

                    }
                    if (currentString.Length > 0)
                    {
                        stringList.Add(currentString.ToString());
                    }
                    //checkByte within string range
                    //build string until nonprintable char encountered
                    //if at threshold add string to a list, else toss string.
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}", ex.Message);
                    Console.ResetColor();
                    throw ex;
                }
            }
        }
        #endregion
        /// <summary>
        /// Parse string list for phone numbers.
        /// </summary>
        /// <param name="foundStringList">input list to search</param>
        /// <returns>list of matches</returns>
        public static List<Match> ParsePhoneNumbers(List<string> foundStringList)
        {
            //american phone number +1 or (333)-333-3333
            Regex r = new Regex(@"^(\+1)?\(?\d{3}?\)?[- .]?\d{3}[- .]?\d{4}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //TODO international numbers
            //Regex r = new Regex(@"^\+(?:\d[ ]?){6,14}\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }
        /// <summary>
        /// Parses string list for errors.
        /// </summary>
        /// <param name="foundStringList">list of strings</param>
        /// <returns>list of matches</returns>
        public static List<Match> ParseErrors(List<string> foundStringList)
        {

            Regex r = new Regex(@"^\w+\s?((error)|(fail))+\s?(\w+\s?)*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }
        /// <summary>
        /// parses strings for dll references
        /// </summary>
        /// <param name="foundStringList">list of strings</param>
        /// <returns>list of matches</returns>
        public static List<Match> ParseDLLs(List<string> foundStringList)
        {
            //onestring.dll shouldnt have 
            //TODO shoudlnt be able to start with a number right? maybe it flies... double check this
            Regex r = new Regex(@"\b\w(\w|\d)*\.dll$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }
        /// <summary>
        /// parses string list for websites
        /// </summary>
        /// <param name="foundStringList">list of strings</param>
        /// <returns>list of matches</returns>
        public static List<Match> ParseWebsites(List<string> foundStringList)
        {
            //TODO refine website Regex
            //types of protocols https, http, ftp
            //special chars allowed in URL +&@#/%?=~_|$!:,.;
            Regex r = new Regex(@"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }
        /// <summary>
        /// parse string list for social security numbers
        /// </summary>
        /// <param name="foundStringList">list of strings</param>
        /// <returns>list of matches</returns>
        public static List<Match> ParseSSNs(List<string> foundStringList)
        {

            Regex r = new Regex(@"^[0-9]{3}-[0-9]{2}-[0-9]{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }




        public static List<Match> ParseEmails(List<string> foundStringList)
        {

            Regex r = new Regex(@"[A-Z0-9+_.-]+@(?:[A-Z0-9]+\.)+[A-Z]{2,3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }

        /// <summary>
        /// Parses string list for IPv4 addresses
        /// </summary>
        /// <param name="foundStringList">list of strings</param>
        /// <returns>list of matches</returns>
        public static List<Match> ParseIPv4(List<string> foundStringList)
        {
            //IPv4
            //bool loopBool = true;
            Regex r = new Regex(@"([0-9]{1,3}\.){3}[0-9]{1,3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            string st = "";
            foreach (string s in foundStringList)
                st = s;
                while (r.IsMatch(st))
                {
                    Console.WriteLine((st));
                    if (IPChecker.IsValidIPv4(st) != IPChecker.IPv4Class.NiR)
                {
                        Console.WriteLine($"Match: {r.Match(st).Value}");
                        matches.Add(r.Match(st));
                }
                    st = st.Substring(r.Match(st).Index + r.Match(st).Length); 
                }
            return matches;
        }
    }



    //for future work scanning for malicious code... I thought header matching was going to be more straight forward. It took much more time than anticipated.
    //it may be better to just use the already established yara framework...

    //TODO adapt the below algorithms to return a list of offsets to eventually use for input sanitization and malware id.

    /// <summary>
    /// Boyer-Moore String Search algorithm adapted for bytes.
    /// </summary>
    public class BoyerMoore
    {
        #region Boyer-Moore string search algorithm
        /// <summary>
        /// Returns an index of the needle in the haystack
        /// </summary>
        /// <param name="haystack">Bytes you want to parse through</param>
        /// <param name="needle">Subset of bytes you seek to find</param>
        /// <returns>Index of the substring in the </returns>
        public static int IndexOf(byte[] haystack, byte[] needle)
        {
            try
            {
                int needleLen = needle.Length;
                if (needleLen == 0)
                {
                    return 0;
                }
                int[] charTable = MakeByteTable(needle);
                int[] offsetTable = MakeOffsetTable(needle);
                for (int i = needleLen - 1, j; i < haystack.Length;)
                {
                    for (j = needleLen - 1; needle[j] == haystack[i]; --i, --j)
                    {
                        if (j == 0)
                        {
                            return i;
                        }
                    }
                    // i += needleLen - j; // For naive method
                    i += Math.Max(offsetTable[needleLen - 1 - j], charTable[haystack[i]]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }


        /// <summary>
        /// Makes the jump table based on the mismatched byte information.
        /// </summary>
        /// <param name="needle">Subset of bytes you seek to find</param>
        /// <returns>Returns byte table</returns>
        private static int[] MakeByteTable(byte[] needle)
        {
            int needleLen = needle.Length;
            int ALPHABET_SIZE = Byte.MaxValue;    // 65536
            int[] table = new int[ALPHABET_SIZE];
            for (int i = 0; i < table.Length; ++i)
            {
                table[i] = needleLen;
            }
            for (int i = 0; i < needleLen - 2; ++i)
            {
                table[needle[i]] = needleLen - 1 - i;
            }
            return table;
        }


        /// <summary>
        /// Makes the jump table based on the scan offset which mismatch occurs. (bad character rule).
        /// </summary>
        /// <param name="needle"></param>
        /// <returns></returns>
        private static int[] MakeOffsetTable(byte[] needle)
        {
            int needleLen = needle.Length;
            int[] table = new int[needleLen];
            int lastPrefixPosition = needleLen;
            for (int i = needleLen; i > 0; --i)
            {
                if (IsPrefix(needle, i))
                {
                    lastPrefixPosition = i;
                }
                table[needleLen - i] = lastPrefixPosition - i + needleLen;
            }
            for (int i = 0; i < needleLen - 1; ++i)
            {
                int slen = SuffixLength(needle, i);
                table[slen] = needleLen - 1 - i + slen;
            }
            return table;
        }

        /// <summary>
        /// Checks status of needle[p:end] to see if it is a prefix of needle
        /// </summary>
        /// <param name="needle"></param>
        /// <param name="p"></param>
        /// <returns>Boolean of prefix status</returns>
        private static bool IsPrefix(byte[] needle, int p)
        {
            int needleLen = needle.Length;
            for (int i = p, j = 0; i < needleLen; ++i, ++j)
            {
                if (needle[i] != needle[j])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the maximum length of the subset of bytes theat ends at p and is a suffix
        /// </summary>
        /// <param name="needle"></param>
        /// <param name="p"></param>
        /// <returns>Max langth of subset of bytes</returns>
        private static int SuffixLength(byte[] needle, int p)
        {
            int needleLen = needle.Length;
            int len = 0;
            for (int i = p, j = needleLen - 1;
                     i >= 0 && needle[i] == needle[j]; --i, --j)
            {
                len += 1;
            }
            return len;
        }
        #endregion


    }

    //May use this when implementing input sanitization

    /// <summary>
    /// Boyer-Moore String Search algorithm
    /// </summary>
    public class BoyerMooreString
    {
        #region Boyer-Moore string search algorithm
        /// <summary>
        /// Returns an index of the needle in the haystack
        /// </summary>
        /// <param name="haystack">strings you want to parse through</param>
        /// <param name="needle">Subset of strings you seek to find</param>
        /// <returns>Index of the substring in the </returns>
        public static int IndexOf(string haystack, string needle)
        {
            try
            {
                int needleLen = needle.Length;
                if (needleLen == 0)
                {
                    return 0;
                }
                int[] charTable = MakestringTable(needle);
                int[] offsetTable = MakeOffsetTable(needle);
                for (int i = needleLen - 1, j; i < haystack.Length;)
                {
                    for (j = needleLen - 1; needle[j] == haystack[i]; --i, --j)
                    {
                        if (j == 0)
                        {
                            return i;
                        }
                    }
                    // i += needleLen - j; // For naive method
                    i += Math.Max(offsetTable[needleLen - 1 - j], charTable[haystack[i]]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }


        /// <summary>
        /// Makes the jump table based on the mismatched string information.
        /// </summary>
        /// <param name="needle">Subset of strings you seek to find</param>
        /// <returns>Returns string table</returns>
        private static int[] MakestringTable(string needle)
        {
            int needleLen = needle.Length;
            int ALPHABET_SIZE = 65536;
            int[] table = new int[ALPHABET_SIZE];
            for (int i = 0; i < table.Length; ++i)
            {
                table[i] = needleLen;
            }
            for (int i = 0; i < needleLen - 2; ++i)
            {
                table[needle[i]] = needleLen - 1 - i;
            }
            return table;
        }


        /// <summary>
        /// Makes the jump table based on the scan offset which mismatch occurs. (bad character rule).
        /// </summary>
        /// <param name="needle"></param>
        /// <returns></returns>
        private static int[] MakeOffsetTable(string needle)
        {
            int needleLen = needle.Length;
            int[] table = new int[needleLen];
            int lastPrefixPosition = needleLen;
            for (int i = needleLen; i > 0; --i)
            {
                if (IsPrefix(needle, i))
                {
                    lastPrefixPosition = i;
                }
                table[needleLen - i] = lastPrefixPosition - i + needleLen;
            }
            for (int i = 0; i < needleLen - 1; ++i)
            {
                int slen = SuffixLength(needle, i);
                table[slen] = needleLen - 1 - i + slen;
            }
            return table;
        }

        /// <summary>
        /// Checks status of needle[p:end] to see if it is a prefix of needle
        /// </summary>
        /// <param name="needle"></param>
        /// <param name="p"></param>
        /// <returns>Boolean of prefix status</returns>
        private static bool IsPrefix(string needle, int p)
        {
            int needleLen = needle.Length;
            for (int i = p, j = 0; i < needleLen; ++i, ++j)
            {
                if (needle[i] != needle[j])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the maximum length of the subset of strings theat ends at p and is a suffix
        /// </summary>
        /// <param name="needle"></param>
        /// <param name="p"></param>
        /// <returns>Max langth of subset of strings</returns>
        private static int SuffixLength(string needle, int p)
        {
            int needleLen = needle.Length;
            int len = 0;
            for (int i = p, j = needleLen - 1;
                     i >= 0 && needle[i] == needle[j]; --i, --j)
            {
                len += 1;
            }
            return len;
        }
        #endregion


    }
}
