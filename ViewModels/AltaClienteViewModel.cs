using System.ComponentModel.DataAnnotations; 
public class AltaClienteViewModel{
    private string nombre;
    private string email;
    private string telefono;
    public AltaClienteViewModel(){

    }

    [Required(ErrorMessage = "Nombre del cliente obligatorio")]
    public string Nombre { get => nombre; set => nombre = value;}

    [Required(ErrorMessage = "Email del cliente obligatorio")]
    [EmailAddress(ErrorMessage = "Verifique que sea una direccion de correo electrónico válida")]
    public string Email { get => email; set => email = value;}

    [Required(ErrorMessage = "Teléfono del cliente obligatorio")]
    [Phone(ErrorMessage = "Verifique que sea un teléfono válido")]
    public string Telefono { get => telefono; set => telefono = value;}
}