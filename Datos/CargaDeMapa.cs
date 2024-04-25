using ProyectoFinal.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace ProyectoFinal.Datos
{
    public class CargaDeMapa
    {
        public async Task CargarMapaSesionActual()
        {
            try
            {
                var estatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (estatus != PermissionStatus.Granted)
                {
                    estatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (estatus != PermissionStatus.Granted)
                    {
                        // No se concedió el permiso
                        return;
                    }
                }
                //Se obtienen las coordenadas
                Location coordenadasActual = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default));
                Position posicionCentral = new Position(coordenadasActual.Latitude, coordenadasActual.Longitude);
                MapSpan areaDelMapa = new MapSpan(posicionCentral, 0.01, 0.01);
                Pin pin = new Pin() { Position = posicionCentral };

                //Se guardan en un objeto singleton
                MapaModel.Instancia().Location = coordenadasActual;
                MapaModel.Instancia().Mapa = new Xamarin.Forms.Maps.Map(areaDelMapa);
                MapaModel.Instancia().Mapa.Pins.Add(pin);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CargaMapaPorIdSesionActual(int id)
        {
            try
            {
                var coordenadas = SesionActualModel.Instancia().ObtenerIncidencia(id).ubicacion.Split(',');
                Position posicionCentral = new Position(double.Parse(coordenadas[0]), double.Parse(coordenadas[1]));
                MapSpan areaDelMapa = new MapSpan(posicionCentral, 0.01, 0.01);
                Pin pin = new Pin() { Position = posicionCentral };

                MapaModel.Instancia().Mapa = new Xamarin.Forms.Maps.Map(areaDelMapa);
                MapaModel.Instancia().Mapa.Pins.Add(pin);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+" "+ex.StackTrace);
            }
        }
    }
}
