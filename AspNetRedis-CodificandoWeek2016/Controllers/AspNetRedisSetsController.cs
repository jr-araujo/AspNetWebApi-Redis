using System.Collections.Generic;
using ServiceStack.Redis;
using System.Web.Http;
using System.Text;

namespace AspNetRedis_CodificandoWeek2016.Controllers
{
    [RoutePrefix("api/AspNetRedis/sets")]
    public class AspNetRedisSetsController : ApiController
    {
        public AspNetRedisSetsController()
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                redis.Db = 0;
                redis.FlushDb();

                var redisClient = ((RedisClient)redis);

                redisClient.SAdd("chave-A", Encoding.UTF8.GetBytes("1"));
                redisClient.SAdd("chave-A", Encoding.UTF8.GetBytes("2"));
                redisClient.SAdd("chave-A", Encoding.UTF8.GetBytes("3"));

                redisClient.SAdd("chave-B", Encoding.UTF8.GetBytes("4"));
                redisClient.SAdd("chave-B", Encoding.UTF8.GetBytes("1"));
                redisClient.SAdd("chave-B", Encoding.UTF8.GetBytes("2"));
            }
        }

        [HttpGet]
        [Route("sdiff")]
        public dynamic Get_SDIFF()
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                redis.Db = 0;
                redis.FlushDb();

                var redisClient = ((RedisClient)redis);

                //SDIFF
                var diff = redisClient.SDiff("chave-A", "chave-B");
                var resultadoDiff = Encoding.UTF8.GetString(diff[0]);
                List<string> diferencas = new List<string>();
                foreach (var item in diff)
                {
                    diferencas.Add(Encoding.UTF8.GetString(item));
                }

                return new
                {
                    ValoresDiferentes = diferencas
                };
            }
        }

        [HttpGet]
        [Route("sinter")]
        public dynamic Get_SINTER()
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                var redisClient = ((RedisClient)redis);
                
                //SINTER
                var inter = redisClient.SInter("chave-A", "chave-B");
                List<string> intesecao = new List<string>();
                foreach (var item in inter)
                {
                    intesecao.Add(Encoding.UTF8.GetString(item));
                }

                return new
                {
                    ValoresIntersecao = intesecao
                };
            }
        }

        [HttpGet]
        [Route("sunion")]
        public dynamic Get_SUNION()
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                redis.Db = 0;
                redis.FlushDb();

                var redisClient = ((RedisClient)redis);

                //SUNION
                var resultadoUniao = redisClient.SUnion("chave-A", "chave-B");
                List<string> unioes = new List<string>();
                foreach (var item in resultadoUniao)
                {
                    unioes.Add(Encoding.UTF8.GetString(item));
                }
                
                return new
                {
                    ValoresUniao = unioes
                };                
            }
        }

        [HttpGet]
        [Route("smembers")]
        public dynamic Get_SMEMBERS()
        {
            using (var redis = new RedisClient("localhost", 6379))
            {
                redis.Db = 0;
                redis.FlushDb();

                var redisClient = ((RedisClient)redis);
                
                List<string> membros = new List<string>();

                //SMEMBERS
                string setKey = "setKey";
                int total = 25;
                var temp = "Meu primeiro valor SADD";


                redisClient.Del(setKey);
                for (var i = 0; i < total; ++i)
                {
                    string key = setKey + i;
                    redisClient.SAdd(setKey, Encoding.UTF8.GetBytes(key));
                    redisClient.Set(key, temp + ": " + i);
                }

                var keys = redisClient.SMembers(setKey);
                var results = redisClient.MGet(keys);
                foreach (var item in results)
                {
                    membros.Add(Encoding.UTF8.GetString(item));
                }

                return new
                {
                    Membros = membros
                };
            }
        }
    }
}
