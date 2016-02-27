using AspNetRedis_CodificandoWeek2016.Models;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AspNetRedis_CodificandoWeek2016.Controllers
{
    [RoutePrefix("api/AspNetRedis/transaction")]
    public class AspNetRedisTransactionController : ApiController
    {
        [HttpPost]
        [Route("processar-lote-usuarios")]
        public int Post_CadastrarLoteUsuarios([FromBody] List<Usuario> usuarios)
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                IRedisTransaction trans = null;

                try
                {
                    trans = redis.CreateTransaction();

                    using (trans = redis.CreateTransaction())
                    {
                        foreach (var usuario in usuarios)
                        {
                            trans.QueueCommand(s => s.StoreAsHash<Usuario>(usuario));
                        }

                        trans.QueueCommand(s => s.Set<string>("chave1", "valor 1"));
                        trans.QueueCommand(s => s.Set<string>("chave2", "valor 2"));

                        trans.QueueCommand(s => s.StoreAsHash<object>(new
                        {
                            Id = 1,
                            CodigoCarrinho = "AXDG11112016",
                            Nome = "TV LED 32\" LG",
                            Preco = 1500.00,
                            Quantidade = 2
                        }));

                        //throw new Exception("Erro ao salvar Carrinho :: AXDG11112016 ");

                        //EXEC
                        trans.Commit();
                    }

                    return usuarios.Count();
                }
                catch (Exception)
                {
                    //  DISCARD
                    //trans.Rollback();
                    throw;
                }
            }
        }
    }
}
