using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.EF;
using DevExpress.Persistent.BaseImpl.EF;
using Castle.Core.Resource;
using Bogus;
using MySolution.Module.BusinessObjects;

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



        var empFaker = new Faker<Employee>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Employee>())
            .RuleFor(o => o.LastName, f => f.Person.FirstName)
            .RuleFor(o => o.FirstName, f => f.Person.LastName)
            .RuleFor(o => o.TitleOfCourtesy , f => f.PickRandom<TitleOfCourtesy>())
            .RuleFor(o => o.Email, (f, c) => f.Person.Email);
        
        empFaker.Generate(10);
        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }
    public override void UpdateDatabaseBeforeUpdateSchema()
    {
        base.UpdateDatabaseBeforeUpdateSchema();
    }
}
