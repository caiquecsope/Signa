using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy("mypolicy",
    policy => {

    policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials();} 
    );
    
});
var app = builder.Build();
app.UseCors("mypolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pessoa", () =>
{
              using (var connection = new MySqlConnection("Server=localhost;Database=signa;Uid=root;Pwd=1234;"))
            {
                var result = connection.Query<Pessoa>("SELECT PESSOA_ID, NOME_FANTASIA, CNPJ_CPF FROM Pessoa");

                return result;
            }
});

app.MapPost("/pessoa",(Pessoa pessoa)=>{
     using (var connection = new MySqlConnection("Server=localhost;Database=signa;Uid=root;Pwd=1234;"))
            {
                var sql = $"update Pessoa SET NOME_FANTASIA ='{pessoa.NOME_FANTASIA}' where PESSOA_ID = {pessoa.PESSOA_ID} ";
                var result = connection.Query<Pessoa>(sql);

                var resultSelect = connection.QueryFirst<Pessoa>($"SELECT * FROM Pessoa where PESSOA_ID = {pessoa.PESSOA_ID} ");

                return resultSelect;

            }
});
app.Run();


public class Pessoa
    {
        public string NOME_FANTASIA { get; set; }
        public string CNPJ_CPF { get; set; }
        public int PESSOA_ID { get; set; }
    }