using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileAnalyzer
{
    public class FAUtilities
    {
        public static bool GetUserInput(string prompt)
        {
            TextWriter streamWriter=null;
          
            if(Console.IsOutputRedirected)
            {
                streamWriter = Console.Out;
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));//set back to stdout
            }
            Console.WriteLine(prompt);
            string response = Console.ReadLine();
            bool VerifiedInput = false;

            while (!VerifiedInput)
            {
                response = response.ToLower().Trim();
                switch (response)
                {
                    case "n":
                    case "no":       
                        if (Console.IsOutputRedirected)                        
                            Console.SetOut(streamWriter);                  
                        return false;
                    case "y":
                    case "yes":
                        if (Console.IsOutputRedirected)                                               
                            Console.SetOut(streamWriter);
                        return true;
                    default:
                        Console.WriteLine("Please enter yes/no.\n"+prompt);
                        response = Console.ReadLine();
                        break;
                }             

            }
            return false;
        }
    }
}
