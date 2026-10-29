// Zadatak 1: izmjereno ubrzanje i Amdahlov zakon
// Program zbraja brojeve od 1 do N. Udio f posla mora se izvesti slijedno, a ostatak se smije dijeliti.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).

using System.Diagnostics;

static class Zadatak1_Amdahl
{
    const long N = 2_000_000_000L;

    // Zbroj brojeva iz [od, do]
    static long Zbroji(long od, long @do)
    {
        long s = 0;
        for (long i = od; i <= @do; i++) s += i;
        return s;
    }

    // Najkraće od tri mjerenja
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

    // Prvih f · N brojeva mora se zbrojiti slijedno, a tek nakon toga ostatak.
    static long Program(double f)
    {
        long ns = (long)(f * N);
        long zbroj = Zbroji(1, ns);          // slijedni dio
        zbroj += Zbroji(ns + 1, N);          // dio koji se smije podijeliti
        return zbroj;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        long ocekivano = N * (N + 1) / 2;
        Console.WriteLine($"Jezgri (logičkih): {Environment.ProcessorCount}");
        foreach (double f in new[] { 0.0, 0.1, 0.5 })
        {
            var (r, ms) = Izmjeri(() => Program(f));
            Console.WriteLine($"f = {f:F1}: {ms} ms, zbroj {(r == ocekivano ? "ispravan" : "KRIV")}");
        }
    }
}
