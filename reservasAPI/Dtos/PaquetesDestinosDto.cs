using reservasAPI.Models;

namespace reservasAPI.Dtos
{
    public class PaquetesDestinosDto
    {
        public string? NomPaquete { get; set; }
        public string? Descripcion { get; set; }
        public string? Precio { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }

        public List<DestinosModels>? Destinos { get; set; }
    }
}
