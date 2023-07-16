using DevExpress.Blazor;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp;

namespace MyXafSolution.Blazor.Server.Controllers
{
    public class DataGridListViewController : ViewController<ListView>
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                var dataGridAdapter = gridListEditor.GetGridAdapter();
                dataGridAdapter.GridModel.CssClass += " grid-striped";
                dataGridAdapter.GridModel.ColumnResizeMode = GridColumnResizeMode.ColumnsContainer;
                dataGridAdapter.GridModel.ShowFilterRow = true;
                dataGridAdapter.GridModel.PageSizeSelectorVisible = true;

                foreach (var column in gridListEditor.Columns)
                {
                    if (column.Width < 80)
                    {
                        column.Width = 80;
                    }
                }
            }
        }
    }
}
