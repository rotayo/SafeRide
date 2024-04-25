using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProyectoFinal.Model
{
    public class UsuarioModel: INotifyPropertyChanged
    {
        private string _id;
        private string _nombre;
        private string _apellido;
        private string _telefono;
        private string _email;
        private string _contrasena;
        private string _idAutomovil;
        private int _idRol;
        private bool _estado;


        public string id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged();
            }
        }

        public string apellido
        {
            get => _apellido;
            set
            {
                _apellido = value;
                OnPropertyChanged();
            }
        }

        public string telefono
        {
            get => _telefono;
            set
            {
                _telefono = value;
                OnPropertyChanged();
            }
        }

        public string email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string contrasena
        {
            get => _contrasena;
            set
            {
                _contrasena = value;
                OnPropertyChanged();
            }
        }

        public string id_automovil
        {
            get => _idAutomovil;
            set
            {
                _idAutomovil = value;
                OnPropertyChanged();
            }
        }

        public int id_rol
        {
            get => _idRol;
            set
            {
                _idRol = value;
                OnPropertyChanged();
            }
        }

        public bool estado
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
