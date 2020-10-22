using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using compresionLZW;
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
    public class LZWController : ControllerBase
    {
        [HttpPost("compress/{name}")]
        public async Task<ActionResult> PostCompress(string name, [FromForm] IFormFile file)
        {
   
            string nombreArchivoOriginal = file.FileName;
            string nombreArchivoCompreso = name;




            try
            {

                
                using (var stream = new StreamReader(file.OpenReadStream()))
                {

                    CompresionLzw compress = new CompresionLzw();
                    compress.setNombreOriginalArchivo(nombreArchivoOriginal);
                    compress.setNombreArchivoNuevo(nombreArchivoCompreso);
                    compress.escribirArchivoCompreso(compress.getNombreOriginalArchivo() + "|");
                    do
                    {
                        
                        char[] buffer = new char[5000];
                        int n = stream.ReadBlock(buffer, 0, 5000);
                        char[] result = new char[n];
                        Array.Copy(buffer, result, n);

                        string res = new string(result);

                        compress.ObtenerCaracteres(res);
                        

                    }

                    while (!stream.EndOfStream);
                    compress.escribirArchivoCompreso(compress.getContenidoTabla());
                    compress.escribirArchivoCompreso("||");

                    var path = compress.ubicacionArchivo;

                    using (var stream2 = new StreamReader(file.OpenReadStream()))
                    {

                        do
                        {
                            
                            char[] buffer = new char[5000];
                            int n = stream2.ReadBlock(buffer, 0, 5000);
                            char[] result = new char[n];
                            Array.Copy(buffer, result, n);

                            string res = new string(result);


                            compress.Comprimir(res);
                            compress.escribirArchivoCompreso(compress.TextoCompreso);
    
                        }

                        while (!stream2.EndOfStream);
                    }


                    Compressions compresionData = new Compressions();
                    compresionData.NombreOriginal = compress.getNombreOriginalArchivo();
                    compresionData.NombreComprimido = compress.getNombreArchivoNuevo();
                    compresionData.RutaComprimido = compress.ubicacionArchivo;
                    compresionData.RazonCompresion = compress.Razon.ToString();
                    compresionData.PorcentajeReduccion = compress.getPorcentajeReduccion();
                    compresionData.FactorCompresion = compress.Factor.ToString();


                    Storage.Instance.listaCompresiones.Add(compresionData);
                    var streamCompress = System.IO.File.OpenRead(path);

                    return new FileStreamResult(streamCompress, "application/lzw")
                    {
                        FileDownloadName = nombreArchivoCompreso + ".lzw"
                    };
                }

            }
            catch(Exception e) {
                return BadRequest();
            }
           
           

           
         

        }
        
        [HttpPost("decompress/")]
        public async Task<ActionResult> PostDecompress([FromForm] IFormFile file)
        {
            string textoArchivo = "";

            try {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {

                    textoArchivo += stream.ReadToEnd();
                }

                CompresionLzw lzw = new CompresionLzw();

                lzw.Descomprimir();

                var path = lzw.ubicacionArchivo;


                var streamCompress = System.IO.File.OpenRead(path);
                return new FileStreamResult(streamCompress, "application/txt")
                {
                    FileDownloadName = lzw.getNombreOriginalArchivo() + ".txt"
                };

            }
            catch (Exception e) {
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
