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
            najkrace = Math.Min(najkrace, sat.ElapsedMilliseconds);
        }
        return (r, najkrace);
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
