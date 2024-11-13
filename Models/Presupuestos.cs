using System.Collections.Generic;
using System.Linq;
    public class Presupuestos{
        private int idPresupuesto;
        private string? nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestosDetalle> detalle;
        public Presupuestos(int IdPresupuesto, string? NombreDestinatario, DateTime FechaCreacion){
            idPresupuesto=IdPresupuesto;
            nombreDestinatario=NombreDestinatario;
            fechaCreacion=FechaCreacion;
            detalle=new List<PresupuestosDetalle>();
        }
        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto=value; }
        public string? NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario=value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion=value; }
        public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle=value; }
        public int MontoPresupuesto(){
            int total=0;
            foreach(PresupuestosDetalle d in Detalle){
                total+=d.Cantidad*d.Producto.Precio;
            }
            return total;
        }
        public double MontoPresupuestoConIva(){
            return MontoPresupuesto()*1.21;
        }
        public int CantidadProductos(){
            int total=0;
            foreach(PresupuestosDetalle d in Detalle){
                total+=d.Cantidad;
            }
            return total;
        }
    }

