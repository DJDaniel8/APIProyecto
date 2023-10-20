using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    IStockDAO stockDAO;

    public StockController(IStockDAO stockDAO)
    {
        this.stockDAO = stockDAO;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(stockDAO.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Stock stock)
    {
        return Ok(stockDAO.Insert(stock));
    }
}