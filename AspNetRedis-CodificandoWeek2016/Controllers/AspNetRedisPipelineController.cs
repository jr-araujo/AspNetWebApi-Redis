using ServiceStack.Redis;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Web.Http;
using Dapper;
using System.Diagnostics;

namespace AspNetRedis_CodificandoWeek2016.Controllers
{
    [RoutePrefix("api/AspNetRedis/pipeline")]
    public class AspNetRedisPipelineController : ApiController
    {
        [HttpPost]
        [Route("processar-lote-categorias")]
        public string Post_ProcessarLoteCategoria()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (var conexao = new SqlConnection("Server=NTB-ROBERTO\\MSSQLEXPRESS;Database=AspNetRedis;Trusted_Connection=True;"))
            {
                conexao.Open();

                using (var redis = new RedisClient("localhost", 6379))
                {
                    var pipeline = redis.CreatePipeline();

                    var categorias = conexao.Query<Categoria>("SELECT * FROM CATEGORIA (NOLOCK)").ToList();


                    foreach (var categoria in categorias)
                    {
                        pipeline.QueueCommand(s => s.Set($"urn:categoria:{categoria.Id}", categoria));
                    }

                    pipeline.Flush();
                }
            }

            sw.Stop();

            return "Tempo de processamento: " + sw.Elapsed;
        }
    }

    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
    }
}