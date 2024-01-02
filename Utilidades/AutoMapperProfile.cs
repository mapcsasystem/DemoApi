using AutoMapper;
using Demo.DTOs;
using Demo.Models;
using System.Globalization;

namespace Demo.Utilidades
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() { 
        #region Usuario
        CreateMap<Usuario,UsuarioDTO>().ReverseMap();
        #endregion
        }
    }
}
