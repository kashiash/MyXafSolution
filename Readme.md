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



wiecej o ustawianiu polaczenia do bazy tutaj: https://docs.devexpress.com/eXpressAppFramework/113155/business-model-design-orm/connect-an-xaf-application-to-a-database-provider





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



## Modyfikacja modelu

rozbudowujemy nasza klase Employee o dodatkowe pola:



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







i nasz peikny program powinien w winforms wygladac tak:

![image-20230714002336395](image-20230714002336395.png)





## Dane początkowe



W module wspolnym jest klasa do aktualizacji bazy danych o nazwie `Updater` a w niej metoda:  `UpdateDatabaseAfterUpdateSchema`



wykonywana jest ona przy kazdej aktualizacji bazy danych, mozemy tam dodac dane poczatkowe dla aplikacji :

```csharp
using MySolution.Module.BusinessObjects;
//...

public class Updater : DevExpress.ExpressApp.Updating.ModuleUpdater {
    //...
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();

        Employee employeeMary = ObjectSpace.FirstOrDefault<Employee>(x => x.FirstName == "Mary" && x.LastName == "Tellitson");
        if(employeeMary == null) {
            employeeMary = ObjectSpace.CreateObject<Employee>();
            employeeMary.FirstName = "Mary";
            employeeMary.LastName = "Tellitson";
            employeeMary.Email = "tellitson@example.com";
            employeeMary.Birthday = new DateTime(1980, 11, 27);
        }

        ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
    }
}
```



powiązane linki, ktore moga byc przydatne:

[Data Seeding - EF Core | Microsoft Learn](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding)



mozna uzyc generatora danych testowych https://github.com/bchavez/Bogus



```csharp
    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();

        var empFaker = new Faker<Employee>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Employee>())
            .RuleFor(o => o.LastName, f => f.Person.FirstName)
            .RuleFor(o => o.FirstName, f => f.Person.LastName)
            .RuleFor(o => o.Email, (f, c) => f.Internet.Email());
        empFaker.Generate(100);
        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }
```

tu przyklad dla innej klasy:

```csharp
            var cusFaker = new Faker<Customer>("pl")
                .CustomInstantiator(f => ObjectSpace.CreateObject<Customer>())

                .RuleFor(o => o.Notes, f => f.Company.CatchPhrase())
                .RuleFor(o => o.CustomerName, f => f.Company.CompanyName())
                .RuleFor(o => o.Segment, f => f.PickRandom<Segment>())
                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.PostalCode, f => f.Address.ZipCode())
                .RuleFor(o => o.Street, f => f.Address.StreetName())
                .RuleFor(o => o.Phone, f => f.Person.Phone)
                .RuleFor(o => o.Email, (f, c) => f.Internet.Email());
            cusFaker.Generate(100);
            ObjectSpace.CommitChanges(); //This line persists created object(s).
```







## Relacja Many To Many



dodajemy liste zadan, Task jest zajete wiec uyjmy DemoTask



```csharp
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MySolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    //Use this attribute to define the name of the objects of this type in the user interface.
    [ModelDefault("Caption", "Task")]
    public class DemoTask : BaseObject
    {
        public virtual DateTime? DateCompleted { get; set; }

        public virtual String Subject { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual String Description { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual int PercentCompleted { get; set; }

        private TaskStatus status;

        public virtual TaskStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                if (isLoaded)
                {
                    if (value == TaskStatus.Completed)
                    {
                        DateCompleted = DateTime.Now;
                    }
                    else
                    {
                        DateCompleted = null;
                    }
                }
            }
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted()
        {
            Status = TaskStatus.Completed;
        }

        private bool isLoaded = false;
        public override void OnLoaded()
        {
            isLoaded = true;
        }

     }
     public enum TaskStatus
     {
         [ImageName("State_Task_NotStarted")]
         NotStarted,
         [ImageName("State_Task_InProgress")]
         InProgress,
         [ImageName("State_Task_WaitingForSomeoneElse")]
         WaitingForSomeoneElse,
         [ImageName("State_Task_Deferred")]
         Deferred,
         [ImageName("State_Task_Completed")]
         Completed
     }
}
```



i relacje do pracownikow i odwrotna relacje w pracownikach:



```csharp

    public class DemoTask : BaseObject
    {
        // ...
        public virtual IList<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

    }

```





```csharp
public class Employee : BaseObject {
    public Employee() {
        //...
        public virtual IList<DemoTask> DemoTasks { get; set; } = new ObservableCollection<DemoTask>();
    }
    //...
}
```





niesmiertelna migracja

```
add-migration MyInitialMigrationNameXMTM -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"

update-database -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
```



## Relacja One To Many

dodajemy kolejną klasę:

```csharp
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel;

namespace MySolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Title))]
    public class Department : BaseObject
    {
        public virtual string Title { get; set; }

        public virtual string Office { get; set; }

    }
}
```



rejestrujemy ja w dbcontext:



```csharp
public class MyXafSolutionEFCoreDbContext : DbContext {
    //...
    public DbSet<Department> Departments { get; set; }
}
```

 w pracowniku dodajemy powiązanie do Departamnetu:



```csharp
//...
public class Employee : BaseObject {
    //...
    public virtual Department Department { get; set; }
}
```

 i w departamentach dodajemy pracowników:

```csharp
// ...
using System.Collections.ObjectModel;
//...
public class Department : baseObject {
    //..
    public virtual IList<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
}
```





stanadard z migracją: 

```
 add-migration MyInitialMigrationNameXOTM -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"

 update-database -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"

```





teraz dodajmy klientom liste numerow telefonicznych: (sprobuj zrobic to sam.)



1. zakladamy klase phone numbers
2. dodajemy powiaznie w phone do pracownika
3. w pracownikach dodajemy kolekcje  telefonow
4. dopisujemy phonenumbers do dbcontext.cs
5. migracja bazy 

1.

```csharp
[DefaultProperty(nameof(Number))]
public class PhoneNumber : BaseObject
{

    public virtual String Number { get; set; }

    public virtual String PhoneType { get; set; }

    public override String ToString()
    {
        return Number;
    }

}
```



2.

```csharp
[DefaultProperty(nameof(Number))]
public class PhoneNumber : BaseObject
{

   ...


    public virtual Employee Employee {get; set;}
}
```



3.

```csharp
public class Employee : BaseObject
{
    ...

	public virtual IList<PhoneNumber> PhoneNumbers { get; set; } = new ObservableCollection<PhoneNumber>();

}
```



4.

```csharp
public class MyXafSolutionEFCoreDbContext : DbContext {
	public MyXafSolutionEFCoreDbContext(DbContextOptions<MyXafSolutionEFCoreDbContext> options) : base(options) {
	}
    ...
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    ...
    }
}
```

5.

```powershell
PM> add-migration MyInitialMigrationNameXOTM3 -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"

PM> update-database -StartupProject "MyXafSolution.Module" -Project "MyXafSolution.Module"
```



Dodajemy klase Stanowisko :

```csharp

    [DefaultClassOptions]
    [DefaultProperty(nameof(Title))]
    public class Position : BaseObject
    {
        public virtual string Title { get; set; }
    }

```

rejestrujemy tą klasę:



```csharp
public class MySolutionEFCoreDbContext : DbContext {
    //...
    public DbSet<Position> Positions { get; set; } 
}
```



i dodajemy ją w pracowniku:



```csharp
//...
using System.Collections.ObjectModel;

namespace MySolution.Module.BusinessObjects;

[DefaultClassOptions]
public class Employee : BaseObject {
    //...
    public virtual Position Position { get; set; }
}
```



w Updater.cs dodajmy kod ktory bedzie dodawal nam dzialy i stanowiska i przypisywal je do nowodanych pracownikow 



```csharp
        Department sales = ObjectSpace.FirstOrDefault<Department>(x => x.Title == "Sales");
        if (sales == null)
        {
            sales = ObjectSpace.CreateObject<Department>();
            sales.Title = "Developer";
        }
        Department quality = ObjectSpace.FirstOrDefault<Department>(x => x.Title == "Quality And Assurance");
        if (quality == null)
        {
            quality = ObjectSpace.CreateObject<Department>();
            quality.Title = "Quality And Assurance";
        }
        Department randd = ObjectSpace.FirstOrDefault<Department>(x => x.Title == "Research And Development");
        if (randd == null)
        {
            randd = ObjectSpace.CreateObject<Department>();
            randd.Title = "Research And Development";
        }

        List<Department> departments = new List<Department> { randd, quality, sales };


        Position developer = ObjectSpace.FirstOrDefault<Position>(x => x.Title == "Developer" );
        if (developer == null)
        {
            developer = ObjectSpace.CreateObject<Position>();
            developer.Title = "Developer";
        }

        Position manager = ObjectSpace.FirstOrDefault<Position>(x => x.Title == "Manager");
        if (manager == null)
        {
            manager = ObjectSpace.CreateObject<Position>();
            manager.Title = "Manager";
        }

        Position tester = ObjectSpace.FirstOrDefault<Position>(x => x.Title == "Tester");
        if (tester == null)
        {
            tester = ObjectSpace.CreateObject<Position>();
            tester.Title = "tester";
        }

        List<Position> positions = new List<Position> { developer,manager , tester };

        var empFaker = new Faker<Employee>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Employee>())
            .RuleFor(o => o.LastName, f => f.Person.FirstName)
            .RuleFor(o => o.FirstName, f => f.Person.LastName)
            .RuleFor(o => o.TitleOfCourtesy, f => f.PickRandom<TitleOfCourtesy>())
            .RuleFor(o => o.Email, (f, c) => f.Person.Email)

            .RuleFor(o=> o.Postion, f=> f.PickRandom(positions))
            .RuleFor(o => o.Department, f => f.PickRandom(departments))
        ;
        empFaker.Generate(10);
        ObjectSpace.CommitChanges(); //This line persists created object(s).
```





teraz dodajemy adresy:



```csharp
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel;

namespace MyXafSolution.Module.BusinessObjects;

[DefaultProperty(nameof(FullAddress))]
public class Address : BaseObject
{
    private const string defaultFullAddressFormat = "{Country}; {StateProvince}; {City}; {Street}; {ZipPostal}";

    public virtual String Street { get; set; }

    public virtual String City { get; set; }

    public virtual String StateProvince { get; set; }

    public virtual String ZipPostal { get; set; }

    public virtual String Country { get; set; }

    public String FullAddress
    {
        get { return ObjectFormatter.Format(defaultFullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
    }
}
```



a w pracownikach dodajemy adres glowny i korespondencyjny:

    public virtual Address Address { get; set; }
    public virtual Address CorespondenceAddress { get; set; }

}
