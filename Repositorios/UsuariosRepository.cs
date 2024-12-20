using Microsoft.Data.Sqlite;
using SQLitePCL;

public class UsuariosRepository : IUsuariosRepository{
    string connectionString;
    public UsuariosRepository(){
        connectionString = @"Data Source = Tienda.db;Cache=Shared;";
    }
    public Usuarios GetUsuarios(string user, string contraseña){
        Usuarios usuario = null;
        string queryString = @"SELECT * FROM Usuarios WHERE Usuario=@usuario AND Contraseña=@contraseña;";
        using(SqliteConnection connection = new SqliteConnection(connectionString)){
            connection.Open();
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@usuario", user);
            command.Parameters.AddWithValue("@contraseña", contraseña);
            using(SqliteDataReader reader = command.ExecuteReader()){
                if(reader.Read()){
                    usuario = new Usuarios();
                    usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                    usuario.Usuario = reader["Usuario"].ToString();
                    usuario.Nombre = reader["Nombre"].ToString();
                    usuario.Contraseña = reader["Contraseña"].ToString();
                    usuario.Rol = (Rol)Convert.ToInt32(reader["IdRol"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }
}