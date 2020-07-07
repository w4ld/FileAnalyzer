using System;

namespace FileAnalyzer
{
    public class FASignature
    {
        private string _hexSignature;
        private string _extension;
        private string _description;
        private int _offset;


        public string HexSignature
        {
            get { return _hexSignature; }
            set { _hexSignature = value; }
        }
        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }


        /// <summary>
        /// File signature class. Holds a magic file header signature 
        /// </summary>
        /// <param name="hexsignature">Hex Magic file signature as a string array. ex "BA DF 00 D5" </param>
        /// <param name="extension">Typical extension for the signature. ex ".exe" </param>
        /// <param name="offset">Integer offset value from head of file</param>
        /// <param name="description">Description of the file type</param>
        public FASignature(string hexsignature, string extension, int offset, string description)
        {
            _hexSignature = hexsignature;
            _extension = extension;
            _offset = offset;
            _description = description;
        }

        /// <summary>
        /// Returns the converted hexidecimal signature as a byte array.
        /// </summary>
        /// <returns>hex signature as a byte[]</returns>
        public byte[] GetHexSignature()
        {
            string[] splitSig = _hexSignature.Split(' ');
            int length = splitSig.Length;
            byte[] retArr = new byte[length];
            for (int i = 0; i < length; i++)
            {
                int bits1, bits2;
                bits1 = CharToIntB(splitSig[i][0]);
                bits2 = CharToIntB(splitSig[i][1]);
                byte b = (byte)(16 * bits1 + bits2);
                //  Console.WriteLine($"\tOutput: {b}");
                retArr[i] = b;
            }
            return retArr;

        }
        /// <summary>
        /// Supporting function to convert string hex signature to a correct byte[]. Takes in a hexidecimal as a char and returns integer value. 
        /// </summary>
        /// <param name="c">Hexidecimal as char to convert.</param>
        /// <returns>Integer value of char c</returns>
        private static int CharToIntB(char c)
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



        public override string ToString()
        {
            return $"{_extension}\t\tDescription: {_description}";
        }
        /// <summary>
        /// ToString with optional full or part signature display
        /// </summary>
        /// <param name="full">display full signature</param>
        /// <returns>signature string</returns>
        public string ToString(bool full = false)
        {
            if (!full)
                return $"{_extension}\t\tDescription: {_description}";
            else
                return $"Signature: {_hexSignature}\tOffset:{_offset}\tExtension:{_extension} \n\t\tDescription: {_description}";
        }
        /// <summary>
        /// Static worker for converting magic header signatures into usable signatures
        /// </summary>
        /// <param name="extSig"></param>
        /// <param name="hexExtSig"></param>
        public static void GetHexSignatureBytes(string extSig, ref byte[] hexExtSig)
        {
            extSig = extSig.Trim();
            string[] splitSig = extSig.Split(' ');
            int bits1, bits2;
            for (int i = 0; i < splitSig.Length; i++)
            {
                // Console.WriteLine($"Processing byte:{splitSig[i]}");
                bits1 = CharToIntB(splitSig[i][0]);
                bits2 = CharToIntB(splitSig[i][1]);
                byte b = (byte)(16 * bits1 + bits2);
                //  Console.WriteLine($"\tOutput: {b}");
                hexExtSig[i] = b;
            }
        }
    }
}
