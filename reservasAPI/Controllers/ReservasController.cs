using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservasAPI.Dtos;
using reservasAPI.Models;
using reservasAPI.Repository;

namespace reservasAPI.Controllers
{
    [Route("api/Reservas")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ReservasRep reservasRep;
        private readonly IWebHostEnvironment webHost;

        public ReservasController(IWebHostEnvironment _webHost)
        {
            reservasRep = new();
            webHost = _webHost;
        }

        [HttpGet("VerPaquetesDestinos/{codigo}")]
        public ActionResult VerPaquetesDestinos(int codigo)
        {
            PaquetesModels? paquetes = new PaquetesRep().BuscarPaquete(codigo);
            PaquetesDestinosDto? model = new()
            {
                NomPaquete = paquetes.NomPaquete,
                Descripcion = paquetes.Descripcion,
                Precio = "$ " + Convert.ToString(paquetes.Precio),
                FechaIni = paquetes.FechaIni,
                FechaFin = paquetes.FechaFin,
                Destinos = new List<DestinosModels>()
            };

            model.Destinos = new DestinosRep().BuscarDestinoPorPaquete(codigo);

            if (model != null)
            {
                model.Destinos = new DestinosRep().BuscarDestinoPorPaquete(codigo);
                return Ok(model);
            }
            else
            {
                return BadRequest("No se encontraron Paquetes con el código: " + codigo);
            }
        }
    
        [HttpGet("MostrarReservas")]
        public ActionResult MostrarRservas()
        {
            List<MostrarReservasDto>? list = new();

            list = reservasRep.ListaReservas();
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        /// <summary>
        /// Puede buscar por: Código de Reserva, Código de Cliente o Código de Paquete
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpGet("BuscarReservas/{codigo}")]
        public ActionResult BuscarReservas(int codigo)
        {
            MostrarReservasDto? model = new();

            model = reservasRep.BuscarReservas(codigo);
            if (model != null)
            {
                return Ok(model);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpPost("RegistrarReserva")]
        public ActionResult RegistrarReserva(ReservasModels reservas)
        {
            if (reservas.CodigoCliente == 0)
            {
                return BadRequest("El código del cliente es obligatorio.");
            }
            if (reservas.CodigoPaquete == 0)
            {
                return BadRequest("El código del paquete es obligatorio.");
            }

            if (reservas.FechaReserva <= DateTime.Now)
            {
                return BadRequest("La fecha de reserva debe ser mayor que la fecha actual.");
            }


            int resultado = reservasRep.InsertarReserva(reservas);
            if (resultado == -1)
            {
                return BadRequest("La fecha de reserva debe ser menor a la fecha de inicio del Paquete turístico.");
            }
            else if (resultado == 0)
            {
                return BadRequest("Error inesperado al intentar guardar el registro.");
            }
            else
            {
                return Ok(BuscarReservas(resultado));
            }
        }

        [HttpPut("ActualizarReserva")]
        public ActionResult ActualizarReserva(ReservasModels reservas)
        {
            if (reservas.CodigoCliente == 0)
            {
                return BadRequest("El código del cliente es obligatorio.");
            }
            if (reservas.CodigoPaquete == 0)
            {
                return BadRequest("El código del paquete es obligatorio.");
            }

            if (reservas.FechaReserva <= DateTime.Now)
            {
                return BadRequest("La fecha de reserva debe ser mayor que la fecha actual.");
            }

            int resultado = reservasRep.ActualizarReserva(reservas);
            if (resultado == -1)
            {
                return BadRequest("La fecha de reserva debe ser menor a la fecha de inicio del Paquete turístico.");
            }
            else if (resultado == 0)
            {
                return BadRequest("Error inesperado al intentar actualizar el registro.");
            }
            else
            {
                return Ok(BuscarReservas(resultado));
            }
        }

        [HttpDelete("EliminarReserva")]
        public ActionResult EliminarReserva(int codigo)
        {
            int resultado = reservasRep.EliminarReserva(codigo);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar eliminar el registro.");
            }
            else
            {
                return Ok("Registro eliminado con éxito.");
            }
        }
    }
}
