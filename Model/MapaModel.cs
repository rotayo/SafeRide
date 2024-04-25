using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace ProyectoFinal.Model
{
    public class MapaModel
    {
        private static MapaModel instancia;
        
        //Se pone el nombre completo por conflicto usings
        public Xamarin.Forms.Maps.Map Mapa { get; set; }
        public Location Location { get; set; }

        private MapaModel() { 
        
        }

        public static MapaModel Instancia() {
            if(instancia == null) 
                instancia = new MapaModel();
            return instancia;
        }
    }
}
