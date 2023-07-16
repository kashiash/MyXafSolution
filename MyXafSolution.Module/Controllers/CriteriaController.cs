using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using MyXafSolution.Module.BusinessObjects;
using DevExpress.ExpressApp.Templates;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.SystemModule;


namespace MyXafSolution.Module.Controllers
{
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
            //else if (GridAdapter != null && GridAdapter.GridInstance != null)
            //{
            //    var criteria = GridAdapter.GridInstance.GetFilterCriteria();
            //    var stringCriteria = CriteriaOperator.ToString(criteria);
            //    if (string.IsNullOrEmpty(stringCriteria)) return;

            //    criteriaObjectSpace = Application.CreateObjectSpace(typeof(FilteringCriterion));
            //    var filteringCriterion = criteriaObjectSpace.CreateObject<FilteringCriterion>();
            //    filteringCriterion.ObjectType = View.ObjectTypeInfo.Type;
            //    filteringCriterion.Criterion = stringCriteria;
            //    filteringCriterion.AllowPublic = true;
            //    filteringCriterion.Employee = criteriaObjectSpace.FindObject<Pracownicy>(new BinaryOperator("UserName", SecuritySystem.CurrentUserName));
            //    filteringCriterion.OwnerName = SecuritySystem.CurrentUserName;

            //    string filteringDetailId = Application.FindDetailViewId(typeof(FilteringCriterion));
            //    var dialogController = new DialogController();
            //    var view = Application.CreateDetailView(criteriaObjectSpace, filteringDetailId, true, filteringCriterion);
            //    e.ShowViewParameters.CreatedView = view;
            //    e.ShowViewParameters.Controllers.Add(dialogController);
            //    e.ShowViewParameters.Context = TemplateContext.View;
            //    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            //    dialogController.AcceptAction.Execute += AcceptAction_Execute;
            //}
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
}
