using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LZWCompression
{
    class LZW
    {
        int id = 0;
        Dictionary<int, byte> diccionario = new Dictionary<int, byte>();
        public FileStream compression(string ruta, string nombre)
        {
            using var fileWritten = new FileStream(ruta, FileMode.OpenOrCreate);
            using var reader = new BinaryReader(fileWritten);
            var buffer = new byte[5000];
            while (fileWritten.Position < fileWritten.Length)
            {
                buffer = reader.ReadBytes(5000);
                hacerDiccionarioInicial(buffer);
            }

        }
        public void hacerDiccionarioInicial (byte[] arr)
        {
            foreach (var caracter in arr)
            {
                if (!diccionario.ContainsValue(caracter)){
                    id++;
                    diccionario.Add(id, caracter);
                }
                else
                {
                    break;
                }
            }
        }
        public void aumentarDiccionario(byte [] arr)
        {
            while (arr.Length > 0){
                
            }
        }


    }
}
