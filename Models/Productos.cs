public class Productos{
    private int idProducto;
    private string? descripcion;
    private int precio;
    public Productos(){}
    public Productos(int IdProducto, string? Descripcion, int Precio){
        idProducto = IdProducto;
        descripcion= Descripcion;
        precio= Precio;
    }
    public Productos(AltaProductoViewModel productoVM){
        Descripcion = productoVM.Descripcion;
        Precio = productoVM.Precio;
    }

    public Productos(ModificarProductoViewModel productoVM){
        IdProducto = productoVM.IdProducto;
        Descripcion = productoVM.Descripcion;
        Precio = productoVM.Precio;
    }
    public int IdProducto{ get => idProducto; set => idProducto=value; }
    public string? Descripcion{ get => descripcion; set => descripcion=value; }
    public int Precio{ get => precio; set => precio=value; }
}