using Newtonsoft.Json;
using ProyectoFinal.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Conexion
{
    public class UsuarioConexion
    {
        //Objeto para realizar conexiones y peticiones HTTP a la api
        private HttpClient cliente;

        //Constructor que configura la conexion, se setea la uri de la api en render.com, y se establece el tipo de respuesta json
        public UsuarioConexion()
        {
            cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://saferide-api-mocz.onrender.com/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //Metodo que hace una peticion a la api para verificar si los datos son correctos y retorna un statusCode http
        public async Task<HttpResponseMessage> Login(string email, string password)
        {
            //Se crea un objeto para serializarlo en json
            //Se setean los parametros Email y Password
            var peticionLogin = new
            {
                Email = email,
                Password = password
            };

            //Se parsea el objeto a Json, para enviarlo en el body de la peticion a la api
            var json = JsonConvert.SerializeObject(peticionLogin);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Envia el json al endpoint(usuarios/login) de tipo POST del api y espera la respuesta de esta ultima
            var response = await cliente.PostAsync("usuarios/login", data);
            //Se obtiene la respuesta y se retorna: OK si es correcto, BadRequest si hubo un error
            return response;
        }

        //Metodo que hace una peticion a la api para registra un usuario en la base de datos retorna un statusCode http
        public async Task<HttpResponseMessage> Register(UsuarioModel usuario)
        {
            //Se setea el id rol a 1
            //1=usuario 2=agente 3=administrador
            usuario.id_rol = 1;

            //Se parsea el objeto a Json, para enviarlo en el body de la peticion a la api
            var json = JsonConvert.SerializeObject(usuario);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Envia el json al endpoint(usuarios) del api y espera la respuesta de esta ultima
            var response = await cliente.PostAsync("usuarios", data);
            //Se obtiene la respuesta y se retorna: OK si es correcto, BadRequest si hubo un error
            return response;
        }

        //Metodo que hace una peticion a la api para actualizar un usuario en la base de datos retorna un statusCode http
        public async Task<HttpResponseMessage> Actualizar(UsuarioModel usuario)
        {
            //Se parsea el objeto a Json, para enviarlo en el body de la peticion a la api
            var json = JsonConvert.SerializeObject(usuario);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Envia el json al endpoint(usuarios) del api y espera la respuesta de esta ultima
            var response = await cliente.PutAsync("usuarios", data);
            //Se obtiene la respuesta y se retorna: OK si es correcto, BadRequest si hubo un error
            return response;
        }

        //Metodo que hace una peticion a la api y retorna la lista de usuarios totales
        public async Task<HttpResponseMessage> ObtenerLista()
        {
            var response = await cliente.GetAsync("usuarios");
            //Se obtiene la respuesta y se retorna: OK si es correcto, BadRequest si hubo un error
            return response;
        }

        //Metodo que verifica si el id o el email de un usuario a crear ya estan utilizados
        public async Task<HttpResponseMessage> Verificar(string id, string email)
        {
            var response = await cliente.GetAsync($"usuarios/verificar/{id}/{email}");
            //Se obtiene la respuesta y se retorna: OK si no estan en uso, BadRequest si estan ocupadas o hubo un error
            return response;
        }

        //Metodo que obtiene un usuario por el id
        public async Task<HttpResponseMessage> ObtenerPorID(string id)
        {
            var response = await cliente.GetAsync($"usuarios/obtener/{id}");
            //Se obtiene la respuesta y se retorna: OK si existe el usuario, BadRequest del caso contrario
            return response;
        }

        //Metodo que elimina un usuario por el id
        public async Task<HttpResponseMessage> Eliminar(string id)
        {
            var response = await cliente.DeleteAsync($"usuarios/{id}");
            //Se obtiene la respuesta y se retorna: OK si se borro el usuario, BadRequest del caso contrario
            return response;
        }
    }
}