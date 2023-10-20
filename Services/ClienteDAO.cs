using System.Data.SqlClient;
using APIREST.Config;

namespace APIREST.Services;
public class ClienteDAO : IClienteDAO
{
    private SqlConnection miconexion;

    public ClienteDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Cliente> Get()
    {
        return GetAll();
    }

    private List<Cliente> GetAll()
    {
        string consulta = "SELECT * FROM Clientes";
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        List<Cliente> lista = new();
        if(reader.HasRows)
        {
            while (reader.Read())
            {
                Cliente cliente = new Cliente();
                cliente.ClienteId = reader.GetInt32(0);
                cliente.Nombre = reader.GetString(1);
                cliente.Apellido = reader.GetString(2);
                cliente.Genero = reader.GetString(3);
                cliente.Nit = reader.GetString(4);
                if(!reader.IsDBNull(5)) cliente.Direccion = reader.GetString(5);
                if (!reader.IsDBNull(6))  cliente.Telefono = reader.GetString(6);
                if (!reader.IsDBNull(7))  cliente.Email = reader.GetString(7);
                lista.Add(cliente);
            }
        }
        miconexion.Close();
        return lista;
    }

    public int Insert(Cliente cliente)
    {
        string consulta = @"INSERT INTO Clientes (nombre, apellido, sexo, nit, direccion, telefono, email)
                            VALUES (@nombre, @apellido, @sexo, @nit, @direccion, @telefono, @email)";
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
        comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
        comando.Parameters.AddWithValue("@sexo", cliente.Genero);
        comando.Parameters.AddWithValue("@nit", cliente.Nit);
        if(!String.IsNullOrEmpty(cliente.Direccion)) comando.Parameters.AddWithValue("@direccion", cliente.Direccion);
        else comando.Parameters.AddWithValue("@direccion", DBNull.Value);
        if (!String.IsNullOrEmpty(cliente.Telefono)) comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
        else comando.Parameters.AddWithValue("@telefono", DBNull.Value);
        if (!String.IsNullOrEmpty(cliente.Email)) comando.Parameters.AddWithValue("@email", cliente.Email);
        else comando.Parameters.AddWithValue("@email", DBNull.Value);

        miconexion.Open();
        int respuesta = comando.ExecuteNonQuery();
        miconexion.Close();
        return respuesta;
    }

    public int Update(int id, Cliente cliente)
    {
        string consulta = @"UPDATE Clientes SET nombre = @nombre, apellido = @apellido, sexo = @sexo, nit = @nit, direccion = @direccion, 
                                telefono = @telefono, email = @email WHERE clienteId = @id";
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
        comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
        comando.Parameters.AddWithValue("@sexo", cliente.Genero);
        comando.Parameters.AddWithValue("@nit", cliente.Nit);
        if (!String.IsNullOrEmpty(cliente.Direccion)) comando.Parameters.AddWithValue("@direccion", cliente.Direccion);
        else comando.Parameters.AddWithValue("@direccion", DBNull.Value);
        if (!String.IsNullOrEmpty(cliente.Telefono)) comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
        else comando.Parameters.AddWithValue("@telefono", DBNull.Value);
        if (!String.IsNullOrEmpty(cliente.Email)) comando.Parameters.AddWithValue("@email", cliente.Email);
        else comando.Parameters.AddWithValue("@email", DBNull.Value);
        comando.Parameters.AddWithValue("@id", id);

        miconexion.Open();
        int respuesta = comando.ExecuteNonQuery();
        miconexion.Close();
        return respuesta;
    }

    public int Delete(int id)
    {
        string consulta = "DELETE FROM Clientes WHERE clienteId = @id";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }
}

public interface IClienteDAO
{
    List<Cliente> Get();
    int Insert(Cliente categoria);
    int Update(int id, Cliente categoria);
    int Delete(int id);
}
