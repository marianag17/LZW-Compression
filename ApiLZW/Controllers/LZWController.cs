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
            string textoArchivo = "";
            string nombreArchivoOriginal = file.FileName;
            string nombreArchivoCompreso = name;




            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {

                    textoArchivo += stream.ReadToEnd();
                }

                CompresionLzw compresor = new CompresionLzw(textoArchivo);
                compresor.setNombreOriginalArchivo(nombreArchivoOriginal);
                compresor.setNombreArchivoNuevo(nombreArchivoCompreso);
                compresor.Comprimir();
                var path = compresor.ubicacionArchivo;

                Compressions compresionData = new Compressions();

                compresionData.NombreOriginal = compresor.getNombreOriginalArchivo();
                compresionData.NombreComprimido = compresor.getNombreArchivoNuevo();
                compresionData.RutaComprimido = compresor.ubicacionArchivo;
                compresionData.RazonCompresion = compresor.Razon.ToString();
                compresionData.PorcentajeReduccion = "45 %";
                compresionData.FactorCompresion = compresor.Factor.ToString(); 


                Storage.Instance.listaCompresiones.Add(compresionData);
                var streamCompress = System.IO.File.OpenRead(path);


                return new FileStreamResult(streamCompress, "application/lzw")
                {
                    FileDownloadName = nombreArchivoCompreso + ".lzw"
                };

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

                CompresionLzw lzw = new CompresionLzw(textoArchivo);

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
