using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions]
    //Use this attribute to specify the caption format for the objects of the entity class.
    [ObjectCaptionFormat("{0:FullName}")]
    [DefaultProperty(nameof(FullName))]
    public class Customer: BaseObject
    {
        public virtual string ShortName { get; set; }

        public virtual string CompanyName { get; set; }
        public virtual string ExtendedName { get; set; }
        public virtual string VatID { get; set; }

        public virtual Address CompanyAddress { get; set; }
        public virtual Address CorrespondenceAddress { get; set; }

        //Use this attribute to specify the maximum number of characters that users can type in the editor of this property.
        [FieldSize(255)]
        public virtual String Email { get; set; }

        //Use this attribute to define a pattern that the property value must match.
        [RuleRegularExpression(@"(((http|https)\://)[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])*)|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})", CustomMessageTemplate = @"Invalid ""Web Page Address"".")]
        public virtual string WebPageAddress { get; set; }

        //Use this attribute to specify the maximum string length allowed for this data field.
        [StringLength(4096)]
        public virtual string Notes { get; set; }

        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public String FullName
        {
            get { return ObjectFormatter.Format(FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public String DisplayName
        {
            get { return FullName; }
        }

        public virtual Segment Segment { get; set; }

        public static String FullNameFormat = "{CompanyName} {ExtendedName} {VatID}";
    }
    public enum Segment
    {
        Corporate = 2,
        Consumer = 7,
        [XafDisplayName("Home Office")]
        HomeOffice = 0,
        [XafDisplayName("Small Business")]
        SmallBusiness = 9

    }
}
