
using System.Data.SqlClient;

namespace APIREST.Config;

public class DBConection : IDBConection
{
    private string CadenaDeConexion;
    private SqlConnection miconexion;

    public DBConection(string cadenaDeConexion)
    {
        this.CadenaDeConexion = cadenaDeConexion;
        this.miconexion = new SqlConnection(cadenaDeConexion);
    }

    public SqlConnection GetConnection(){
        return miconexion;
    }
}

public interface IDBConection
{
    SqlConnection GetConnection();
}