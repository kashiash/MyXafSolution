
using System.ComponentModel;
using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Others")]
    [DefaultProperty(nameof(Symbol))]
    public class VatRate :BaseObject
    {
        [FieldSize(3)]
       
        public virtual string Symbol { get; set; }
        [Precision(18, 2)]
        public virtual decimal RateValue { get; set; }
    }
}
