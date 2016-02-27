using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetRedis_CodificandoWeek2016.Models
{
    public class RedisConnectionManager
    {
        private static readonly object InternalLock = new object();
        private static IRedisClientsManager _connection;

        public static IRedisClient RedisConnection
        {
            get
            {
                if (_connection == null)
                {
                    lock (InternalLock)
                    {
                        if (_connection == null)
                        {
                            var connectionString = "@localhost:6379";

                            _connection = new PooledRedisClientManager(new string[] { connectionString });
                        }
                    }
                }

                return _connection.GetClient();
            }
        }
    }
}