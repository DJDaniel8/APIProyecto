using System.Data.SqlClient;
using APIREST.Config;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace APIREST.Services;
public class VentaDAO : IVentaDAO
{
    private SqlConnection miconexion;

    public VentaDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Venta> Get()
    {
        return GetAll();
    }

    private List<Venta> GetAll()
    {
        string consulta = @"SELECT TOP(100) *, 
                            (SELECT SUM(cantidad*precio_venta) FROM Productos_Ventas WHERE ventaId = v.ventaId)
                            FROM Ventas as v
                            LEFT JOIN Clientes as c ON v.clienteId = c.clienteId
                            INNER JOIN trabajadores as t ON v.trabajadorId = t.trabajadorId

                            ORDER BY v.ventaId Desc";
        SqlCommand comando = new SqlCommand(consulta, miconexion);


        return TransformarData(comando);
    }

    public List<Stock> GetProductosById(int id)
    {
        string consulta = @"SELECT * FROM Productos_Ventas as pv
                    INNER JOIN Stocks as s ON pv.stockId = s.stockId
                    INNER JOIN Productos as p ON s.productoId = p.productoId
                    WHERE pv.ventaId = @id";
        SqlCommand comando = new(consulta,miconexion);
        comando.Parameters.AddWithValue("@id", id);
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        List<Stock> lista = new();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                Stock s = new();
                Producto p = new();
                p.Codigo = reader.GetString(12);
                p.Nombre = reader.GetString(13);
                s.PrecioVenta = reader.GetDecimal(4);
                s.Existencia = reader.GetInt32(3);
                s.Producto = p;
                lista.Add(s);
            }
        }
        return lista;
    }

    private List<Venta> TransformarData(SqlCommand comando)
    {
        List<Venta> lista = new();
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        using (reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    if (reader.IsDBNull(1))
                    {
                        cliente.Nombre = reader.GetString(4);
                        cliente.Nit = reader.GetString(5);
                    }
                    else
                    {
                        cliente.ClienteId = reader.GetInt32(7);
                        cliente.Nombre = reader.GetString(8);
                        cliente.Apellido = reader.GetString(9);
                        cliente.Genero = reader.GetString(10);
                        cliente.Nit = reader.GetString(11);
                        if (!reader.IsDBNull(12)) cliente.Direccion = reader.GetString(12);
                        if (!reader.IsDBNull(13)) cliente.Telefono = reader.GetString(13);
                    }

                    Trabajador trabajador = new Trabajador();
                    trabajador.trabajadorId = reader.GetInt32(14);
                    trabajador.nombre = reader.GetString(15);
                    trabajador.apellido = reader.GetString(16);
                    trabajador.sexo = reader.GetString(17);
                    trabajador.puesto = reader.GetString(18);
                    trabajador.usuario = reader.GetString(19);
                    if (!reader.IsDBNull(21)) trabajador.direccion = reader.GetString(21);
                    if (!reader.IsDBNull(22)) trabajador.telefono = reader.GetString(22);

                    Venta venta = new Venta(cliente, trabajador);
                    venta.Id = reader.GetInt32(0);
                    venta.Fecha = reader.GetDateTime(3);
                    venta.Total = reader.GetDecimal(26);

                    lista.Add(venta);
                }
            }
        }
        miconexion.Close();
        return lista;
    }

    public int Insert(Venta venta)
    {
        string consulta = @"INSERT INTO Ventas (clienteId, trabajadorId, fecha, nombre, nit) 
                            VALUES (@clienteId, @trabajadorId, @fecha, @nombre, @nit)";
        SqlCommand comando = new SqlCommand(consulta, miconexion);
        if (venta.Cliente.ClienteId > 0) comando.Parameters.AddWithValue("@clienteId", venta.Cliente.ClienteId);
        else comando.Parameters.AddWithValue("@clienteId", DBNull.Value);
        comando.Parameters.AddWithValue("@trabajadorId", venta.Trabajador.trabajadorId);
        comando.Parameters.AddWithValue("@fecha", venta.Fecha);
        if (venta.Cliente.ClienteId == 0) comando.Parameters.AddWithValue("@nombre", venta.Nombre);
        else comando.Parameters.AddWithValue("@nombre", DBNull.Value);
        if (venta.Cliente.ClienteId == 0) comando.Parameters.AddWithValue("@nit", venta.Cliente.Nit);
        else comando.Parameters.AddWithValue("@nit", DBNull.Value);
        miconexion.Open();
        int resultado = comando.ExecuteNonQuery();
        miconexion.Close();

        return getLastId();
    }

    public int Insert2(List<Stock> productos)
    {
        int id = getLastId();
        foreach (var item in productos)
        {
            insertProductosVenta(item, id);
            restarStock(item);
        }
        return 1;
    }

    private void insertProductosVenta(Stock stock, int ventaId)
    {
        string consulta = @"INSERT INTO Productos_Ventas (ventaId, stockId, cantidad, precio_venta)
                    VALUES (@ventaId, @stockId, @cantidad, @precio_venta)";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@ventaId", ventaId);
        comando.Parameters.AddWithValue("@stockId", stock.StockId);
        comando.Parameters.AddWithValue("@cantidad", stock.Existencia);
        comando.Parameters.AddWithValue("@precio_venta", stock.PrecioVenta);
        miconexion.Open();
        comando.ExecuteReader();
        miconexion.Close();
    }

    private void restarStock(Stock stock)
    {
        string consulta = "UPDATE stocks SET stock = ( (SELECT stock FROM stocks WHERE stockId = @stockId) - @cantidad) WHERE stockId = @stockId2";
        SqlCommand comando = new(consulta,miconexion);
        comando.Parameters.AddWithValue("@stockId", stock.StockId);
        comando.Parameters.AddWithValue("@stockId2", stock.StockId);
        comando.Parameters.AddWithValue("@cantidad", stock.Existencia);
        miconexion.Open();
        comando.ExecuteNonQuery();
        miconexion.Close();
    }

    public int getLastId()
    {
        string consulta = @"SELECT TOP(1) * From Ventas ORDER BY ventaId desc";
        SqlCommand comando = new(consulta, miconexion);
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        int resultado = 0;
        using (reader)
        {
            if (reader.HasRows)
            {
                reader.Read();
                resultado = reader.GetInt32(0);
            }
        }
        miconexion.Close();
        return resultado;
    }

}

public interface IVentaDAO
{
    List<Venta> Get();
    int Insert(Venta venta);
    int Insert2(List<Stock> productos);
    List<Stock> GetProductosById(int id);
}
