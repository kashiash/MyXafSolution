using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.EF;
using DevExpress.Persistent.BaseImpl.EF;
using Castle.Core.Resource;
using Bogus;
using MyXafSolution.Module.BusinessObjects;
using TaskStatus = MyXafSolution.Module.BusinessObjects.TaskStatus;
using System;

namespace MyXafSolution.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater
{
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion)
    {
    }
    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();

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


        Position developer = ObjectSpace.FirstOrDefault<Position>(x => x.Title == "Developer");
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
            tester.Title = "Tester";
        }

        List<Position> positions = new List<Position> { developer, manager, tester };

        var empFaker = new Faker<Employee>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Employee>())
            .RuleFor(o => o.LastName, f => f.Person.FirstName)
            .RuleFor(o => o.FirstName, f => f.Person.LastName)
            .RuleFor(o => o.TitleOfCourtesy, f => f.PickRandom<TitleOfCourtesy>())
            .RuleFor(o => o.Email, (f, c) => f.Person.Email)

            .RuleFor(o => o.Position, f => f.PickRandom(positions))
            .RuleFor(o => o.Department, f => f.PickRandom(departments))
        ;
        var emps = empFaker.Generate(10);


        var faker = new Faker<DemoTask>()
            .CustomInstantiator(f => ObjectSpace.CreateObject<DemoTask>())
            .RuleFor(t => t.DateCompleted, f => f.Date.Past())
            .RuleFor(t => t.Subject, f => f.Company.Random.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .RuleFor(t => t.DueDate, f => f.Date.Future())
            .RuleFor(t => t.StartDate, (f, t) => f.Date.Between(t.DueDate.Value.AddDays(-7), t.DueDate.Value))
            .RuleFor(t => t.PercentCompleted, f => f.Random.Int(0, 100))
            //.RuleFor(t=> t.Employees, f=> f.PickRandom(emps,3).())
            .RuleFor(t => t.Priority, f => f.PickRandom<Priority>())
            .RuleFor(t => t.Status, f => f.PickRandom<TaskStatus>());

        var tasks = faker.Generate(100);

        foreach (var task in tasks)
        {
            task.Employees.Add(GetRandomElement(emps));
            task.Employees.Add(GetRandomElement(emps));
            task.Employees.Add(GetRandomElement(emps));
            task.Employees.Add(GetRandomElement(emps));
        }



        var rates = ObjectSpace.GetObjectsQuery<VatRate>().ToList();
        if (rates.Count == 0)
        {
            rates.Add(NowaStawka("23%", 23M));
            rates.Add(NowaStawka("0%", 0M));
            rates.Add(NowaStawka("7%", 7M));
            rates.Add(NowaStawka("ZW", 0M));
        }

        tester = CreateProductGroup(tester);

        List<Position> groups = new List<Position> { developer, manager, tester };

        var prodFaker = new Faker<Product>("pl")




  .CustomInstantiator(f => ObjectSpace.CreateObject<Product>())
      .RuleFor(o => o.Name, f => f.Commerce.ProductName())
      .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
      .RuleFor(o => o.ShortName, f => f.Commerce.Product())
      .RuleFor(o => o.UnitPrice, f => f.Random.Decimal(0.01M, 100M))
      .RuleFor(o => o.VatRate, f => f.PickRandom(rates))
      .RuleFor(o => o.Gtin, f => f.Commerce.Ean13());

        prodFaker.Generate(100);

        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }

    private Position CreateProductGroup(string name)
    {
        ProductGroup group = ObjectSpace.FirstOrDefault<ProductGroup>(x => x.Name == "Hardware");
        if (group == null)
        {
            group = ObjectSpace.CreateObject<ProductGroup>();
            group.Name = "Hardware";
        }

        return group;
    }

    private VatRate NowaStawka(string symbol, decimal val)
    {
        var vat = ObjectSpace.FindObject<VatRate>(CriteriaOperator.Parse("Symbol = ?", symbol));
        if (vat == null)
        {
            vat = ObjectSpace.CreateObject<VatRate>();
            vat.Symbol = symbol;
            vat.RateValue = val;
        }
        return vat;
    }
    private static T GetRandomElement<T>(List<T> emps)
    {
        Random random = new Random();
        var emp = emps.OrderBy(x => random.Next()).Take(1).FirstOrDefault();
        return emp;
    }

    public override void UpdateDatabaseBeforeUpdateSchema()
    {
        base.UpdateDatabaseBeforeUpdateSchema();
    }
}
