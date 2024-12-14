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
    [HttpGet]
    public IActionResult AltaPresupuesto(){
        Presupuestos presupuesto=new Presupuestos();
        presupuesto.IdPresupuesto=repositorioPresupuestos.BuscarIdMasGrande();
        presupuesto.FechaCreacion= DateTime.Now;
        return View(presupuesto);
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuestos presupuesto){
        repositorioPresupuestos.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult AgregarProductosAPresupuesto(int id){
        ProductosRepository repositorioProductos=new ProductosRepository();
        List<Productos> productos = repositorioProductos.ListarProductosRegistrados();
        ViewData["Productos"]=productos;
        return View(id);
    }
    [HttpPost]
    public IActionResult AgregarProductos(int idPresupuesto, List<int> cantidades, List<int> idsProductos){
        for(int i = 0; i < idsProductos.Count; i++){
            if(cantidades[i] > 0){
                repositorioPresupuestos.AgregarProducto(idPresupuesto, idsProductos[i], cantidades[i]);
            }
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarProductosDePresupuesto(int id){
        List<PresupuestosDetalle> detalles=repositorioPresupuestos.MostrarDetallePorId(id);
        ViewData["PresupuestosDetalle"]=detalles;
        return View(id);
    }
    [HttpPost]
    public IActionResult EliminarProductos(int idPresupuesto, List<int> idsProductos, List<int> cantidadesVieja, List<int> cantidadesNueva){
        for(int i=0; i<idsProductos.Count; i++){
            if(cantidadesVieja[i]>cantidadesNueva[i]){
                repositorioPresupuestos.EliminarProducto(idPresupuesto, idsProductos[i], cantidadesVieja[i], cantidadesNueva[i]);
            }
        }
        return RedirectToAction("Index");
    }
    public IActionResult MostrarPresupuesto(int id){
        Presupuestos? presupuesto=repositorioPresupuestos.ObtenerPresupuestoPorId(id);
        return View(presupuesto);
    }
    [HttpGet]
    public IActionResult ModificarPresupuesto(int id){
        Presupuestos? presupuesto=new Presupuestos();
        presupuesto=repositorioPresupuestos.ObtenerPresupuestoPorId(id);
        return View(presupuesto);
    }
    [HttpPost]
    public IActionResult ModificarElPresupuesto(int idPresupuesto, int idCliente, DateTime FechaCreacion){
        repositorioPresupuestos.ModificarPresupuesto(idPresupuesto, idCliente, FechaCreacion);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarPresupuesto(int id){
        return View(repositorioPresupuestos.ObtenerPresupuestoPorId(id));
    }
    [HttpGet]
    public IActionResult EliminarElPresupuesto(int id){
        repositorioPresupuestos.EliminarPresupuestoPorId(id);
        return RedirectToAction("Index");
    }
}