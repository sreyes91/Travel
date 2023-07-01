using reservasAPI.Models;
using System.Data;

namespace reservasAPI.Repository
{
    public class ClientesRep
    {
        readonly FuncionesCRUD Crud;
        private Dictionary<string, string> parametros;

        public ClientesRep()
        {
            Crud = new FuncionesCRUD();
        }

        public List<ClientesModels>? ListaClientes()
        {
            DataTable dt = Crud.CargarDatos("select_all_clientes");

            if (dt.Rows.Count == 0) return null;

            List<ClientesModels> list = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ClientesModels model = new()
                {
                    Codigo = Convert.ToInt32(dt.Rows[i]["codigo"].ToString()),
                    Nombres = dt.Rows[i]["nombres"].ToString(),
                    Apellidos = dt.Rows[i]["apellidos"].ToString(),
                    Telefono = dt.Rows[i]["telefono"].ToString(),
                    Email = dt.Rows[i]["email"].ToString()
                };
                list.Add(model);
            }

            return list;
        }

        public ClientesModels? BuscarCliente(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            DataTable dt = Crud.CargarDatos("select_clientes_codigo", parametros);

            if (dt.Rows.Count == 0) return null;

            ClientesModels model = new()
            {
                Codigo = Convert.ToInt32(dt.Rows[0]["codigo"].ToString()),
                Nombres = dt.Rows[0]["nombres"].ToString(),
                Apellidos = dt.Rows[0]["apellidos"].ToString(),
                Telefono = dt.Rows[0]["telefono"].ToString(),
                Email = dt.Rows[0]["email"].ToString()
            };

            return model;
        }

        public int IsertarCliente(ClientesModels clientesModels)
        {
            parametros = new Dictionary<string, string>
            {
                { "@nombres", clientesModels.Nombres.ToString() },
                { "@apellidos", clientesModels.Apellidos.ToString() },
                { "@telefono", clientesModels.Telefono.ToString() },
                { "@email", clientesModels.Email.ToString() }
            };

            return Crud.EjecutarInsDelUpd("insert_clientes", parametros);
        }

        public int ActualizarCliente(ClientesModels clientesModels)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", clientesModels.Codigo.ToString() },
                { "@nombres", clientesModels.Nombres.ToString() },
                { "@apellidos", clientesModels.Apellidos.ToString() },
                { "@telefono", clientesModels.Telefono.ToString() },
                { "@email", clientesModels.Email.ToString() }
            };

            return Crud.EjecutarInsDelUpd("update_clientes", parametros);
        }

        public int EliminarCliente(int codigo)
        {
            parametros = new Dictionary<string, string>
            {
                { "@codigo", Convert.ToString(codigo) }
            };

            return Crud.EjecutarInsDelUpd("delete_clientes", parametros);
        }
    }
}
