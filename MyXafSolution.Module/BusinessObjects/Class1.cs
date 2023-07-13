using DevExpress.Persistent.BaseImpl.EF;

namespace MySolution.Module.BusinessObjects;

public class Employee : BaseObject
{

    public virtual String FirstName { get; set; }

    public virtual String LastName { get; set; }

    public virtual String MiddleName { get; set; }

}