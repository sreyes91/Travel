using reservasAPI.Dtos;
using reservasAPI.Models;
using System.Data;

namespace reservasAPI.Repository
{
    public class ReservasRep
    {
        readonly FuncionesCRUD Crud;
        private Dictionary<string, string> parametros;

        public ReservasRep()
        {
            Crud = new FuncionesCRUD();
        }

        public List<MostrarReservasDto>? ListaReservas()
        {
            DataTable dt = Crud.CargarDatos("select_all_reservas");

            if (dt.Rows.Count == 0) return null;

            List<MostrarReservasDto> list = new();

            int codigo_paquete = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MostrarReservasDto model = new()
                {
                    CodigoReserva = Convert.ToInt32(dt.Rows[i]["codigo_reserva"].ToString()),
                    CodigoReferencia = dt.Rows[i]["codigo_referencia"].ToString(),
                    FechaReserva = Convert.ToDateTime(dt.Rows[i]["fecha_reserva"].ToString()),
                };

                codigo_paquete = Convert.ToInt32(dt.Rows[i]["codigo_paquete"].ToString());

                PaquetesModels? paquetes = new PaquetesRep().BuscarPaquete(codigo_paquete);
                ClientesModels? cliente = new ClientesRep().BuscarCliente(Convert.ToInt32(dt.Rows[i]["codigo_cliente"].ToString()));
                model.PaquetesDestinos = new()
                {
                    NomPaquete = paquetes.NomPaquete,
                    Descripcion = paquetes.Descripcion,
                    Precio = "$ " + Convert.ToString(paquetes.Precio),
                    FechaIni = paquetes.FechaIni,
                    FechaFin = paquetes.FechaFin,
                    Destinos = new List<DestinosModels>()
                };

                model.Cliente = cliente;
                model.PaquetesDestinos.Destinos = new DestinosRep().BuscarDestinoPorPaquete(codigo_paquete);
                
                list.Add(model);
            }

            return list;
        }

        public MostrarReservasDto? BuscarReservas(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            DataTable dt = Crud.CargarDatos("select_reservas_codigo", parametros);

            if (dt.Rows.Count == 0) return null;

            int codigo_paquete = Convert.ToInt32(dt.Rows[0]["codigo_paquete"].ToString());
            MostrarReservasDto model = new()
            {
                CodigoReserva = Convert.ToInt32(dt.Rows[0]["codigo_reserva"].ToString()),
                CodigoReferencia = dt.Rows[0]["codigo_referencia"].ToString(),
                FechaReserva = Convert.ToDateTime(dt.Rows[0]["fecha_reserva"].ToString()),
            };

            PaquetesModels? paquetes = new PaquetesRep().BuscarPaquete(codigo_paquete);
            ClientesModels? cliente = new ClientesRep().BuscarCliente(Convert.ToInt32(dt.Rows[0]["codigo_cliente"].ToString()));
            model.PaquetesDestinos = new()
            {
                NomPaquete = paquetes.NomPaquete,
                Descripcion = paquetes.Descripcion,
                Precio = "$ " + Convert.ToString(paquetes.Precio),
                FechaIni = paquetes.FechaIni,
                FechaFin = paquetes.FechaFin,
                Destinos = new List<DestinosModels>()
            };

            model.Cliente = cliente;
            model.PaquetesDestinos.Destinos = new DestinosRep().BuscarDestinoPorPaquete(codigo_paquete);

            return model;
        }

        public int InsertarReserva(ReservasModels reservasModels)
        {
            parametros = new Dictionary<string, string>
            {
                {"@fecha_reserva", Convert.ToString(reservasModels.FechaReserva)},
                {"@codigo_cliente", Convert.ToString(reservasModels.CodigoCliente) },
                {"@codigo_paquete", Convert.ToString(reservasModels.CodigoPaquete) },
            };

            return Crud.EjecutarInsDelUpd("insert_reservas", parametros);
        }

        public int ActualizarReserva(ReservasModels reservasModels)
        {
            parametros = new Dictionary<string, string>
            {
                {"@fecha_reserva", Convert.ToString(reservasModels.FechaReserva)},
                {"@codigo_cliente", Convert.ToString(reservasModels.CodigoCliente) },
                {"@codigo_paquete", Convert.ToString(reservasModels.CodigoPaquete) },
                {"@codigo", Convert.ToString(reservasModels.Codigo) },
            };

            return Crud.EjecutarInsDelUpd("update_reservas", parametros);
        }

        public int EliminarReserva(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            return Crud.EjecutarInsDelUpd("delete_reservas", parametros);
        }
    }
}
