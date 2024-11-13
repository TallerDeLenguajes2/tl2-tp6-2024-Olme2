using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Controllers;
public class PresupuestosController : Controller{
    private readonly ILogger<PresupuestosController> _logger;
    private PresupuestosRepository repositorioPresupuestos;
    public PresupuestosController(ILogger<PresupuestosController> logger){
        _logger=logger;
        repositorioPresupuestos=new PresupuestosRepository();
    }
    public IActionResult Index(){
        return View(repositorioPresupuestos.ListarPresupuestosGuardados());
    }
    public IActionResult CrearPresupuesto(){
        return View();
    }
}