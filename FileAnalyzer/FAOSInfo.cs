using System;

namespace FileAnalyzer
{
    class FAOSInfo
    {

        #region OS Information
        /// <summary>
        /// Dumps all information gleaned from system environment
        /// </summary>
        public static void OS_Information()
        {
            //enumerating the Environment
            PlatformID x = new PlatformID();
            OperatingSystem os = Environment.OSVersion;

            var user = Environment.UserName;

            int processor_count = Environment.ProcessorCount;
            string cl = Environment.CommandLine;
            bool is64bit = Environment.Is64BitOperatingSystem;

            String networkDomainofUser = Environment.UserDomainName;
            int currentThreadID = Environment.CurrentManagedThreadId;
            string sysDir = Environment.SystemDirectory;
            string machineName = Environment.MachineName;
            string[] logicalDrives = Environment.GetLogicalDrives();
            var evs = Environment.GetEnvironmentVariables();


            //public static string GetEnvironmentVariable(string variable);
            //public static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option);
            //public static string GetFolderPath(SpecialFolder folder);
            //public static void SetEnvironmentVariable(string variable, string value);

            //PRINTS
            Console.WriteLine("++++++++OS Information++++++++");
            Console.WriteLine($"\tUser: {user}");
            Console.WriteLine($"\tNetwork Domain: {networkDomainofUser}");
            Console.WriteLine($"\tMachine: {machineName}");
            Console.WriteLine($"\tOS Version: {os}");
            Console.WriteLine($"\tPlatformID: {x}");
            Console.WriteLine($"\tProcessor Count: {processor_count}");
            Console.WriteLine($"\tCommandline: {cl}");
            Console.WriteLine($"\tAre we 64bit? {is64bit}");
            Console.WriteLine($"\tCurrent Thread ID: {currentThreadID}");
            Console.WriteLine($"\tSystem Dir: {sysDir}");
            Console.WriteLine($"\tDrives:");
            foreach (var drive in logicalDrives)
            {
                Console.WriteLine($"\t\t{drive}");
            }
            Console.WriteLine("$\tEnvironmental Variables:");
            var evEnum = evs.GetEnumerator();
            while (evEnum.MoveNext())
            {
                Console.WriteLine($"\t\t{evEnum.Key}:\t {evEnum.Value}");
            }


        }
        #endregion
    }
}
