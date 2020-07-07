using System;
using System.IO;
using System.Security.Cryptography;

namespace FileAnalyzer
{
    public class FAFileInfo
    {
        #region Basic File Info
        /// <summary>
        /// Print the basic file information for the file
        /// </summary>
        /// <param name="fileInfo">FileInfo object to interate</param>
        public static void PrintFileInfo(FileInfo fileInfo)
        {
            Console.WriteLine("\n\n++++++++File Information++++++++");
            try
            {
                //takes FileInfo object printing the attributes to user
                Console.WriteLine($"\tFile Name: {fileInfo.Name}");
                Console.WriteLine($"\tExtension: {fileInfo.Extension}");
                Console.WriteLine($"\tSize: {SizeToProperOrder(fileInfo.Length)}");//convert this later
                Console.WriteLine($"\tCreated: {fileInfo.CreationTime}");
                Console.WriteLine($"\tLast Write: {fileInfo.LastWriteTime}");
                Console.WriteLine($"\tLast Access: {fileInfo.LastAccessTime}");
                Console.WriteLine($"\tRead-Only: {fileInfo.IsReadOnly}");

            }
            catch (FileNotFoundException fnfe)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError:File path incorrect\n");
                Console.ResetColor();
                Console.WriteLine("{0}", fnfe.Message);
                return;
            }

        }
        /// <summary>
        /// Formatter to take a long bytes and make it the correct order of magnitude
        /// </summary>
        /// <param name="length">number of bytes</param>
        /// <returns>string format of bytes in correct magnitude</returns>
        private static string SizeToProperOrder(long length)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order;
            for (order = 0; (length >= 1024 && order < sizes.Length - 1); order++, length /= 1024)
            {
                ;//love doing this
            }
            return $"{length} {sizes[order]}";
        }

        #endregion
        #region Hashes
        /// <summary>
        /// Display the hashes of the file
        /// </summary>
        /// <param name="fileName"></param>
        public static void DisplayHashes(string fileName)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {

                Console.WriteLine("\n\n++++++++Hashes++++++++");
                try
                {
                    FileInfo info = new FileInfo(fileName);
                    long size = info.Length;
                    byte[] buffer = new byte[size]; //buffer for file
                    fileStream.Seek(0, SeekOrigin.Begin);   //start at beginning then 
                    fileStream.Read(buffer, 0, (int)size);
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] md5Hash = md5.ComputeHash(buffer);
                        Console.WriteLine("\tMD5:    " + BitConverter.ToString(md5Hash).Replace("-", String.Empty));
                    }
                    using (SHA1 sha1 = SHA1.Create())
                    {
                        byte[] sha1Hash = sha1.ComputeHash(buffer);
                        Console.WriteLine("\tSHA1:   " + BitConverter.ToString(sha1Hash).Replace("-", String.Empty));
                    }
                    using (SHA512 sha512 = SHA512.Create())
                    {
                        byte[] sha512Hash = sha512.ComputeHash(buffer);
                        Console.WriteLine("\tSHA512: " + BitConverter.ToString(sha512Hash).Replace("-", String.Empty));
                    }
                    using (SHA384 sha384 = SHA384.Create())
                    {
                        byte[] sha384Hash = sha384.ComputeHash(buffer);
                        Console.WriteLine("\tSHA384: " + BitConverter.ToString(sha384Hash).Replace("-", String.Empty));
                    }
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] sha256Hash = sha256.ComputeHash(buffer);
                        Console.WriteLine("\tSHA256: " + BitConverter.ToString(sha256Hash).Replace("-", String.Empty));


                    }
                    Console.WriteLine("\tSSDEEP: " + PythonScript.PPDeepHash(fileName)); //execute python script for ssdeep hash. 
                }
                catch
                {
                    Console.WriteLine("Error trying to compute hash");
                }
            }
        }

        #endregion

        /// <summary>
        /// Print out bytes of a file header.
        /// </summary>
        /// <param name="fileName">file to read</param>
        /// <param name="numBytes">number of bytes to read</param>
        public static void PrintFileHeaderBytes(string fileName, int numBytes)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                try
                {
                    byte[] buffer = new byte[numBytes];
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, numBytes);
                    Console.WriteLine(GuessFileFormat.BToChar(buffer));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error encountered when reading file");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// Iterate a directory printing file header bytes
        /// </summary>
        /// <param name="directory">path to directory to iterate</param>
        /// <param name="numBytes">number of bytes to print</param>
        public static void DirectoryOutlook(string directory, int numBytes = 64)
        {
            if (string.IsNullOrEmpty(directory))    //handle null input
                throw new FileNotFoundException();
            var files = Directory.GetFiles(directory);

            foreach (string f in files)
            {
                Console.Write($"Testing {f}\n\t");
                PrintFileHeaderBytes(f, numBytes);//print file header bytes

            }
        }
    }
}
