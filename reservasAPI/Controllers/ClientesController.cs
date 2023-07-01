using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservasAPI.Models;
using reservasAPI.Repository;

namespace reservasAPI.Controllers
{
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesRep clientesRep;
        private readonly IWebHostEnvironment webHost;

        public ClientesController(IWebHostEnvironment _webHost)
        {
            clientesRep = new();
            webHost = _webHost;
        }

        [HttpGet("MostrarClientes")]
        public ActionResult MostrarClientes()
        {
            List<ClientesModels>? list = new();

            list = clientesRep.ListaClientes();
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpGet("BuscarCliente/{codigo}")]
        public ActionResult MostrarClientesCodigo(int codigo)
        {
            ClientesModels? list = new();

            list = clientesRep.BuscarCliente(codigo);
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpPost("RegistrarCliente")]
        public ActionResult RegistrarCliente(ClientesModels clientes)
        {
            int resultado = clientesRep.IsertarCliente(clientes);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar guardar el registro.");
            }
            else
            {
                return Ok(MostrarClientesCodigo(resultado));
            }
        }

        [HttpPut("ActualizarCliente")]
        public ActionResult ActualizarCliente(ClientesModels clientes)
        {
            int resultado = clientesRep.ActualizarCliente(clientes);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar actualizar los datos del registro.");
            }
            else
            {
                return Ok(MostrarClientesCodigo(resultado));
            }
        }

        [HttpDelete("EliminarCliente/{codigo}")]
        public ActionResult EliminarCliente(int codigo)
        {
            int resultado = clientesRep.EliminarCliente(codigo);
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
