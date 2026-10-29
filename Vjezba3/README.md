# Vježba 3

Početni projekti za studente. Zadaci su opisani u `../Vjezba3_upute.html`.

## Struktura

Svaki zadatak je zaseban projekt u rješenju `Vjezba3.sln`, pa se pokreće samostalno, bez izbornika. Cijeli zadatak, zajedno s početnim naredbama, nalazi se u jednoj datoteci.

| Zadatak | Mapa projekta | Datoteka |
|---|---|---|
| 1 | `Zadatak1_Amdahl/` | `Zadatak1_Amdahl/Zadatak1_Amdahl.cs` |
| 2 | `Zadatak2_Podjela/` | `Zadatak2_Podjela/Zadatak2_Podjela.cs` |
| 3 | `Zadatak3_Granularnost/` | `Zadatak3_Granularnost/Zadatak3_Granularnost.cs` |
| 4 | `Zadatak4_PetNaredbi/` | `Zadatak4_PetNaredbi/Zadatak4_PetNaredbi.cs` |

## Otvaranje i pokretanje

- **Visual Studio:** otvorite `Vjezba3.sln`. Desni klik na projekt zadatka > *Set as Startup Project*, zatim F5 (ili Ctrl+F5).
- **JetBrains Rider:** File > Open i odaberite `Vjezba3.sln`. U popisu konfiguracija pokretanja (gore desno) odaberite projekt zadatka i pokrenite ga.
- **Visual Studio Code:** File > Open Folder i odaberite ovu mapu. Instalirajte predloženo proširenje C# Dev Kit (ili C#). Pokretanje: F5, konfiguracija »Vježba 3: zadatak N (Debug)«. Za mjerenje vremena (svi zadaci) odaberite konfiguraciju **Release**.
- **Naredbeni redak:** svaki zadatak pokreće se iz svoje mape, samo s `dotnet run`, npr. `cd Zadatak1_Amdahl` pa `dotnet run`. Za mjerenja: `dotnet run -c Release`. Iz ove mape radi i `dotnet run --project Zadatak1_Amdahl`.

Projekti ciljaju .NET 10. Ako imate samo Visual Studio 2022, u datoteci `.csproj` svakog zadatka (`<TargetFramework>`) promijenite `net10.0` u `net8.0`, a u `.vscode/launch.json` isto u putanjama programa.
