// Zadatak 2: loša podjela posla
// Brojimo proste brojeve do N. Provjera je li x prost traje dulje što je x veći.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).


using System;
using System.Diagnostics;
using System.Threading.Tasks;

static class Zadatak2_Podjela
{
    const int N = 5_000_000;
    const int K = 4;

    static bool JeProst(int x)
    {
        if (x < 2) return false;

        for (int d = 2; (long)d * d <= x; d++)
            if (x % d == 0) return false;

        return true;
    }

    // Broj prostih brojeva iz [od, do]
    static int BrojProstih(int od, int @do)
    {
        int broj = 0;

        for (int x = od; x <= @do; x++)
            if (JeProst(x)) broj++;

        return broj;
    }

    // Najkrace od tri mjerenja
    static (int Rezultat, long Ms) Izmjeri(Func<int> posao)
    {
        int r = 0;
        long najkrace = long.MaxValue;

        for (int i = 0; i < 3; i++)
        {
            var sat = Stopwatch.StartNew();
            r = posao();
            sat.Stop();

            najkrace = Math.Min(najkrace, sat.ElapsedMilliseconds);
        }

        return (r, najkrace);
    }

    // Podjela na K jednakih dijelova
    static int JednakiRaspon()
    {
        Task<int>[] zadaci = new Task<int>[K];
        long[] vremena = new long[K];

        int velicina = N / K;

        // Kreiranje taskova
        for (int i = 0; i < K; i++)
        {
            int od = i * velicina + 1;
            int doBroja;

            if (i == K - 1)
            {
                doBroja = N;
            }
            else
            {
                doBroja = (i + 1) * velicina;
            }

            int indeks = i;

            zadaci[i] = Task.Run(() =>
            {
                var sat = Stopwatch.StartNew();

                int broj = BrojProstih(od, doBroja);

                sat.Stop();
                vremena[indeks] = sat.ElapsedMilliseconds;

                return broj;
            });
        }

        Task.WaitAll(zadaci);

        int ukupno = 0;

        for (int i = 0; i < K; i++)
        {
            ukupno += zadaci[i].Result;
        }

        Console.WriteLine("Taskovi (ms): " + string.Join(" ", vremena));

        return ukupno;
    }

    // Popravljena podjela na manje raspone
    static int BoljaPodjela()
    {
        int ukupno = 0;
        int velicina = 10_000;

        int brojRaspona = (N + velicina - 1) / velicina;
        int[] rezultati = new int[brojRaspona];

        ParallelOptions opcije = new ParallelOptions
        {
            MaxDegreeOfParallelism = K
        };

        Parallel.For(0, brojRaspona, opcije, i =>
        {
            int od = i * velicina + 1;
            int doBroja = Math.Min((i + 1) * velicina, N);

            rezultati[i] = BrojProstih(od, doBroja);
        });

        for (int i = 0; i < brojRaspona; i++)
        {
            ukupno += rezultati[i];
        }

        return ukupno;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine($"Prosti brojevi do {N}, K = {K}");

        BrojProstih(1, 100_000); // zagrijavanje

        var (slijedno, ms1) = Izmjeri(() => BrojProstih(1, N));

        Console.WriteLine($"Slijedno: {slijedno} ({ms1} ms)");

        var (jednaki, ms2) = Izmjeri(() => JednakiRaspon());

        Console.WriteLine($"Jednaki rasponi, K = {K}: {jednaki} ({ms2} ms)");

        var (bolja, ms3) = Izmjeri(() => BoljaPodjela());

        Console.WriteLine($"Bolja podjela, K = {K}: {bolja} ({ms3} ms)");

        Console.WriteLine();
        Console.WriteLine("Provjera rezultata:");

        if (slijedno == jednaki && slijedno == bolja && slijedno == 348513)
            Console.WriteLine("Svi rezultati su ispravni.");
        else
            Console.WriteLine("Rezultati nisu jednaki.");

        Console.WriteLine();
        Console.WriteLine($"Ubrzanje jednakih raspona: {(double)ms1 / ms2:F2}");
        Console.WriteLine($"Ubrzanje bolje podjele: {(double)ms1 / ms3:F2}");
    }
}
