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
                    Console.WriteLine("Ingrese el nuevo nombre del archivo");
                    string nuevoNombre = Console.ReadLine();
                    archivoacomprimir = ruta;
                    FileInfo archivoOriginal = new FileInfo(archivoacomprimir);
                    ANSI ascii = new ANSI();
                    ascii.WriteToFile();

                    string text = File.ReadAllText(archivoacomprimir, System.Text.Encoding.Default);
                    LZWEncoder encoder = new LZWEncoder();
                    
                    byte[] b = encoder.EncodeToByteList(text);
                    string[] st = archivoacomprimir.Split('.');
                    
                    archivocodificado = st[0] + ".lzw";

                    string metadata = nuevoNombre + "||";


                    File.WriteAllBytes(archivocodificado, System.Text.Encoding.Default.GetBytes(metadata));
                    using (var stream = new FileStream(archivocodificado, FileMode.Append))
                    {
                        stream.Write(b, 0, b.Length);
                    }
                    
                    //File.WriteAllBytes(archivocodificado, b);

                    Console.WriteLine("archivo " + archivoacomprimir + " codificado a " + archivocodificado);
                    Console.ReadLine();
                    ultimarutacompresion = archivocodificado;//Nuevo path
                    FileInfo comprimido = new FileInfo(ultimarutacompresion);
                    List<double> EstCompresion = new List<double>();
                    EstCompresion.Add(archivoOriginal.Length);//largo Inicial
                    EstCompresion.Add(comprimido.Length);//largo final
                    EstCompresion.Add(Math.Round((EstCompresion[1] / EstCompresion[0]), 2));//Ratio comp
                    EstCompresion.Add(Math.Round((EstCompresion[0] / EstCompresion[1]), 2));//Factor de comp
                    EstCompresion.Add(Math.Round(((EstCompresion[1] * 100 ) / EstCompresion[0]), 2));//porcentaje ahorrado
                }
                else
                {
                    string[] nombre = ruta.Split('.');

                    FileInfo archivoOriginal = new FileInfo(archivocodificado);
                    var stream2 = new StreamReader(archivoOriginal.OpenRead());
                    int position = 0;
                    byte[] bo;
                    string nombreArchivoOriginal = "";
                    do
                    {

                        char[] buffer = new char[150];
                        int n = stream2.ReadBlock(buffer, 0, 150);
                        char[] result = new char[n];
                        Array.Copy(buffer, result, n);

                        string res = new string(result);

                        for (int i = 0; i < res.Length; i++)
                        {
                            if (res[i].Equals('|') && res[(i + 1)].Equals('|'))
                            {
                                position = i + 2;
                                res = res.Substring(i + 2);
                                i = res.Length;


                            }
                            else
                            {
                                nombreArchivoOriginal += (res[i]).ToString();
                            }
                        }

                        stream2.Close();
                        

                        BinaryReader b = new BinaryReader(File.Open(archivocodificado,FileMode.Open));

                        b.BaseStream.Seek(position, SeekOrigin.Begin);
                        

                       


                        bo = b.ReadBytes(((int)archivoOriginal.Length-position));
                        b.Close();
                        archivodecodificado = nombre[0] + nombreArchivoOriginal + ".decodificado";
                        LZWDecoder decoder = new LZWDecoder();
                        //byte[] bo2 = File.ReadAllBytes(archivocodificado);


                        string decodedOutput = decoder.DecodeFromCodes(bo);
                        File.WriteAllText(archivodecodificado, decodedOutput, System.Text.Encoding.Default);
                        Console.WriteLine("archivo " + archivocodificado + " decodificado a: " + archivodecodificado);
                        Console.ReadLine();
                    }

                    while (!stream2.EndOfStream);
                    


                }

                Console.WriteLine("Desea continuar? (n o y)");
                opcion2 = Console.ReadLine();


            }
                
            }
        }
    
}
