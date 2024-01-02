using System;
using System.Globalization;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Demo.Services.Contrato;
using Demo.Services.Implementacion;
using AutoMapper;
using Demo.DTOs;
using Demo.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


var builder = WebApplication.CreateBuilder(args);
var cultureInfo = new CultureInfo("es-Es");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<CsharpContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


#region PETICIONES

app.MapGet("/user/list", async(
    IUsuarioService _usuarioServicio,
    IMapper _mapper
    ) => 
{ 
    var listaUsuario =await _usuarioServicio.GetListUsuario();
    var listaUsuarioDTO = _mapper.Map<List<UsuarioDTO>>(listaUsuario);
    if(listaUsuarioDTO.Count > 0)
    {
        return Results.Ok(listaUsuarioDTO);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/user/save", async (
    UsuarioDTO modelo,
    IUsuarioService _usuarioServicio,
    IMapper _mapper
    ) =>
{
    var _usuario = _mapper.Map<Usuario>(modelo);
    string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    Match match = Regex.Match(_usuario.Email, pattern);

    if (!match.Success)
    {
        return Results.NotFound("Error de email");
    }
    if (_usuario.Name.Length < 10 && _usuario.Name.Length > 50)
    {
        return Results.NotFound("Error name mayor a 10 y menor a 50");
    }
    var _usuarioCreado = await _usuarioServicio.AddUsuario(_usuario);

    if (_usuarioCreado.Id != 0)
    {
        return Results.Ok(_mapper.Map<UsuarioDTO>(_usuarioCreado));
    }
    else
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.MapPut("/user/update/{id}", async (
    int id,
    UsuarioDTO modelo,
    IUsuarioService _usuarioServicio,
    IMapper _mapper
    ) =>
{
    var _encontrado = await _usuarioServicio.GetUsuario(id);
    if (_encontrado == null)
    {
        return Results.NotFound();
    }
    string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    Match match = Regex.Match(_encontrado.Email, pattern);

    if (!match.Success)
    {
        return Results.NotFound("Error de email");
    }
    if (_encontrado.Name.Length<10 && _encontrado.Name.Length > 50)
    {
        return Results.NotFound("Error name mayor a 10 y menor a 50");
    }
    var _usuario = _mapper.Map<Usuario>(modelo);
    _encontrado.Name = _usuario.Name;
    _encontrado.Email = _usuario.Email;
    _encontrado.Age = _usuario.Age;

    var respuesta = await _usuarioServicio.UpdateUsuario(_encontrado);
    if (respuesta)
    {
        return Results.Ok(_mapper.Map<UsuarioDTO>(_encontrado));
    }
    else
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.MapDelete("/user/update/{id}", async (
    int id,
    IUsuarioService _usuarioServicio
    //IMapper _mapper
    ) =>
{
    var _encontrado = await _usuarioServicio.GetUsuario(id);
    if (_encontrado == null)
    {
        return Results.NotFound();
    }

    var respuesta = await _usuarioServicio.DeleteUsuario(_encontrado);
    if (respuesta)
    {
        return Results.Ok(_encontrado);
    }
    else
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});
#endregion

app.Run();




