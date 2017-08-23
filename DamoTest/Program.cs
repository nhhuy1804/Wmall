using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DamoTest
{
    class Program
    {
        // 관련 scpdb_agent.dll IMPORT
        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Init", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Init(string iniFilePath);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_ReInit", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_ReInit(string iniFilePath);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_CreateContextServiceID", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_CreateContextServiceID(
          string serviceID,         /*[IN]*/
          string account,       /*[IN]*/
          StringBuilder outContextBuf,     /*[OUT]*/
          int outContextBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_CreateContextImportFile", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_CreateContextImportFile(
          string keyFilePath,       /*[IN]*/
          StringBuilder outContextBuf,     /*[OUT]*/
          int outContextBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_CreateContext", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_CreateContext(
          string agent_id,          /*[IN]*/
          string db_name,           /*[IN]*/
          string db_owner,          /*[IN]*/
          string table_name,        /*[IN]*/
          string column_name,       /*[IN]*/
          StringBuilder outContextBuf,     /*[OUT]*/
          int outContextBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Encrypt_Str", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Encrypt_Str(
          string contextBuf,
          string plain,
          int plainLen,
          StringBuilder cipher,
          ref int cipherLen,
          int cipherBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Decrypt_Str", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Decrypt_Str(
          string contextBuf,
          string cipher,
          int cipherLen,
          StringBuilder plain,
          ref int plainLen,
          int plainBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Encrypt_B64", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Encrypt_B64(
          string contextBuf,
          string plain,
          int plainLen,
          StringBuilder cipher,
          ref int cipherLen,
          int cipherBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Decrypt_B64", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Decrypt_B64(
          string contextBuf,
          string cipher,
          int cipherLen,
          StringBuilder plain,
          ref int plainLen,
          int plainBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Encrypt_Str_Number", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Encrypt_Str_Number(
          string contextBuf,
          string numberStr,
          int numberStrLen,
          StringBuilder cipher,
          ref int cipherLen,
          int cipherBufMax,
        string type);


        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Decrypt_Str_Number", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Decrypt_Str_Number(
          string contextBuf,
          string cipher,
          int cipherLen,
          StringBuilder numberStr,
          ref int numberStrLen,
          int numberStrBufMax,
          string type);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Encrypt_Str_Int", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Encrypt_Str_Int(
          string contextBuf,
          int number,
          StringBuilder cipher,
          ref int cipherLen,
          int cipherBufMax,
          string type);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Decrypt_Str_Int", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Decrypt_Str_Int(
          string contextBuf,
          string cipher,
          int cipherLen,
          ref int number,
          string type);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Index_Str", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Index_Str(
          string contextBuf,
          string plain,
          int plainLen,
          StringBuilder cipher,
          ref int cipherLen,
          int cipherBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Encrypt_FilePath", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Encrypt_FilePath(
          string contextBuf,
          string inFilePath,
          int inFileSize,
          string outFilePath,
          ref int outFileSize,
          int outFileMaxSize);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_Decrypt_FilePath", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_Decrypt_FilePath(
          string contextBuf,
          string inFilePath,
          int inFileSize,
          string outFilePath,
          ref int outFileSize,
          int outFileMaxSize);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_HASH_B64", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_HASH_B64(
          int hashId,
          string input,
          int inputLen,
          StringBuilder outData,
          ref int outLen,
          int outBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_HASH_Str", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_HASH_Str(
          int hashId,
          string input,
          int inputLen,
          StringBuilder outData,
          ref int outLen,
          int outBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_HexToB64", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_HexToB64(
          string input,
          int inputLen,
          StringBuilder outData,
          ref int outLen,
          int outBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "ScpAgt_B64ToHex", CharSet = CharSet.Ansi)]
        public static extern int
        ScpAgt_B64ToHex(
          string input,
          int inputLen,
          StringBuilder outData,
          ref int outLen,
          int outBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "SCP_EncB64")]
        public static extern int SCP_EncB64(string iniFilePath, string iniKeyName,
            string plain, int plainLen, StringBuilder cipher,
            ref int cipherLen, int cipherBufMax);

        [DllImport("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.dll", EntryPoint = "SCP_DecB64")]
        public static extern int SCP_DecB64(string iniFilePath, string iniKeyName, string cipher,
            int cipherLen, StringBuilder plain, ref int plainLen, int plainBufMax);

        // 초기 설정
        public const string iniFilePath = "D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\scpdb_agent.ini";
        public const string keyFilePath = "D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\AES_128_E_FIXED_IV_CBC.SCPS";
        public const string inFilePath = "D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\test.txt";
        public const string encFilePath = "D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\test_enc.txt";
        public const string decFilePath = "D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\test_dec.txt";
        public const int MAX_ENC_STRING = 128;
        public const int MAX_DEC_STRING = 128;
        public const int MAX_HASH_STRING = 256;

        static void Main(string[] args)
        {
            AddPathSegments("D:\\repo_remote\\02.Deployment\\POS\\debug\\bin\\lib\\damo\\");

            string context;
            //string plain = "123456-1234567";
            string plain = "1252125212521252=12891252125352525752";
            string encData;
            string decData;
            string hashData;

            int ret = -1;

            StringBuilder enc_string;
            int e_len = 0;

            StringBuilder dec_string;
            int d_len = 0;

            StringBuilder hash_string;
            int h_len = 0;

            StringBuilder convert_string;
            int c_len = 0;

            int outFileSize = 0;

            StringBuilder contextOut;
            contextOut = null;
            contextOut = new StringBuilder(64);

            // DAMO_API 모듈 초기화
            ret = ScpAgt_Init(iniFilePath);

            //ret = ScpAgt_CreateContextServiceID("ServiceID", "AccountName", contextOut, 32);
            ret = ScpAgt_CreateContextImportFile(keyFilePath, contextOut, 32);
            //ret = ScpAgt_CreateContext("agent_id", "db_name", "owner", "table_name", "column_name", contextOut, 32);
            context = contextOut.ToString();

            Dencrypt("AhrV7RpWdCIHbkbjfdtjqbiVBaQGmcUY5YbV/9vb6GA=");

            Dencrypt("hSF0SFAIPwB9EIMbFCUVE2BxkHVNRxZe7i1qKN6UoTk=");
            Dencrypt("/UAzXVsR/erjlLBv8mdV7AxewBkMD75ddo2vA0L8/0k=");
            // 닫기
            Console.ReadKey();
            return;




            /*******************************************/
            /* ScpAgt_Encrypt_Str, ScpAgt_Decrypt_Str  */
            /*******************************************/
            enc_string = null;
            enc_string = new StringBuilder(MAX_ENC_STRING);
            dec_string = null;
            dec_string = new StringBuilder(MAX_DEC_STRING);
            convert_string = null;
            convert_string = new StringBuilder(MAX_HASH_STRING);
            // 암호화 합니다.
            ret = ScpAgt_Encrypt_Str(context, plain, Encoding.Default.GetByteCount(plain), enc_string, ref e_len, MAX_ENC_STRING);
            Console.WriteLine("/* ScpAgt_Encrypt_Str, ScpAgt_Decrypt_Str  */");
            Console.Write("PLAIN    : ");
            Console.WriteLine(plain);

            encData = enc_string.ToString();
            Console.Write("ENCRYPT  : ");
            Console.WriteLine(encData + "-" + encData.Length.ToString());

            ret = ScpAgt_HexToB64(encData, Encoding.Default.GetByteCount(encData), convert_string, ref c_len, MAX_HASH_STRING);
            Console.Write("HexToB64 : ");
            Console.WriteLine(convert_string.ToString());

            // 암호화된 결과를 복호화 합니다.
            ret = ScpAgt_Decrypt_Str(context, encData, encData.Length, dec_string, ref d_len, MAX_DEC_STRING);
            decData = dec_string.ToString();
            Console.Write("DECRYPT  : ");
            Console.WriteLine(decData);

            /*******************************************/
            /* ScpAgt_Encrypt_B64, ScpAgt_Decrypt_B64  */
            /*******************************************/
            enc_string = null;
            enc_string = new StringBuilder(MAX_ENC_STRING);
            dec_string = null;
            dec_string = new StringBuilder(MAX_DEC_STRING);
            convert_string = null;
            convert_string = new StringBuilder(MAX_HASH_STRING);
            // 암호화 합니다.
            ret = ScpAgt_Encrypt_B64(context, plain, Encoding.Default.GetByteCount(plain), enc_string, ref e_len, MAX_ENC_STRING);
            Console.WriteLine("");
            Console.WriteLine("/* ScpAgt_Encrypt_B64, ScpAgt_Decrypt_B64  */");
            Console.Write("PLAIN    : ");
            Console.WriteLine(plain);

            encData = enc_string.ToString();
            Console.Write("ENCRYPT  : ");
            Console.WriteLine(encData);

            ret = ScpAgt_B64ToHex(encData, Encoding.Default.GetByteCount(encData), convert_string, ref c_len, MAX_HASH_STRING);
            Console.Write("B64ToHex : ");
            Console.WriteLine(convert_string.ToString());

            // 암호화된 결과를 복호화 합니다.
            ret = ScpAgt_Decrypt_B64(context, encData, encData.Length, dec_string, ref d_len, MAX_DEC_STRING);
            decData = dec_string.ToString();
            Console.Write("DECRYPT  : ");
            Console.WriteLine(decData);

            /*******************************************/
            /* ScpAgt_Encrypt_FilePath, ScpAgt_Decrypt_FilePath  */
            /*******************************************/
            ret = ScpAgt_Encrypt_FilePath(context, inFilePath, inFilePath.Length, encFilePath, ref outFileSize, 1024);
            Console.WriteLine("");
            Console.WriteLine("/* ScpAgt_Encrypt_FilePath, ScpAgt_Decrypt_FilePath  */");
            Console.Write("ENC SIZE : ");
            Console.WriteLine(outFileSize);

            ret = ScpAgt_Decrypt_FilePath(context, encFilePath, encFilePath.Length, decFilePath, ref outFileSize, 1024);
            Console.Write("DEC SIZE : ");
            Console.WriteLine(outFileSize);


            /* 
              HASH 처리후 base64 인코딩하여 출력한다
              HASH Algorithm ID :
              SHA1 = 70
              SHA256 = 71
              SHA384 = 72
              SHA512 = 73
              HAS160 = 74
              MD5 = 75  

              SHA256(71) 일때 
              1234567890 => x3Xnt1ft5jDNCqERO9ECZhqziCnKUqZCKreChi8mhkY=
            */
            /*  HASH 처리후 base64 인코딩하여 출력한다 */
            hash_string = null;
            hash_string = new StringBuilder(MAX_HASH_STRING);
            convert_string = null;
            convert_string = new StringBuilder(MAX_HASH_STRING);

            Console.WriteLine("");
            Console.WriteLine("/* ScpAgt_HASH_B64, ScpAgt_B64ToHex  */");
            Console.Write("PLAIN    : ");
            Console.WriteLine(plain);
            ret = ScpAgt_HASH_B64(71, plain, Encoding.Default.GetByteCount(plain), hash_string, ref h_len, MAX_HASH_STRING);
            hashData = hash_string.ToString();
            Console.Write("HASH_B64 : ");
            Console.WriteLine(hashData);
            ret = ScpAgt_B64ToHex(hashData, Encoding.Default.GetByteCount(hashData), convert_string, ref c_len, MAX_HASH_STRING);
            Console.Write("HexToB64 : ");
            Console.WriteLine(convert_string.ToString());


            /*  HASH 처리후 Hexa String 하여 출력한다 */
            hash_string = null;
            hash_string = new StringBuilder(MAX_HASH_STRING);
            convert_string = null;
            convert_string = new StringBuilder(MAX_HASH_STRING);

            Console.WriteLine("");
            Console.WriteLine("/* ScpAgt_HASH_Str, ScpAgt_HexToB64  */");
            Console.Write("PLAIN    : ");
            Console.WriteLine(plain);
            ret = ScpAgt_HASH_Str(71, plain, Encoding.Default.GetByteCount(plain), hash_string, ref h_len, MAX_HASH_STRING);
            hashData = hash_string.ToString();
            Console.Write("HASH_Str : ");
            Console.WriteLine(hashData);
            ret = ScpAgt_HexToB64(hashData, Encoding.Default.GetByteCount(hashData), convert_string, ref c_len, MAX_HASH_STRING);
            Console.Write("HexToB64 : ");
            Console.WriteLine(convert_string.ToString());

            /* ret = SCP_EncB64(iniFilePath, "KEY1", plainText, Encoding.Default.GetByteCount(plainText), enc_string,
                ref e_len, MAX_ENC_STRING); 
             */
            enc_string = null;
            enc_string = new StringBuilder(MAX_ENC_STRING);

            ret = SCP_EncB64(iniFilePath, "KEY1", plain, Encoding.Default.GetByteCount(plain), enc_string,
                ref e_len, MAX_ENC_STRING);
            Console.WriteLine("");
            Console.WriteLine("/* SCP_EncB64, SCP_DecB64 */");
            Console.Write("PLAIN    : ");
            Console.WriteLine(plain);

            encData = enc_string.ToString();
            Console.Write("ENCRYPT  : ");
            Console.WriteLine(encData);

            dec_string = null;
            dec_string = new StringBuilder(MAX_ENC_STRING);

            // 암호화 합니다.
            ret = SCP_DecB64(iniFilePath, "KEY1", encData, Encoding.Default.GetByteCount(encData), dec_string,
                ref e_len, MAX_ENC_STRING);
            decData = dec_string.ToString();
            Console.Write("DECRYPT  : ");
            Console.WriteLine(decData);

            // 닫기
            Console.ReadKey();
        }

        static void AddPathSegments(string pathSegment)
        {
            string allPaths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            if (allPaths != null)
                allPaths = pathSegment + "; " + allPaths;
            else
                allPaths = pathSegment;
            Environment.SetEnvironmentVariable("PATH", allPaths, EnvironmentVariableTarget.Process);
        }

        static void Dencrypt(string encData)
        {
            Console.Write("ENCRYPT  : ");
            Console.WriteLine(encData);
            int e_len = 0;
            StringBuilder dec_string;
            int d_len = 0;
            string decData;
            dec_string = null;
            dec_string = new StringBuilder(MAX_ENC_STRING);
            int ret = SCP_DecB64(iniFilePath, "KEY1", encData, Encoding.Default.GetByteCount(encData), dec_string,
                ref e_len, MAX_ENC_STRING);
            decData = dec_string.ToString();
            Console.Write("DECRYPT  : ");
            Console.WriteLine(decData);

            
        }
    }

}
