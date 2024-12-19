public interface IClientesRepository{
    List<Clientes> ListarClientesGuardados();
    void CrearCliente(Clientes clinete);
    Clientes ObtenerClientePorId(int id);
    void ModificarCliente(Clientes cliente);
    void EliminarCliente(int id);
}