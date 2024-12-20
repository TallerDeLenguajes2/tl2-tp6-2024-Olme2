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
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Admin";
        return View(repositorioProductos.ListarProductosRegistrados());
    }
    [HttpGet]
    public IActionResult AltaProducto(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        return View();
    }
    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM){
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
    }
    [HttpGet]
    public IActionResult ModificarProducto(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        var productoVM = new ModificarProductoViewModel(producto);
        return View(productoVM);
    }
    [HttpPost]
    public IActionResult ModificarProductoPorId(Productos producto){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        repositorioProductos.ModificarProducto(producto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarProducto(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        return View(producto);
    }
    [HttpGet]
    public IActionResult EliminarProductoPorId(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        repositorioProductos.EliminarProductoPorId(id);
        return RedirectToAction("Index");
    }
}