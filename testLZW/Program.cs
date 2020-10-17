using compresionLZW;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace testLZW
{
    class Program
    {
        static void Main(string[] args)
        {
            string textoArchivo = "A cuesta le cuesta subir la cuesta, y en medio de la cuesta va y se acuesta";
            string nombreArchivoOriginal = "original.txt";
            string nombreArchivoCompreso = "prueba1";


            CompresionLzw compresor = new CompresionLzw(textoArchivo);
            compresor.setNombreOriginalArchivo(nombreArchivoOriginal);
            compresor.setNombreArchivoNuevo(nombreArchivoCompreso);
            compresor.Comprimir();

            string decompress = "";
            
            byte[] readText = File.ReadAllBytes("C:\\jasolis\\c#\\LZW-Compression\\CompresionesEstructuras\\prueba1.lzw");
            decompress += Encoding.UTF8.GetString(readText);

            CompresionLzw lzw = new CompresionLzw(decompress);
            lzw.Descomprimir();
        }
    }
}
