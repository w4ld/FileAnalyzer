using System;

namespace FileAnalyzer
{
    public class FAMenu
    {
        /// <summary>
        /// Prints menu options for this program
        /// </summary>
        public static void PrintHelp()
        {
            Console.WriteLine("FileAnalyzer:\n  A multifunction file tool.");
            Console.WriteLine("  --help\t\tPrint this menu");
            Console.WriteLine("  --file\t\tthe file to subject inspection");
            Console.WriteLine("  --folder\t\tthe folder to scan");
            Console.WriteLine("  --report\t\toutput to text file");

            Console.WriteLine("Usage:\n  fileanalyzer [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  --os \t\t\tPrint operating system information");
            Console.WriteLine("  --strings\t\t\tSearch for strings");
            Console.WriteLine("  --guess\t\tGuess file format");
            Console.WriteLine("  --pii\t\tSearch for personal identifiable information");
            Console.WriteLine("  --version \t\tPrint FileAnalyzer version");
            Console.WriteLine("  --yara\t\tYara scan");

        }
    }
}


/* Desired format
 * imageconv:
  Converts an image file from one format to another.
Usage:
  imageconv [options]
Options:
  --input          The path to the image file that is to be converted.
  --output         The target name of the output after conversion.
  --x-crop-size    The X dimension size to crop the picture.
                   The default is 0 indicating no cropping is required.
  --y-crop-size    The Y dimension size to crop the picture.
                   The default is 0 indicating no cropping is required.
  --version        Display version information
*/
