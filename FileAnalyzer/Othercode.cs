//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace FileAnalyzer
//{
//    class Class1
//    {
//        public class Indexed
//        {
//            public string this[int index]
//            {
//                get => index < 5 ? "Foo" : "bar";
//            }
//        }
//        #region var and keyword play
//        static void LocalVarTest()
//        {
//            string rose = null; ref string rosaIndica = ref rose; rosaIndica = "smell as sweet"; Console.WriteLine($"A rose by any other name would {rose}");
//        }
//        public static void Blame(string perpetrator = "the youth of today",
//        string problem = "the downfall of society")
//        {
//            Console.WriteLine($"I blame {perpetrator} for {problem}.");
//        }
//        #endregion

//        #region tuple play

//        public readonly struct Size
//        {
//            public Size(double w, double h)
//            {
//                W = w;
//                H = h;
//            }

//            public void Deconstruct(out double w, out double h)
//            {
//                w = W;
//                h = H;
//            }

//            public double W { get; }
//            public double H { get; }
//        }
//        static string DescribeSize(Size s) => s switch
//        {
//            (0, 0) => "Empty",
//            (0, _) => "Extremely narrow",
//            (double w, 0) => $"Extremely short, and this wide: {w}",
//            _ => "Normal"
//        };
//        #endregion

//        #region function play

//        static void function1()
//        {
//            int c = 0;
//            c = function2(6);
//            int function2(int q)//a function in a function
//            {
//                return 6 * function3(q);//calling another function
//            }
//            int function3(int q)
//            {
//                return q / 2;
//            }
//            Console.WriteLine("C, {0}", c);
//        }
//        #endregion



//        #region Networking
//        public static void Networking_Information()
//        {
//            string url = "http://192.168.56.105/?searchquery=hello+world&action=search&x=0&y=0";
//            var splitUrl = url.Split('?');
//            string baseUrl = splitUrl[0];
//            string paramsUrl = splitUrl[1];
//            var splitParams = paramsUrl.Split('&');
//            Console.WriteLine("URL Parsing");
//            Console.WriteLine($"Parsing: {url}");
//            Console.WriteLine($"\tBase URL: {baseUrl}");
//            Console.WriteLine("\tParameters:");
//            foreach (var p in splitParams)
//            {
//                Console.WriteLine($"\t\tParameter Found: {p}");
//            }
//            //NetworkAccess.Connect();

//            //Sample request
//            string url2 = "http://192.168.56.105/?searchquery=hello+world<alert>XSS<\alert>&action=search&x=0&y=0";
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url2);
//            request.Method = "GET";

//            string response = string.Empty;
//            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
//                response = reader.ReadToEnd();

//            //Console.WriteLine("Network Request");
//            //Console.WriteLine($"HTTP Request:\n{request.ToString()}\n\n");
//            //Console.WriteLine($"HTTP Response:\n{response}\n\n");
//        }
//        #endregion

//        #region Parsing

//        //string vuln0 = "<alert>XSS";
//        //string vuln0Type = "XSS Attack";
//        //if (response.Contains(vuln0))
//        //    Console.WriteLine("Request may be vulnerable to {0}.", vuln0Type);
//        //else
//        //    Console.WriteLine("May not be vulnerable to {0}", vuln0Type);




//        //request = (HttpWebRequest)WebRequest.Create(url2);
//        #endregion

//        #region Fuzzing Example
//        public static void Fuzzing_Information()
//        {
//            string url = "http://192.168.56.105/?searchquery=hello+world&action=search&x=0&y=0";

//            //Vuln setup
//            WebVulnerability sqlvuln = new SQLInjectVulnerability("basic sql injection", 5.0, "fd'sa", "error in your SQL syntax");
//            WebVulnerability xssvuln = new XSSVulnerability("basic xss attack vector", 4.8, "fd<xss>", "<xss>");
//            List<WebVulnerability> vulnlist = new List<WebVulnerability>();
//            vulnlist.Add(sqlvuln);
//            vulnlist.Add(xssvuln);

//            UrlFuzz(url, vulnlist);
//            UrlFuzz("http://192.168.56.105/cgi-bin/badstore.cgi?searchquery=test    &action=search", vulnlist);
//        }
//        public static void UrlFuzz(string url, List<WebVulnerability> vulnlist)
//        {
//            int index = url.IndexOf('?');
//            string[] parameters = url.Remove(0, index + 1).Split('&');
//            foreach (var vuln in vulnlist)
//            {
//                foreach (string p in parameters)
//                {
//                    string fuzzurl = url.Replace(p, p + ((WebVulnerability)vuln).Tester);
//                    //Sample request
//                    Console.WriteLine($"\tTrying URL: {fuzzurl}");
//                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fuzzurl);
//                    request.Method = "GET";

//                    string response = string.Empty;
//                    using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
//                        response = reader.ReadToEnd();
//                    if (response.Contains(((WebVulnerability)vuln).Detector))
//                        Console.WriteLine($"Vulnerability {vuln.Name} found with a CVE of {vuln.Score}!");
//                    else
//                        Console.WriteLine($"\t\tVulnerability {vuln.Name} Not found!");
//                }
//            }
//        }
//        #endregion
//        /*

//            #region Misc
//            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
//            foreach (var item in list)
//            {

//                //if (item % 2 ==0)
//                //    continue;

//                if (item == 5)
//                    break;
//                Console.WriteLine(item);
//            }
//            //Garbage collection
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            GC.Collect();

//            //var path = "Z:\\testfile.txt";
//            //using(var file = File.Open(path, FileMode.Open))
//            //{
//            //    //this should handle closing correctly.
//            //}
//            #endregion
//        }
//*/
//        #region Vuln Classes
//        public abstract class Vulnerability
//        {
//            private string _Name;
//            public string Name
//            {
//                get { return _Name; }
//                set { _Name = value; }
//            }

//            private double _Score;
//            public double Score
//            {
//                get { return _Score; }
//                set { _Score = value; }
//            }



//        }
//        public abstract class WebVulnerability : Vulnerability
//        {
//            private string _Detector;

//            public string Detector
//            {
//                get { return _Detector; }
//                set { _Detector = value; }
//            }

//            private string _Tester;

//            public string Tester
//            {
//                get { return _Tester; }
//                set { _Tester = value; }
//            }



//        }
//        public class XSSVulnerability : WebVulnerability
//        {
//            public XSSVulnerability(string name, double score, string tester, string detector)
//            {
//                this.Name = name;
//                this.Score = score;
//                this.Tester = tester;
//                this.Detector = detector;
//            }
//        }
//        public class SQLInjectVulnerability : WebVulnerability
//        {
//            public SQLInjectVulnerability(string name, double score, string tester, string detector)
//            {
//                this.Name = name;
//                this.Score = score;
//                this.Tester = tester;
//                this.Detector = detector;
//            }
//        }

//        #endregion
//    }
//}
//}
