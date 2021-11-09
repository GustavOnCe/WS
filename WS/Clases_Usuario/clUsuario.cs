using System;
using System.Data;

namespace PruebaDeClases.Clases
{
    class clUsuario
    {
        #region Propiedades
        //define el procedimiento almacenado encargado re realizar las acciones en la base de datos
        public static string nombreProcedimientoAlmacenado = "dbo.sp_usuario_tu";
        // propiedades
        private int _id_tipo;
        private int _id_usuario_datos;
        private string _usuario;
        private string _contraseña;
        private string _telefono;
        private string _nombre;
        private string _apellido_paterno;
        private string _apellido_materno;
        private string _correo;
        private int _id_forma_pago;
        private bool _habilitado;
        private int _id_usuario;

        // getters 
        public int id_tipo { get { return _id_tipo; } }
        public int id_usuario_datos { get { return _id_usuario_datos; } }
        public string usuario { get { return _usuario; } }
        public string contraseña { get { return _contraseña; } }
        public string telefono { get { return _telefono; } }
        public string nombre { get { return _nombre; } }
        public string apellido_paterno { get { return _apellido_paterno; } }
        public string apellido_materno { get { return _apellido_materno; } }
        public string correo { get { return _correo; } }
        public int id_forma_pago { get { return _id_forma_pago; } }
        public bool habilitado { get { return _habilitado; } }
        public int id_usuario { get { return _id_usuario; } }
        #endregion

        #region Constructor
        // clUsuario es el constructor de clase, determina los parametros a utilizar en la cosnulta
        public clUsuario()
        {
            _id_tipo = 0;
            _id_usuario_datos = 0;
            _usuario = "";
            _contraseña = "";
            _telefono = "";
            _nombre = "";
            _apellido_paterno = "";
            _apellido_materno = "";
            _correo = "";
            _id_forma_pago = 0;
            _habilitado = true;
            _id_usuario = 0;    
        }
        // clUsuario espera un parametro entero y sobrecarga el metodo clUsuario
        public clUsuario(int usuario)
        {
            // inicializar los paramentros del arreglo
            object[] param = { 3, 0, "", "", "", "", "", "", "", 0, true, 0 };

            // realizar la consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.ejecutaProcAlmacenadoDataSet(nombreProcedimientoAlmacenado, param))
            {

              
                
                // validar que existan los datos
                //if (Herramientas.ValidarOrigenDatos) {
                foreach (DataRow r in DS.Tables["Table"].Rows)
                {
                    _id_usuario_datos = Convert.ToInt32(r["id_usuario_tu"]);
                    _usuario = r["usuario_tu"].ToString();
                    _contraseña = r["contraseña_tu"].ToString();
                    _telefono = r["telefono_tu"].ToString();
                    _nombre = r["nombre_tu"].ToString();
                    _apellido_paterno = r["apellido_paterno_tu"].ToString();
                    _apellido_materno = r["apellido_materno_tu"].ToString();
                    _correo = r["correo_tu"].ToString();
                    _id_forma_pago = Convert.ToInt32(r["id_forma_pago_tu"]);
                    _habilitado = Convert.ToBoolean(r["habilitado_tu"]);
                    _id_usuario = Convert.ToInt32(r["id_usuario_tu"]);
                } 
                //}
            }
        }
        #endregion
        // En esta sección s e encuentran todos los metodos publcos
        #region Metodos Publicos
        public clRetornoOperacionBD EditaRegistroUsuario(string nombreUsuario, string Contraseña, string telefono,
                                                         string nombre, string apellido_paterno, string apellido_materno,
                                                         string correo, int formaDePago, bool habilitado,
                                                         int idUsuarioMod)
        {
            // inicializando el arreglo de parametros
            object[] param = { 2, this.id_usuario_datos, nombreUsuario, Contraseña, telefono, nombre, apellido_paterno, apellido_materno, correo, formaDePago, habilitado, idUsuarioMod };
            // realizando actualización
            return CapaDatos.m_capaDeDatos.ejecutaProcAlmacenadoObjeto(nombreProcedimientoAlmacenado, param);
        }
        #endregion
        // en esta region se encuentran los metodos estaticos
        #region Metodos Estaticos
        public static clRetornoOperacionBD InsertarUsuario(string nombreUsuario, string Contraseña, string telefono,
                                                         string nombre, string apellido_paterno, string apellido_materno,
                                                         string correo,  int formaDePago, bool habilitado, int idUsuarioMod)
        {
            //region de la capa de datos
            // inicializando el arreglo de parametros
            object[] param = { 1, 0, nombreUsuario, Contraseña, telefono, nombre, apellido_paterno, apellido_materno, correo,  formaDePago, habilitado, idUsuarioMod };
            // realiazamos la insercción del registro
            return CapaDatos.m_capaDeDatos.ejecutaProcAlmacenadoObjeto(nombreProcedimientoAlmacenado, param);
        }
        #endregion
    }
}
