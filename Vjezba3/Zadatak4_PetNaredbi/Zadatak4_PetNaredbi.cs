// Zadatak 4: pet naredbi s predavanja
//   S1: x, y, z := ulaz();   S2: a := p(x);   S3: b := q(y);   S4: c := r(z);   S5: ispiši(a, b, c);
// Metoda za svaku naredbu nosi njezin broj: S1_Ulaz, S2_P, S3_Q, S4_R, S5_Ispis. Svaka traje jednu jedinicu
// posla (oko 0,1 s). S3_QTriJedinice je ista naredba S3, ali q traje tri jedinice.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).

using System.Diagnostics;

static class Zadatak4_PetNaredbi
{
    // Jedna jedinica posla (oko 0,1 s u Release)
    const long JEDINICA = 100_000_000L;

    static long Posao(long jedinica, long ulaz)
    {
        long s = ulaz;
        for (long i = 0; i < jedinica * JEDINICA; i++) s += i % 7;
        return s % 1000;
    }

    // S1: x, y, z := ulaz()
    static (long, long, long) S1_Ulaz()
    {
        long s = Posao(1, 0);
        return (s % 7, s % 11, s % 13);
    }

    static long S2_P(long x) => Posao(1, x);              // S2: a := p(x)
    static long S3_Q(long y) => Posao(1, y);              // S3: b := q(y)
    static long S3_QTriJedinice(long y) => Posao(3, y);   // S3, ali q traje tri jedinice
    static long S4_R(long z) => Posao(1, z);              // S4: c := r(z)

    // S5: ispiši(a, b, c); i ispis traje jednu jedinicu, kao na predavanju
    static long S5_Ispis(long a, long b, long c) => Posao(1, a + b + c);

    static long Slijedno(Func<long, long> q)
    {
        var (x, y, z) = S1_Ulaz();       // S1
        long a = S2_P(x);                // S2
        long b = q(y);                   // S3: S3_Q ili S3_QTriJedinice
        long c = S4_R(z);                // S4
        return S5_Ispis(a, b, c);        // S5
    }

    static (long Rezultat, long Ms) Izmjeri(Func<long> posao)
    {
        posao();   // zagrijavanje
        long r = 0, najkrace = long.MaxValue;
        for (int i = 0; i < 5; i++)
        {
            var sat = Stopwatch.StartNew();
            r = posao();
            sat.Stop();
            najkrace = Math.Min(najkrace, sat.ElapsedMilliseconds);
        }
        return (r, najkrace);
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var (r1, t1) = Izmjeri(() => Slijedno(S3_Q));
        Console.WriteLine($"Slijedno, S3_Q:            {t1} ms  (rezultat {r1})");
        var (r3, t3) = Izmjeri(() => Slijedno(S3_QTriJedinice));
        Console.WriteLine($"Slijedno, S3_QTriJedinice: {t3} ms  (rezultat {r3})");
    }
}
