using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Others")]
    [DefaultProperty(nameof(Symbol))]
    public class VatRate : BaseObject
    {
        [FieldSize(3)]
        public virtual string Symbol { get; set; }
        public virtual decimal RateValue { get; set; }
    }
}
