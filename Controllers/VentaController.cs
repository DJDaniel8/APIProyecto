using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class VentaController : ControllerBase
{
    IVentaDAO ventaDAO;

    public VentaController(IVentaDAO ventaDAO)
    {
        this.ventaDAO = ventaDAO;
    }

    [HttpGet]
    [Route("get")]
    public IActionResult Get()
    {
        return Ok(ventaDAO.Get());
    }

    [HttpGet]
    [Route("getProductosVentas/{id}")]
    public IActionResult getProductosVenta(int id)
    {
        return Ok(ventaDAO.GetProductosById(id));
    }

    [HttpPost]
    [Route("Insert")]
    public IActionResult Post([FromBody] Venta venta)
    {
        return Ok(ventaDAO.Insert(venta));
    }

    [HttpPost]
    [Route("InsertarProductos")]
    public IActionResult InsertarProductos([FromBody] List<Stock> productos)
    {
        return Ok(ventaDAO.Insert2(productos));
    }

}