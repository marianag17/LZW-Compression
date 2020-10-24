using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealeseFinalLZW;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ApiLZW.Utils;

namespace ApiLZW.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LZWController: ControllerBase
    {
      
        [HttpPost("compress/{name}")]
       
        
        public async Task<IActionResult> PostCompress(string name, [FromForm] IFormFile file)
        {
            string archivoacomprimir = "";
            string nombreArchivoOriginal = file.FileName.Split('.')[0];
            string nombreArchivoCompreso = name;
            string archivocodificado = "";

            string workingDirectory = Environment.CurrentDirectory;
            string pathFolderActual = Directory.GetParent(workingDirectory).FullName;
            string pathDirectorioCompresiones = pathFolderActual + "\\CompresionesEstructuras\\";


            
            
            Directory.CreateDirectory(pathDirectorioCompresiones);


            string pathDirectorioArchivosSubidos = pathFolderActual + "\\Uploads\\";


            

            Directory.CreateDirectory(pathDirectorioArchivosSubidos);


       

            using (var fileStream = new FileStream((pathDirectorioArchivosSubidos + file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                archivoacomprimir = pathDirectorioArchivosSubidos + file.FileName;
                FileInfo archivoOriginal = new FileInfo(archivoacomprimir);
                ANSI ascii = new ANSI();
                ascii.WriteToFile();

                string text = System.IO.File.ReadAllText(archivoacomprimir, System.Text.Encoding.Default);
                LZWEncoder encoder = new LZWEncoder();

                byte[] b = encoder.EncodeToByteList(text);
                string[] st = archivoacomprimir.Split('.');

                archivocodificado = pathDirectorioCompresiones+ nombreArchivoCompreso + ".lzw";

                string metadata = nombreArchivoOriginal + "||";


                System.IO.File.WriteAllBytes(archivocodificado, System.Text.Encoding.Default.GetBytes(metadata));
                using (var stream = new FileStream(archivocodificado, FileMode.Append))
                {
                    stream.Write(b, 0, b.Length);
                }

                //File.WriteAllBytes(archivocodificado, b);

                
                FileInfo comprimido = new FileInfo(archivocodificado);
                List<double> EstCompresion = new List<double>();
                EstCompresion.Add(archivoOriginal.Length);//largo Inicial
                EstCompresion.Add(comprimido.Length);//largo final
                double Razon = Math.Round((EstCompresion[1] / EstCompresion[0]), 2);//Ratio comp
                double Factor = Math.Round((EstCompresion[0] / EstCompresion[1]), 2); //Factor de comp
                double Porcentaje = Math.Round(((EstCompresion[1] * 100) / EstCompresion[0]), 2); //porcentaje ahorrado
              

                Compressions compresionData = new Compressions();
                compresionData.NombreOriginal = nombreArchivoOriginal;
                compresionData.NombreComprimido = name;
                compresionData.RutaComprimido = archivocodificado;
                compresionData.RazonCompresion = Razon.ToString();
                compresionData.PorcentajeReduccion = Factor.ToString();
                compresionData.FactorCompresion = Porcentaje.ToString() + "%";


                Storage.Instance.listaCompresiones.Add(compresionData);
                var streamCompress = System.IO.File.OpenRead(archivocodificado);

                return new FileStreamResult(streamCompress, "application/lzw")
                {
                    FileDownloadName = nombreArchivoCompreso + ".lzw"
                };

            }
            catch (Exception e) {
                return BadRequest();
            }
           
           

           
         

        }
        
        [HttpPost("decompress/")]
        public async Task<ActionResult> PostDecompress([FromForm] IFormFile file)
        {

            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                string pathFolderActual = Directory.GetParent(workingDirectory).FullName;

                string pathDirectorioArchivosSubidos = pathFolderActual + "\\Uploads\\";
                string pathDirectorioDescompresiones = pathFolderActual + "\\DescompresionesEstructuras\\";

                string archivodecodificado = "";

                

                Directory.CreateDirectory(pathDirectorioDescompresiones);


                

                Directory.CreateDirectory(pathDirectorioArchivosSubidos);

                

                using (var fileStream = new FileStream((pathDirectorioArchivosSubidos + file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }



                FileInfo archivoCompreso = new FileInfo((pathDirectorioArchivosSubidos + file.FileName));

                int position = 0;
                byte[] bo;
                string nombreArchivoOriginal = "";

                using (var stream2 = new StreamReader(archivoCompreso.OpenRead()))
                {
                    bool seguir = false;
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
                                seguir = false;

                            }
                            else
                            {
                                nombreArchivoOriginal += (res[i]).ToString();
                                seguir = true;
                            }
                        }

                        

                    }

                    while (seguir);
                }

                BinaryReader b = new BinaryReader(System.IO.File.Open((pathDirectorioArchivosSubidos + file.FileName), FileMode.Open));

                b.BaseStream.Seek(position, SeekOrigin.Begin);





                bo = b.ReadBytes(((int)archivoCompreso.Length - position));
                b.Close();
                archivodecodificado = pathDirectorioDescompresiones + nombreArchivoOriginal + ".txt";
                LZWDecoder decoder = new LZWDecoder();
                //byte[] bo2 = File.ReadAllBytes(archivocodificado);


                string decodedOutput = decoder.DecodeFromCodes(bo);
                System.IO.File.WriteAllText(archivodecodificado, decodedOutput, System.Text.Encoding.Default);

                var streamCompress = System.IO.File.OpenRead(archivodecodificado);
                return new FileStreamResult(streamCompress, "application/txt")
                {
                    FileDownloadName = nombreArchivoOriginal + ".txt"
                };





            }
            catch {
                
                return BadRequest();
            
            }
          
            

        }

        [HttpGet("compressions/")]
        public IEnumerable<Compressions> GetCompressions()
        {

            var result = Storage.Instance.listaCompresiones;

            return result;

        }
    }
}
