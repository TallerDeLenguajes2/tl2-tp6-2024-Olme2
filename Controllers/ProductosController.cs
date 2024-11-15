using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Controllers;

public class ProductosController : Controller{
    private readonly ILogger<ProductosController> _logger;
    private ProductosRepository repositorioProductos;
    public ProductosController(ILogger<ProductosController> logger){
        _logger=logger;
        repositorioProductos=new ProductosRepository();
    }
    public IActionResult Index(){
        return View(repositorioProductos.ListarProductosRegistrados());
    }
    [HttpGet]
    public IActionResult AltaProducto(){
        var producto=new Productos();
        producto.IdProducto=repositorioProductos.BuscarIdMasGrande()+1;
        return View(producto);
    }
    [HttpPost]
    public IActionResult CrearProducto(Productos producto){
        repositorioProductos.CrearNuevoProducto(producto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarProducto(int id){
        var producto = repositorioProductos.ObtenerDetallesDeProductoPorId(id);
        return View(producto);
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
    [HttpPost]
    public IActionResult EliminarProductoPorId(int id){
        repositorioProductos.EliminarProductoPorId(id);
        return RedirectToAction("Index");
    }
}