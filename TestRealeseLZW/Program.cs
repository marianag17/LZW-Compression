using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RealeseFinalLZW;
using System.IO;

namespace TestRealeseLZW
{
    
    class Program
    {
        static string archivoacomprimir = "";
        static string archivocodificado = "";
        static string archivodecodificado = "";
        static void Main()
        {
            string opcion2 = "y";
            while (opcion2 != "n")
            {

                string ruta = "";
                
                Console.WriteLine("Ingrese la ruta del archivo");
                ruta = Console.ReadLine();


                Console.WriteLine("Ingrese c si desea comprimir o d si desea descomprimir");
                string opcion = Console.ReadLine();
                string ultimarutacompresion = "";
                string ultimarutadescompresion = "";

                if (opcion == "c")
                {

                    archivoacomprimir = ruta;
                    ANSI ascii = new ANSI();
                    ascii.WriteToFile();

                    string text = File.ReadAllText(archivoacomprimir, System.Text.Encoding.Default);
                    LZWEncoder encoder = new LZWEncoder();
                    byte[] b = encoder.EncodeToByteList(text);
                    string[] st = archivoacomprimir.Split('.');
                    archivocodificado = st[0] + ".lzw";

                    File.WriteAllBytes(archivocodificado, b);

                    Console.WriteLine("archivo " + archivoacomprimir + " codificado a " + archivocodificado);
                    Console.ReadLine();
                    ultimarutacompresion = archivocodificado;//Nuevo path
                    FileInfo comprimido = new FileInfo(ultimarutacompresion);
                    List<double> EstCompresion = new List<double>();
                    //EstCompresion.Add(comprimido.Length);//largo final
                    //EstCompresion.Add(Math.Round((EstCompresion[1] / EstCompresion[0]), 2));//Ratio comp
                    //EstCompresion.Add(Math.Round((EstCompresion[0] / EstCompresion[1]), 2));//Factor de comp
                    //EstCompresion.Add(Math.Round(((EstCompresion[0] - EstCompresion[1]) / EstCompresion[1]), 2));//porcentaje ahorrado
                }
                else
                {
                    string[] nombre = ruta.Split('.');

                    archivodecodificado = nombre[0] + ".decodificado";
                    LZWDecoder decoder = new LZWDecoder();
                    byte[] bo = File.ReadAllBytes(archivocodificado);
                    string decodedOutput = decoder.DecodeFromCodes(bo);
                    File.WriteAllText(archivodecodificado, decodedOutput, System.Text.Encoding.Default);
                    Console.WriteLine("archivo " + archivocodificado + " decodificado a: " + archivodecodificado);
                    Console.ReadLine();
                    ultimarutadescompresion = archivodecodificado;//Nuevo path
                    FileInfo descomprimido = new FileInfo(ultimarutadescompresion);
                    //    List<double> EstDescompresion = new List<double>();
                    //    EstDescompresion.Add(descomprimido.Length);//largo final
                    //    EstDescompresion.Add(Math.Round((EstDescompresion[1] / EstDescompresion[0]), 2));//Ratio descomp
                    //    EstDescompresion.Add(Math.Round((EstDescompresion[0] / EstDescompresion[1]), 2));//Factor de descomp
                    //    EstDescompresion.Add(Math.Round(((EstDescompresion[0] - EstDescompresion[1]) / EstDescompresion[1]), 2));//porcentaje ahorrado
                }
                Console.WriteLine("Desea continuar? (n o y)");
                opcion2 = Console.ReadLine();
            }
        }
    }
}
