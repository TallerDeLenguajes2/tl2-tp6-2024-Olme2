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
        Clientes cliente=new Clientes();
        cliente.ClienteId=repositorioClientes.BuscarIdCorrespondiente();
        return View(cliente);
    }
    [HttpPost]
    public IActionResult CrearCliente(Clientes cliente){
        repositorioClientes.CrearCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarCliente(int id){
        Clientes cliente=repositorioClientes.ObtenerClientePorId(id);
        return View(cliente);
    }
    [HttpPost]
    public IActionResult ModificarElCliente(Clientes cliente){
        repositorioClientes.ModificarCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarCliente(int id){
        return View(id);
    }
    [HttpPost]
    public IActionResult EliminarElCliente(int id){
        repositorioClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}