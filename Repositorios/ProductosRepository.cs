using Microsoft.Data.Sqlite;

class ProductosRepository{
    public void CrearNuevoProducto(Productos producto){
        string connectionString=@"Data Source=Tienda.db; Cache=Shared";
        string queryString=@"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
        using(SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
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
    public Productos? ObtenerDetallesDeProductoPorId(int id){
        Productos? producto=null;
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
        string queryString=@"SELECT MAX(idProducto) AS idProducto FROM Productos";
        int id=0;
        using (SqliteConnection connection=new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader=command.ExecuteReader()){
                if(reader.Read()){
                    id=Convert.ToInt32(reader["idProducto"]);
                }
                else
                {
                    id = 999;
                }
            }
            connection.Close();
        }
        return id;
    }
}