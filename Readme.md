# Aplikaca XAf z EF Pierwsza krew



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