# Zapamiętywanie filtrów

Warto poczytać: 

https://docs.devexpress.com/eXpressAppFramework/113564/business-model-design-orm/data-types-supported-by-built-in-editors/criteria-properties

https://docs.devexpress.com/eXpressAppFramework/113143/ui-construction/view-items-and-property-editors/property-editors/use-criteria-property-editors



 



Wersja dla XPO:

klasa przechowujaca definicje filtra:



```csharp
	[DefaultClassOptions]
	[ImageName("Action_Filter")]
	[NavigationItem("Administracyjne")]
	[XafDisplayName("Filtr")]

	public class FilteringCriterion : BaseObject
	{
		public FilteringCriterion(Session session) : base(session) { }

		[XafDisplayName("Kryterium")]
		[CriteriaOptions(nameof(ObjectType))]
		[Size(SizeAttribute.Unlimited)]
		[EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
		public string Criterion
		{
			get => GetPropertyValue<string>(nameof(Criterion));
			set => SetPropertyValue(nameof(Criterion), value);
		}


		[XafDisplayName("Opis")]
		[RuleRequiredField("DescriptionIsRequiredinFilterCriteria", DefaultContexts.Save, CustomMessageTemplate = "Wpisz nazwę filtra")]
		public string Description
		{
			get => GetPropertyValue<string>(nameof(Description));
			set => SetPropertyValue(nameof(Description), value);
		}


		[XafDisplayName("Typ obiektu")]
		[ValueConverter(typeof(TypeToStringConverter))]
		[ImmediatePostData]
		[TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
		public Type ObjectType
		{
			get => GetPropertyValue<Type>(nameof(ObjectType));
			set => SetPropertyValue(nameof(ObjectType), value);
		}


		bool allowPublic;
		[XafDisplayName("Widoczny dla wszystkich")]
		public bool AllowPublic
		{
			get => allowPublic;
			set => SetPropertyValue(nameof(AllowPublic), ref allowPublic, value);
		}


		string ownerName;
		[XafDisplayName("Nazwa właściciela")]
		[ModelDefault("AllowEdit", "False")]
		public string OwnerName
		{
			get => ownerName;
			set => SetPropertyValue(nameof(OwnerName), ref ownerName, value);
		}


		[MemberDesignTimeVisibility(false)]
		Pracownicy employee;
		[XafDisplayName("Właściciel")]
		public Pracownicy Employee
		{
			get => employee;
			set
			{
				bool modified = SetPropertyValue(nameof(Employee), ref employee, value);
				if (modified && !IsSaving && !IsLoading)
				{
					OwnerName = value?.UserName;
				}
			}
		}


		private bool _default;
		[XafDisplayName("Domyślny filtr dla typu obiektu")]
		public bool Default
		{
			get => _default;
			set => SetPropertyValue(nameof(Default), ref _default, value);
		}

        // ponizsze sluzy do warunkowego ukrywania pokazywania filtrów np w zlaeznosci od uprawnien uzytkonika - przydatne w wiekszych firmach gdzie uzytkownikow tysiace i kazdy potrzebuuje innych filtrow

		[Association("FilteringCriterion-Role"), DevExpress.Xpo.Aggregated]
		[Appearance("HideRole", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Criteria = "Default = false")]
		public XPCollection<FilteringCriterionRole> Role => GetCollection<FilteringCriterionRole>(nameof(Role));
	}
```



jeśli mamy warunkowe ukrywanie filtrów, to potrzeba jeszcze klasa ktora mówi nam który filtr jest widoczny  dla której roli 

```csharp
	[XafDefaultProperty(nameof(EmployeeRole))]
	public class FilteringCriterionRole : BaseObject
	{
		public FilteringCriterionRole(Session session) : base(session) { }

		FilteringCriterion filteringCriterion;
		EmployeeRole employeeRole;

		public EmployeeRole EmployeeRole
		{
			get => employeeRole;
			set => SetPropertyValue(nameof(EmployeeRole), ref employeeRole, value);
		}


		[Browsable(false)]
		[Association("FilteringCriterion-Role")]
		public FilteringCriterion FilteringCriterion
		{
			get => filteringCriterion;
			set => SetPropertyValue(nameof(FilteringCriterion), ref filteringCriterion, value);
		}
	}
```



```csharp
	public class CriteriaController : ViewController<ListView>
	{
		private IObjectSpace criteriaObjectSpace;
        public IDxGridAdapter GridAdapter
        {
            get
            {
                if (View.Editor is not DxGridListEditor) return null;
                else return ((DxGridListEditor)View.Editor).GetGridAdapter();
            }
        }

        private const string CHOICE_ACTION_ITEM_ALL = "ALL";

		private ChoiceActionItem defaultItem;
		private readonly SingleChoiceAction filteringCriterionAction;
		private readonly SimpleAction saveGridFilterAction;
		private readonly SimpleAction wyczyscFiltry;

		public CriteriaController()
		{
			TargetViewNesting = Nesting.Root;

			filteringCriterionAction = new SingleChoiceAction(this, "FilteringCriterion", PredefinedCategory.Filters);
			filteringCriterionAction.Caption = "Filtruj";
			filteringCriterionAction.ToolTip = "Ustaw predefiniowany filtr na liscie";
			filteringCriterionAction.Execute += new SingleChoiceActionExecuteEventHandler(FilteringCriterionAction_Execute);

			saveGridFilterAction = new SimpleAction(this, "SaveGridFilter", PredefinedCategory.Filters);
			saveGridFilterAction.Caption = "Zapisz filtr";
			saveGridFilterAction.ToolTip = "Zapisz filtr zastosowany na liście w celu powtórnego wykorzystania";
			saveGridFilterAction.ImageName = "EditFilter";
			saveGridFilterAction.Execute += SaveGridFilterAction_Execute;

			wyczyscFiltry = new SimpleAction(this, nameof(wyczyscFiltry), PredefinedCategory.Unspecified)
			{
				Caption = "Wyczyćś filtry",
				ImageName = "ClearFilter",
				PaintStyle = ActionItemPaintStyle.CaptionAndImage
			};
			wyczyscFiltry.Execute += WyczyscFiltry_Execute;
		}

		protected override void OnActivated()
		{
			try
			{
				RefreshActionItems();
				if (string.IsNullOrEmpty(View.Model.Filter))
				{
					SetDefaultItem();
				}
				else
				{
					ChoiceActionItem selectedItem = filteringCriterionAction.Items.FirstOrDefault(x => (string)x.Data == View.Model.Filter);
					filteringCriterionAction.SelectedItem = selectedItem;
				}
			}
			catch (Exception)
			{
				ClearFilters();
			}
		}

		private void FilteringCriterionAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
		{
			try
			{
				string data = e.SelectedChoiceActionItem.Data as string;
				View.CollectionSource.Criteria["CriteriaController"] = View.ObjectSpace.ParseCriteria(data);

				if (View.Editor is ISupportFilter editor)
				{
					editor.Filter = data;
				}
			}
			catch (Exception)
			{
				ClearFilters();
			}

			View.Refresh();
		}

		private void RefreshActionItems()
		{
			filteringCriterionAction.Items.Clear();
			foreach (FilteringCriterion criterion in ObjectSpace.GetObjects<FilteringCriterion>().OrderBy(x => x.Description))
			{
				if (criterion.ObjectType != null && criterion.ObjectType.IsAssignableFrom(View.ObjectTypeInfo.Type))
				{
					var currentUser = SecuritySystem.CurrentUser as Pracownicy;

					if (criterion.Employee != null && criterion.AllowPublic == false
						&& criterion.Employee.Oid != currentUser.Oid)
					{
						continue;
					}

					var choiceActionItem = new ChoiceActionItem(criterion.Description, criterion.Criterion);
					filteringCriterionAction.Items.Add(choiceActionItem);
					if (criterion.Default)
					{
						if (criterion.Role == null || criterion.Role.Count <= 0)
						{
							defaultItem = choiceActionItem;
							continue;
						}

						foreach (var role in criterion.Role)
						{
							if (currentUser.EmployeeRoles.Any(x => x.Oid == role.EmployeeRole.Oid))
							{
								defaultItem = choiceActionItem;
							}
						}
					}
				}
			}

			if (filteringCriterionAction?.Items?.Count > 0)
			{
				filteringCriterionAction.Items.Add(new ChoiceActionItem(CHOICE_ACTION_ITEM_ALL, "Wszystkie", null));
			}
		}

		private void SaveGridFilterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
		{
			if (View.Editor is ISupportFilter editor && !string.IsNullOrEmpty(editor.Filter))
			{
				var objectSpace = Application.CreateObjectSpace(typeof(FilteringCriterion));
				var filteringCriterion = objectSpace.CreateObject<FilteringCriterion>();
				filteringCriterion.ObjectType = View.ObjectTypeInfo.Type;
				filteringCriterion.Criterion = editor.Filter;
				filteringCriterion.AllowPublic = true;
				filteringCriterion.Employee = objectSpace.FindObject<Pracownicy>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName));
				filteringCriterion.OwnerName = SecuritySystem.CurrentUserName;

				string filteringDetailId = Application.FindDetailViewId(typeof(FilteringCriterion));
				var view = Application.CreateDetailView(objectSpace, filteringDetailId, true, filteringCriterion);
				e.ShowViewParameters.CreatedView = view;
				e.ShowViewParameters.Context = TemplateContext.View;
				e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;

				view.ModelSaved += View_ModelSaved;
			}
			else if (GridAdapter != null && GridAdapter.GridInstance != null)
			{
                var criteria = GridAdapter.GridInstance.GetFilterCriteria();
				var stringCriteria = CriteriaOperator.ToString(criteria);
				if (string.IsNullOrEmpty(stringCriteria)) return;

                criteriaObjectSpace = Application.CreateObjectSpace(typeof(FilteringCriterion));
                var filteringCriterion = criteriaObjectSpace.CreateObject<FilteringCriterion>();
                filteringCriterion.ObjectType = View.ObjectTypeInfo.Type;
                filteringCriterion.Criterion = stringCriteria;
                filteringCriterion.AllowPublic = true;
                filteringCriterion.Employee = criteriaObjectSpace.FindObject<Pracownicy>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName));
                filteringCriterion.OwnerName = SecuritySystem.CurrentUserName;

                string filteringDetailId = Application.FindDetailViewId(typeof(FilteringCriterion));
				var dialogController = new DialogController();
                var view = Application.CreateDetailView(criteriaObjectSpace, filteringDetailId, true, filteringCriterion);
                e.ShowViewParameters.CreatedView = view;
				e.ShowViewParameters.Controllers.Add(dialogController);
                e.ShowViewParameters.Context = TemplateContext.View;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                dialogController.AcceptAction.Execute += AcceptAction_Execute;
            }
		}

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
			criteriaObjectSpace.CommitChanges();
            RefreshActionItems();
        }

        private void SetDefaultItem()
		{
			if (filteringCriterionAction.Items.Count > 0)
			{
				ChoiceActionItem selectedItem = defaultItem ?? filteringCriterionAction.Items.FindItemByID(CHOICE_ACTION_ITEM_ALL);
				filteringCriterionAction.SelectedItem = selectedItem;
			}
		}

		private void View_ModelSaved(object sender, EventArgs e) { RefreshActionItems(); }

		private void WyczyscFiltry_Execute(object sender, SimpleActionExecuteEventArgs e)
		{
			ClearFilters();
		}

		private void ClearFilters()
		{
			ChoiceActionItem allchoiceActionItem = filteringCriterionAction.Items.FindItemByID(CHOICE_ACTION_ITEM_ALL);
			if (allchoiceActionItem != null)
			{
				filteringCriterionAction.SelectedItem = allchoiceActionItem;
				View.CollectionSource.Criteria["CriteriaController"] = View.ObjectSpace.ParseCriteria(allchoiceActionItem.Data as string);
			}
			else
			{
				View.CollectionSource.Criteria["CriteriaController"] = null;
			}

			View.Model.Filter = string.Empty;
			View.Model.Criteria = string.Empty;

			if (View.Editor is DxGridListEditor grid)
			{
				var adapter = grid.GetGridAdapter();
				if (adapter != null && adapter.GridInstance != null) adapter.GridInstance.ClearFilter();
			}

			if (View.Editor is ISupportFilter editor)
			{
				editor.Filter = string.Empty;
			}
			View.Refresh();
		}
	}
```





kontroler do formatki filtra:



```csharp
public class FilteringCriterionDetailController : EventsObjectViewController<DetailView, FilteringCriterion>
	{
		protected override Dictionary<string, string> ParyNazwaWlasnosciNazwaMetodyValueStored => new Dictionary<string, string>
		{
			{ nameof(FilteringCriterion.ObjectType), nameof(ObjectType_ValueStored) }
		};

		private void ObjectType_ValueStored(object sender, EventArgs e)
		{
			ViewCurrentObject.Criterion = string.Empty;
		}

		protected override void OnActivated()
		{
			base.OnActivated();
			if (View.CurrentObject != null)
			{
				((FilteringCriterion)View.CurrentObject).Changed += FilteringCriterionDetailController_Changed;
			}
			View.CurrentObjectChanged += View_CurrentObjectChanged;
		}

		private void View_CurrentObjectChanged(object sender, EventArgs e)
		{
			if (View.CurrentObject != null)
			{
				((FilteringCriterion)View.CurrentObject).Changed += FilteringCriterionDetailController_Changed;
			}
		}

		private void FilteringCriterionDetailController_Changed(object sender, DevExpress.Xpo.ObjectChangeEventArgs e)
		{
			if (e.PropertyName == nameof(FilteringCriterion.Default))
			{
				var propertyEditors = View.GetItems<PropertyEditor>();
				if (propertyEditors == null || propertyEditors.Count <= 0)
				{
					return;
				}

				var rolePropertyEditor = propertyEditors.Where(x => x.PropertyName == nameof(FilteringCriterion.Role)).FirstOrDefault();
				if (rolePropertyEditor == null)
				{
					return;
				}

				if (((FilteringCriterion)View.CurrentObject)?.Default == true)
				{
					((IAppearanceVisibility)rolePropertyEditor).Visibility = ViewItemVisibility.Show;
				}
				else
				{
					((IAppearanceVisibility)rolePropertyEditor).Visibility = ViewItemVisibility.Hide;
				}
			}
		}
	}
```





Wersja EF (uproszczona)

`FilteringCriterion` przechuje definicje filtra

```csharp
public class TypeToStringConverter : ValueConverter<Type, string> {
    public TypeToStringConverter() : base(
            v => v.FullName,
            v => ReflectionHelper.FindType(v)) { }
}

[DefaultClassOptions,ImageName("Action_Filter")]
public class FilteringCriterion : BaseObject {
    public virtual string Description { get; set; }
    [ImmediatePostData]
    [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
    public virtual Type ObjectType { get; set; }
    [CriteriaOptions("ObjectType"),FieldSize(FieldSizeAttribute.Unlimited)]
    [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
    public virtual string Criterion { get; set; }
}


```



Rejestrujemy ta klasę i konwerter

```csharp
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
//...

public class ApplicationDbContext : DbContext {
    // ...
    public DbSet<FilteringCriterion> FilteringCriterions { get; set;}
    // ...
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // ...
        modelBuilder.Entity<FilteringCriterion>()
            .Property(t => t.ObjectType)
            .HasConversion(new TypeToStringConverter());
    }
}


```



Tworzymy kontroler który pozwala wybrać zdefiniowany filtr, dodać nowy lub wyczyścić filtrowanie



```csharp
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Templates;
// ...
 public class CriteriaController : ViewController<ListView>
    {
        private SingleChoiceAction filteringCriterionAction;
        private readonly SimpleAction saveGridFilterAction;
        private readonly SimpleAction wyczyscFiltry;

        public CriteriaController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                this, "FilteringCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            TargetViewType = ViewType.ListView;


            saveGridFilterAction = new SimpleAction(this, "SaveGridFilter", PredefinedCategory.Filters);
            saveGridFilterAction.Caption = "Zapisz filtr";
            saveGridFilterAction.ToolTip = "Zapisz filtr zastosowany na liście w celu powtórnego wykorzystania";
            saveGridFilterAction.ImageName = "EditFilter";
            saveGridFilterAction.Execute += SaveGridFilterAction_Execute;

            wyczyscFiltry = new SimpleAction(this, nameof(wyczyscFiltry), PredefinedCategory.Filters)
            {
                Caption = "Wyczyćś filtry",
                ImageName = "ClearFilter",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage
            };
            wyczyscFiltry.Execute += WyczyscFiltry_Execute;
        }

        private void WyczyscFiltry_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            View.CollectionSource.Criteria[nameof(CriteriaController)] = null;
            View.Refresh();
        }

        private void SaveGridFilterAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.Editor is ISupportFilter editor && !string.IsNullOrEmpty(editor.Filter))
            {
                var objectSpace = Application.CreateObjectSpace(typeof(FilteringCriterion));
                var filteringCriterion = objectSpace.CreateObject<FilteringCriterion>();
                filteringCriterion.ObjectType = View.ObjectTypeInfo.Type;
                filteringCriterion.Criterion = editor.Filter;


                string filteringDetailId = Application.FindDetailViewId(typeof(FilteringCriterion));
                var view = Application.CreateDetailView(objectSpace, filteringDetailId, true, filteringCriterion);
                e.ShowViewParameters.CreatedView = view;
                e.ShowViewParameters.Context = TemplateContext.View;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;

                view.ModelSaved += View_ModelSaved;
            }

        }
        private void View_ModelSaved(object sender, EventArgs e) { RefreshActionItems(); }
        protected override void OnActivated()
        {
            RefreshActionItems();
        }

        private void RefreshActionItems()
        {
            filteringCriterionAction.Items.Clear();
            foreach (FilteringCriterion criterion in ObjectSpace.GetObjects<FilteringCriterion>())
                if (criterion.ObjectType.IsAssignableFrom(View.ObjectTypeInfo.Type))
                {
                    filteringCriterionAction.Items.Add(
                        new ChoiceActionItem(criterion.Description, criterion.Criterion));
                }
            if (filteringCriterionAction.Items.Count > 0)
                filteringCriterionAction.Items.Add(new ChoiceActionItem("All", null));
        }

        private void FilteringCriterionAction_Execute(
            object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ((ListView)View).CollectionSource.Criteria[nameof(CriteriaController)] =
                CriteriaEditorHelper.GetCriteriaOperator(
                e.SelectedChoiceActionItem.Data as string, View.ObjectTypeInfo.Type, ObjectSpace);
        }
    }
```

ta wersja w przypadku Blazora wymaga jeszcze dopracowania !!!
