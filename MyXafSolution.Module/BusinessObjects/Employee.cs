using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace MySolution.Module.BusinessObjects;

[DefaultClassOptions]
public class Employee : BaseObject
{

    public virtual String FirstName { get; set; }

    public virtual String LastName { get; set; }

    public virtual String MiddleName { get; set; }

}