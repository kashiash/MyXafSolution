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
            tester.Title = "Tester";
        }

        List<Position> positions = new List<Position> { developer,manager , tester };

        var empFaker = new Faker<Employee>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Employee>())
            .RuleFor(o => o.LastName, f => f.Person.FirstName)
            .RuleFor(o => o.FirstName, f => f.Person.LastName)
            .RuleFor(o => o.TitleOfCourtesy, f => f.PickRandom<TitleOfCourtesy>())
            .RuleFor(o => o.Email, (f, c) => f.Person.Email)

            .RuleFor(o=> o.Position, f=> f.PickRandom(positions))
            .RuleFor(o => o.Department, f => f.PickRandom(departments))
        ;
        //empFaker.Generate(10);


        var faker = new Faker<DemoTask>()
            .RuleFor(t => t.DateCompleted, f => f.Date.Past())
            .RuleFor(t => t.Subject, f => f.Company.Random.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .RuleFor(t => t.DueDate, f => f.Date.Future())
            .RuleFor(t => t.StartDate, (f,t) => f.Date.Between(t.DueDate.Value.AddDays(-7), t.DueDate.Value))
            .RuleFor(t => t.PercentCompleted, f => f.Random.Int(0, 100))
          //  .RuleFor(t=> t.Employees, f=> empFaker.Generate(5))
            .RuleFor(t => t.Status, f => f.PickRandom<TaskStatus>());

            faker.Generate(100);
  


        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }
    public override void UpdateDatabaseBeforeUpdateSchema()
    {
        base.UpdateDatabaseBeforeUpdateSchema();
    }
}
