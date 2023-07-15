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
    public class TasksController : ViewController
    {
        SimpleAction clearTaskAction;
        public TasksController() : base()
        {
            // Target required Views (use the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.DetailView;
            //Specify the type of objects that can use the Controller
            TargetObjectType = typeof(Employee);


            clearTaskAction = new SimpleAction(this, $"{GetType().FullName}{nameof(clearTaskAction)}", PredefinedCategory.View) {
                //Specify the Action's button caption.
                Caption = "Clear tasks",
                //Specify the confirmation message that pops up when a user executes an Action.
                ConfirmationMessage = "Are you sure you want to clear the Tasks list?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            clearTaskAction.Execute += clearTaskAction_Execute;
            
        }
        private void clearTaskAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            while (((Employee)View.CurrentObject).DemoTasks.Count > 0)
            {
                ((Employee)View.CurrentObject).DemoTasks.Remove(((Employee)View.CurrentObject).DemoTasks[0]);
                ObjectSpace.SetModified(View.CurrentObject, View.ObjectTypeInfo.FindMember(nameof(Employee.DemoTasks)));
            }
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
