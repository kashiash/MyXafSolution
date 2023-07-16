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

namespace MyXafSolution.Module.Controllers
{
    public class CriteriaController : ObjectViewController
    {
        private SingleChoiceAction filteringCriterionAction;
        public CriteriaController()
        {
            filteringCriterionAction = new SingleChoiceAction(
                this, "FilteringCriterion", PredefinedCategory.Filters);
            filteringCriterionAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FilteringCriterionAction_Execute);
            TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
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
