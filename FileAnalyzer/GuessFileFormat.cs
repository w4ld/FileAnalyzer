using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace FileAnalyzer
{
    public class GuessFileFormat
    {
        const int FILE_HEADER_SIZE = 64;
        #region Guess File Format

        public static void Guess(string filename)
        {
            Console.WriteLine("\n\n++++++++Magic File Headers Matches++++++++");
            List<FASignature> sigs = GuessFileFormat.ReadFileHeaders(filename);
            if (sigs.Count == 1) //easy no duplicate IDs
            {
                Console.WriteLine("\t" + sigs[0].ToString());
            }
            else if (sigs.Count > 1)
            {
                //TODO handle this so that if check zip has occured then filter accordingly
                //Refactor this into something for zips
                //TODO do the same for other formats.
                FASignature fasig;
                foreach (var sig in sigs)
                {
                    if (sig.HexSignature == "50 4B 03 04")    //this is the first match in the database for zip
                    {

                        fasig = IdentifyZip(filename);
                        if (fasig != null)
                        {
                            Console.WriteLine("\t" + fasig.ToString(full: true));
                            break;
                        }
                        else
                            Console.WriteLine("\t" + sig.ToString(full: true));
                    }
                    else if (sig.HexSignature == "FF D8 FF E0")
                    {
                        fasig = IdentifyJPEG(filename);
                        Console.WriteLine("\t" + fasig.ToString(full: true));
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\t" + sig.ToString(full: true));
                    }
                    //heres where you can try further ID
                }
            }
            else
            {
                if (!TestTSQL(filename))
                    Console.WriteLine("\tNo Header Matched in DB. Possible ambiguous file type.");
            }
            Console.WriteLine("\n");
        }



        /// <summary>
        /// Currently reads formats list from an excel file. Looks for a possible match
        /// </summary>
        /// <param name="filename">File name to check</param>
        public static List<FASignature> ReadFileHeaders(String filename)
        {
            List<FASignature> detectedSignatures = new List<FASignature>();
            using (FileStream fileStream = File.OpenRead(filename))
            {

                //FileHeader fileHeader = new FileHeader();
                try
                {
                    byte[] buffer = new byte[FILE_HEADER_SIZE];
                    //fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, FILE_HEADER_SIZE);


                    const string fileName = @"..\..\..\..\FileExtensions.xlsx";
                    //var sheet = "Wikipedia";
                    var sheet = "FileSignatures.net";
                    //this likes .xls, but not the newer format .xlsx
                    var connXLSX = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties='Excel 12.0 Xml; HDR=YES'";
                    //var connXLS = "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;";
                    var connectionString = string.Format(connXLSX, fileName);
                    DataTable db = null;
                    OleDbDataAdapter adapter;
                    DataSet ds;
                    try
                    {
                        string query = $"SELECT * FROM [{sheet}$]"; //TODO adjust query to specify columns.
                        adapter = new OleDbDataAdapter(query, connectionString);
                        ds = new DataSet();
                        string tablename = "fileextensions";
                        adapter.Fill(ds, tablename);
                        db = ds.Tables[tablename];
                    }
                    catch (Exception ex) //handle chance file is currently open.
                    {
                        Console.WriteLine("Please save and close Excel file extension file then retry.");
                        Console.WriteLine(ex.Message);
                    }
                    int rc = 0;
                    if (null != db && null != db.Rows)
                    {
                        foreach (DataRow dataRow in db.Rows)
                        {
                            //handle for nulls; pass to check function
                            string hexSignature = "";
                            string extension = "";
                            string description = "";
                            int offset;
                            //fill variables to pass into object instance
                            if (!dataRow.IsNull(0))
                                hexSignature = dataRow.Field<string>(0).Trim();

                            if (!dataRow.IsNull(3))
                                extension = dataRow.Field<string>(3);

                            offset = (!dataRow.IsNull(2)) ? (int)dataRow.Field<double>(2) : 0;

                            if (!dataRow.IsNull(4))
                                description = dataRow.Field<string>(4);

                            FASignature faSig = new FASignature(hexSignature, extension, 0, description);

                            if (hexSignature.Length > 0 && (extension.Length > 0 || description.Length > 0))
                                if (CheckSignature(buffer, faSig))
                                    detectedSignatures.Add(faSig);
                            rc++;

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}", ex.Message);
                    Console.ResetColor();
                    throw ex;

                }
            }
            return detectedSignatures;
        }
        /// <summary>
        /// Takes signature string and checks it against file header buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="hexSignature"></param>
        /// <param name="extension"></param>
        /// <param name="description"></param>
        /// <returns>bool signature found</returns>
        public static bool CheckSignature(byte[] buffer, string hexSignature, string extension, string description)
        {
            //check that the signature read is complete enough to yeld useful info
            if (!String.IsNullOrEmpty(extension) || !String.IsNullOrEmpty(description))
            {

                //  Console.Write("Checking Extension: {0}\t", extension);

                if (!String.IsNullOrEmpty(hexSignature))//checks for a hexSig to look for
                {


                    byte[] hexSigBuffer = new byte[hexSignature.Trim().Split(' ').Length];
                    FASignature.GetHexSignatureBytes(hexSignature, ref hexSigBuffer);

                    for (int i = 0; i < hexSigBuffer.Length; i++)
                    {
                        //Console.WriteLine($"Buf:{buffer[i]} \t Sig:{hexSigBuffer[i]}");
                        if (buffer[i] != hexSigBuffer[i])
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Takes as FAsignature and checks it against file header buffer
        /// </summary>
        /// <param name="buffer">file header buffer</param>
        /// <param name="faSig">signature</param>
        /// <returns>bool signature found</returns>
        public static bool CheckSignature(byte[] buffer, FASignature faSig)
        {

            if (!String.IsNullOrEmpty(faSig.Extension))
            {
                if (!String.IsNullOrEmpty(faSig.HexSignature))//checks for a hexSig to look for
                {
                    byte[] hexSigBuffer = faSig.GetHexSignature();

                    for (int i = 0; i < hexSigBuffer.Length; i++)
                    {
                        //Console.WriteLine($"Buf:{buffer[i]} \t Sig:{hexSigBuffer[i]}");
                        if (buffer[i + faSig.Offset] != hexSigBuffer[i])
                            return false;

                    }
                    return true;
                }
            }
            return false;
        }


        public static string BToChar(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", " ");
        }

        #endregion
        #region experimental
        /// <summary>
        /// primitive scanner using boyer moore to identify something.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static bool TestTSQL(string filename)
        {
            string[] tsql_keywords = { "SELECT", "FROM", "WHERE", "ORDER BY", "GROUP BY", "HAVING" }; //simple example of keywords
            string text = File.ReadAllText(filename);
            int count = 0, threshold = 6;
            foreach (string keyword in tsql_keywords)
            {
                if (BoyerMooreString.IndexOf(text, keyword) > -1)
                    count++;
            }
            if (count > threshold)
            {
                Console.WriteLine("Possible TSQL File!");
                return true;
            }
            return false;
        }

        public static string BasicByteBoyerMoore(string filename, string findString)
        {
            byte[] findBytes = Encoding.ASCII.GetBytes(findString);
            string detect_string = "NOTHING DETECTED!";
            using (FileStream fileStream = File.OpenRead(filename))
            {
                try
                {
                    FileInfo info = new FileInfo(filename);
                    long size = info.Length;
                    byte[] buffer = new byte[size]; //buffer for file
                    //fileStream.Seek(0, SeekOrigin.Begin);   //start at beginning then 
                    fileStream.Read(buffer, 0, (int)size);
                    int offset = BoyerMoore.IndexOf(buffer, findBytes);
                    if (offset > -1)
                    {
                        Console.WriteLine("Bytes DETECTED");
                        detect_string = "Bytes DETECTED!";
                    }

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}", ex.Message);
                    Console.ResetColor();
                    throw ex;
                }
                return detect_string;
            }
        }
        #endregion
        #region Unzip for ID
        /// <summary>
        /// A method to quickly examine compressed and zipped file formats.
        /// </summary>
        /// <param name="filename">Zipped filepath</param>
        /// <param name="cleanup">Leave extracted folder open or clean it up</param>
        public static void BasicZipAndExamine(string filename, bool cleanup = true)
        {
            /******Experimental functionality to help identify Office suite files
             * TODO research safety of unzipping random files. practicality. etc.
             * not 100% confidence in uniqueness of ID
             */
            string tempDirectory = @"C:\Users\Derek\OneDrive\Documents\CSC205\ProjectTestFiles\TestFolder";

            // Extract identified zip file to temp dir
            ZipFile.ExtractToDirectory(filename, tempDirectory);
            //Print files
            Console.WriteLine("Files");
            foreach (var file in Directory.GetFiles(tempDirectory))
                Console.WriteLine($"\t{file}");




            //XDocument unpackedXML = XDocument.Load(@"C:\Users\Derek\OneDrive\Documents\CSC205\ProjectTestFiles\Brigitte\word\document.xml");
            //foreach (var des in unpackedXML.Descendants())
            //    Console.WriteLine(des.Name);
            if (cleanup)
                if (Directory.Exists(tempDirectory))
                    Directory.Delete(tempDirectory, recursive: true);

        }

        /// <summary>
        /// Haldling function for Zip files. Attempts to identify them further.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static FASignature IdentifyZip(string filename)
        {
            /******Experimental functionality to help identify Office suite files
             * TODO research safety of unzipping random files. practicality. etc.
             * not 100% confidence in uniqueness of ID
             */
            FASignature retSig = null;
            const string tempDirectory = @"..\..\..\..\Temp\TestFolder";
            // Extract identified zip file to temp dir
            ZipFile.ExtractToDirectory(filename, tempDirectory);

            //MS Word
            if (Directory.Exists(tempDirectory + "\\word"))
            {
                Console.WriteLine("MS Word Identified");
                retSig = new FASignature("", "docx", 0, "Microsoft Word Document");
            }

            //MS Excel
            else if (Directory.Exists(tempDirectory + @"\xl\worksheets"))
            {
                Console.WriteLine("MS Excel Identified");
                retSig = new FASignature("", "xlsx", 0, "Microsoft Excel Document");
            }
            //MS PPT
            else if (Directory.Exists(tempDirectory + @"\ppt\slides"))
            {
                Console.WriteLine("MS Powerpoint Identified");
                retSig = new FASignature("", "xlsx", 0, "Microsoft Powerpoint Document");
            }

            //ODT
            else if (Directory.Exists(tempDirectory + "\\META-INF"))
            {
                Console.WriteLine("Settings Exists");
                XDocument xDoc = XDocument.Load(tempDirectory + @"\settings.xml");

                if (xDoc.Root.Name.LocalName == "document-settings")
                {
                    Console.WriteLine("Open Document Format Extension: .odt");
                    retSig = new FASignature("", "odt", 0, "Open Document Text");
                }
            }
            //TODO eexamine and identify more filetypes using this extension
            else
                Console.WriteLine("Zip of otherwise not currently implemented filetype.");

            //cleanup after yourself...
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, recursive: true);

            return retSig;


        }


        /// <summary>
        /// Identifying method for JFIF/JPEG/JPG
        /// </summary>
        /// <param name="filename">fiel name</param>
        /// <returns>identified sig</returns>
        public static FASignature IdentifyJPEG(string filename)
        {
            string buffer = File.ReadAllText(filename);
            int index = BoyerMooreString.IndexOf(buffer, "JFIF");//jfif files have a section with an ASCII JFIF\0 that will identify it. 
            //most JPGs conform to the JFIF standard
            if (index != -1)
            {
                Console.WriteLine(index);
                return new FASignature("FF D8 FF E0", "JFIF", 0, "JPEG File Interchange Format is an image file format standard");
            }
            return new FASignature("FF D8 FF E0", "JPEG", 0, "JPEG/JPG Image Format");
        }


        #endregion
    }
}
/*********************************************************
*TODO change signatures to return a list of possibles.
*   iterate list with further poking. then maybe we can corner various file types....
*TODO adapt process to output initial list then ask to further identify
*
*/
