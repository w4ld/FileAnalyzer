using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FileAnalyzer
{
    class Program
    {
        const int FILE_HEADER_SIZE = 64;
        [System.Flags]
        public enum Ingredients
        {
            Eggs = 0b1,
            Bacon = 0b10,
            Sausages = 0b100,
            Mushrooms = 0b1000,
            Tomato = 0b1_0000,
            BlackPudding = 0b10_0000,
            BakedBeans = 0b100_0000,
            TheFullEnglish = 0b111_1111
        }

        //TODO features to add
        //strings?
        //
        static void Main(string[] args)
        {
            #region Data Types
            //(int x, int y) point = (5, 9);      //tuple example
            //(int day, int month) date = point;
            //var point2 = (X: 5, Y: 10);
            ////deconstruct point2
            //Console.WriteLine($"Point2: X:{point2.X}, Y:{point2.Y}");
            //(var dx, var dy) = point2;

            //Console.WriteLine($"Deconstructed Point2: X:{dx}, Y:{dy}");
            //var tsize1 = new Size(50, 60);
            //string testDescribeSize = DescribeSize(tsize1);
            //Console.WriteLine($"testDescribeSize: {testDescribeSize}");


            //var watch = new System.Diagnostics.Stopwatch();
            //for (int v = 0; v < 5; v++)
            //{
            //    watch.Start();
            //    string test = "";
            //    for (int w = 0; w < 5000; w++)
            //    {
            //        test += "a";
            //    }
            //    watch.Stop();
            //    var elapsedMs1 = watch.ElapsedMilliseconds;
            //    watch.Reset();
            //    watch.Start();
            //    StringBuilder test2 = new StringBuilder();
            //    for (int w = 0; w < 5000; w++)
            //    {
            //        test2.Append("a");
            //    }
            //    watch.Stop();
            //    var elapsedMs2 = watch.ElapsedMilliseconds;
            //    Console.WriteLine($"Test {v}");
            //    Console.WriteLine($"\tregular: {elapsedMs1}");
            //    Console.WriteLine($"\tstringbuilder: {elapsedMs2}");
            //    watch.Reset();
            //}


            #endregion
            //LocalVarTest();
            #region string literals and formatting

            //string location = @"C:\Windows\System32\";
            //string location2 = "C:\\Windows\\System32\\";
            //Console.WriteLine($"Are string A: {location}\t and B:{location2} equal? {location.Equals(location2)}");
            //Console.WriteLine($"{switcheruni("hello")}");
            //Console.WriteLine($"{switcheruni(5)}");
            //Console.WriteLine($"{switcheruni(55.55)}");
            //object switcheruni(object o)
            //{
            //    switch (o)
            //    {
            //        case string s:
            //            Console.WriteLine("String");
            //            return "String";
            //        case int i:
            //            return "Integer";
            //        case double d:
            //            return "Double";
            //        default:
            //            return "Other";
            //    }
            //}
            #endregion
            #region other stuff
            //Console.WriteLine("Blame()");
            //Blame();
            //Console.WriteLine("Blame(1)");
            //Blame("mischevious gnomes");
            //Console.WriteLine("Blame(just poblem");
            //Blame(problem: "ruining all good things");
            //function1();

            //var numbers = new List<int> { 1, 2, 1, 4 };
            //numbers[2] += numbers[1];
            //Console.WriteLine(numbers[0]);
            //Console.WriteLine(numbers[1]);
            //Console.WriteLine(numbers[2]);



            //Indexed ind = new Indexed();
            //Console.WriteLine($"ind 0:{ind[0]}");
            //Console.WriteLine($"ind 8:{ind[8]}");
            #endregion
            OS_Information();
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: FileAnalyzer <file path>");
                return;
            }
            // TODO insert error checking for this then encapsulate into try catch

            string filename = args[0];
            FileInfo fileInfo = new FileInfo(filename);
            var files = Directory.GetFiles(@"C:\Users\Derek\Downloads\");

            foreach (string f in files)
            {
                Console.WriteLine($"Testing {f}");
                ReadFileHeaders(f);
            }
            PrintFileInfo(fileInfo);
            List<string> stringPython = new List<string>();
            GetStrings(filename, 9, ref stringPython);
            Console.WriteLine("Python-3.8.3 Strings >9");
            FoundStrings.ParseDLLs(stringPython);
            FoundStrings.ParseWebsites(stringPython);
            FoundStrings.ParsePhoneNumbers(stringPython);

            DisplayHashes(filename);
            //Console.WriteLine("Testing Python-3.8.3.exe");
            //ReadFileHeaders(filename);

            //Console.WriteLine("Testing LHH Book Pages 1-14.pdf");
            //ReadFileHeaders("C:\\Users\\Derek.Evans\\Downloads\\LHH Book Pages 1-14.pdf");
            //Ingredients breakfast = Ingredients.Bacon | Ingredients.Mushrooms | Ingredients.Tomato;
            //Console.WriteLine($"Breakfast consists of:{breakfast.ToString()}");
        }
        #region Basic File Info
        public static void PrintFileInfo(FileInfo fileInfo)
        {
            //takes FileInfo object printing the attributes to user
            Console.WriteLine($"File Name: {fileInfo.Name}");
            Console.WriteLine($"Extension: {fileInfo.Extension}");
            Console.WriteLine($"Size: {SizeToProperPrefix(fileInfo.Length)}");//convert this later
            Console.WriteLine($"Created: {fileInfo.CreationTime}");
            Console.WriteLine($"Last Write: {fileInfo.LastWriteTime}");
            Console.WriteLine($"Last Access: {fileInfo.LastAccessTime}");
            Console.WriteLine($"Read-Only: {fileInfo.IsReadOnly}");

        }
        public static string SizeToProperPrefix(long length)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int prefix = 0;
            while (length >= 1024 && prefix < sizes.Length - 1)
            {
                prefix++;
                length = length / 1024;
            }
            return $"{length} {sizes[prefix]}";
        }

        #endregion
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

        public static void GetStrings(string fileName, int length, ref List<string> stringList)
        {
            //Adapt this for a wider range of character sets
            using (FileStream fileStream = File.OpenRead(fileName))
            {

                //FileHeader fileHeader = new FileHeader();
                try
                {
                    FileInfo info = new FileInfo(fileName);
                    long size = info.Length;
                    byte[] buffer = new byte[size];
                    fileStream.Seek(0, SeekOrigin.Begin);
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
                    //checkByte within string range
                    //build string until nonprintable char encountered
                    //if at threshold add string to a list, else toss string.
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       
        #endregion
        #region Guess File Format
        public static void ReadFileHeaders(String filename)
        {
            using (FileStream fileStream = File.OpenRead(filename))
            {

                //FileHeader fileHeader = new FileHeader();
                try
                {

                    byte[] buffer = new byte[FILE_HEADER_SIZE];
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, FILE_HEADER_SIZE);

                    //read file extension DB into list
                    //search for headers
                    //return successes
                    //is exe?
                    var fileName = string.Format(@"C:\Users\Derek\OneDrive\Documents\FileExtensions.xlsx");
                    var sheet = "NoQ";
                    //this likes .xls, but not the newer format .xlsx
                    var conn1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties='Excel 12.0 Xml; HDR=YES'";
                    var connXLS = "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;";
                    var connectionString = string.Format(conn1, fileName);


                    string query = string.Format("SELECT * FROM [{0}$]", sheet);
                    //string query = string.Format("SELECT * FROM [{0}$] WHERE ", sheet);
                    DataTable db = null;
                    //  List<double> randoms100 = new List<double>();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connectionString);
                    DataSet ds = new DataSet();
                    string tablename = "fileextensions";
                    adapter.Fill(ds, tablename);
                    db = ds.Tables[tablename];
                    if (null != db && null != db.Rows)
                    {

                        foreach (DataRow dataRow in db.Rows)
                        {
                            //handle for nulls; pass to check function
                            string hexSignature = dataRow.Field<string>(0);
                            string extension = dataRow.Field<string>(3);
                            //string offset = dataRow.Field<string>(2);
                            string description = dataRow.Field<string>(4);
                            CheckSignature(buffer, hexSignature, extension, description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

        }
        public static void CheckSignature(byte[] buffer, string hexSignature, string extension, string description)
        {

            if (!String.IsNullOrEmpty(extension))
            {
                //Console.Write("Checking Extension: {0}\t", extension);
                if (String.IsNullOrEmpty(description))
                    description = "Needs description";
                if (!String.IsNullOrEmpty(hexSignature))//checks for a hexSig to look for
                {
                    byte[] hexSigBuffer = new byte[hexSignature.Split(' ').Length];
                    GetHexSignatureBytes(hexSignature, ref hexSigBuffer);
                    bool possibleMatch = true;
                    // Console.WriteLine($"Signature Length: {hexSigBuffer.Length}");
                    for (int i = 0; i < hexSigBuffer.Length; i++)
                    {
                        //  Console.WriteLine($"Buf:{buffer[i]} \t Sig:{hexSigBuffer[i]}");
                        if (buffer[i] != hexSigBuffer[i])
                            possibleMatch = false;
                    }
                    if (possibleMatch)
                    {
                        //  Console.WriteLine(" POSSIBLE");
                        Console.WriteLine($"Possible Match found: {extension} \n\t\tDescription: {description}");
                    }
                    else
                    {
                        //  Console.WriteLine(" Not Detected");
                    }
                }
                else
                {
                    // Console.WriteLine($" Not Detected\t--{extension} needs a signature");
                }
            }
        }
        public static int CharToIntB(char c)
        {
            switch (c)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'A':
                case 'a':
                    return 10;
                case 'B':
                case 'b':
                    return 11;
                case 'C':
                case 'c':
                    return 12;
                case 'D':
                case 'd':
                    return 13;
                case 'E':
                case 'e':
                    return 14;
                case 'F':
                case 'f':
                    return 15;
                default:
                    throw new Exception("Case for char conversion not implemented. Bad Data Char");
            }

        }
        public static void GetHexSignatureBytes(string extSig, ref byte[] hexExtSig)
        {
            string[] splitSig = extSig.Split(' ');
            for (int i = 0; i < splitSig.Length; i++)
            {
                // Console.WriteLine($"Processing byte:{splitSig[i]}");
                int bits1 = 0, bits2 = 0;
                bits1 = CharToIntB(splitSig[i][0]);
                bits2 = CharToIntB(splitSig[i][1]);
                byte b = (byte)(16 * bits1 + bits2);
                //  Console.WriteLine($"\tOutput: {b}");
                hexExtSig[i] = b;
                //for each 
            }

        }
        #endregion
        #region OS Information
        public static void OS_Information()
        {

            PlatformID x = new PlatformID();
            OperatingSystem os = Environment.OSVersion;

            var version = Environment.Version;
            var user = Environment.UserName;


            int processor_count = Environment.ProcessorCount;
            string cl = Environment.CommandLine;
            bool six4bit = Environment.Is64BitOperatingSystem;

            String networkDomainofUser = Environment.UserDomainName;
            int currentThreadID = Environment.CurrentManagedThreadId;
            string sysDir = Environment.SystemDirectory;
            string machineName = Environment.MachineName;
            string[] logicalDrives = Environment.GetLogicalDrives();
            string[] clargs = Environment.GetCommandLineArgs();
            //public static string GetEnvironmentVariable(string variable);
            //public static IDictionary GetEnvironmentVariables();
            //public static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option);
            //public static string GetFolderPath(SpecialFolder folder);
            //public static void SetEnvironmentVariable(string variable, string value);

            //PRINTS
            Console.WriteLine("OS INFORMATION");
            Console.WriteLine($"\tOS Version: {os.ToString()}");
            Console.WriteLine($"\tPlatformID: {x.ToString()}");
            Console.WriteLine($"\tProcessor Count: {processor_count}");
            Console.WriteLine($"\tCommandline: {cl}");
            Console.WriteLine($"\tAre we 64bit? {six4bit}");
            Console.WriteLine($"\tCurrent Thread ID: {currentThreadID}");
            Console.WriteLine($"\tSystem Dir: {sysDir}");
            Console.WriteLine($"\tDrives:");
            foreach (var drive in logicalDrives)
            {
                Console.WriteLine($"\t\t{drive.ToString()}");
            }
            //launch new cmdprompt
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = "cmd.exe",
            //    UseShellExecute = true,
            //    WindowStyle = ProcessWindowStyle.Maximized,
            //});

        }
        #endregion
        public static void DisplayHashes(string fileName)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {

                //FileHeader fileHeader = new FileHeader();
                try
                {
                    FileInfo info = new FileInfo(fileName);
                    long size = info.Length;
                    byte[] buffer = new byte[size];
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, (int)size);
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] md5Hash = md5.ComputeHash(buffer);
                        Console.WriteLine("MD5: "+BitConverter.ToString(md5Hash).Replace("-",String.Empty));
                    }
                    using (SHA1 sha1 = SHA1.Create())
                    {
                        byte[] sha1Hash = sha1.ComputeHash(buffer);
                        Console.WriteLine("SHA1: " + BitConverter.ToString(sha1Hash).Replace("-", String.Empty));
                    }
                }
                catch
                {
                    Console.WriteLine("Error tryign to compute hash");
                }
            }
        }
    }
}
