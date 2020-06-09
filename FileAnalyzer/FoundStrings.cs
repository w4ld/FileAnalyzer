using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FileAnalyzer
{
   public class FoundStrings
    {
        public static List<Match> ParsePhoneNumbers(List<string> foundStringList)
        {
            //american phone number +1 or (333)-333-3333
            Regex r = new Regex(@"^(\+1)?\(?\d{3}?\)?\-?\d{3}\-?\d{4}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                     matches.Add(r.Match(s));
            return matches;
        }
        public static List<Match> ParseErrors(List<string> foundStringList)
        {
            Console.WriteLine("Error Strings");
            Regex r = new Regex(@"^\w+\s?((error)|(fail))+\s?(\w+\s?)*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }
        public static List<Match> ParseDLLs(List<string> foundStringList)
        {
            //onestring.dll shouldnt have 
            //TODO shoudlnt be able to start with a number right? maybe it flies... double check this
            Regex r = new Regex(@"^\w(\w|\d)*\.dll$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Console.WriteLine("DLLs Found:");
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }
        public static List<Match> ParseWebsites(List<string> foundStringList)
        {
            //TODO refine website Regex
            string pattern1 = @"(http://)+(www\.)+\w+[\.\w]+";
            string pattern2 = @"https://\w+[\.\w]+";
            string pattern3 = @"www.\w+[\.\w]+";

            Regex rx1 = new Regex(pattern1, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rx2 = new Regex(pattern2, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rx3 = new Regex(pattern3, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string text = @"http://www.google.com http://thisshouldalsoworm.gov/osadjfo/oasdjf oadsodfjo.mil the quick brown fox fox www.facebook.com/thiswebsitesucks https://thisishoweverywebsitezhouldbe.biz jumps over the lazy dog dog.";
            Regex r = new Regex(@"^http(s)?://\w+(\.\w+)+(/(\w|\d)*)*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<Match> matches = new List<Match>();
            foreach (string s in foundStringList)
                if (r.IsMatch(s))
                    matches.Add(r.Match(s));
            return matches;
        }

    }
}
