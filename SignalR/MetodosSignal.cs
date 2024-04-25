using Acr.UserDialogs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SignalR
{
    public class MetodosSignal
    {
        private HubConnection conexionSignalR;

        public MetodosSignal()
        {
            conexionSignalR = new HubConnectionBuilder()
                .WithUrl($"https://saferide-api-mocz.onrender.com/SafeRideHub")
                .Build();
        }

        public void AbrirConexionSignalR(string metodo, Action funcion)
        {
            conexionSignalR.On($"{metodo}", async () => funcion());

            conexionSignalR.StartAsync().Wait();
        }

        public void RecibirMensajes(Action<string> funcion)
        {
            conexionSignalR.On<string>($"Mensaje", mensaje => funcion(mensaje));

            conexionSignalR.StartAsync().Wait();
        }

        public void EnviarMensaje(string mensaje)
        {
            conexionSignalR.StartAsync().Wait();

            conexionSignalR.InvokeAsync("SendMessage", mensaje);

            conexionSignalR.StopAsync();
        }

        public void CerrarConexion()
        {
            conexionSignalR.StopAsync();
        }
    }
}
