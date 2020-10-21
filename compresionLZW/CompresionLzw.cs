using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace compresionLZW
{
    public class CompresionLzw
    {
        private string TextoArchivo;
      
        private Dictionary<string, int> TablaCaracteres = new Dictionary<string, int>();
        private Dictionary<int, string> TablaCaracteresInversa = new Dictionary<int, string>();
        private ArrayList TablaEscribir = new ArrayList();
        int contadorCaracteres = 2;
        public double Factor;
        public double Razon;
 
        public string NombreArchivoNuevo = "";
        public string NombreOriginalArchivo = "";
        public string ubicacionArchivo = "";

        public CompresionLzw(string contenido)
        {
            TextoArchivo = contenido;
      
        }

        // Proceso de compresión
        public void Comprimir()
        {
            ObtenerCaracteres();
            List<int> Compreso = new List<int>();
            string actual = "", siguiente = "";
            int compreso = 0;
            Compreso.Add(1);
            for (int i = 1; i < TextoArchivo.Length - 1; i++)
            {
                actual = (TextoArchivo[i]).ToString();
                for (int j = i + 1; j < TextoArchivo.Length; j++)
                {
                    siguiente = (TextoArchivo[j]).ToString();
                    if (TablaCaracteres.ContainsKey(actual + siguiente))
                    {
                        i++;
                        actual += siguiente;
                        siguiente = (TextoArchivo[j]).ToString();
                    }
                    else
                    {
                        compreso = TablaCaracteres[actual];
                        TablaCaracteres.Add((actual + siguiente), contadorCaracteres);
                        contadorCaracteres++;
                        j = TextoArchivo.Length;
                    }
                }
                Compreso.Add(TablaCaracteres[actual]);
            }
            string TextoCompreso = "";
            int NumeroActual = 0;
            for (int i = 0; i < Compreso.Count; i++)
            {
                NumeroActual = Compreso[i];
                TextoCompreso += ((char)NumeroActual).ToString();
            }
            String ContenidoTabla = getContenidoTabla();
            double cantidadTemp = (NombreOriginalArchivo.Length + 1 + ContenidoTabla.Length + 2 + TextoCompreso.Length);
            double originalTemp = TextoArchivo.Length;
            Factor = originalTemp / cantidadTemp;
            String temp;
            if ((Factor.ToString()).Length > 7)
            {
                temp = (Factor.ToString()).Substring(0, 7);
            }
            else
            {
                temp = (Factor.ToString());
            }
            Factor = double.Parse(temp);
            Razon = cantidadTemp / originalTemp;
            if (Razon.ToString().Length > 7)
            {
                temp = Razon.ToString().Substring(0, 7);
            }
            else
            {
                temp = (Razon.ToString());
            }
            Razon = double.Parse(temp);
            escribirArchivoCompreso(NombreArchivoNuevo, NombreOriginalArchivo + "|" + ContenidoTabla + "||" + TextoCompreso);

        }

        private String getContenidoTabla()
        {
            String contenido = "";
            for (int i = 0; i < TablaEscribir.Count; i++)
            {
                contenido += TablaEscribir[i];
            }
            return contenido;
        }

        private bool escribirArchivoCompreso(String nombreArchivo, String contenido)
        {
       
            string workingDirectory = Environment.CurrentDirectory;
            string pathFolderActual = Directory.GetParent(workingDirectory).FullName;
                    
            string pathDirectorioCompresiones = pathFolderActual + "\\CompresionesEstructuras\\";
           
           
            
            if (!Directory.Exists(pathDirectorioCompresiones))
                Directory.CreateDirectory(pathDirectorioCompresiones);

            using (FileStream fs = File.Create(pathDirectorioCompresiones + nombreArchivo + ".lzw")) {
                byte[] byteArray = new UTF8Encoding(true).GetBytes(contenido);
                fs.Write(byteArray , 0, byteArray.Length);
                ubicacionArchivo = pathDirectorioCompresiones + nombreArchivo + ".lzw";
            }
           
           
            return true;
        }

        private void ObtenerCaracteres()
        {
            string caracter = ((char)TextoArchivo[0]).ToString();
            TablaCaracteres.Add(caracter, contadorCaracteres);
            TablaEscribir.Add(caracter + "01");
            for (int i = 1; i < TextoArchivo.Length ; i++)
            {
                caracter = ((char)TextoArchivo[i]).ToString();
                if (!TablaCaracteres.ContainsKey(caracter))
                {
                    TablaCaracteres.Add(caracter, contadorCaracteres);
                    if (contadorCaracteres <= 9)
                    {
                        TablaEscribir.Add(caracter + "0" + contadorCaracteres.ToString());
                    }
                    else
                    {
                        TablaEscribir.Add(caracter + contadorCaracteres.ToString());
                    }
                    contadorCaracteres++;
                }
            }
        }

        // Proceso de descompresión
        public void Descomprimir() 
        {
            separarContenido();
            string CadenaDescompresa = "";
            int asciiAnterior = (int)TextoArchivo[0];
            string anterior = "";
            int asciiActual = 0;
            string actual = "";

            if (TablaCaracteresInversa.ContainsKey(asciiActual))
            {
                CadenaDescompresa = TablaCaracteresInversa[asciiActual];
            }
            

            for (int i = 1; i<TextoArchivo.Length; i++)
            {
                asciiActual = (int) TextoArchivo[i];
                actual = (TablaCaracteresInversa[asciiActual]).ToString();
                actual = actual[0].ToString();
                if (!TablaCaracteres.ContainsKey((anterior + actual)))
                {
                    contadorCaracteres++;
                    TablaCaracteres.Add((anterior + actual), contadorCaracteres);
                    TablaCaracteresInversa.Add(contadorCaracteres, (anterior + actual));
                }
                CadenaDescompresa += TablaCaracteresInversa[asciiActual];
                asciiAnterior = asciiActual;
                anterior = TablaCaracteresInversa[asciiAnterior];
            }
            escribirArchivoDescompreso(getNombreOriginalArchivo(), CadenaDescompresa);
    }

    public void escribirArchivoDescompreso(String nombreArchivo, String contenido) 
    {
        string workingDirectory = Environment.CurrentDirectory;
        string pathFolderActual = Directory.GetParent(workingDirectory).FullName;
        string pathDirectorioDescompresiones = pathFolderActual + "\\DescompresionesEstructuras\\";

            if (!Directory.Exists(pathDirectorioDescompresiones)) {
                Directory.CreateDirectory(pathDirectorioDescompresiones);
            }
        

        using (FileStream fs = File.Create(pathDirectorioDescompresiones + nombreArchivo))
        {
            byte[] byteArray = new UTF8Encoding(true).GetBytes(contenido);
            fs.Write(byteArray, 0, byteArray.Length);
            ubicacionArchivo = pathDirectorioDescompresiones + nombreArchivo;
        }
          
    }

    private void separarContenido()
    {
        string nombre = "";
        for (int i = 0; i < TextoArchivo.Length; i++)
        {
            if (TextoArchivo[i] != '|')
            {
                nombre += (TextoArchivo[i]).ToString();
            }
            else
            {
                TextoArchivo = TextoArchivo.Substring(i + 1);
                i = TextoArchivo.Length;
            }
        }
        setNombreOriginalArchivo(nombre);
        string tablaCaracteres = "";
        for (int i = 0; i < TextoArchivo.Length; i++)
        {
            if (TextoArchivo[i].Equals('|') && TextoArchivo[(i + 1)].Equals('|'))
            {
                TextoArchivo = TextoArchivo.Substring(i + 2);
                i = TextoArchivo.Length;
            }
            else
            {
                tablaCaracteres += TextoArchivo[i];
            }
        }

        string caracterAparicion = "";
        string caracter = "";
        while (tablaCaracteres.Length > 0)
        {
            caracter = tablaCaracteres[0].ToString();
            tablaCaracteres = tablaCaracteres.Substring(1);
            for (int i = 0; i < 2; i++)
            {
                caracterAparicion += ((char)tablaCaracteres[i]).ToString();
            }
            tablaCaracteres = tablaCaracteres.Substring(2);
            TablaCaracteres.Add(caracter, int.Parse(caracterAparicion));
            TablaCaracteresInversa.Add(int.Parse(caracterAparicion), caracter);
            caracter = "";
            caracterAparicion = "";
        }
        contadorCaracteres = TablaCaracteresInversa.Count;
    }

// Extras
    public void setNombreOriginalArchivo(String nombre)
    {
        NombreOriginalArchivo = nombre;
    }

    public String getNombreOriginalArchivo()
    {
        return NombreOriginalArchivo;
    }

    public void setNombreArchivoNuevo(String nombre)
    {
        NombreArchivoNuevo = nombre;
    }

    public String getNombreArchivoNuevo()
    {
        return NombreArchivoNuevo;
    }
}

}
