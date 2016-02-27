using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetRedis_CodificandoWeek2016.Models
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Login { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Usuario(string login, string nome, string senha)
        {
            Nome = nome;
            Login = login;
            Senha = senha;

            DataCriacao = DateTime.Now;
        }
    }
}