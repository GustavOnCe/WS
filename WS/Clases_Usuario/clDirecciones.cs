using System;
using System.Data;

namespace WS.Clases_Usuario
{
    class clDirecciones
    {
        #region propiedades
        //define el nombre del procedimiento alamacenado encargado de realizar la acciones en base de datos
        public static string nombreProcediminetoAlmacenado = "dbo.sp_usuario_direcciones_tdu";

        // propiedades
        private int _id_direccion;
        private int _id_usuario_datos;
        private int _id_colonia;
        private string _calle;
        private string _numero_exterior;
        private string _numero_interior;
        private string _observaciones;
        private bool _habilitado;
        private int _id_usuario;


        public int id_direccion { get { return _id_direccion; } }
        public int id_usuario_datos { get { return _id_usuario_datos; } }
        public int id_colonia { get { return _id_colonia; } }
        public string calle { get { return _calle; } }
        public string numero_exterior { get { return _numero_exterior; } }
        public string numero_interior { get { return _numero_interior; } }
        public string observaciones { get { return _observaciones; } }
        public bool habilitado { get { return _habilitado; } }
        public int id_usuario { get { return _id_usuario; } }

        #endregion

        #region Constructor
        public clDirecciones()
        {
            _id_direccion = 0;
            _id_usuario_datos = 0;
            _id_colonia = 0;
            _calle = "";
            _numero_exterior = "";
            _numero_interior = "";
            _observaciones = "";
            _habilitado = true;
            _id_usuario = 0;
        }
        public clDirecciones(int direccion)
        {
            object[] param = { 3, 0, 0, 0, "", "", "", "", 1, 1 };

            using (DataSet DS = CapaDatos.m_capaDeDatos.ejecutaProcAlmacenadoDataSet(nombreProcediminetoAlmacenado, param))
            {
                foreach (DataRow r in DS.Tables["Table"].Rows)
                {
                    _id_direccion = Convert.ToInt32(r["id_direccion_tud"]);
                    _id_usuario_datos = Convert.ToInt32(r["id_usuario_datos_tud"]);
                    _id_colonia = Convert.ToInt32(r["id_colonia_tud"]);
                    _calle = r["calle_tud"].ToString();
                    _numero_exterior = r["numero_exterior_tud"].ToString();
                    _numero_interior = r["numero_interior_tud"].ToString();
                    _observaciones = r["observaciones_tud"].ToString();
                    _habilitado = Convert.ToBoolean(r["habilitado_tud"]);
                    _id_usuario = Convert.ToInt32(r["id_usuario_tud"]);
                }
            }
        }

        #endregion 
                    
        #region Metodos Publicos

        public clRetornoOperacionBD EditarRegistroDireccion(int idColonia,
                                                            string calle,
                                                            string numext,
                                                            string numint,
                                                            string obs,
                                                            bool habilitado,
                                                            int idUsuarioBit)
        {
            object[] param = { 2, this.id_usuario_datos, 0, idColonia, calle, numext, numint, obs, habilitado, idUsuarioBit };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.ejecutaProcAlmacenadoObjeto(nombreProcediminetoAlmacenado, param);

        }
       
        #endregion
          
        #region Metodos Estaticos
        public static clRetornoOperacionBD InsertarDireccion(int idUsuarioDatos,  
                                                             int idColonia,
                                                             string calle,
                                                             string numext,
                                                             string numint,
                                                             string obs,
                                                             bool habilitado,
                                                             int idUsuarioBit)
        {
            object[] param = { 2, 0, 0, idColonia, calle, numext, numint, obs, habilitado, idUsuarioBit };
            //Realizamos la inserción del registro
            return CapaDatos.m_capaDeDatos.ejecutaProcAlmacenadoObjeto(nombreProcediminetoAlmacenado, param);
        }
        #endregion
    }
}
