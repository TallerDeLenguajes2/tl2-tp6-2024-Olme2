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
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Admin";
            return View(repositorioPresupuestos.ListarPresupuestosGuardados());
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage("No se cargó el listado de presupuestos correctamente");
            return RedirectToAction("Index", "Home");
        }
    }
    [HttpGet]
    public IActionResult AltaPresupuesto(){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            List<Clientes> clientes = repositorioClientes.ListarClientesGuardados();
            ViewData["clientes"] = clientes.Select(c=> new SelectListItem
            {
                Value = c.ClienteId.ToString(), 
                Text = c.Nombre
            }).ToList();
            return View();
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage("No se cargó el formulario de creación de presupuesto correctamente");
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupuestoVM){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            if(!ModelState.IsValid) return RedirectToAction ("Index");
            var presupuesto = new Presupuestos(presupuestoVM);
            repositorioPresupuestos.CrearPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }catch (Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Creación de presupuesto sin éxito";
            return RedirectToAction("AltaPresupuesto");
        }
    }
    [HttpGet]
    public IActionResult AgregarProductosAPresupuesto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            List<Productos> productos = repositorioProductos.ListarProductosRegistrados();
            ViewData["Productos"]=productos.Select(p => new SelectListItem
            {
                Value = p.IdProducto.ToString(), 
                Text = p.Descripcion 
            }).ToList();
            var model = new AgregarProductosAPresupuestoViewModel();
            model.IdPresupuesto=id;
            return View(model);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó la lista de productos para el presupuesto correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult AgregarLosProductos(AgregarProductosAPresupuestoViewModel productos){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            if(!ModelState.IsValid) return RedirectToAction("Index");
            repositorioPresupuestos.AgregarProducto(productos.IdPresupuesto, productos.IdProducto, productos.Cantidad);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Agregación de producto en presupuesto sin éxito";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarProductosDePresupuesto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            List<PresupuestosDetalle> detalles=repositorioPresupuestos.MostrarDetallePorId(id);
            ViewData["PresupuestosDetalle"]=detalles;
            return View(id);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó la lista de productos para eliminar correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult EliminarProductos(int idPresupuesto, List<int> idsProductos, List<int> cantidadesVieja, List<int> cantidadesNueva){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            for(int i=0; i<idsProductos.Count; i++){
                if(cantidadesVieja[i]>cantidadesNueva[i]){
                    repositorioPresupuestos.EliminarProducto(idPresupuesto, idsProductos[i], cantidadesVieja[i], cantidadesNueva[i]);
                }
            }
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Eliminación de productos del presupuesto sin éxito";
            return RedirectToAction("Index");
        }
    }
    public IActionResult MostrarPresupuesto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            Presupuestos? presupuesto=repositorioPresupuestos.ObtenerPresupuestoPorId(id);
            return View(presupuesto);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage("No se mostró el presupuesto correctamente");
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult ModificarPresupuesto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
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
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó el presupuesto a modificar correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult ModificarElPresupuesto(ModificarPresupuestoViewModel presupuestoVM){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            if(!ModelState.IsValid) return RedirectToAction("Index");
            var presupuesto = new Presupuestos(presupuestoVM);
            repositorioPresupuestos.ModificarPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Modificación de presupuesto sin éxito";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarPresupuesto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            return View(repositorioPresupuestos.ObtenerPresupuestoPorId(id));
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó el presupuesto a eliminar correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarElPresupuesto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            repositorioPresupuestos.EliminarPresupuestoPorId(id);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Eliminación de presupuesto sin éxito";
            return RedirectToAction("Index");
        }
    }
}