using System;
using System.Text.RegularExpressions;

namespace FileAnalyzer
{
    public class IPChecker
    {

        //TODO Add IPchecker
        //
        //
        /*************************************
         * method check ip address
         * https://whatismyipaddress.com/ip/193.148.18.41
         * https://whatismyipaddress.com/ip/ + [IP]
         * TODO research best method for checking ips
         * 
         */
        //TODO add website checker
        //
        public enum IPv4Class
        {
            A = 0,            //1.0.0.1 to 126.255.255.255
            B = 1,            //128.1.0.1 to 191.255.255.255
            C = 2,            //192.0.1.1 to 223.255.255.255
            D = 3,            //224.0.0.0 to 239.255.255.255
            E = 4,             //240.0.0.0 to 254.255.255.255
            NiR = 5               //Not in Range
        }
        /// <summary>
        /// Check validity of ip string
        /// </summary>
        /// <param name="ip">IPv4 string</param>
        /// <returns>class of string</returns>
        public static IPv4Class IsValidIPv4(string ip)
        {
            Regex r = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (r.IsMatch(ip))
            {
                int i = 0;
                int[] ipblocks = new int[4];
                foreach (var subnet in r.Match(ip).Value.Split('.'))
                {
                    ipblocks[i++] = int.Parse(subnet);//TODO doublecheck this doesnt need error handling. Regex should handle...
                }
                //Console.WriteLine($"CHECKING IP: {ipblocks[0]} {ipblocks[1]} {ipblocks[2]} {ipblocks[3]} ");

                if (ipblocks[2] >= 0 && ipblocks[2] < 256 && ipblocks[3] >= 0 && ipblocks[3] < 256 && ipblocks[1] >= 0 && ipblocks[1] < 256)
                {
                    if (ipblocks[0] > 1 && ipblocks[0] < 129)//
                        return IPv4Class.A;
                    else if (ipblocks[0] > 127 && ipblocks[0] < 192)//
                        return IPv4Class.B;
                    else if (ipblocks[0] > 191 && ipblocks[0] < 224)//
                        return IPv4Class.C;
                    else if (ipblocks[0] > 223 && ipblocks[0] < 240)//
                        return IPv4Class.D;
                    else if (ipblocks[0] > 239 && ipblocks[0] < 255)//
                        return IPv4Class.E;
                }
            }
            return IPv4Class.NiR;
        }

        /*******************************
        * takes ip, checks each number is 0-255
        * checks structure 8.8.8.8
        * checks the outgoing range
        * returns class if legit
        */

        public static bool IsValidIPv6(string ip)
        {
            /*******************************
                * takes ip, checks each number 
                * checks structure for ipv6
                * * checks the outgoing range
                * returns bool if legit
                */
            throw new NotImplementedException();
        }
    }
}
