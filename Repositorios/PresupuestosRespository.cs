using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Data.Sqlite;
class PresupuestosRepository{
    public void CrearPresupuesto(Presupuestos presupuesto){
        ProductosRepository repositorioProductos= new ProductosRepository();
        foreach(PresupuestosDetalle d in presupuesto.Detalle){
            if(repositorioProductos.ObtenerDetallesDeProductoPorId(d.Producto.IdProducto)==null){
                return;
            }
        }
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString1=@"INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@IdPresupuesto, @nombreDestinatario, @fechaCreacion)";
        string queryString2=@"SELECT MAX(idPresupuesto) AS IdMax FROM Presupuestos";
        string queryString3=@"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@IdPresupuesto, @IdProducto, @cantidad)";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString1, connection);
            command.Parameters.AddWithValue("@IdPresupuesto", presupuesto.IdPresupuesto);
            command.Parameters.AddWithValue("@nombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fechaCreacion", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            SqliteCommand command2= new SqliteCommand(queryString2, connection);
            using(SqliteDataReader reader=command2.ExecuteReader()){
                if(reader.Read()){
                    foreach(PresupuestosDetalle d in presupuesto.Detalle){
                        SqliteCommand command3= new SqliteCommand(queryString3, connection);
                        command3.Parameters.AddWithValue("@IdPresupuesto", Convert.ToInt32(reader["IdMax"]));
                        command3.Parameters.AddWithValue("@IdProducto", d.Producto.IdProducto);
                        command3.Parameters.AddWithValue("@cantidad", d.Cantidad);
                        command3.ExecuteNonQuery();
                    }
                }
            }
            connection.Close();
        }
    }
    public List<Presupuestos> ListarPresupuestosGuardados(){
        List<Presupuestos> presupuestos=new List<Presupuestos>();
        string connectionString=@"DataSource=Tienda.db; Cache=Shared";
        string queryString=@"SELECT P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion, PD.idProducto, PD.Descripcion, PD.Precio, D.Cantidad FROM Presupuestos P JOIN PresupuestosDetalle D ON P.idPresupuesto=D.idPresupuesto JOIN Productos PD ON D.idProducto=PD.idProducto ORDER BY P.idPresupuesto;";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                int id=-1;
                Presupuestos ultimo= new Presupuestos(-1, "", DateTime.Now);
                while(reader.Read()){
                    if(id==-1 || id!=Convert.ToInt32(reader["idPresupuesto"])){
                        if(id != -1){
                            presupuestos.Add(ultimo);
                        }
                        ultimo=new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    Productos producto=new Productos(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                    PresupuestosDetalle detalle= new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    ultimo.Detalle.Add(detalle);
                    id=Convert.ToInt32(reader["idPresupuesto"]);
                }
                presupuestos.Add(ultimo);
            }
            connection.Close();
        }
        return presupuestos;
    }
    public Presupuestos? ObtenerPresupuestoPorId(int id){
        Presupuestos? presupuesto=null;
        string connectionString=@"DataSource=Tienda.db; Cache=Shared";
        string queryString=@"SELECT P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion, PD.idProducto, PD.Descripcion, PD.Precio, D.Cantidad FROM Presupuestos P JOIN PresupuestosDetalle D ON P.idPresupuesto=D.idPresupuesto JOIN Productos PD ON D.idProducto=PD.idProducto WHERE P.idPresupuesto=@id;";
        using(SqliteConnection connection = new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            int aux=1;
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    if(aux==1){
                        presupuesto=new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    Productos producto=new Productos(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                    PresupuestosDetalle detalle= new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    if(presupuesto!=null)presupuesto.Detalle.Add(detalle);
                    aux++;
                }
            }
            connection.Close();
        }
        return presupuesto;
    }
    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad){
        ProductosRepository repositorioProductos = new ProductosRepository();
        if(ObtenerPresupuestoPorId(idPresupuesto)==null || repositorioProductos.ObtenerDetallesDeProductoPorId(idProducto) == null){
            return;
        }
        string connectionString=@"Data Source = db/Tienda.db;Cache=Shared";
        string queryString=@"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
        using (SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.Parameters.AddWithValue("@cantidad", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void EliminarPresupuestoPorId(int id){
        string connectionString=@"DataSource=Tienda.db; Cache=Shared";
        string queryString=@"DELETE FROM Presupuestos WHERE idPresupuesto=@idP;";
        string queryString2=@"DELETE FROM PresupuestosDetalle WHERE idPresupuesto=@idD;";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            SqliteCommand command2=new SqliteCommand(queryString2, connection);
            command.Parameters.AddWithValue("@idP", id);
            command2.Parameters.AddWithValue("@idD", id);
            command2.ExecuteNonQuery();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}