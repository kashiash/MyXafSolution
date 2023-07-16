using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraCharts;
using Microsoft.EntityFrameworkCore;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(ShortName))]
    public class Product : BaseObject
    {
        [FieldSize(25)]
        public virtual string ShortName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual ProductGroup Group { get; set; }
        [Precision(18, 4)]
        public virtual decimal UnitPrice { get; set; }
        public virtual VatRate VatRate { get; set; }
        public virtual string Gtin { get; set; }
    }
}
