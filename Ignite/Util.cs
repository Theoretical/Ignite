using System;
using System.Text;
namespace Ignite
{
    class Util
    {
        public static string CStringToString(byte[] str)
        {
            var terminator = 0;

            while (str[terminator] != 0) terminator++;
            return ASCIIEncoding.ASCII.GetString(str, 0, terminator);
        }
    }
}
