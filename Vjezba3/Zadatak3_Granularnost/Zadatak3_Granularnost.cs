// Zadatak 3: premali zadaci
// Na svaki element polja primjenjuje se funkcija F, koja traje svega nekoliko nanosekundi.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).


using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;

static class Zadatak3_Granularnost
{
    const int N = 1_000_000;
    const int PROLAZA = 20;

    // Vrlo malo posla po elementu
    static double F(double x) => Math.Sqrt(x * x + 1.0);

    // Najkrace od pet mjerenja, nakon jednog zagrijavanja
    static double Izmjeri(Action prolaz)
    {
        prolaz();

        double najkrace = double.MaxValue;

        for (int m = 0; m < 5; m++)
        {
            var sat = Stopwatch.StartNew();

            for (int p = 0; p < PROLAZA; p++)
                prolaz();

            sat.Stop();

            najkrace = Math.Min(najkrace, sat.Elapsed.TotalMilliseconds);
        }

        return najkrace;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var a = new double[N];

        for (int i = 0; i < N; i++)
            a[i] = i;

        Console.WriteLine($"N = {N} elemenata, {PROLAZA} prolaza, jezgri (logickih): {Environment.ProcessorCount}");

        // 1. Slijedno izvodenje
        double slijedno = Izmjeri(() =>
        {
            for (int i = 0; i < N; i++)
                a[i] = F(a[i]);
        });

        Console.WriteLine($"Slijedno: {slijedno:F1} ms");

        // 2. Parallel.For - svaki element jedna iteracija
        double paralelno = Izmjeri(() =>
        {
            Parallel.For(0, N, i =>
            {
                a[i] = F(a[i]);
            });
        });

        double s1 = slijedno / paralelno;

        Console.WriteLine($"Parallel.For po elementu: {paralelno:F1} ms  S = {s1:F2}");

        // 3. Parallel.ForEach - automatski rasponi
        double rasponi = Izmjeri(() =>
        {
            Parallel.ForEach(Partitioner.Create(0, N), r =>
            {
                for (int i = r.Item1; i < r.Item2; i++)
                {
                    a[i] = F(a[i]);
                }
            });
        });

        double s2 = slijedno / rasponi;

        Console.WriteLine($"Parallel.ForEach, rasponi: {rasponi:F1} ms  S = {s2:F2}");

        // 4. Razlicite velicine raspona
        Console.WriteLine();
        Console.WriteLine($"{"Velicina raspona",18} {"Vrijeme",12} {"S",8}");

        int[] velicine = { 10, 100, 1000, 10000, 100000, 500000 };

        double najboljeVrijeme = double.MaxValue;
        int najboljaVelicina = 0;

        foreach (int velicina in velicine)
        {
            double vrijeme = Izmjeri(() =>
            {
                Parallel.ForEach(Partitioner.Create(0, N, velicina), r =>
                {
                    for (int i = r.Item1; i < r.Item2; i++)
                    {
                        a[i] = F(a[i]);
                    }
                });
            });

            double s = slijedno / vrijeme;

            Console.WriteLine($"{velicina,18} {vrijeme,10:F1} ms {s,8:F2}");

            if (vrijeme < najboljeVrijeme)
            {
                najboljeVrijeme = vrijeme;
                najboljaVelicina = velicina;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Najbolja velicina raspona: {najboljaVelicina}");
        Console.WriteLine($"Najkrace vrijeme: {najboljeVrijeme:F1} ms");
    }
}
