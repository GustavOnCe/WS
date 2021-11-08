using System;
using System.Diagnostics;
//Espacio de nombres para manipulación de flujo de datos y archivos
using System.IO;
//Espacio de nombres para la obtencion del usuario de Windows en la maquina Cliente
using System.Security.Principal;


namespace WS
{
    /// <summary>
    /// Clase que permite el registro de Excepciones en archivos o Registro de Eventos del Sistema.
    /// </summary>
    public static class clExcepciones
    {

        #region Métodos públicos

        /*/// <summary>
        /// Escribe una excepción dentro de un archivo XML de Errores.
        /// </summary>
        /// <param name="rutaArchivo">Ruta Relativa del Archivo</param>
        /// <param name="e">Excepción a reagistrar</param>
        /// <returns>Confirmación de registro en archivo</returns>
        public static bool RegistraExcepcionXML(string rutaArchivo, Exception e)
        {
            //Declarando variable para almacenar ruta fisica
            string rutaFisica = HttpContext.Current.Server.MapPath(rutaArchivo);

            //Si existe el archivo de excepciones
            if (verificaExistenciaArchivo(rutaFisica))
            {
                //Registrando excepción en archivo correspondiente,
                //Si no se registró correctamente
                if (!escribeXml(rutaFisica, e))
                    //Devolviendo error
                    return false;
            }

            //Devolviendo valor de éxito
            return true;
        }*/

        /// <summary>
        /// Metodo encargado de escribir sobre el EventLog de registros del servidor
        /// </summary>
        /// <param name="entradaEventos">Nombre del Registro de eventos a utilizar para la escritura de la excepción</param>
        /// <param name="e">Excepción a registrar</param>
        public static bool RegistraExcepcionEventLog(string entradaEventos, Exception e)
        {
            //Verificando existencia de la entrada de registro,
            //Si no existe
            if (!EventLog.SourceExists(entradaEventos))
            {
                //Se intentará crear la entrada de Registro
                try
                {
                    //Creando Origen de Excepción
                    EventSourceCreationData origenEvento = new EventSourceCreationData(entradaEventos, entradaEventos);

                    //Creando origen de Eventos
                    EventLog.CreateEventSource(origenEvento);
                }
                //En caso de error
                catch (Exception)
                {
                    //Devolviendo error en creación de Entrada de registro
                    return false;
                }
            }

            //Tratando de escribir excepción
            try
            {
                //Declarando variable para almacenar el mensaje a registrar
                string excepcion = "";
                //Declarando variable separador de contenido
                string separador = "\n--------------------------------------\n";

                //Generando mensaje a registrar (Detalle de excepción)             
                excepcion = e.Message /*Mensaje*/ + separador +
                            e.TargetSite.ToString() /*Método Origen*/ + separador +
                            e.StackTrace /*Pila de Llamadas*/ + separador +
                            DateTime.Now.ToString() /*Fecha */ + separador +
                            WindowsIdentity.GetCurrent().Name;

                //EventLog.WriteEvent(entradaEventos,
                EventLog.WriteEntry(entradaEventos, excepcion, EventLogEntryType.Error);
            }
            //En caso de error
            catch (Exception)
            {
                //Devolviendo error
                return false;
            }

            //Devolviendo código de éxito
            return true;
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Metodo encargado de llamar al metodo para la conversion del error a Xml
        /// </summary>
        /// <param name="rutaArchivoXML">Ruta física del archivo xml</param>
        /// <param name="e">Excepción que será escrita</param>
        private static bool escribeXml(string rutaArchivoXML, Exception e)
        {
            //Declarando arreglo de string
            string[] excepcion = new string[5];

            //Llenando arreglo con contenido de la excepcion  
            excepcion[0] = e.Message;               //Mensaje
            excepcion[1] = e.TargetSite.ToString(); //Metodo que provocó la excepcion
            excepcion[2] = e.StackTrace;            //Pila de llamadas      
            excepcion[3] = DateTime.Now.ToString(); //Fecha en que ocurrió la excepcion
            excepcion[4] = WindowsIdentity.GetCurrent().Name;      ////Usuario de Windows en la maquina Cliente

            //Si no fue añadida correctamente la excepción
            if (!clXML.AñadeRegistroXML(rutaArchivoXML, "Excepciones", excepcion))
                //Devolviendo error
                return false;

            //Devolviendo código de éxito
            return true;

        }

        /// <summary>
        /// Metodo que verifica la existencia del archivo de control de errores
        /// </summary>
        /// <param name="rutaArchivo">Ruta Física del archivo</param>
        private static bool verificaExistenciaArchivo(string rutaArchivo)
        {
            //Si no existe el archivo
            if (!File.Exists(rutaArchivo))
            {
                //Creando directorios requeridos
                if (creaDirectoriosRuta(rutaArchivo))
                {
                    //Definimos los campos para el archivo XML
                    string[] columnas = { "Mensaje", "Metodo", "Pila", "Fecha", "Usuario" };

                    //Creamos el archivo XML, Si so es creado correctamente
                    if (!clXML.CreaArchivoXML(rutaArchivo, "Excepciones", columnas))
                        //Devolviendo error
                        return false;
                }
                //De lo contrario
                else
                    //Devolvolviendo error
                    return false;
            }

            //Devolviendo valor correcto
            return true;
        }

        /// <summary>
        /// Método que crea las carpetas necesarias para almacenar el archivo de excepciones
        /// </summary>
        /// <param name="rutaArchivo">Ruta Física del archivo</param>
        /// <returns>Confirmación de creación</returns>
        private static bool creaDirectoriosRuta(string rutaArchivo)
        {
            //Declarando e inicializando ruta a localizar
            string rutaDirectorio = "";

            //Separando elementos de la ruta
            string[] elementos = rutaArchivo.Split('\\');

            //Recorriendo array de directorios a crear
            for (int x = 0; x < elementos.Length - 1; x++)
            {
                //Añadiendo nombre de directorio a crear
                rutaDirectorio = rutaDirectorio + "\\" + elementos[x];
                //Verificando si existe el directorio
                //Si no existe
                if (!Directory.Exists(rutaDirectorio.Substring(1)))
                {
                    //Creando directorio
                    try
                    {
                        Directory.CreateDirectory(rutaDirectorio.Substring(1));
                    }
                    //En caso de error
                    catch (Exception)
                    {
                        //Devolviendo error
                        return false;
                    }
                }
            }
            //Devolviendo codigo de éxito
            return true;
        }
        #endregion
    }
}
