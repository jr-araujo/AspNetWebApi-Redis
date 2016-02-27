using AspNetRedis_CodificandoWeek2016.Models;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNetRedis_CodificandoWeek2016.Controllers
{
    [RoutePrefix("api/AspNetRedis/lists")]
    public class AspNetRedisListsController : ApiController
    {
        private readonly string chave = "usuarios";

        [HttpPost]
        [Route("cadastrar-lote-usuarios")]
        public dynamic Post_Usuarios([FromBody] List<Usuario> usuarios)
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                //LPUSH
                foreach (var usuario in usuarios)
                {
                    redis.PushItemToList(chave, JsonConvert.SerializeObject(usuario));
                }
                
                return new
                {
                    Usuarios = usuarios
                };
            }
        }

        [HttpGet]
        [Route("retirar-usuario-da-lista")]
        public dynamic Get_Usuarios()
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                //LPOP
                var itemRetiradoDaLista = redis.PopItemFromList(chave);
                                
                return new
                {
                    UsuarioRetiradoDaLista = itemRetiradoDaLista
                };
            }
        }


        [HttpPost]
        [Route("remover-usuarios-da-lista/{inicio:int}/{fim:int}")]
        public HttpResponseMessage Get_Usuarios(int inicio, int fim)
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                //LTRIM
                redis.LTrim(chave, inicio, fim);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}