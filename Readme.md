# Aplikacja XAF z EF Pierwsza krew



Generujemy jak zwykle projekt ale zamiast XPO wybieramy EF - tzn. nic nie musimy wybierać bo już jest wybrane. Nazywamy go np `MyXafSolution`



ustawiamy ścieżkę do bazy, jak masz sql w wersji developerskiej to zmień wpis 

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



uwaga! to trzeba ustawić w *Blazor.Server w plik `appseting.json` oraz w  *Win `App.config`





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



proponuje teraz w VisualStudio wejść do menadżera pakietów i zaktualizować wszystko, żeby nie mieć konfliktów wersji szczególnie dl EFCore, które Microsoft aktualizuje częściej niz system Windows.

oczywiście można to zrobić w konsoli:

```powershell
 Update-Package
```



Szukamy w module wspolnym `MyXafSolutionEFCoreDbContext.cs`

i odkomentowujemy kod w metodzie, komentując jednocześnie lub usuwając wywolanie wyjątku:

```csharp
	public MyXafSolutionEFCoreDbContext CreateDbContext(string[] args) {
		//throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
		var optionsBuilder = new DbContextOptionsBuilder<MyXafSolutionEFCoreDbContext>();
		optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=.;Initial Catalog=MyXafSolution");
		optionsBuilder.UseChangeTrackingProxies();
		optionsBuilder.UseObjectSpaceLinkProxies();
		return new MyXafSolutionEFCoreDbContext(optionsBuilder.Options);
	}
```

następnie dodajemy/generujemy kod zarządzający wersjami:

```powershell
add-migration MyInitialMigrationName -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
```





teraz generujemy aktualizacje bazy danych:

```
update-database -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
```





jak zobaczycie taki straszny komuniukat:



```
Microsoft.Data.SqlClient.SqlException (0x80131904): A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - Łańcuch certyfikatów został wystawiony przez urząd, którego nie jest zaufany.)

```

tzn ze tam co przed chwila odkomentowaliśmy trzeba tez dodać zaufanie do certyfikatów :



```csharp
		optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=.;Initial Catalog=MyXafSolution;TrustServerCertificate=true");
```

