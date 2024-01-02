using Microsoft.EntityFrameworkCore;
using Demo.Models;
using Demo.Services.Contrato;

namespace Demo.Services.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private  CsharpContext _context;
        
        public UsuarioService(CsharpContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginUsuario(Usuario modelo)
        {
            try { 
            
            _context.Usuarios.Find(modelo);
            await _context.SaveChangesAsync();
            return true;
            
            
            }catch (Exception ex) {
                throw ex;
            }

        }

        public async Task<bool> RegisterUsuario(Usuario modelo)
        {
            try
            {

                _context.Usuarios.Add(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Usuario>> GetListUsuario()
        {
            try 
            { 
                List<Usuario> list = new List<Usuario>();
                list = await _context.Usuarios.ToListAsync();
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            try
            {
                Usuario? encontrado = new Usuario();
                encontrado=await _context.Usuarios.Include(usr=> usr.Id)
                .Where(usr=>usr.Id==id).FirstOrDefaultAsync();
                return encontrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Usuario> AddUsuario(Usuario modelo)
        {
            try
            {
                
                _context.Usuarios.Add(modelo);
                await _context.SaveChangesAsync();
                return modelo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public async Task<bool> UpdateUsuario(Usuario modelo)
        {
            try
            {
                _context.Usuarios.Update(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUsuario(Usuario modelo)
        {
            try
            {
                _context.Usuarios.Remove(modelo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
