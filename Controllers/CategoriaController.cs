using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriaController : ControllerBase
{
    ICategoriaDAO categoriaDAO;

    public CategoriaController(ICategoriaDAO categoriaDAO)
    {
        this.categoriaDAO = categoriaDAO;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(categoriaDAO.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Categoria categoria)
    {
        return Ok(categoriaDAO.Insert(categoria));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Categoria categoria)
    {
        return Ok(categoriaDAO.Update(id,categoria));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(categoriaDAO.Delete(id));
    }
}