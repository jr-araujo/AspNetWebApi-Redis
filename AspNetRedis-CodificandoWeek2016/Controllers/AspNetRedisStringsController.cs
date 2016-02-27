using AspNetRedis_CodificandoWeek2016.Models;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNetRedis_CodificandoWeek2016.Controllers
{
    [RoutePrefix("api/AspNetRedis/strings")]
    public class AspNetRedisStringsController : ApiController
    {
        [HttpPost]
        [Route("cadastrar-usuario")]
        public HttpResponseMessage Post_Usuario([FromBody] Models.Usuario usuario)
        {
            string chave = $"urn:usuario:{usuario.Login}";

            using (var redis = new RedisClient("localhost", 6379))
            {
                redis.Db = 0;
                redis.FlushDb();

                var usuarioSerializado = JsonConvert.SerializeObject(usuario);

                //SET
                redis.Set<string>(chave, usuarioSerializado);
            }


            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpGet]
        [Route("obter-usuario/{login}")]
        public Usuario Get_Usuario(string login)
        {
            string chave = $"urn:usuario:{login}";

            using (var redis = new RedisClient("localhost", 6379))
            {
                //GET
                var jsonUsuario = redis.Get<string>(chave);

                return JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            }
        }

        [HttpPut]
        [Route("alterar-usuario/{login}")]
        public dynamic Put_Usuario(string login, [FromBody]Usuario usuario)
        {
            string chave = $"urn:usuario:{login}";

            using (var redis = new RedisClient("localhost", 6379))
            {
                //GET
                var jsonUsuario = redis.Get<string>(chave);

                //GETSET
                var estruturaUsuarioAntigo = redis.GetAndSetValue(chave, JsonConvert.SerializeObject(usuario));

                //GET
                var estruturaUsuarioNovo = redis.Get<string>(chave);

                return new
                {
                    UsuarioAntigo = estruturaUsuarioAntigo,
                    UsuarioNovo = estruturaUsuarioNovo
                };
            }
        }
    }
}