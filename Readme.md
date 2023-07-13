# Aplikacja XAF z EF Pierwsza krew



Generujemy jak zwyjkle projekt ale zamiast XPO wybieramy EF - tzn. nic nie musimy wybierać bo już jest wybrane



ustawiamy ścieżkę do bazy, jak masz sql w wersji developerskiej to zmien wpis 

```ini
connectionString="Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=(localdb)\mssqllocaldb;Initial Catalog=MyXafSolution" providerName="System.Data.SqlClient" />
  
```

na 

```ini
connectionString="Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=.;Initial Catalog=MyXafSolution" providerName="System.Data.SqlClient" />
```



```
czyli zamien (localdb)\mssqllocaldb na localhost (lub . w windows)
```

potem definiujemy klasy EF np. :

```csharp

[DefaultClassOptions]
public class Employee : BaseObject
{

    public virtual String FirstName { get; set; }

    public virtual String LastName { get; set; }

    public virtual String MiddleName { get; set; }

}
```

jak widać zero filozofii, nawet prościej niż w xpo



uwaga jak się czepia o certyfikaty do bazy to trzeba dodać by ufał deweloperskim certyfikatom:

`;TrustServerCertificate=true`

```ini
connectionString="Integrated Security=SSPI;MultipleActiveResultSets=True;Data Source=.;Initial Catalog=DXApplicationEF;TrustServerCertificate=true" 
  
```



uwaga! to trzeba ustawić w blazor server w plik appseting.json oraz w * Win App.config





## Aktualizacja bazy danych:

Instalujemy pierdolety potrzebne Ef do aktualizacji bazy :

w package Manager console dla MySolution.Module instalujemy pakiety:



```powershell
Install-Package Microsoft.EntityFrameworkCore.Tools
```

 

```powershell
Update-Package Microsoft.EntityFrameworkCore.Tools
```

mozemy sprawdzic czy bangla: 



```powershell
Get-Help about_EntityFrameworkCore
```



proponuje teraz w VisualStudio wejsc do magaera pakietow i zaktualizować wszystko, żeby nie mieć konfliktów wersji szczególnie dl EFCore, które Microsoft aktualizuje częściej niz system Windows.

oczywiście można to zrobić w konsoli:

```
 Update-Package
```

