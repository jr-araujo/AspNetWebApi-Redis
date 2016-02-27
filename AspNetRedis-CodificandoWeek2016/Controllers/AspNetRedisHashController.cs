using AspNetRedis_CodificandoWeek2016.Models;
using ServiceStack.Redis;
using System.Web.Http;

namespace AspNetRedis_CodificandoWeek2016.Controllers
{
    [RoutePrefix("api/AspNetRedis/hash")]
    public class AspNetRedisHashController : ApiController
    {
        [HttpPost]
        [Route("cadastrar-usuario")]
        public void Post_Usuario([FromBody] Usuario usuario)
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                redis.StoreAsHash<Usuario>(usuario);
            }
        }

        [Route("obter-usuario/{id}")]
        public Usuario Get_Usuario(int id)
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                var usuario = redis.GetFromHash<Usuario>(id);

                return usuario;
            }
        }
    }
}
