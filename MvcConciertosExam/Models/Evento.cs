using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcConciertosExam.Models {

    public class Evento {


        public int IdEvento { get; set; }


        public string Nombre { get; set; }


        public string Artista { get; set; }


        public int IdCategoria { get; set; }


        public string Imagen { get; set; }

    }
}
