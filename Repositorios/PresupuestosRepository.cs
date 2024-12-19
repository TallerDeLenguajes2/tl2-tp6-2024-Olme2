using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Data.Sqlite;
class PresupuestosRepository{
    string connectionString;
    public PresupuestosRepository(){
        connectionString = @"DataSource=Tienda.db; Cache=Shared";
    }
    public void CrearPresupuesto(Presupuestos presupuesto){
        string queryString=@"INSERT INTO Presupuestos (ClienteId, FechaCreacion) VALUES (@IdCliente, @fechaCreacion)";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@IdCliente", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@fechaCreacion", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Presupuestos> ListarPresupuestosGuardados(){
        List<Presupuestos> presupuestos=new List<Presupuestos>();
        string queryString=@"SELECT idPresupuesto, FechaCreacion, ClienteId, Nombre, Email, Telefono FROM Presupuestos INNER JOIN Clientes USING(ClienteId);";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    Clientes cliente = new Clientes();
                    cliente.ClienteId=Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre=reader["Nombre"].ToString();
                    cliente.Email=reader["Email"].ToString();
                    cliente.Telefono=reader["Telefono"].ToString();
                    Presupuestos presupuesto=new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), cliente, Convert.ToDateTime(reader["FechaCreacion"]));
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }
     public Presupuestos ObtenerPresupuestoPorId(int id)
    {
        ClientesRepository repo= new ClientesRepository();
        Presupuestos presupuesto = new Presupuestos();
        string query = @"SELECT P.idPresupuesto, ClienteId, P.FechaCreacion, PR.idProducto, PR.Descripcion AS Producto, PR.Precio, PD.Cantidad FROM Presupuestos P LEFT JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto LEFT JOIN Productos PR ON PD.idProducto = PR.idProducto WHERE P.idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int aux = 1;
            using (SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    if(aux == 1){
                        Clientes cliente = repo.ObtenerClientePorId(Convert.ToInt32(reader["ClienteId"]));
                        presupuesto = new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), cliente, Convert.ToDateTime(reader["FechaCreacion"]));
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
    public List<PresupuestosDetalle> MostrarDetallePorId(int id){
        ProductosRepository repoProductos=new ProductosRepository();
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
    public void ModificarPresupuesto(Presupuestos presupuesto){
        string queryString=@"UPDATE Presupuestos SET ClienteId=@IdCliente, FechaCreacion=@fech WHERE idPresupuesto=@idPresu;";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString ,connection);
            command.Parameters.AddWithValue("@IdCliente", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@fech", presupuesto.FechaCreacion);
            command.Parameters.AddWithValue("@idPresu", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}