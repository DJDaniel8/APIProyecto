using System.Data.SqlClient;
using APIREST.Config;

namespace APIREST.Services;
public class StockDAO : IStockDAO
{
    private SqlConnection miconexion;

    public StockDAO(IDBConection dBConection)
    {
        this.miconexion = dBConection.GetConnection();
    }

    public List<Stock> Get()
    {
        return GetAll();
    }

    private List<Stock> GetAll()
    {
        string consulta = @"select * from [dbo].[Productos] as p
                            INNER JOIN [dbo].[Stocks] as s ON p.productoId = s.productoId";
        SqlCommand comando = new(consulta, miconexion);
        List<Stock> list = new();
        miconexion.Open();
        SqlDataReader reader = comando.ExecuteReader();
        using(reader)
        {
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    Stock s = new();
                    Producto p = new();
                    p.ProductoId = reader.GetInt32(0);
                    p.Codigo = reader.GetString(1);
                    p.Nombre = reader.GetString(2);
                    p.Descripcion = reader.GetString(3);
                    p.ProveedorId = reader.GetInt32(5);
                    s.StockId = reader.GetInt32(6);
                    s.Existencia = (int)reader.GetDecimal(7);
                    s.PrecioCompra = reader.GetDecimal(8);
                    s.PrecioVenta = reader.GetDecimal(9);
                    s.PrecioVentaMinimo = reader.GetDecimal(10);
                    s.Producto = p;
                    list.Add(s);
                }
            }
        }
        miconexion.Close();
        return list;
    }

    public int Insert(Stock stock)
    {
        string consulta = "INSERT INTO Stocks (stock, precio_compra, precio_venta_sugerido, precio_minimo, productoId) VALUES (@stock, @precio_compra, @precio_venta_sugerido, @precio_minimo, @productoId)";
        SqlCommand comando = new(consulta, miconexion);
        comando.Parameters.AddWithValue("@stock",stock.Existencia);
        comando.Parameters.AddWithValue("@precio_compra",stock.PrecioCompra);
        comando.Parameters.AddWithValue("@precio_venta_sugerido",stock.PrecioVenta);
        comando.Parameters.AddWithValue("@precio_minimo",stock.PrecioVentaMinimo);
        comando.Parameters.AddWithValue("@productoId",stock.Producto.ProveedorId);
        miconexion.Open();
        
        int respuesta = comando.ExecuteNonQuery();

        miconexion.Close();
        return respuesta;
    }
}

public interface IStockDAO
{
    List<Stock> Get();
    int Insert(Stock stock);
}
