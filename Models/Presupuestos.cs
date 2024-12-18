using System.Collections.Generic;
using System.Linq;
public class Presupuestos{
    private int idPresupuesto;
    private Clientes cliente;
    private DateTime fechaCreacion;
    private List<PresupuestosDetalle> detalle;
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto=value; }
    public Clientes Cliente { get => cliente; set => cliente=value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion=value; }
    public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle=value; }
    public Presupuestos(){
        cliente=new Clientes();
        detalle=new List<PresupuestosDetalle>();
        }
    public Presupuestos(int IdPresupuesto, Clientes Cliente, DateTime FechaCreacion){
        idPresupuesto=IdPresupuesto;
        cliente=Cliente;
        fechaCreacion=FechaCreacion;
        detalle=new List<PresupuestosDetalle>();
    }
    public Presupuestos(AltaPresupuestoViewModel presupuestoVM){
        cliente = new Clientes();
        cliente.ClienteId = presupuestoVM.IdCliente;
        FechaCreacion = presupuestoVM.FechaCreacion;
    }
    public Presupuestos(ModificarPresupuestoViewModel presupuestoVM){
        idPresupuesto = presupuestoVM.IdPresupuesto;
        cliente = new Clientes();
        cliente.ClienteId = presupuestoVM.IdCliente;
        fechaCreacion = presupuestoVM.FechaCreacion;
    }
}

