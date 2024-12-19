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
        return View(repositorioProductos.ListarProductosRegistrados());
    }
    [HttpGet]
    public IActionResult AltaProducto(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM){
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var producto = new Productos(productoVM);
        repositorioProductos.CrearNuevoProducto(producto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarProducto(int id){
        var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        var productoVM = new ModificarProductoViewModel(producto);
        return View(productoVM);
    }
    [HttpPost]
    public IActionResult ModificarProductoPorId(Productos producto){
        repositorioProductos.ModificarProducto(producto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarProducto(int id){
        var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        return View(producto);
    }
    [HttpGet]
    public IActionResult EliminarProductoPorId(int id){
        repositorioProductos.EliminarProductoPorId(id);
        return RedirectToAction("Index");
    }
}