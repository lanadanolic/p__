// Zadatak 1: izmjereno ubrzanje i Amdahlov zakon
// Program zbraja brojeve od 1 do N. Udio f posla mora se izvesti slijedno, a ostatak se smije dijeliti.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).


using System;
using System.Diagnostics;
using System.Threading.Tasks;

static class Zadatak1_Amdahl
{
    const long N = 2_000_000_000L;

    // Zbroj brojeva iz [od, do]
    static long Zbroji(long od, long @do)
    {
        long s = 0;

        for (long i = od; i <= @do; i++)
            s += i;

        return s;
    }

    // Najkrace od tri mjerenja
    static (long Rezultat, long Ms) Izmjeri(Func<long> posao)
    {
        long r = 0, najkrace = long.MaxValue;

        for (int i = 0; i < 3; i++)
        {
            var sat = Stopwatch.StartNew();
            r = posao();
            sat.Stop();

            najkrace = Math.Min(najkrace, sat.ElapsedMilliseconds);
        }

        return (r, Math.Max(1, najkrace));
    }

    // Slijedni dio, zatim paralelni dio
    static long Program(double f, int k)
    {
        long ns = (long)(f * N);

        // Slijedni dio
        long zbroj = Zbroji(1, ns);

        // Paralelni dio
        long preostalo = N - ns;
        long velicina = preostalo / k;

        Task<long>[] zadaci = new Task<long>[k];

        for (int i = 0; i < k; i++)
        {
            long od = ns + i * velicina + 1;
            long doBroja;

            if (i == k - 1)
                doBroja = N;
            else
                doBroja = ns + (i + 1) * velicina;

            zadaci[i] = Task.Run(() => Zbroji(od, doBroja));
        }

        Task.WaitAll(zadaci);

        for (int i = 0; i < k; i++)
        {
            zbroj += zadaci[i].Result;
        }

        return zbroj;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        long ocekivano = N * (N + 1) / 2;

        Console.WriteLine($"Jezgri (logickih): {Environment.ProcessorCount}");

        foreach (double f in new[] { 0.0, 0.1, 0.5 })
        {
            Console.WriteLine();
            Console.WriteLine($"f = {f:F1}");
            Console.WriteLine($"{"k",-5} {"vrijeme",-12} {"S",-10} {"Amdahl",-10} {"e",-10} {"zbroj"}");

            long t1 = 0;

            foreach (int k in new[] { 1, 2, 4, 8, 16 })
            {
                var (r, ms) = Izmjeri(() => Program(f, k));

                if (k == 1)
                    t1 = ms;

                double S = (double)t1 / ms;

                double amdahl = 1.0 / (f + (1.0 - f) / k);

                string e = "-";

                if (k > 1)
                {
                    double karpFlatt = (1.0 / S - 1.0 / k)
                                      / (1.0 - 1.0 / k);

                    e = karpFlatt.ToString("F3");
                }

                string provjera;

                if (r == ocekivano)
                    provjera = "ispravan";
                else
                    provjera = "KRIV";

                Console.WriteLine($"{k,-5} {ms + " ms",-12} {S,-10:F2} {amdahl,-10:F2} {e,-10} {provjera}");
            }
        }
    }
}
