using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.usuario
{
    public class EditarAutoViewModel
    {
        private AutoConexion autoConexion { get; set; }
        public AutoModel Auto { get; set; }
        public Command Back {  get; set; }
        public Command Actualizar { get; set; }
        private INavigation navigation;

        public EditarAutoViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            Auto = SesionActualModel.Instancia().Auto;
            autoConexion = new AutoConexion();
            Back = new Command(() => navigation.PopAsync());
            Actualizar = new Command(async () => await ActualizarCommand());
        }

        private async Task ActualizarCommand()
        {
            if (string.IsNullOrEmpty(Auto.marca) ||
                string.IsNullOrEmpty(Auto.poliza) ||
                string.IsNullOrEmpty(Auto.modelo) ||
                string.IsNullOrEmpty(Auto.tipo) ||
                string.IsNullOrEmpty(Auto.anio)
                )
            {
                await UserDialogs.Instance.AlertAsync("Uno o más campos están vacios", "Campos Vacios", "Ok");
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                if (Auto.poliza == SesionActualModel.Instancia().Auto.poliza)
                {
                    await autoConexion.Actualizar(Auto);
                    await UserDialogs.Instance.AlertAsync("Datos Actualizados con Exito", "Confirmacion", "Ok");
                    SesionActualModel.Instancia().Auto = Auto;
                    navigation.InsertPageBefore(new MainPageUsuario(), navigation.NavigationStack[0]);
                    await navigation.PopToRootAsync();
                }
                else
                {
                    HttpResponseMessage respuesta = await autoConexion.Verificar("_", Auto.poliza);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        await autoConexion.Actualizar(Auto);
                        await UserDialogs.Instance.AlertAsync("Datos Actualizados con Exito", "Confirmacion", "Ok");
                        SesionActualModel.Instancia().Auto = Auto;
                        navigation.InsertPageBefore(new MainPageUsuario(), navigation.NavigationStack[0]);
                        await navigation.PopToRootAsync();
                    }
                    else
                    {

                        await UserDialogs.Instance.AlertAsync(await respuesta.Content.ReadAsStringAsync(), "Poliza no valida", "Ok");
                    }
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
