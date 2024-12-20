public class Usuarios{
    private int idUsuario;
    private string usuario;
    private string nombre;
    private string contraseña;
    private Rol rol;
    public int IdUsuario { get => idUsuario ; set => idUsuario = value; }
    public string Usuario { get => usuario ; set => usuario = value; }
    public string Nombre { get => nombre ; set => nombre = value; }
    public string Contraseña { get => contraseña ; set => contraseña = value; }
    public Rol Rol { get => rol ; set => rol = value; }
}

public enum Rol{
    Admin,
    Cliente
}