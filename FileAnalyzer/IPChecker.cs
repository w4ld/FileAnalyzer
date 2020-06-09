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
    class IPChecker
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
    public static bool isValidIPv4(string ip)
        {
            /*******************************
             * takes ip, checks each number is 0-255
             * checks structure 8.8.8.8
             * checks the outgoing range
             * returns bool if legit
             */
            throw new NotImplementedException();
        }
            public static bool isValidIPv6(string ip)
        {
            /*******************************
             * takes ip, checks each number is 0-255
             * checks structure for ipv6
             * * checks the outgoing range
             * returns bool if legit
             */
            throw new NotImplementedException();
        }
    }
}
