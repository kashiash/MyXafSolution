﻿using System;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using System.ComponentModel;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DevExpress.ExpressApp.DC;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions, ImageName("Action_Filter")]
    public class FilteringCriterion : BaseObject
    {
        public virtual string Description { get; set; }
        [ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        public virtual Type ObjectType { get; set; }
        [CriteriaOptions("ObjectType"), FieldSize(FieldSizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public virtual string Criterion { get; set; }
    }

    public class TypeToStringConverter : ValueConverter<Type, string>
    {
        public TypeToStringConverter() : base(
                v => v.FullName,
                v => ReflectionHelper.FindType(v))
        { }
    }
}
