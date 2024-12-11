using Microsoft.Data.Sqlite;

class ProductosRepository{
    public void CrearNuevoProducto(Productos producto){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"INSERT INTO Productos (idProducto, Descripcion, Precio) VALUES (@Id, @Descripcion, @Precio)";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", producto.IdProducto);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void ModificarProducto(Productos producto){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"UPDATE Productos SET Descripcion=@Descripcion, Precio=@Precio WHERE idProducto=@Id";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Id", producto.IdProducto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public List<Productos> ListarProductosRegistrados(){
        List<Productos> productos=new List<Productos>();
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"SELECT * FROM Productos";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    Productos nuevoProducto=new Productos();
                    nuevoProducto.IdProducto=Convert.ToInt32(reader["idProducto"]);
                    nuevoProducto.Descripcion=reader["Descripcion"].ToString();
                    nuevoProducto.Precio=Convert.ToInt32(reader["Precio"]);
                    productos.Add(nuevoProducto);
                }
            }
            connection.Close();
        }
        return productos;
    }
    public Productos ObtenerDetallesDeProductoPorId(int id){
        Productos producto=new Productos();
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"SELECT * FROM Productos WHERE idProducto=@id";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader=command.ExecuteReader()){
                if(reader.Read()){
                    producto=new Productos();
                    producto.IdProducto=Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion=reader["Descripcion"].ToString();
                    producto.Precio=Convert.ToInt32(reader["Precio"]);
                }
            }
            connection.Close();
        }
        return producto;
    }
    public void EliminarProductoPorId(int id){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"DELETE FROM Productos WHERE idProducto=@id";
        using(SqliteConnection connection= new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public int BuscarIdMasGrande(){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"SELECT idProducto FROM Productos";
        List<int> ids=new List<int>();
        using (SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    ids.Add(Convert.ToInt32(reader["idProducto"]));
                }
            }
            connection.Close();
        }
        if(ids.Count == 0){
            return 1;
        }
        ids.Sort();
        for(int i=1; i<=ids[^1]; i++){
            if(!ids.Contains(i)){
                return i;
            }
        }
        return ids[^1]+1;
    }
}