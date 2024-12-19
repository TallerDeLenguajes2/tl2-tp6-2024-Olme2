using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller{
    private readonly ILogger<ClientesController> _logger;
    private ClientesRepository repositorioClientes;
    public ClientesController(ILogger<ClientesController> logger){
        _logger=logger;
        repositorioClientes=new ClientesRepository();
    }
    public IActionResult Index(){
        return View(repositorioClientes.ListarClientesGuardados());
    }
    [HttpGet]
    public IActionResult AltaCliente(){
        return View();
    }
    [HttpPost]
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM){
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Clientes(clienteVM);
        repositorioClientes.CrearCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarCliente(int id){
        var cliente=repositorioClientes.ObtenerClientePorId(id);
        var clienteVM = new ModificarClienteViewModel(cliente);
        return View(clienteVM);
    }
    [HttpPost]
    public IActionResult ModificarElCliente(ModificarClienteViewModel clienteVM){
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Clientes(clienteVM);
        repositorioClientes.ModificarCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarCliente(int id){
        Clientes cliente=repositorioClientes.ObtenerClientePorId(id);
        return View(cliente);
    }
    [HttpGet]
    public IActionResult EliminarElCliente(int id){
        repositorioClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}
