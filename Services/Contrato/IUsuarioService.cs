using Demo.Models;
namespace Demo.Services.Contrato
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetListUsuario();
        Task<Usuario> GetUsuario(int id);
        Task<Usuario> AddUsuario(Usuario modelo);
        Task<bool> UpdateUsuario(Usuario modelo);
        Task<bool> DeleteUsuario(Usuario modelo);
        Task<bool> RegisterUsuario(Usuario modelo);
        Task<bool> LoginUsuario(Usuario modelo);
    }
}
