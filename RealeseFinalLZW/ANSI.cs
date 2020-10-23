using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace RealeseFinalLZW
{
    public class ANSI
    {
        Dictionary<string, int> table = new Dictionary<string, int>();
        public Dictionary<string, int> Table
        {
            get
            {
                return table;
            }
        }

        public ANSI()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("windows-1252");
            for (int i = 0; i < 256; i++)
            {
                if (!table.ContainsKey(encoding.GetString(new byte[1] { Convert.ToByte(i) })))
                {
                    table.Add(encoding.GetString(new byte[1] { Convert.ToByte(i) }), i);
                }
               
            }
        }

        public void WriteLine()
        {
            table.WriteLine();
        }

        public void WriteToFile()
        {
            File.WriteAllText("ANSI.txt", table.ToStringLines(), Encoding.Default);
        }
    }
}
