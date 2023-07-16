using DevExpress.Utils;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using MyXafSolution.Module.BusinessObjects;

namespace MyXafSolution.Win.Controllers
{

    public class CheckBoxRowSelectController : ObjectViewController<ListView,Employee>
    {
        GridListEditor gridListEditor;
        ListViewProcessCurrentObjectController listViewProcessCurrentObjectController;
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            gridListEditor = View.Editor as GridListEditor;
            if (gridListEditor != null)
            {
                listViewProcessCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (listViewProcessCurrentObjectController != null)
                {
                    listViewProcessCurrentObjectController.ProcessCurrentObjectAction.SelectionDependencyType = SelectionDependencyType.Independent;
                }
                gridListEditor.ControlDataSourceChanged += GridListEditor_ControlDataSourceChanged;
                gridListEditor.GridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
                gridListEditor.GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
        }
        private void GridListEditor_ControlDataSourceChanged(object sender, EventArgs e)
        {
            gridListEditor.GridView.ClearSelection();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (gridListEditor != null)
            {
                gridListEditor.ControlDataSourceChanged -= GridListEditor_ControlDataSourceChanged;
                gridListEditor = null;
            }
            if (listViewProcessCurrentObjectController != null)
            {
                listViewProcessCurrentObjectController.ProcessCurrentObjectAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
                listViewProcessCurrentObjectController = null;
            }
        }
    }
}
