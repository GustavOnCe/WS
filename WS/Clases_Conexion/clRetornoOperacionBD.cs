using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace WS
{
    /// <summary>
    /// Permite la gestión del resultado de una operación de actualización (Inserción/Edición) en BD.
    /// </summary>
    public class clRetornoOperacionBD
    {
        #region Atributos

        /// <summary>
        /// Gestiona el valor que retorna la operacion
        /// </summary>
        private object _retorno;
        /// <summary>
        /// Objeto de retorno
        /// </summary>
        public object Retorno
        {
            get { return _retorno; }
        }

        private bool _operacionExitosa;
        /// <summary>
        /// Indica si la operacion realizada ha sido exitosa
        /// </summary>
        public bool OperacionExitosa
        {
            get { return _operacionExitosa; }
        }

        private int _idRegistro;
        /// <summary>
        /// Contiene el Id devuelto por la operación realizada en BD
        /// </summary>
        public int IdRegistro
        {
            get { return _idRegistro; }
        }
        private string _mensaje;
        /// <summary>
        /// Contiene el mensaje generado por la operación
        /// </summary>
        public string Mensaje
        {
            get { return _mensaje; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por default
        /// </summary>
        public clRetornoOperacionBD()
        {
            _retorno = null;
            _operacionExitosa = false;
            _idRegistro = -2;
            _mensaje = "";
        }
        /// <summary>
        /// Constructor que inicializa la instancia en razon al valor devuelto por la BD
        /// </summary>
        /// <param name="resultado"></param>
        public clRetornoOperacionBD(object resultado)
        {
            try
            {
                _retorno = resultado;
                _idRegistro = Convert.ToInt32(_retorno);
                estableceRetorno(_idRegistro);
            }
            catch (Exception e)
            {
                _mensaje = e.Message;
                estableceRetorno(-2);
            }

        }
        /// <summary>
        /// Constructor que inicializa al objeto como operacion no valida
        /// </summary>
        /// <param name="excepcion"></param>
        public clRetornoOperacionBD(string excepcion)
        {
            _mensaje = excepcion;
            _operacionExitosa = false;
            _retorno = null;
            _idRegistro = -2;
        }
        /// <summary>
        /// Constructor que inicializa al objeto con resultado especificado
        /// </summary>
        /// <param name="mensaje">Mensaje de resultado</param>
        /// <param name="valor">True para resultado correcto</param>
        public clRetornoOperacionBD(string mensaje, bool valor)
        {
            _mensaje = mensaje;
            _operacionExitosa = valor;
            _retorno = valor;
            _idRegistro = valor ? 1 : -2;
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Establece los valores del objeto en relacion al codigo asignado 
        /// </summary>
        /// <param name="id"></param>
        private void estableceRetorno(int id)
        {
            switch (id)
            {
                //Se genero un  error en la operacion de la BD
                case -2:
                    {
                        _operacionExitosa = false;
                        _mensaje = "Error de BD, error no clasificado.";
                        break;
                    }
                //Registro duplicado 
                case -1:
                    {
                        _operacionExitosa = false;
                        _mensaje = "El valor que intenta actualizar en la BD, ya existe.";
                        break;
                    }
                //Todos aquellos valores que no esten contemplados en el catalogo de errores
                default:
                    {
                        _operacionExitosa = true;
                        _mensaje = "El registro ha sido actualizado exitosamente";
                        break;
                    }
            }
        }

        #endregion
    }
}