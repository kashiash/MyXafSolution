using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraCharts;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Others")]
    [DefaultProperty(nameof(Name))]
    public class ProductGroup: BaseObject
    {

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
}
