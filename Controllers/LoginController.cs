using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller{
    private readonly IUsuariosRepository _usuariosRepository;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IUsuariosRepository usuariosRepository, ILogger<LoginController> logger){
        _logger = logger;
        _usuariosRepository = usuariosRepository;
    }
    public IActionResult Index(){
        try{    
            var model = new LoginViewModel{
                Autenticado = HttpContext.Session.GetString("Autenticado") == "true"
            };
            return View(model);
        }catch(Exception e){
            _logger.LogError(e.Message);
            ViewBag.ErrorMessage = "No se cargó correctamente la página";
            return View("Index");
        }
    }
    public IActionResult Login(LoginViewModel model){
        try{
            if(string.IsNullOrEmpty(model.Usuario) || string.IsNullOrEmpty(model.Contraseña)){
                model.Error = "Por favor ingreas usuario y contraseña";
                return View("Index", model);
            }
            Usuarios usuario = _usuariosRepository.GetUsuarios(model.Usuario, model.Contraseña);
            if(usuario != null){
                HttpContext.Session.SetString("Autenticado", "true");
                HttpContext.Session.SetString("usuario", usuario.Usuario);
                HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
                return RedirectToAction("Index", "Home");
            }
            model.Error = "Usuario o contraseña incorrectos";
            model.Autenticado = false;
            return View("Index", model);
        }catch(Exception e){
            _logger.LogError(e.Message);
            ViewBag.ErrorMessage = "Autenticación de usuario sin éxito";
            return View("Index", model);
        }
    }
    public IActionResult Logout(){
        try{
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Deslogueo sin éxito";
            return View("Index");
        }
    }
    [HttpGet]
    public IActionResult CrearUsuario(){
        try{
            return View();
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "No se cargó correctamente el formulario de creación de usuario";
            return View("Index");
        }
    }
    [HttpPost]
    public IActionResult CrearElUsuario(CrearUsuarioViewModel usuarioVM){
        try{
            if(!ModelState.IsValid) return RedirectToAction("CrearUsuario");
            Usuarios usuario = new Usuarios(usuarioVM);
            _usuariosRepository.CrearUsuario(usuario);
            return RedirectToAction("Index");
        }catch(Exception e){
            _logger.LogError(e.ToString());
            ViewBag.ErrorMessage = "Creación de usuario sin éxito";
            return View("Index");
        }
    }
}