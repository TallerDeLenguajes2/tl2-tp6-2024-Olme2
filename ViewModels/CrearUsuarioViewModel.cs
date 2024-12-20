using System.ComponentModel.DataAnnotations;
public class CrearUsuarioViewModel{
    private string usuario;
    private string nombre;
    private string contraseña;
    private Rol rol;
    
    
    [Required(ErrorMessage = "Usuario obligatorio")]
    public string Usuario { get => usuario; set => usuario = value; }

    [Required(ErrorMessage = "Nombre obligatorio")]
    public string Nombre { get => nombre; set => nombre = value; }

    [Required(ErrorMessage = "Contraseña obligatoria")]
    [MinLength(6, ErrorMessage = "Contraseña con al menos 6 caracteres")]
    public string Contraseña { get => contraseña; set => contraseña = value; }

    [Required(ErrorMessage = "El nivel de acceso es obligatorio.")]
    [EnumDataType(typeof(Rol), ErrorMessage = "Nivel de acceso no válido")]
    public Rol Rol { get => rol; set => rol = value; }
}