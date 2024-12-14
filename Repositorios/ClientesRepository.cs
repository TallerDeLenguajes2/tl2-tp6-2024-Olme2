using Microsoft.Data.Sqlite;

class ClientesRepository{
    public List<Clientes> ListarClientesGuardados(){
        List<Clientes> clientes = new List<Clientes>();
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString= @"SELECT * FROM Clientes;";
        using(SqliteConnection connection = new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            using(SqliteDataReader reader = command.ExecuteReader()){
                while(reader.Read()){
                    Clientes cliente = new Clientes(Convert.ToInt32(reader["ClienteId"]), reader["Nombre"].ToString(), reader["Email"].ToString(), reader["Telefono"].ToString());
                    clientes.Add(cliente);
                }
            }
            connection.Close();
        }
        return clientes;
    }
    public int BuscarIdCorrespondiente() {
        string connectionString = @"Data Source=Tienda.db; Cache=Shared";
        string queryString = @"SELECT ClienteId FROM Clientes";
        List<int> ids = new List<int>();
        using (SqliteConnection connection = new SqliteConnection(connectionString)) {
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            using (SqliteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    ids.Add(Convert.ToInt32(reader["ClienteId"]));
                }
            }
            connection.Close();
        }
        if (ids.Count == 0){
            return 1;
        }       
        for(int i=1; i<ids[^1]; i++){
            if(!ids.Contains(i)){
                return i; 
            }
        }       
        return ids[^1] + 1;
    }   
    public void CrearCliente(Clientes cliente){
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString= @"INSERT INTO Clientes (ClienteId, Nombre, Email, Telefono) VALUES (@id, @nombre, @mail, @tel);";
        using(SqliteConnection connection = new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", cliente.ClienteId);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@mail", cliente.Email);
            command.Parameters.AddWithValue("@tel", cliente.Telefono);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public Clientes ObtenerClientePorId(int id){
        Clientes cliente= new Clientes();
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString= @"SELECT * FROM Clientes WHERE ClienteId=@id;";
        using(SqliteConnection connection= new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader= command.ExecuteReader()){
                if(reader.Read()){
                    cliente.ClienteId= Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre= reader["Nombre"].ToString();
                    cliente.Email= reader["Email"].ToString();
                    cliente.Telefono= reader["Telefono"].ToString();
                }
            }
            connection.Close();
        }
        return cliente;
    }
    public void ModificarCliente(Clientes cliente){
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString= @"UPDATE Clientes SET Nombre=@nombre, Email=@mail, Telefono=@tel WHERE ClienteId=@id;";
        using(SqliteConnection connection= new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@mail", cliente.Email);
            command.Parameters.AddWithValue("@tel", cliente.Telefono);
            command.Parameters.AddWithValue("@id", cliente.ClienteId);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void EliminarCliente(int id){
        string connectionString= @"DataSource=Tienda.db; Cache=Shared";
        string queryString= @"DELETE FROM Clientes WHERE ClienteId=@Id;";
        using(SqliteConnection connection= new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command= new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}