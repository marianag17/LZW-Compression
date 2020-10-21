using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiLZW.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public DescriptorAPI getAPIDescriptor()
        {
            DescriptorAPI descriptor = new DescriptorAPI();
            descriptor.Compresion = new DescriptorRuta()
            {
                Ruta = "/api/compress/{name}",
                Descripcion = "En esta ruta se puede mandar como parametro el nombre del nuevo archivo compreso por LZW y un archivo de texto que se desea comprimir.",
                Metodo = "POST"
            };
            descriptor.Descompresion = new DescriptorRuta()
            {
                Ruta = "/api/decompress",
                Descripcion = "En esta ruta se manda un archivo .LZW para decompromir",
                Metodo = "POST"
            };

            descriptor.ReporteCompresiones = new DescriptorRuta()
            {
                Ruta = "/api/compressions",
                Descripcion = "En esta ruta se devuelve un JSON con informacion de las compresiones realizadas.",
                Metodo = "GET"
            };
       

            return descriptor;

        }
    }
}
