using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace RedisSubscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-*-*-*-*-*-*-*- Subscribe -*-*-*-*-*-*-*-");

            var connection = ConnectionMultiplexer.Connect("");
            var sub = connection.GetSubscriber();

            sub.Subscribe("Perguntas", (channel, message) => {
                var db = connection.GetDatabase();
                var quebrarPergunta = message.ToString().Split(":");
                var pergunta = quebrarPergunta[0];

                var list = new List<HashEntry>
                {
                    new HashEntry("---", $"Reposta da pergunta {pergunta}")
                };

                Console.WriteLine($"Pergunta: {pergunta} -  Grupo: {list[0].Name} - Resposta: {list[0].Value}");
                db.HashSet(pergunta, list.ToArray());
            });

            Console.ReadKey();
        }
    }
}
