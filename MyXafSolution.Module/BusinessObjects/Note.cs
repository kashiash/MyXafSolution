using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyXafSolution.Module.BusinessObjects;

[DefaultProperty(nameof(Text))]
[ImageName("BO_Note")]
public class Note
{

    [Key, Browsable(false)]
    [DevExpress.ExpressApp.Data.Key]
    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    public virtual Guid ID { get; set; }
    public virtual String Author { get; set; }
    public virtual DateTime? DateTime { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual String Text { get; set; }
}