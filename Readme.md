# Aplikacja XAF z EF Pierwsza krew



Notatki z walki z tutorialem : 

[In-Depth .NET 6+ WinForms & Blazor UI Tutorial (Employee Manager) | eXpressApp Framework | DevExpress Documentation](https://docs.devexpress.com/eXpressAppFramework/402125/getting-started/in-depth-tutorial-blazor)



Z tego względu ze DevExpress rekomenduje przejście z XPO na EF, aktualizuję swoja wiedzę o XAF + EF



[Why We Recommend EF Core over XPO for New Development | eXpressApp Framework | DevExpress Documentation](https://docs.devexpress.com/eXpressAppFramework/404186/why-we-recommend-ef-core-over-xpo)



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

Instalujemy pierdolety potrzebne EF do aktualizacji bazy :

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



Szukamy w module wspólnym `MyXafSolutionEFCoreDbContext.cs`

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



```csharp
Microsoft.Data.SqlClient.SqlException (0x80131904): A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - Łańcuch certyfikatów został wystawiony przez urząd, którego nie jest zaufany.)

```

tzn ze tam co przed chwila odkomentowaliśmy trzeba tez dodać zaufanie do certyfikatów :



```csharp
optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=.;Initial Catalog=MyXafSolution;TrustServerCertificate=true");
```





Całość powinna dać sie uruchomić, jak nie to sugeruje przejrzenie tego co robiliscie, bo skoro takiemu jełopowi jak ja sie udalo a wam nie to wnioski wyciągnijcie sami ...



rozbowujemy nasza klase Employee o dodatkowe pola:



```cs

public class Employee : BaseObject
{

    public virtual String FirstName { get; set; }

    public virtual String LastName { get; set; }

    public virtual String MiddleName { get; set; }

    public virtual DateTime? Birthday { get; set; }

    //Use this attribute to hide or show the editor of this property in the UI.
    [Browsable(false)]
    public virtual int TitleOfCourtesy_Int { get; set; }

    //Use this attribute to exclude the property from database mapping.
    [NotMapped]
    public virtual TitleOfCourtesy TitleOfCourtesy { get; set; }

}
public enum TitleOfCourtesy
{
    Dr,
    Miss,
    Mr,
    Mrs,
    Ms
}
```



i uruchamiamy ponownie ...



wywaliło sie ... goście od DevExpress mają szczęście ze nie ma ich w pobliżu jednak w XPO bylo latwiej. Trzeba znów zrobić upgrade:

```
PM> add-migration MyInitialMigrationName -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
Build started...
Build succeeded.

PM> update-database -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
Build started...
Build succeeded.
No migrations were applied. The database is already up to date.
Done.
PM> 
```





POWYZSZA APLIAKCJA POWINNA DAC SIE URUCHOMIC !!!

Jak coo snie dziala to odsylam do oryginalnego tutoriala :

[Extend the Data Model | eXpressApp Framework | DevExpress Documentation](https://docs.devexpress.com/eXpressAppFramework/404256/getting-started/in-depth-tutorial-blazor/define-data-model-and-set-initial-data/define-data-model-and-set-initial-data-with-ef-core/extend-the-data-model?p=netframework)



```csharp
[DefaultClassOptions]
//Use this attribute to specify the caption format for the objects of the entity class.
[ObjectCaptionFormat("{0:FullName}")]
[DefaultProperty(nameof(FullName))]
public class Employee : BaseObject
{

...

    [SearchMemberOptions(SearchMemberMode.Exclude)]
    public String FullName
    {
        get { return ObjectFormatter.Format(FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public String DisplayName
    {
        get { return FullName; }
    }

    public static String FullNameFormat = "{FirstName} {MiddleName} {LastName}";

    //Use this attribute to specify the maximum number of characters that users can type in the editor of this property.
    [FieldSize(255)]
    public virtual String Email { get; set; }

    //Use this attribute to define a pattern that the property value must match.
    [RuleRegularExpression(@"(((http|https)\://)[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])*)|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})", CustomMessageTemplate = @"Invalid ""Web Page Address"".")]
    public virtual string WebPageAddress { get; set; }

    //Use this attribute to specify the maximum string length allowed for this data field.
    [StringLength(4096)]
    public virtual string Notes { get; set; }

}
```



oczywiscie nie zdziwcie sie ze :

![image-20230714001320510](image-20230714001320510.png)



znow urok EF:

```
add-migration MyInitialMigrationNameX1 -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"

update-database -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
```





juz go nie lubie ....

uwaga nazwa migracji `MyInitialMigrationNameX1`musi byc unikalna, bo inaczej bedzi bład ze jest duplkat nazwy: 

```
add-migration MyInitialMigrationNameX1 -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
```

