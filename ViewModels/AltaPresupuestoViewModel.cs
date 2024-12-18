using System.ComponentModel.DataAnnotations; 
public class AltaPresupuestoViewModel{
    private int idCliente;
    private DateTime fechaCreacion;

    public AltaPresupuestoViewModel(){
    }
    [Required(ErrorMessage = "Cliente obligatorio")]
    public int IdCliente { get => idCliente; set => idCliente = value; }

    [Required(ErrorMessage = "Fecha obligatoria")]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}