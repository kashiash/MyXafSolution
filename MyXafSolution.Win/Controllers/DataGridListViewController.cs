using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyXafSolution.Win.Controllers
{
    public class DataGridListViewController : ViewController<ListView>
    {

      
        public DataGridListViewController() : base()
        {
            ViewControlsCreated += DataGridListViewController_ViewControlsCreated;
        }

        private void DataGridListViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            GridListEditor listEditor = View.Editor as GridListEditor;

            if (listEditor != null)
            {
                GridView gridView = listEditor.GridView;

                SetListView(gridView);
            }
        }


        private static void SetListView(GridView gridView)
        {
            gridView.OptionsView.EnableAppearanceOddRow = true;

            gridView.OptionsView.ShowFooter = false;
            gridView.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
            gridView.OptionsMenu.ShowGroupSummaryEditorItem = true;
            gridView.OptionsMenu.ShowConditionalFormattingItem = true;
            gridView.OptionsPrint.ExpandAllGroups = false;
            //  właczamy filtry pod nagłowkami
            gridView.OptionsView.ColumnAutoWidth = true;
            //  właczamy zmiane rozmiru kolumn
            gridView.OptionsView.RowAutoHeight = true;

            gridView.OptionsView.ShowAutoFilterRow = false;
            gridView.OptionsFind.AlwaysVisible = false;
            gridView.UserCellPadding = new System.Windows.Forms.Padding(0);
            gridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Default;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
    }
}
