using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Diagnostics;
using System.IO;

namespace FileAnalyzer
{
    public class PythonScript
    {
        /// <summary>
        /// Simple python script call to aqcuire a files ssdeep hash in C#
        /// </summary>
        /// <param name="filename">filename</param>
        /// <returns>ssdeep hash</returns>
        public static string PPDeepHash(string filename)
        {
            const string pythonFile = @"..\..\..\..\PythonScripts\ppdeep_script.py";
            string pythonArgs = $" -f \"{filename}\"";
            return ExecutePythonScript(pythonFile, pythonArgs);
        }
        #region TEST BUCKET abandoned IronPython....
        public static void IronPythonScriptWithModules()
        {
            ScriptEngine pythonEngine = Python.CreateEngine();
            /***********************SEARCH PATHS**************************************/
            // Print the default search paths
            Console.Out.WriteLine("Search paths:");
            var searchPaths = pythonEngine.GetSearchPaths();

            //Add module path to search paths
            searchPaths.Add(@"C:\Users\Derek\source\pyrepo\Modules");
            pythonEngine.SetSearchPaths(searchPaths);
            foreach (string path in searchPaths)
                Console.Out.WriteLine(path);

            /*************************************************************************/
            /*********************SCOPE***********************************************/
            ScriptScope scope = pythonEngine.CreateScope();
            Console.Out.WriteLine("List of variables in the scope:");
            foreach (string name in scope.GetVariableNames())
                Console.Out.Write(name + " ");
            /*************************************************************************/
            ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromFile(@"C:\Users\Derek\source\pyrepo\iron_python.py");
            pythonScript.Execute();

            //Console.WriteLine( scope.GetVariable("variableName")) ;//getting a variable from python
        }
        #endregion
        /// <summary>
        /// Simple yara scanner to scan a file for malware or phishing signatures
        /// </summary>
        /// <param name="filename">file name</param>
        /// <returns>scan results</returns>
        public static string YaraScan(string filename)
        {
            Console.WriteLine("\n\n++++++++Yara Scan++++++++");
            const string pythonFile = @"..\..\..\..\PythonScripts\yara_script.py";
            string pythonArgs = $" -f \"{filename}\"";
            return ExecutePythonScript(pythonFile, pythonArgs);
        }

        /// <summary>
        /// Simple script utilizing the geolite2 database and geoip python module for an location based on an IP address
        /// </summary>
        /// <param name="ip">IPv4 addresss</param>
        /// <returns>location string</returns>
        public static string GeoipCheck(string ip)
        {
            const string pythonFile = @"..\..\..\..\PythonScripts\geoip_script.py";
            string pythonArgs = $" -a {ip}";
            return ExecutePythonScript(pythonFile, pythonArgs);
        }



        /// <summary>
        /// Executes python script given the full path and arguments
        /// </summary>
        /// <param name="pythonFilePath">Path to python file to be executed</param>
        /// <param name="pythonArgString">Optional--any arguments the program requires</param>
        /// <returns>Any output of from the program executing</returns>
        public static string ExecutePythonScript(string pythonFilePath, string pythonArgString = "")
        {

            ProcessStartInfo start = new ProcessStartInfo
            {
                //ProcessStartInfo region
                FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\python.exe", //program to execute
                Arguments = pythonFilePath + " " + pythonArgString,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); //divert error messages 

                    string scriptOutput = reader.ReadToEnd(); //read output
                    if (stderr.Length > 0)
                        scriptOutput += "\nErrors:" + stderr;
                    return scriptOutput;
                }
            }
        }
    }
}

