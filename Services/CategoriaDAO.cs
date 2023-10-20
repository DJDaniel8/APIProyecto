using System.Data.SqlClient;
using APIREST.Config;

namespace APIREST.Services;
public class CategoriaDAO : ICategoriaDAO
{
    private SqlConnection miconexion;

    public CategoriaDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Categoria> Get()
    {
        return GetAll();
    }

    private List<Categoria> GetAll()
    {
        string consulta = "SELECT * FROM Categorias";
        SqlCommand comando = new(consulta, miconexion);
        List<Categoria> list = new();
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        using(reader)
        {
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    list.Add(new(reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }
        miconexion.Close();
        return list;
    }

    public int Insert(Categoria categoria)
    {
        string consulta = "INSERT INTO Categorias (nombre) VALUES (@nombre)";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@nombre",categoria.Nombre);
        miconexion.Open();
        
        int respuesta = comando.ExecuteNonQuery();

        miconexion.Close();
        return respuesta;
    }

    public int Update(int id, Categoria categoria)
    {
        string consulta = "UPDATE Categorias SET nombre = @nombre WHERE categoriaId = @id";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@nombre", categoria.Nombre);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }

    public int Delete(int id)
    {
        string consulta = "DELETE FROM Categorias WHERE categoriaId = @id";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }
}

public interface ICategoriaDAO
{
    List<Categoria> Get();
    int Insert(Categoria categoria);
    int Update(int id, Categoria categoria);
    int Delete(int id);
}
