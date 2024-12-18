using System.ComponentModel.DataAnnotations; 
public class ModificarProductoViewModel{
    private int idProducto;
    private string descripcion;
    private int precio;
    
    public ModificarProductoViewModel(){
    }

    public ModificarProductoViewModel(Productos producto){
        idProducto = producto.IdProducto;
        descripcion = producto.Descripcion;
        precio = producto.Precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value;}

    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres")]
    public string Descripcion { get => descripcion; set => descripcion = value;}

    [Required(ErrorMessage = "Precio obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Precio positivo")]
    public int Precio { get => precio; set => precio = value;}
}