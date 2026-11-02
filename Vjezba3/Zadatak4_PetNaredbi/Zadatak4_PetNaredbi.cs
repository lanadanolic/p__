// Zadatak 4: pet naredbi s predavanja
//   S1: x, y, z := ulaz();   S2: a := p(x);   S3: b := q(y);   S4: c := r(z);   S5: ispiši(a, b, c);
// Metoda za svaku naredbu nosi njezin broj: S1_Ulaz, S2_P, S3_Q, S4_R, S5_Ispis. Svaka traje jednu jedinicu
// posla (oko 0,1 s). S3_QTriJedinice je ista naredba S3, ali q traje tri jedinice.
// VAŽNO: vrijeme mjerite u Release konfiguraciji (Build > Configuration: Release, Ctrl+F5).


using System;
using System.Diagnostics;
using System.Threading.Tasks;

static class Zadatak4_PetNaredbi
{
    // Jedna jedinica posla
    const long JEDINICA = 100_000_000L;

    static long Posao(long jedinica, long ulaz)
    {
        long s = ulaz;

        for (long i = 0; i < jedinica * JEDINICA; i++)
            s += i % 7;

        return s % 1000;
    }

    // S1
    static (long, long, long) S1_Ulaz()
    {
        long s = Posao(1, 0);
        return (s % 7, s % 11, s % 13);
    }

    // S2, S3 i S4
    static long S2_P(long x) => Posao(1, x);
    static long S3_Q(long y) => Posao(1, y);
    static long S3_QTriJedinice(long y) => Posao(3, y);
    static long S4_R(long z) => Posao(1, z);

    // S5
    static long S5_Ispis(long a, long b, long c)
        => Posao(1, a + b + c);

    // Slijedno izvrsavanje
    static long Slijedno(Func<long, long> q)
    {
        var (x, y, z) = S1_Ulaz();

        long a = S2_P(x);
        long b = q(y);
        long c = S4_R(z);

        return S5_Ispis(a, b, c);
    }

    // Paralelno izvrsavanje
    static long Paralelno(Func<long, long> q, int k)
    {
        // S1 - slijedno
        var (x, y, z) = S1_Ulaz();

        long a = 0;
        long b = 0;
        long c = 0;

        // Ogranicenje paralelnog izvrsavanja
        ParallelOptions opcije = new ParallelOptions
        {
            MaxDegreeOfParallelism = k
        };

        // S2, S3 i S4 - paralelno
        Parallel.Invoke(opcije,
            () => a = S2_P(x),
            () => b = q(y),
            () => c = S4_R(z)
        );

        // S5 - nakon zavrsetka svih naredbi
        return S5_Ispis(a, b, c);
    }

    // Najkrace od pet mjerenja
    static (long Rezultat, long Ms) Izmjeri(Func<long> posao)
    {
        posao();

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

        // KORAK 1 - sve naredbe traju jednu jedinicu
        Console.WriteLine("KORAK 1: p, q, r po 1 jedinicu, 3 niti");

        var (r1, t1) = Izmjeri(() => Slijedno(S3_Q));
        var (r2, t2) = Izmjeri(() => Paralelno(S3_Q, 3));

        double s1 = (double)t1 / t2;
        double amdahl1 = 1.0 / (0.4 + 0.6 / 3);

        Console.WriteLine($"Slijedno: {t1} ms");
        Console.WriteLine($"Paralelno: {t2} ms");
        Console.WriteLine($"S = {s1:F2}, Amdahl = {amdahl1:F2}");
        Console.WriteLine($"Rezultat: {(r1 == r2 ? "ispravan" : "KRIV")}");

        Console.WriteLine();

        // KORAK 2 - q traje tri jedinice, 3 niti
        Console.WriteLine("KORAK 2: q traje 3 jedinice, 3 niti");

        double amdahl2 = 1.0 / (2.0 / 7 + (5.0 / 7) / 3);
        double graf2 = 7.0 / 5;

        Console.WriteLine($"Predvidanje Amdahla: {amdahl2:F2}");
        Console.WriteLine($"Predvidanje grafa: {graf2:F2}");

        var (r3, t3) = Izmjeri(() => Slijedno(S3_QTriJedinice));
        var (r4, t4) = Izmjeri(() => Paralelno(S3_QTriJedinice, 3));

        double s2 = (double)t3 / t4;

        Console.WriteLine($"Slijedno: {t3} ms");
        Console.WriteLine($"Paralelno: {t4} ms");
        Console.WriteLine($"S = {s2:F2}");
        Console.WriteLine($"Rezultat: {(r3 == r4 ? "ispravan" : "KRIV")}");

        Console.WriteLine();

        // KORAK 3 - q traje tri jedinice, 2 niti
        Console.WriteLine("KORAK 3: q traje 3 jedinice, 2 niti");

        double amdahl3 = 1.0 / (2.0 / 7 + (5.0 / 7) / 2);
        double graf3 = 7.0 / 5;

        Console.WriteLine($"Predvidanje Amdahla: {amdahl3:F2}");
        Console.WriteLine($"Predvidanje grafa: {graf3:F2}");

        var (r5, t5) = Izmjeri(() => Paralelno(S3_QTriJedinice, 2));

        double s3 = (double)t3 / t5;

        Console.WriteLine($"Slijedno: {t3} ms");
        Console.WriteLine($"Paralelno: {t5} ms");
        Console.WriteLine($"S = {s3:F2}");
        Console.WriteLine($"Rezultat: {(r3 == r5 ? "ispravan" : "KRIV")}");
    }
}
