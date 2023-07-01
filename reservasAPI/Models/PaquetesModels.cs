using System.ComponentModel.DataAnnotations;

namespace reservasAPI.Models
{
    public class PaquetesModels
    {
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del paquete es obligatorio.")]
        public string? NomPaquete { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "La descripción del paquete es obligatorio.")]
        public string? Descripcion { get; set; }
        public double Precio { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
