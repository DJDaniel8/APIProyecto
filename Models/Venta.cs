public class Venta
{
    public Venta(Cliente cliente, Trabajador trabajador)
    {
        Cliente = cliente;
        Trabajador = trabajador;
        Nombre = cliente.Nombre + " " + cliente.Apellido;
        NombreTrabajador = trabajador.nombre + " " + trabajador.apellido;
    }

    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public Trabajador Trabajador { get; set; }
    public DateTime Fecha { get; set; }
    public bool EsAuditorada { get; set; }
    public string Nombre { get; set; }
    public string NombreTrabajador { get; set; }
    public decimal Total { get; set; }
}