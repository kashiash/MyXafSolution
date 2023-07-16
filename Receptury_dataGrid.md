# Data Grid / List View



https://docs.devexpress.com/eXpressAppFramework/113189/ui-construction/list-editors

## Konfigurowanie DataGrid:



grid można dowolnie konfigurować, aby sie do niego dostać należy utworzyć kontroler 

https://docs.devexpress.com/eXpressAppFramework/402154/ui-construction/list-editors/how-to-access-list-editor-control

WinForms

```csharp
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using YourApplicationName.Module.BusinessObjects;

namespace YourApplicationName.Win.Controllers;

public class GridViewController : ObjectViewController<ListView, TargetClassName> {
    protected override void OnViewControlsCreated() {
        base.OnViewControlsCreated();
        // Obtain the List Editor: XAF's abstraction over the UI control.
        if (View.Editor is GridListEditor gridListEditor && gridListEditor.GridView != null) {
            // Access the GridView object (part of the DevExpress WinForms Grid Control architecture). 
            GridView gridView = gridListEditor.GridView;
            // Specify the behavior of the grid's columns.
            // Access grid columns.
            // Use column settings to disable the sorting and grouping functionality. 
            foreach (GridColumn columnModel in gridView.Columns) {
                columnModel.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                columnModel.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            }
        }
    }
}
```

Blazor:

```csharp
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Models;
using YourApplicationName.Module.BusinessObjects;

namespace YourApplicationName.Blazor.Server.Controllers;

public class GridViewController : ObjectViewController<ListView, TargetClassName> {
    protected override void OnViewControlsCreated() {
        base.OnViewControlsCreated();
        if (View.Editor is DxGridListEditor gridListEditor) {
            // Obtain the Component Adapter.
            IDxGridAdapter dataGridAdapter = gridListEditor.GetGridAdapter();

            // Access grid property.
            dataGridAdapter.GridModel.PagerVisible = false;

            // Access grid columns.
            // Use column settings to disable the sorting and grouping functionality. 
            foreach (DxGridDataColumnModel columnModel in dataGridAdapter.GridDataColumnModels) {
                columnModel.AllowSort = false;
                columnModel.AllowGroup = false;
            }
        }
    }
}
```



### Naprzemienne kolorowanie wierszy na liscie  oraz domyslne ustawienia na wszystkich listach np AutoFilter



```csharp
    public class GridCustomizeController : ViewController<ListView>
    {

        GridListEditor gridListEditor = null;
        public GridCustomizeController() : base()
        {
            ViewControlsCreated += GridCustomizeController_ViewControlsCreated;
        }

        private void GridCustomizeController_ViewControlsCreated(object sender, EventArgs e)
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
```



## Zapamietywanie filtrów





## zapmaiertyweanie 