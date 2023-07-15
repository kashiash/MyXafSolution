using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using MyXafSolution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyXafSolution.Module.Controllers
{
    public class PopupNotesController : ViewController
    {
        PopupWindowShowAction showNotesAction;
        public PopupNotesController() : base()
        {
            TargetObjectType = typeof(DemoTask);
            TargetViewType = ViewType.DetailView;
            // Target required Views (use the TargetXXX properties) and create their Actions.
            showNotesAction = new PopupWindowShowAction(this, $"{GetType().FullName}{nameof(showNotesAction)}",
                  PredefinedCategory.View)
            {
                Caption = "Show Notes"
            };
            showNotesAction.Execute += showNotesAction_Execute;
            showNotesAction.CustomizePopupWindowParams += showNotesAction_CustomizePopupWindowParams;
            
        }
        private void showNotesAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var selectedPopupWindowObjects = e.PopupWindowViewSelectedObjects;
            var selectedSourceViewObjects = e.SelectedObjects;
            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112723/).
        }
        private void showNotesAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            // Set the e.View parameter to a newly created view (https://docs.devexpress.com/eXpressAppFramework/112723/).
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
