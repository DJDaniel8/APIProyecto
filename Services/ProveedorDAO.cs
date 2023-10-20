using System.Data.SqlClient;
using APIREST.Config;

namespace APIREST.Services;
public class ProveedorDAO : IProveedorDAO
{
    private SqlConnection miconexion;

    public ProveedorDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Proveedor> Get()
    {
        return GetAll();
    }

    private List<Proveedor> GetAll()
    {
        string consulta = @"SELECT * FROM Proveedores ORDER BY razonSocial ASC";
        SqlCommand comando = new(consulta,miconexion);
        List<Proveedor> lista = new();
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        using(reader)
        {
            if(reader.HasRows)
            {
                while (reader.Read())
                {

                    Proveedor proveedor = new Proveedor();
                    proveedor.ProveedorId = reader.GetInt32(0);
                    proveedor.RazonSocial = reader.GetString(1);
                    if(!reader.IsDBNull(2)) proveedor.Direccion = reader.GetString(2);
                    if (!reader.IsDBNull(3)) proveedor.Telefono = reader.GetString(3);
                    lista.Add(proveedor);
                }
            }
        }
        miconexion.Close();
        return lista;
    }

    public int Insert(Proveedor proveedor)
    {
        string consulta = @"INSERT INTO Proveedores (razonSocial, direccion, telefono)
                            VALUES (@razonSocial, @direccion, @telefono) ";
        SqlCommand comando = new(consulta,miconexion);
        comando.Parameters.AddWithValue("@razonSocial", proveedor.RazonSocial);
        if (String.IsNullOrEmpty(proveedor.Direccion)) comando.Parameters.AddWithValue("direccion", DBNull.Value);
        else comando.Parameters.AddWithValue("@direccion", proveedor.Direccion);
        if (String.IsNullOrEmpty(proveedor.Telefono)) comando.Parameters.AddWithValue("telefono", DBNull.Value);
        else comando.Parameters.AddWithValue("@telefono", proveedor.Telefono);

        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }

    public int Update(int id, Proveedor proveedor)
    {
        string consulta = @"UPDATE Proveedores SET razonSocial = @razonSocial, direccion = @direccion, telefono = @telefono WHERE proveedorId = @id";
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        comando.Parameters.AddWithValue("@razonSocial", proveedor.RazonSocial);
        comando.Parameters.AddWithValue("@direccion", proveedor.Direccion);
        comando.Parameters.AddWithValue("@telefono", proveedor.Telefono);
        comando.Parameters.AddWithValue("@id", proveedor.ProveedorId);

        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }

    public int Delete(int id)
    {
        string consulta = @"DELETE FROM Proveedores WHERE proveedorId = @id";
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }
}

public interface IProveedorDAO
{
    List<Proveedor> Get();
    int Insert(Proveedor categoria);
    int Update(int id, Proveedor categoria);
    int Delete(int id);
}
