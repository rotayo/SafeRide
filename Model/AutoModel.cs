using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProyectoFinal.Model
{
    public class AutoModel : INotifyPropertyChanged
    {
        private string _id;
        private string _poliza;
        private string _marca;
        private string _modelo;
        private string _tipo;
        private string _anio;

        public string id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string poliza
        {
            get => _poliza;
            set
            {
                _poliza = value;
                OnPropertyChanged();
            }
        }

        public string marca
        {
            get => _marca;
            set
            {
                _marca = value;
                OnPropertyChanged();
            }
        }

        public string modelo
        {
            get => _modelo;
            set
            {
                _modelo = value;
                OnPropertyChanged();
            }
        }

        public string tipo
        {
            get => _tipo;
            set
            {
                _tipo = value;
                OnPropertyChanged();
            }
        }

        public string anio
        {
            get => _anio;
            set
            {
                _anio = value;
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
