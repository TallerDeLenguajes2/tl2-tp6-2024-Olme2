using System.ComponentModel.DataAnnotations; 
public class ModificarClienteViewModel{
    private int clienteId;
    private string nombre;
    private string email;
    private string telefono;

    public ModificarClienteViewModel(){
    }
    public ModificarClienteViewModel(Clientes cliente){
        clienteId = cliente.ClienteId;
        nombre = cliente.Nombre;
        email = cliente.Email;
        telefono = cliente.Telefono;
    } 

    public int ClienteId { get => clienteId; set => clienteId = value;}
    
    [Required(ErrorMessage = "Nombre del cliente obligatorio")]
    public string Nombre { get => nombre; set => nombre = value;}

    [Required(ErrorMessage = "Email del cliente obligatorio")]
    [EmailAddress(ErrorMessage = "Verificar que el email sea válido")]
    public string Email { get => email; set => email = value;}

    [Required(ErrorMessage = "Nombre del cliente obligatorio")]
    [Phone(ErrorMessage = "Verificar que el telefono sea válido")]
    public string Telefono { get => telefono; set => telefono = value;}
}