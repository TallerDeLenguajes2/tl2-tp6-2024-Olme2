using System.ComponentModel.DataAnnotations; 
public class AltaProductoViewModel{
    private string descripcion;
    private int precio;

    public AltaProductoViewModel(){
    }

    [StringLength(250, ErrorMessage = "La descripcion no puede ser mas larga que 250 caracteres")]
    public string Descripcion { get => descripcion; set => descripcion = value;}

    [Required(ErrorMessage = "Precio obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Precio positivo")]
    public int Precio { get => precio; set => precio = value;}
}