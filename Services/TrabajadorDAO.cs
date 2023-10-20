using System.Data.SqlClient;
using APIREST.Config;

namespace APIREST.Services;
public class TrabajadorDAO : ITrabajadorDAO
{
    private SqlConnection miconexion;

    public TrabajadorDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Trabajador> Get()
    {
        return GetAll();
    }

    public bool HasAccess(string usuario, string contraseña)
    {
        if (FindEmployeByUser(usuario))
            {
                if (ValidatePassword(usuario, contraseña)) return true;
                else return false;
            }
        else return false;
    }

    public bool FindEmployeByUser(string usuario)
    {
        string consulta = "SELECT * FROM trabajadores WHERE usuario = @usuario"; // Busca un Trabajador por medio del usuario
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        comando.Parameters.AddWithValue("@usuario", usuario);
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        bool valor = reader.HasRows;
        reader.Close();
        miconexion.Close();
        return valor;
    }

    public bool ValidatePassword(string usuario, string contraseña)
    {
        string consulta = "SELECT * FROM trabajadores WHERE usuario = @usuario AND contraseña = @contraseña"; //Validamos que la contraseña sea la correcta
        SqlCommand comando = new SqlCommand(consulta, miconexion); 
        comando.Parameters.AddWithValue("@usuario", usuario);
        comando.Parameters.AddWithValue("@contraseña", contraseña);
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        bool valor = reader.HasRows;
        reader.Close();
        miconexion.Close();
        return valor;
    }

    private List<Trabajador> GetAll()
    {
        string consulta = "SELECT * FROM trabajadores";
            SqlCommand comando = new SqlCommand(consulta, miconexion);
            List<Trabajador> lista = new();
            miconexion.Open();
            SqlDataReader reader = comando.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    Trabajador trabajador = new Trabajador();
                    trabajador.trabajadorId = reader.GetInt32(0);
                    trabajador.nombre = reader.GetString(1);
                    trabajador.apellido = reader.GetString(2);
                    trabajador.sexo = reader.GetString(3);
                    trabajador.puesto = reader.GetString(4);
                    trabajador.usuario = reader.GetString(5);
                    if (!reader.IsDBNull(7)) trabajador.direccion = reader.GetString(7);
                    if (!reader.IsDBNull(8)) trabajador.telefono = reader.GetString(8);
                    if (!reader.IsDBNull(9)) trabajador.email = reader.GetString(9);
                    if (!reader.IsDBNull(10)) trabajador.sueldo = reader.GetDecimal(10);
                    lista.Add(trabajador);
                }
            }
            miconexion.Close();
            return lista;
    }

    public int Delete(int id)
    {
        string consulta = "DELETE FROM Trabajadores WHERE trabajadorId = @id";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return resultado;
    }
}

public interface ITrabajadorDAO
{
    List<Trabajador> Get();
    int Delete(int id);
    bool HasAccess(string usuario, string contraseña);
}
