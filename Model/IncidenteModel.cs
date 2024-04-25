using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProyectoFinal.Model
{
    public class IncidenteModel : INotifyPropertyChanged
    {
        private int _id;
        private string _fecha;
        private string _hora;
        private string _ubicacion;
        private string _descripcion;
        private string _idReportador;
        private string _idAfectado;
        private string _idAgente;
        private string _estado;

        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string fecha
        {
            get => _fecha;
            set
            {
                string fechaReal = "";
                for (int i = 0; i < 10; i++)
                {
                    fechaReal += value[i];
                }
                _fecha = fechaReal;
                OnPropertyChanged();
            }
        }

        public string hora
        {
            get => _hora;
            set
            {
                _hora = value;
                OnPropertyChanged();
            }
        }

        public string ubicacion
        {
            get => _ubicacion;
            set
            {
                _ubicacion = value;
                OnPropertyChanged();
            }
        }

        public string descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged();
            }
        }

        public string id_reportador
        {
            get => _idReportador;
            set
            {
                _idReportador = value;
                OnPropertyChanged();
            }
        }

        public string id_afectado
        {
            get => _idAfectado;
            set
            {
                _idAfectado = value;
                OnPropertyChanged();
            }
        }

        public string id_agente
        {
            get => _idAgente;
            set
            {
                _idAgente = value;
                OnPropertyChanged();
            }
        }

        public string estado
        {
            get => _estado;
            set
            {
                _estado = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
