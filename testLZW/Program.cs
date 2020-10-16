using compresionLZW;
using System;
using System.IO;
using System.Reflection;

namespace testLZW
{
    class Program
    {
        static void Main(string[] args)
        {
            string textoArchivo = "anita lava la tina";
            string nombreArchivoOriginal = "original.txt";
            string nombreArchivoCompreso = "prueba1";
            CompresionLzw compresor = new CompresionLzw(textoArchivo);
            compresor.setNombreOriginalArchivo(nombreArchivoOriginal);
            compresor.setNombreArchivoNuevo(nombreArchivoCompreso);
            compresor.Comprimir();
        }
    }
}
