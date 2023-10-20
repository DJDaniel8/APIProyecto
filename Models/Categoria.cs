public class Categoria 
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; }

    public Categoria(int id, string nombre)
    {
        this.CategoriaId = id;
        this.Nombre = nombre;
    }

    public Categoria(){}
}