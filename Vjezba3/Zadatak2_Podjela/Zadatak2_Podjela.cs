// Zadatak 2: loša podjela posla
// Brojimo proste brojeve do N. Provjera je li x prost traje dulje što je x veći.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).

using System.Diagnostics;

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

    // Najkraće od tri mjerenja
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


    //podjela na K jednakih dijelova
    //za paralelno brojanje prostih brojeva
    //umj da 1 nit provjerava sve brojeve od 1 do N posao se dijeli na k Taskova
    //svaki task dobiva svoj raspon brojeva, broji koliko je u njemu prostih brojeva i koliko mu je vremena trebalo
    static int JednakiRaspon()
    {
        Task<int>[] zadaci = new Task<int>[K]; //polje taskova
        long[] vremena = new long[K]; //kreiramo polje u koje cemo spremiti vrijeme izvrsavanja

        int velicina = N / K;  //izracun velicine raspona

        //kreiranje taskova
        for (int i = 0; i < K; i++)
        {
            int od = i * velicina + 1;
            int doBroja;

            if (i==K-1)
            {
                doBroja = N;
            }
        }
    }
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine($"Prosti brojevi do {N}, K = {K}");
        BrojProstih(1, 100_000);   // zagrijavanje

        var sat = Stopwatch.StartNew();
        int slijedno = BrojProstih(1, N);
        Console.WriteLine($"Slijedno:                {slijedno}  ({sat.ElapsedMilliseconds} ms)");
    }
}
