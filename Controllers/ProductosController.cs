using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Controllers;

public class ProductosController : Controller{
    private readonly ILogger<ProductosController> _logger;
    private IProductosRepository repositorioProductos;
    public ProductosController(ILogger<ProductosController> logger, IProductosRepository RepositorioProductos){
        _logger=logger;
        repositorioProductos = RepositorioProductos;
    }
    public IActionResult Index(){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Admin";
            return View(repositorioProductos.ListarProductosRegistrados());
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se pudo cargó la lista de productos correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult AltaProducto(){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            return View();
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se pudo cargó el formulario de creación de producto correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            if(!ModelState.IsValid) return RedirectToAction("Index");
            var producto = new Productos(productoVM);
            repositorioProductos.CrearNuevoProducto(producto);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Creación del producto sin éxito";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult ModificarProducto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
            var productoVM = new ModificarProductoViewModel(producto);
            return View(productoVM);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó el formulario de modificación del producto correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult ModificarProductoPorId(Productos producto){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            repositorioProductos.ModificarProducto(producto);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Modificación del producto sin éxito";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarProducto(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
            return View(producto);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó el producto a eliminar correctamente";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarProductoPorId(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            repositorioProductos.EliminarProductoPorId(id);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Eliminación del producto sin éxito";
            return RedirectToAction("Index");
        }
    }
}