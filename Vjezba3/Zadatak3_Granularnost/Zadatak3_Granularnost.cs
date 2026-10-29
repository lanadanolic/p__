// Zadatak 3: premali zadaci
// Na svaki element polja primjenjuje se funkcija F, koja traje svega nekoliko nanosekundi.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).

using System.Diagnostics;

static class Zadatak3_Granularnost
{
    const int N = 1_000_000;
    const int PROLAZA = 20;

    // Vrlo malo posla po elementu
    static double F(double x) => Math.Sqrt(x * x + 1.0);

    // Najkraće od pet mjerenja, nakon jednog zagrijavanja; svako mjerenje je PROLAZA prolaza kroz polje.
    static double Izmjeri(Action prolaz)
    {
        prolaz();
        double najkrace = double.MaxValue;
        for (int m = 0; m < 5; m++)
        {
            var sat = Stopwatch.StartNew();
            for (int p = 0; p < PROLAZA; p++) prolaz();
            sat.Stop();
            najkrace = Math.Min(najkrace, sat.Elapsed.TotalMilliseconds);
        }
        return najkrace;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var a = new double[N];
        for (int i = 0; i < N; i++) a[i] = i;
        Console.WriteLine($"N = {N} elemenata, {PROLAZA} prolaza, jezgri (logičkih): {Environment.ProcessorCount}");

        double slijedno = Izmjeri(() =>
        {
            for (int i = 0; i < N; i++) a[i] = F(a[i]);
        });
        Console.WriteLine($"Slijedno:                    {slijedno,7:F1} ms");
    }
}
