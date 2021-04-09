using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Task_1
{
    public class Program
    {
        private static BigInteger[] Factorization(BigInteger N)
        {
            if (N < 2)
                throw new ArgumentException("Number less than 2.", nameof(N));
            List<BigInteger> result = new List<BigInteger>();

            for (var i = 2; i <= N; i++)
            {
                if (N % i != 0) continue;
                N = N / i;
                result.Add(i);
                i--;
            }

            return result.ToArray();
        }

        private static Task<BigInteger[]> FactorizationAsync(BigInteger N)
        {
            var result = new TaskCompletionSource<BigInteger[]>();

            new Thread(() =>
                {
                    try
                    {
                        result.SetResult(Factorization(N));
                    }
                    catch (Exception e)
                    {
                        result.SetException(e);
                    }
                }
            ).Start();

            return result.Task;
        }

        private static async Task<BigInteger> GetGCD(BigInteger a, BigInteger b)
        {
            var xBigIntegers = await FactorizationAsync(a);
            var yBigIntegers = await FactorizationAsync(b);

            var k = 0;
            var result = BigInteger.One;
            foreach (var t in xBigIntegers)
            {
                for (var j = k; j < yBigIntegers.Length; j++)
                {
                    if (t == yBigIntegers[j])
                    {
                        result *= t;
                        k++;
                        break;
                    }
                }
            }

            return result;
        }

        public static async Task Main(string[] args)
        {
            Console.Write("32 : ");
            foreach (var bigInteger in Factorization(32))
            {
                Console.Write($"{bigInteger} ");
            }

            Console.WriteLine();
            Console.Write("28 : ");
            foreach (var bigInteger in await FactorizationAsync(28))
            {
                Console.Write($"{bigInteger} ");
            }

            Console.WriteLine();
            Console.Write("GCD(32, 28) = ");
            Console.Write(await GetGCD(32, 28));
            Console.ReadLine();
        }
    }
}