using reservasAPI.Models;
using System.Data;

namespace reservasAPI.Repository
{
    public class PaquetesRep
    {
        readonly FuncionesCRUD Crud;
        private Dictionary<string, string> parametros;
        public PaquetesRep()
        {
            Crud = new FuncionesCRUD();
        }

        public List<PaquetesModels>? ListaPaquetes()
        {
            DataTable dt = Crud.CargarDatos("select_all_paquetes");

            if (dt.Rows.Count == 0) return null;

            List<PaquetesModels> list = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PaquetesModels model = new()
                {
                    Codigo = Convert.ToInt32(dt.Rows[i]["codigo"].ToString()),
                    NomPaquete = dt.Rows[i]["nom_paquete"].ToString(),
                    Descripcion = dt.Rows[i]["descripcion"].ToString(),
                    Precio = Convert.ToDouble(dt.Rows[0]["precio"].ToString()),
                    FechaIni = Convert.ToDateTime(dt.Rows[i]["fecha_ini"].ToString()),
                    FechaFin = Convert.ToDateTime(dt.Rows[i]["fecha_fin"].ToString())
                };
                list.Add(model);
            }

            return list;
        }

        public PaquetesModels? BuscarPaquete(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            DataTable dt = Crud.CargarDatos("select_paquete_codigo", parametros);

            if (dt.Rows.Count == 0) return null;

            PaquetesModels model = new()
            {
                Codigo = Convert.ToInt32(dt.Rows[0]["codigo"].ToString()),
                NomPaquete = dt.Rows[0]["nom_paquete"].ToString(),
                Descripcion = dt.Rows[0]["descripcion"].ToString(),
                Precio = Convert.ToDouble( dt.Rows[0]["precio"].ToString()),
                FechaIni = Convert.ToDateTime(dt.Rows[0]["fecha_ini"].ToString()),
                FechaFin = Convert.ToDateTime(dt.Rows[0]["fecha_fin"].ToString())
            };

            return model;
        }

        public int IsertarPaquete(PaquetesModels PaquetesModels)
        {
            parametros = new Dictionary<string, string>
            {
                { "@nom_paquete", PaquetesModels.NomPaquete.ToString() },
                { "@descripcion", PaquetesModels.Descripcion.ToString() },
                { "@precio", PaquetesModels.Precio.ToString() },
                { "@fecha_ini", PaquetesModels.FechaIni.ToString() },
                { "@fecha_fin", PaquetesModels.FechaFin.ToString() }
            };

            return Crud.EjecutarInsDelUpd("insert_paquetes", parametros);
        }

        public int ActualizarPaquete(PaquetesModels PaquetesModels)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", PaquetesModels.Codigo.ToString() },
                { "@nom_paquete", PaquetesModels.NomPaquete.ToString() },
                { "@descripcion", PaquetesModels.Descripcion.ToString() },
                { "@precio", PaquetesModels.Precio.ToString() },
                { "@fecha_ini", PaquetesModels.FechaIni.ToString() },
                { "@fecha_fin", PaquetesModels.FechaFin.ToString() }
            };

            return Crud.EjecutarInsDelUpd("update_paquetes", parametros);
        }

        public int EliminarPaquete(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            return Crud.EjecutarInsDelUpd("delete_paquetes", parametros);
        }
    }
}
