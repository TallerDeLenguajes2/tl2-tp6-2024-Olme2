using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller{
    private readonly IUsuariosRepository _usuariosRepository;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IUsuariosRepository usuariosRepository){
        _usuariosRepository = usuariosRepository;
    }
    public IActionResult Index(){
        var model = new LoginViewModel{
            Autenticado = HttpContext.Session.GetString("Autenticado") == "true"
        };
        return View(model);
    }
    public IActionResult Login(LoginViewModel model){
        if(string.IsNullOrEmpty(model.Usuario) || string.IsNullOrEmpty(model.Contraseña)){
            model.Error = "Por favor ingreas usuario y contraseña";
            return View("Index", model);
        }
        Usuarios usuario = _usuariosRepository.GetUsuarios(model.Usuario, model.Contraseña);
        if(usuario != null){
            HttpContext.Session.SetString("Autenticado", "true");
            HttpContext.Session.SetString("Usuario", usuario.Usuario);
            HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
            return RedirectToAction("Index", "Presupuestos");
        }
        model.Error = "Usuario o contraseña incorrectos";
        model.Autenticado = false;
        return View("Index", model);
    }
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}