using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Data.Sqlite;
class PresupuestosRepository{
    public void CrearPresupuesto(Presupuestos presupuesto){
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString=@"INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@IdPresupuesto, @nombreDestinatario, @fechaCreacion)";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@IdPresupuesto", presupuesto.IdPresupuesto);
            command.Parameters.AddWithValue("@nombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fechaCreacion", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Presupuestos> ListarPresupuestosGuardados(){
        List<Presupuestos> presupuestos=new List<Presupuestos>();
        string connectionString=@"DataSource=Tienda.db; Cache=Shared";
        string queryString=@"SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos;";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    Presupuestos presupuesto=new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }
     public Presupuestos ObtenerPresupuestoPorId(int id)
    {
        Presupuestos presupuesto = new Presupuestos();
        string connectionString = @"Data Source = Tienda.db;Cache=Shared";
        string query = @"SELECT P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion, PR.idProducto, PR.Descripcion AS Producto, PR.Precio, PD.Cantidad FROM Presupuestos P LEFT JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto LEFT JOIN Productos PR ON PD.idProducto = PR.idProducto WHERE P.idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int aux = 1;
            using (SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    if(aux == 1){
                        presupuesto = new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    if(!reader.IsDBNull(reader.GetOrdinal("idProducto"))){
                        Productos producto = new Productos(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto,Convert.ToInt32(reader["Cantidad"]));
                        presupuesto.Detalle.Add(detalle);
                    }
                    aux++;
                }
            }
            connection.Close();
        }
        return presupuesto;
    }
    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad){
        ProductosRepository repositorioProductos = new ProductosRepository();
        string connectionString=@"Data Source = Tienda.db;Cache=Shared";
        string checkQuery=@"SELECT COUNT(1) FROM PresupuestosDetalle WHERE idPresupuesto=@idPresupuesto AND idProducto=@idProducto";
        string updateQuery=@"UPDATE PresupuestosDetalle SET Cantidad=Cantidad+@cantidad WHERE idPresupuesto=@idPresupuesto AND idProducto=@idProducto";
        string insertQuery=@"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
        using (SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand checkCommand = new SqliteCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            checkCommand.Parameters.AddWithValue("@idProducto", idProducto);
            int count=Convert.ToInt32(checkCommand.ExecuteScalar());
            if(count>0){
                SqliteCommand updateCommand=new SqliteCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@cantidad", cantidad);
                updateCommand.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                updateCommand.Parameters.AddWithValue("@idProducto", idProducto);
                updateCommand.ExecuteNonQuery();
            }else{
                SqliteCommand insertCommand=new SqliteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@idPresupuesto",idPresupuesto);
                insertCommand.Parameters.AddWithValue("@idProducto",idProducto);
                insertCommand.Parameters.AddWithValue("@cantidad",cantidad);
                insertCommand.ExecuteNonQuery();
            }
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
    public int BuscarIdMasGrande(){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"SELECT idPresupuesto FROM Presupuestos";
        List<int> ids=new List<int>();
        using (SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    ids.Add(Convert.ToInt32(reader["IdPresupuesto"]));
                }
            }
            connection.Close();
        }
        if(ids.Count == 0){
            return 1;
        }
        ids.Sort();
        for(int i=1; i<ids[^1]; i++){
            if(!ids.Contains(i)){
                return i;
            }
        }
        return ids[^1]+1;
    }
    public bool SeEncuentraProductoPorId(int id){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"SELECT idProducto FROM Presupuestos INNER JOIN PresupuestosDetalle USING(idPresupuesto) WHERE idProducto=@id LIMIT 1;";
        bool encontro;
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader=command.ExecuteReader()){
                if(reader.Read()){
                    encontro= true;
                }else{
                    encontro= false;
                }
            }
        }
        return encontro;
    }
    public List<PresupuestosDetalle> MostrarDetallePorId(int id){
        ProductosRepository repoProductos=new ProductosRepository();
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"SELECT idProducto, cantidad FROM PresupuestosDetalle WHERE idPresupuesto=@id;";
        List<PresupuestosDetalle> detalles=new List<PresupuestosDetalle>();
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    Productos producto=repoProductos.ObtenerDetallesDeProductoPorId(Convert.ToInt32(reader["idProducto"]));
                    int cantidad=Convert.ToInt32(reader["cantidad"]);
                    PresupuestosDetalle detalle=new PresupuestosDetalle(producto, cantidad);
                    detalles.Add(detalle);
                }
            }
            connection.Close();
        }
        return detalles;
    }
    public void EliminarProducto(int idPresupuesto, int idProducto, int cantVieja, int cantNueva){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string deleteQuery=@"DELETE FROM PresupuestosDetalle WHERE idPresupuesto=@idPresu AND idProducto=@idProd;";
        string updateQuery=@"UPDATE PresupuestosDetalle SET Cantidad=@cantidadNueva WHERE idPresupuesto=@idPresu AND idProducto=@idProd;";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand();
            command.Connection=connection;
            command.Parameters.AddWithValue("@idPresu", idPresupuesto);
            command.Parameters.AddWithValue("@idProd", idProducto);
            if(cantNueva==0){
                command.CommandText=deleteQuery;
                command.ExecuteNonQuery();
            }else{
                command.CommandText=updateQuery;
                command.Parameters.AddWithValue("@cantidadNueva", cantNueva);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    public void ModificarPresupuesto(int idPresupuesto, string NombreDestinatario, DateTime fecha){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"UPDATE Presupuestos SET NombreDestinatario=@nombre, FechaCreacion=@fech WHERE idPresupuesto=@idPresu;";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString ,connection);
            command.Parameters.AddWithValue("@nombre", NombreDestinatario);
            command.Parameters.AddWithValue("@fech", fecha);
            command.Parameters.AddWithValue("@idPresu", idPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}