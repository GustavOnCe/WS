using System;
using System.Data;
using System.Linq;
//Espacio de nombres para manipulación de archivos XML
using System.Xml.Linq;
//Espacio de nombres de flujos de datos y archivos
using System.IO;
using System.Collections.Generic;


namespace WS
{
    /// <summary>
    /// Clase que permite la contrucción de archivos XML.
    /// </summary>
    public class clXML
    {
        /// <summary>
        /// 
        /// </summary>
        public clXML()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        /// <summary>
        /// Método que realiza la creación de un archivo XML a partir de un DataSet
        /// </summary>
        /// <param name="nombre_archivo">Nombre del Archivo XML utilizado</param>
        /// <param name="nombre_tabla">Nombre de la tabla con el esquema del archivo</param>
        /// <param name="columnas">Arreglo de Columnas que contendrá la Tabla</param>
        /// <returns></returns>
        public static bool CreaArchivoXML(string nombre_archivo, string nombre_tabla, string[] columnas)
        {
            //Instanciando un objeto DataTable nuevo
            using (DataTable Mit = new DataTable(nombre_tabla))
            {
                //Intentando crear el archivo XML y sus componentes
                try
                {
                    //Realizando el esquema de la tabla
                    foreach (string elemento in columnas)
                    {
                        //Añadiendo la columana a la Tabla
                        Mit.Columns.Add(elemento);
                    }
                    //Escribiendo el DataTable creado en el archivo XML
                    Mit.WriteXml(nombre_archivo, XmlWriteMode.WriteSchema);

                    //Devolviendo el objeto DataTable creado
                    return true;

                }
                //En caso de una excepción
                catch (Exception)
                {
                    //Devolviendo error
                    return false;
                }
            }
        }

        /// <summary>
        /// Método que realiza la inserción de un nuevo registro en una tabla de archivo XML
        /// </summary>
        /// <param name="rutaArchivo">Ruta física del archivo XML</param>
        /// <param name="nombreTabla">Nombre de la tabla</param>
        /// <param name="parametros">Arreglo de valores que se insertarán como nuevo registro</param>
        /// <returns></returns>
        public static bool AñadeRegistroXML(string rutaArchivo, string nombreTabla, object[] parametros)
        {
            //Instanciando un nuevo DataTable para almacenar contenido del archivo
            using (DataTable Mit = CargaArchivoXML(rutaArchivo, nombreTabla))
            {
                //Intentando escribir registro de error en archivo XML
                try
                {
                    //Añadiendo una nueva fila a la tabla
                    Mit.Rows.Add(parametros);

                    //Reescribiendo el archivo XML con el nuevo contenido del DataTable
                    Mit.WriteXml(rutaArchivo, XmlWriteMode.WriteSchema);

                    //Devolviendo código de exito
                    return true;
                }
                //Si no fue posible la escritura
                catch (Exception)
                {
                    //Retornando valor de error
                    return false;
                }
            }
        }

        /// <summary>
        /// Método que carga una Tabla perteneciente a un archivo XML en un DataTable
        /// </summary>
        /// <param name="nombre_archivo">Nombre del Archivo XML</param>
        /// <param name="nombre_tabla">Nombre de la Tabla a cargar</param>
        /// <returns></returns>
        public static DataTable CargaArchivoXML(string nombre_archivo, string nombre_tabla)
        {
            //Obteniendo la ruta física del archivo
            string ruta = nombre_archivo;

            //Si el archivo existe
            if (File.Exists(ruta))
            {
                //Instanciando un nuevo DataTable para almacenar contenido del archivo
                using (DataTable Mit = new DataTable(nombre_tabla))
                {
                    //Obteniendo el esquema de definician de tabla
                    Mit.ReadXmlSchema(ruta);
                    //Cargando el contenido del archivo XML
                    Mit.ReadXml(ruta);

                    //Devolviendo el DataTable modificado
                    return Mit;
                }
            }
            //De lo contrario
            else
                //Retornando valor null
                return null;
        }

        /// <summary>
        /// Método que obtiene un elemento de un XDocument dado su Xname.
        /// (Se devolverá valor NULL si no se encuentra)
        /// </summary>
        /// <param name="nombreElemento">XName del elemento solicitado (Prefijo NameSpace + Nombre Elemento)</param>
        /// <param name="documento">Documento XML donde se realizará la búsqueda</param>
        /// <returns></returns>
        public static XElement ObtieneElementoDocumento(XName nombreElemento, XDocument documento)
        {
            //Declarando objeto de retorno
            XElement elemento = null;

            //Validando que el documento no se encuentre vacío
            if (documento != null)
                //Aplicando recursividad sobre los elementos del nodo raiz
                elemento = obtieneElemento(documento.Root, nombreElemento);

            //Devolviendo el elemento encontrado
            return elemento;
        }

        /// <summary>
        /// Obtiene el subelemento solicitado de un elemento padre, si existe dicho subelemento
        /// </summary>
        /// <param name="elementoPadre">Elemento padre</param>
        /// <param name="nombreElemento">Nombre del subelemento a localizar</param>
        /// <returns></returns>
        private static XElement obtieneElemento(XElement elementoPadre, XName nombreElemento)
        {
            //Declarando elemento de retorno
            XElement elementoRetorno = null;

            //Si el elemento actual no es el solicitado
            if (elementoPadre.Name != nombreElemento)
            {
                //Si el elemento padre tiene subelementos
                if (elementoPadre.HasElements)
                {
                    //Recorriendo los subelementos del nodo actual
                    foreach (XElement elementoHijo in elementoPadre.Elements())
                    {
                        //Aplicando recursividada para localizar el elemento solicitado
                        elementoRetorno = obtieneElemento(elementoHijo, nombreElemento);

                        //Si el elemento existe
                        if (elementoRetorno != null)
                            break;
                    }
                }
            }
            //Si es el elemento solicitado
            else
                //Asignando elemento padre como solicitado
                elementoRetorno = elementoPadre;

            //Devolviendo elemento encontrado
            return elementoRetorno;
        }

        /*
        /// <summary>
        /// Método que obtiene un atributo de un XElement dado su Xname
        /// </summary>
        ///<param name="nombreAtributo">Nombre del atributo a localizar</param>
        ///<param name="elemento">Elemento donde se realizará la búsqueda</param>
        /// <returns></returns>
        public static XAttribute ObtieneAtributoElemento(string nombreAtributo, XElement elemento)
        {
            //Declarando objeto de retorno
            XAttribute atributo = null;

            //Validando que el elemento no se encuentre vacío
            if (elemento != null)
            {
                //Validando que el elemento contenga atributos
                if (elemento.Attributes().Count() > 0)
                {
                    try
                    {
                        //Localizando el atributo solicitado
                        atributo = elemento.Attribute(nombreAtributo);
                    }
                    catch (Exception ex)
                    {
                        //Registrando excepción
                        clExcepciones.RegistraExcepcionXML("~/Errores/ReporteErrores.xml", ex);
                    }
                }
            }

            //Devolviendo el elemento encontrado
            return atributo;
        }
        */
        /// <summary>
        /// Método que obtiene el conjunto de atributos de un XElement con el mismo Xname
        /// </summary>
        ///<param name="nombreAtributo">Nombre del atributo a localizar</param>
        ///<param name="elementoBase">Elemento donde se realizará la búsqueda</param>
        /// <returns></returns>
        public static List<XAttribute> ObtieneAtributosElemento(XName nombreAtributo, XElement elementoBase)
        {
            //Declarando objetos requeridos
            IEnumerable<XElement> elementos = null;
            IEnumerable<XAttribute> atributosElemento = null;
            List<XAttribute> atributos = new List<XAttribute>();

            //Validando que el elemento case no se encuentre vacío
            if (elementoBase != null)
            {
                //Si el elemento contiene subelementos
                if (elementoBase.HasElements)
                {
                    //Obteniendo el conjunto de subelementos existentes
                    elementos = elementoBase.Elements();

                    //Para cada subelemento
                    foreach (XElement e in elementos)
                    {
                        //Obteniendo los atributos del elemento
                        atributosElemento = ObtieneAtributosElemento(nombreAtributo, e);

                        //Si existen atributos que añadir
                        if (atributosElemento != null)
                            //Añadiendo los atributos de este nodo
                            atributos.AddRange(atributosElemento);
                    }
                }

                //Obteniendo los taributos del elemento actual
                atributosElemento = elementoBase.Attributes(nombreAtributo);

                //Si existen atributos que añadir
                if (atributosElemento.Count() != 0)
                    //Añadiendo los atributos de este nodo
                    atributos.AddRange(elementoBase.Attributes(nombreAtributo));

            }

            //Devolvinedo atributos encontrados
            return atributos;
        }
        /*
        /// <summary>
        /// Método que obtiene el valor de un atributo de un XElement dado su Xname
        /// </summary>
        ///<param name="nombreAtributo">Nombre del atributo a localizar</param>
        ///<param name="elemento">Elemento donde se realizará la búsqueda</param>
        /// <returns></returns>
        public static string ObtieneValorAtributoElemento(string nombreAtributo, XElement elemento)
        {
            //Declarando objeto de retorno
            XAttribute atributo = null;

            //Validando que el elemento no se encuentre vacío
            if (elemento != null)
            {
                //Validando que el elemento contenga atributos
                if (elemento.Attributes().Count() > 0)
                {
                    try
                    {
                        //Localizando el atributo solicitado
                        atributo = elemento.Attribute(nombreAtributo);

                        //Devolvinedo el valor del atributo
                        return atributo.Value;
                    }
                    catch (Exception ex)
                    {
                        //Registrando excepción
                        clExcepciones.RegistraExcepcionXML("~/Errores/ReporteErrores.xml", ex);
                    }
                }
            }

            //Devolviendo cadena vacía
            return "";
        }*/

    }
}
