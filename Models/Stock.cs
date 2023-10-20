public class Stock
{
    public int StockId {get; set;}
    public Producto Producto { get; set; }
    public int Existencia { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVentaMinimo { get; set; }
    public decimal PrecioVenta { get; set; }
}