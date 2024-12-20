public class LoginViewModel{
    private string usuario;
    private string contraseña;
    private string error;
    private bool autenticado;
    public string Usuario { get => usuario; set => usuario = value;}
    public string Contraseña { get => contraseña; set => contraseña = value;}
    public string Error { get => error; set => error = value;}
    public bool Autenticado { get => autenticado; set => autenticado = value; }
}