using System;
using System.Collections.Generic;
using System.IO;

namespace FileAnalyzer
{
    class Program
    {
        const int STRING_THRESHOLD = 8;
        const string VERSION_INFO = "v0.0.1";
        const string USAGE = "Usage: FileAnalyzer --help";
        static void Main(string[] args)
        {

            ParseArgs(args);
        }
        static void ParseArgs(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine(USAGE);
                return;
            }


            //TODO check out BinaryInfo class
            FileStream outFile = null;
            string filename = "", folder = "", reportFile = "";
            bool yaraScan = false, toReport = false, isFile = false, isFolder = false, stringSearch = false, guessFile = false, pii = false, osInfo = false;

            for (int i = 0; i < args.Length; i++)     //length -1 to prevent 
            {
                //Console.WriteLine($"Arg{i}: {args[i]}");
                if (args[i] == "--help")
                {
                    FAMenu.PrintHelp();
                    return;
                }
                if (args[i] == "--os")
                    osInfo = true;

                if (args[i] == "--file")
                {
                    if (i < args.Length - 1)
                    {
                        filename = args[++i];
                        isFile = true;
                    }
                    else
                        throw new FileNotFoundException("Please subscribe a file to analyze");
                }

                if (args[i] == "--folder")
                {
                    if (i < args.Length - 1)
                    {
                        folder = args[++i];
                        isFolder = true;
                    }
                    else
                        throw new FileNotFoundException("Please subscribe a folder to analyze");
                }
                if (args[i] == "--report")
                {
                    if (i < args.Length - 1)
                    {
                        reportFile = args[++i];
                        try
                        {
                            outFile = new FileStream(reportFile, FileMode.Create);
                            StreamWriter streamWriter = new StreamWriter(outFile);
                            Console.SetOut(streamWriter);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        toReport = true;
                    }
                    else
                        throw new FileNotFoundException("Please supply an Output file name with path");
                }
                if (args[i] == "--yara")
                    yaraScan = true;
                if (args[i] == "--guess")
                    guessFile = true;
                if (args[i] == "--strings")
                    stringSearch = true;
                if (args[i] == "--pii")
                    pii = true;
                if (args[i] == "--version")
                {
                    Console.WriteLine($"Version: {VERSION_INFO}");
                    return;
                }
            }
            if (osInfo)
                FAOSInfo.OS_Information();

            if (isFile) //file supplied scan it
                ScanFile(filename, yaraScan, stringSearch, guessFile, pii);

            if (isFolder) //directory supplied scan it
                ScanDirectory(folder, yaraScan, stringSearch, guessFile, pii);

            if (toReport) //return output to command prompt
            {
                if (outFile != null) //handle report file
                    outFile.Close();
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput())); //return output

            }



        }
        /// <summary>
        /// Main handles for scanning a file. Takes in the configurations bools to handle what to execute on file.
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="yaraScan">yara scan file</param>
        /// <param name="stringSearch">search for strings</param>
        /// <param name="guessFile">attempt to identify file</param>
        /// <param name="pii">search for pii</param>
        public static void ScanFile(string filename, bool yaraScan, bool stringSearch, bool guessFile, bool pii)
        {
            if (File.Exists(filename))
            {

                FileInfo fInfo = new FileInfo(filename);
                FAFileInfo.PrintFileInfo(fInfo);
                FAFileInfo.DisplayHashes(filename);

                if (guessFile)
                    GuessFileFormat.Guess(filename);

                List<string> foundStrings = new List<string>();

                if (stringSearch)
                {
                    FAStrings.GetStrings(filename, STRING_THRESHOLD, ref foundStrings);
                    FAStrings.DisplayDLLs(foundStrings);
                    FAStrings.DisplayIPv4s(foundStrings);
                    FAStrings.DisplayWebsites(foundStrings);
                    FAStrings.DisplayErrors(foundStrings);
                }
                if (pii)
                {
                    if (foundStrings.Count == 0)
                        FAStrings.GetStrings(filename, STRING_THRESHOLD, ref foundStrings);
                    FAStrings.DisplayPhoneNumbers(foundStrings);
                    FAStrings.DisplaySSNs(foundStrings);
                    FAStrings.DisplayEmails(foundStrings);
                }
                if (yaraScan)
                {
                    string s = PythonScript.YaraScan(filename);
                    Console.WriteLine(s);
                }

            }
            else
                throw new FileNotFoundException("Please enter a filename with the correct/full path.");

        }
        /// <summary>
        /// Main handles for scanning a directory. Takes in the configurations bools to handle what to execute on file.
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="yaraScan">yara scan file</param>
        /// <param name="stringSearch">search for strings</param>
        /// <param name="guessFile">attempt to identify file</param>
        /// <param name="pii">search for pii</param>
        public static void ScanDirectory(string folder, bool yaraScan, bool stringSearch, bool guessFile, bool pii)
        {

            if (Directory.Exists(folder))
            {
                string[] files = Directory.GetFiles(folder);

                foreach (string f in files)
                {
                    Console.WriteLine($"File: {f}");
                    ScanFile(f, yaraScan, stringSearch, guessFile, pii);
                }

            }
            else
                throw new DirectoryNotFoundException("Please enter a directory with the correct/full path");
        }
    }
}

