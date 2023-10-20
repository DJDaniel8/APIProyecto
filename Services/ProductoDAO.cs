using System.Data.SqlClient;
using APIREST.Config;

namespace APIREST.Services;
public class ProductoDAO : IProductoDAO
{
    private SqlConnection miconexion;

    public ProductoDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Producto> Get()
    {
        return GetAll();
    }

    private List<Producto> GetAll()
    {
        string consulta = "SELECT * FROM Productos";
        SqlCommand comando = new(consulta, miconexion);
        List<Producto> list = new();
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        using(reader)
        {
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    Producto p = new();
                    p.ProductoId = reader.GetInt32(0);
                    p.Codigo = reader.GetString(1);
                    p.Nombre = reader.GetString(2);
                    p.Descripcion = reader.GetString(3);
                    p.ProveedorId = reader.GetInt32(5);
                    list.Add(p);
                }
            }
        }
        miconexion.Close();
        return list;
    }

    public int Insert(Producto producto)
    {
        string consulta = "INSERT INTO Productos (codigo, nombre, descripcion, proveedorId) VALUES (@codigo, @nombre, @descripcion, @proveedorId)";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@codigo",producto.Codigo);
        comando.Parameters.AddWithValue("@nombre",producto.Nombre);
        comando.Parameters.AddWithValue("@descripcion",producto.Descripcion);
        comando.Parameters.AddWithValue("@proveedorId",producto.ProveedorId);
        miconexion.Open();
        
        int respuesta = comando.ExecuteNonQuery();

        miconexion.Close();
        return respuesta;
    }

    public int Update(int id, Producto producto)
    {
        string consulta = "UPDATE Productos SET codigo = @codigo, nombre = @nombre, descripcion = @descripcion WHERE productoId = @id";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@codigo",producto.Codigo);
        comando.Parameters.AddWithValue("@nombre",producto.Nombre);
        comando.Parameters.AddWithValue("@descripcion",producto.Descripcion);
        comando.Parameters.AddWithValue("@proveedorId",producto.ProveedorId);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }

    public int Delete(int id)
    {
        string consulta = "DELETE FROM Productos WHERE productoId = @id";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }
}

public interface IProductoDAO
{
    List<Producto> Get();
    int Insert(Producto producto);
    int Update(int id, Producto producto);
    int Delete(int id);
}
