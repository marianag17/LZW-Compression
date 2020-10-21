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

namespace ApiLZW.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LZWController : ApiControllerAttribute
    {
        [HttpPost("compress/{name}")]
        public async Task<FileStreamResult> Post(string name, [FromForm] IFormFile file)
        {
            string textoArchivo = "";
            string nombreArchivoOriginal = file.FileName;
            string nombreArchivoCompreso = name;


            

                using (var stream = new StreamReader(file.OpenReadStream()))
                {

                    textoArchivo += stream.ReadToEnd();
                }

                CompresionLzw compresor = new CompresionLzw(textoArchivo);
                compresor.setNombreOriginalArchivo(nombreArchivoOriginal);
                compresor.setNombreArchivoNuevo(nombreArchivoCompreso);
                compresor.Comprimir();
                var path = compresor.pathDirectorioCompresiones + nombreArchivoCompreso + ".lzw";
                
            
                var streamCompress = File.OpenRead(path);
                return  new  FileStreamResult(streamCompress, "application/octet-stream");
           
           

           
         

        }
    }
}
