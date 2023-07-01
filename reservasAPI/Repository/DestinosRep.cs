using reservasAPI.Models;
using System.Data;

namespace reservasAPI.Repository
{
    public class DestinosRep
    {
        readonly FuncionesCRUD Crud;
        private Dictionary<string, string> parametros;
        public DestinosRep()
        {
            Crud = new FuncionesCRUD();
        }

        public List<DestinosModels>? ListaDestinos()
        {
            DataTable dt = Crud.CargarDatos("select_all_destinos");

            if (dt.Rows.Count == 0) return null;

            List<DestinosModels> list = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DestinosModels model = new()
                {
                    Codigo = Convert.ToInt32(dt.Rows[i]["codigo"].ToString()),
                    NomDestino = dt.Rows[i]["nom_destino"].ToString(),
                    Descripcion = dt.Rows[i]["descripcion"].ToString(),
                    Ubicacion = dt.Rows[0]["ubicacion"].ToString(),
                    CodigoPaquete = Convert.ToInt32(dt.Rows[i]["codigo_paquete"].ToString())
                };
                list.Add(model);
            }

            return list;
        }

        public DestinosModels? BuscarDestino(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            DataTable dt = Crud.CargarDatos("select_destino_codigo", parametros);

            if (dt.Rows.Count == 0) return null;

            DestinosModels model = new()
            {
                Codigo = Convert.ToInt32(dt.Rows[0]["codigo"].ToString()),
                NomDestino = dt.Rows[0]["nom_destino"].ToString(),
                Descripcion = dt.Rows[0]["descripcion"].ToString(),
                Ubicacion = dt.Rows[0]["ubicacion"].ToString(),
                CodigoPaquete = Convert.ToInt32(dt.Rows[0]["codigo_paquete"].ToString())
            };

            return model;
        }
        
        public List<DestinosModels>? BuscarDestinoPorPaquete(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo_paquete", Convert.ToString(codigo) }
            };

            DataTable dt = Crud.CargarDatos("select_all_destinos_por_paquete", parametros);

            if (dt.Rows.Count == 0) return null;

            List<DestinosModels> list = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DestinosModels model = new()
                {
                    Codigo = Convert.ToInt32(dt.Rows[i]["codigo"].ToString()),
                    NomDestino = dt.Rows[i]["nom_destino"].ToString(),
                    Descripcion = dt.Rows[i]["descripcion"].ToString(),
                    Ubicacion = dt.Rows[i]["ubicacion"].ToString(),
                    CodigoPaquete = Convert.ToInt32(dt.Rows[i]["codigo_paquete"].ToString())
                };
                list.Add(model);
            }

            return list;
        }

        public int IsertarDestino(DestinosModels DestinosModels)
        {
            parametros = new Dictionary<string, string>
            {
                { "@nombre", DestinosModels.NomDestino.ToString() },
                { "@descripcion", DestinosModels.Descripcion.ToString() },
                { "@ubicacion", DestinosModels.Ubicacion.ToString() },
                { "@codigo_paquete", DestinosModels.CodigoPaquete.ToString() }
            };

            return Crud.EjecutarInsDelUpd("insert_destinos", parametros);
        }

        public int ActualizarDestino(DestinosModels DestinosModels)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", DestinosModels.Codigo.ToString() },
                { "@nombre", DestinosModels.NomDestino.ToString() },
                { "@descripcion", DestinosModels.Descripcion.ToString() },
                { "@ubicacion", DestinosModels.Ubicacion.ToString() },
                { "@codigo_paquete", DestinosModels.CodigoPaquete.ToString() }
            };

            return Crud.EjecutarInsDelUpd("update_destinos", parametros);
        }

        public int EliminarDestino(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            return Crud.EjecutarInsDelUpd("delete_destinos", parametros);
        }
    }
}
