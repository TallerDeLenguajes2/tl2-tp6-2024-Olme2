public interface IUsuariosRepository{
    public Usuarios GetUsuarios(string usuario, string contraseña);
    public void CrearUsuario(Usuarios usuario);
}