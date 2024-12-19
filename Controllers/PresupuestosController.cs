using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Controllers;
public class PresupuestosController : Controller{
    private readonly ILogger<PresupuestosController> _logger;
    private IPresupuestosRepository repositorioPresupuestos;
    private IClientesRepository repositorioClientes;
    private IProductosRepository repositorioProductos;
    public PresupuestosController(ILogger<PresupuestosController> logger, IPresupuestosRepository RepositorioPresupuestos, IClientesRepository RepositorioClientes, IProductosRepository RepositorioProductos){
        _logger=logger;
        repositorioPresupuestos = RepositorioPresupuestos;
        repositorioClientes = RepositorioClientes;
        repositorioProductos = RepositorioProductos;
    }
    public IActionResult Index(){
        return View(repositorioPresupuestos.ListarPresupuestosGuardados());
    }
    [HttpGet]
    public IActionResult AltaPresupuesto(){
        List<Clientes> clientes = repositorioClientes.ListarClientesGuardados();
        ViewData["clientes"] = clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        return View();
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupuestoVM){
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var presupuesto = new Presupuestos(presupuestoVM);
        repositorioPresupuestos.CrearPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult AgregarProductosAPresupuesto(int id){
        List<Productos> productos = repositorioProductos.ListarProductosRegistrados();
        ViewData["Productos"]=productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(), 
            Text = p.Descripcion 
        }).ToList();
        var model = new AgregarProductosAPresupuestoViewModel();
        model.IdPresupuesto=id;
        return View(model);
    }
    [HttpPost]
    public IActionResult AgregarLosProductos(AgregarProductosAPresupuestoViewModel productos){
        if(!ModelState.IsValid) return RedirectToAction("Index");
        repositorioPresupuestos.AgregarProducto(productos.IdPresupuesto, productos.IdProducto, productos.Cantidad);
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
        List<Clientes> Clientes = repositorioClientes.ListarClientesGuardados();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        var presupuesto  = repositorioPresupuestos.ObtenerPresupuestoPorId(id);
        var presupuestoVM = new ModificarPresupuestoViewModel();
        presupuestoVM.IdPresupuesto = id;
        presupuestoVM.FechaCreacion = presupuesto.FechaCreacion;
        return View(presupuestoVM);
    }
    [HttpPost]
    public IActionResult ModificarElPresupuesto(ModificarPresupuestoViewModel presupuestoVM){
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var presupuesto = new Presupuestos(presupuestoVM);
        repositorioPresupuestos.ModificarPresupuesto(presupuesto);
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