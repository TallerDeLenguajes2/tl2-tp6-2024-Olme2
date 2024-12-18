using System.ComponentModel.DataAnnotations; 
public class AgregarProductosAPresupuestoViewModel{
    private int idPresupuesto;
    private int idProducto;
    private int cantidad;
    public AgregarProductosAPresupuestoViewModel(){
    }
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }

    [Required(ErrorMessage = "Id del producto obligario")]
    [Range(1, int.MaxValue, ErrorMessage = "Número entero positivo")]
    public int IdProducto { get => idProducto; set => idProducto = value; }

    [Required(ErrorMessage = "Cantidad del producto obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Número debe ser entero positivo")]
    public int Cantidad { get => cantidad; set => cantidad = value; }
}