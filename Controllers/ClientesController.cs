using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller{
    private readonly ILogger<ClientesController> _logger;
    private IClientesRepository repositorioClientes;
    public ClientesController(ILogger<ClientesController> logger, IClientesRepository RepositorioClientes){
        _logger=logger;
        repositorioClientes = RepositorioClientes;
    }
    public IActionResult Index(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Admin";
        return View(repositorioClientes.ListarClientesGuardados());
    }
    [HttpGet]
    public IActionResult AltaCliente(){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        return View();
    }
    [HttpPost]
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Clientes(clienteVM);
        repositorioClientes.CrearCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarCliente(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        var cliente=repositorioClientes.ObtenerClientePorId(id);
        var clienteVM = new ModificarClienteViewModel(cliente);
        return View(clienteVM);
    }
    [HttpPost]
    public IActionResult ModificarElCliente(ModificarClienteViewModel clienteVM){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Clientes(clienteVM);
        repositorioClientes.ModificarCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarCliente(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        Clientes cliente=repositorioClientes.ObtenerClientePorId(id);
        return View(cliente);
    }
    [HttpGet]
    public IActionResult EliminarElCliente(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
        if (HttpContext.Session.GetString("Rol") != "Admin")
        {
            TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
            return RedirectToAction("Index");
        }
        repositorioClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}
