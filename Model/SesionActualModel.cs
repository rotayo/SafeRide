using ProyectoFinal.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Model
{
    public class SesionActualModel : INotifyPropertyChanged
    {
        private static SesionActualModel instancia { get; set; }
        private List<MetodosSignal> ConexionesSignal { get; set; }
        private AutoModel _auto;
        private UsuarioModel _usuario;
        private List<UsuarioModel> _listaUsuarios;
        private List<IncidenteModel> _listaIncidentes;

        private SesionActualModel()
        {

        }

        public AutoModel Auto
        {
            get => _auto;
            set
            {
                _auto = value;
                OnPropertyChanged();
            }
        }
        public UsuarioModel Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged();
            }
        }
        public List<UsuarioModel> ListaUsuarios
        {
            get => _listaUsuarios;
            set
            {
                _listaUsuarios = value;
                OnPropertyChanged();
            }
        }
        public List<IncidenteModel> ListaIncidentes
        {
            get => _listaIncidentes;
            set
            {
                _listaIncidentes = value;
                OnPropertyChanged();
            }
        }

        public static SesionActualModel Instancia()
        {
            if (instancia == null) 
                instancia = new SesionActualModel();
            return instancia;
        }

        public int SeEncuentraEnProceso(int rol)
        {
            foreach (var incidente in ListaIncidentes)
            {
                if ( (rol == 1 && (incidente.estado == "Pendiente" || incidente.estado == "Asignado" || incidente.estado == "Procesando"))
                    || (rol == 2 && incidente.estado == "Procesando"))
                {
                    return incidente.id;
                }
            }
            return 0;
        }

        public UsuarioModel ObtenerUsuario(string id)
        {
            foreach (var usuario in ListaUsuarios)
            {
                if(usuario.id == id)
                    return usuario;
            }
            return null;
        }

        public IncidenteModel ObtenerIncidencia(int id)
        {
            foreach (var incidencia in ListaIncidentes)
            {
                if (incidencia.id == id)
                    return incidencia;
            }
            return null;
        }

        public void BorrarIncidente(int id)
        {
            foreach (var incidencia in ListaIncidentes)
            {
                if (incidencia.id == id)
                {
                    ListaIncidentes.Remove(incidencia);
                    break;
                }

            }
        }

        public MetodosSignal CrearConexionSignalR()
        {
            if (ConexionesSignal == null)
                ConexionesSignal = new List<MetodosSignal>();
            var conexion = new MetodosSignal();
            ConexionesSignal.Add(conexion);
            return conexion;
        }

        public void CerrarConexionesSignalR()
        {
            if(ConexionesSignal != null)
            {
                foreach (var conexion in ConexionesSignal)
                {
                    conexion.CerrarConexion();
                }
                ConexionesSignal = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
