using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller{
    private readonly ILogger<ClientesController> _logger;
    private IClientesRepository repositorioClientes;
    public ClientesController(ILogger<ClientesController> logger, IClientesRepository RepositorioClientes){
        _logger=logger;
        repositorioClientes = RepositorioClientes;
    }
    public IActionResult Index(){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("Rol") == "Admin";
            return View(repositorioClientes.ListarClientesGuardados());
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó lista de clientes";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult AltaCliente(){
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
            ViewBag.ErrorMessage = "No se cargó formulario de creación de cliente";
        return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM){
        try{
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
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Creación de cliente sin éxito";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult ModificarCliente(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            var cliente=repositorioClientes.ObtenerClientePorId(id);
            var clienteVM = new ModificarClienteViewModel(cliente);
            return View(clienteVM);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó formulario de modificación de cliente";
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public IActionResult ModificarElCliente(ModificarClienteViewModel clienteVM){
        try{
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
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Modificación de cliente sin éxito";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarCliente(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            Clientes cliente=repositorioClientes.ObtenerClientePorId(id);
            return View(cliente);
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó cliente para eliminar";
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarElCliente(int id){
        try{
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "Sin permisos para realizar esta acción";
                return RedirectToAction("Index");
            }
            repositorioClientes.EliminarCliente(id);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Eliminación de cliente sin éxito";
            return RedirectToAction("Index");
        }
    }
}
