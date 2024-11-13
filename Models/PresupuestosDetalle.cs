public class PresupuestosDetalle{
        private Productos producto;
        private int cantidad;
        public PresupuestosDetalle(Productos Producto, int Cantidad){
            producto = Producto;
            cantidad = Cantidad;
        }
        public Productos Producto { get => producto; set => producto=value; }
        public int Cantidad { get => cantidad; set => cantidad=value; }
    }
