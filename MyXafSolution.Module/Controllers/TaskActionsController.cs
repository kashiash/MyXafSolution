using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;

using MyXafSolution.Module.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyXafSolution.Module.Controllers
{
    public class TaskActionsController : ViewController
    {
        SingleChoiceAction setTaskAction;
        private ChoiceActionItem setPriorityItem;
        private ChoiceActionItem setStatusItem;
        public TaskActionsController() : base()
        {
            TargetObjectType = typeof(DemoTask);

            setTaskAction = new SingleChoiceAction(this, $"{GetType().FullName}{nameof(setTaskAction)}", PredefinedCategory.Edit)
            {

                Caption = "Set Task",
                //Specify the display mode for the Action's items. Here the items are operations that you perform against selected records.
                ItemType = SingleChoiceActionItemType.ItemIsOperation,
                //Set the Action to become available in the Task List View when a user selects one or more objects.
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects

            };
            setTaskAction.Execute += setTaskAction_Execute;

            setPriorityItem =
   new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(DemoTask), "Priority"), null);
            setTaskAction.Items.Add(setPriorityItem);
            FillItemWithEnumValues(setPriorityItem, typeof(Priority));

            setStatusItem =
               new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(DemoTask), "Status"), null);
            setTaskAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(BusinessObjects.TaskStatus));

        }
        private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)
        {
            EnumDescriptor ed = new EnumDescriptor(enumType);
            foreach (object current in ed.Values)
            {
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current);
                item.ImageName = ImageLoader.Instance.GetEnumValueImageName(current);
                parentItem.Items.Add(item);
            }
        }
        private void setTaskAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            /*Create a new ObjectSpace if the Action is used in List View.
  Use this ObjectSpace to manipulate the View's selected objects.*/
            IObjectSpace objectSpace = View is ListView ?
                Application.CreateObjectSpace(typeof(DemoTask)) : View.ObjectSpace;
            ArrayList objectsToProcess = new ArrayList(e.SelectedObjects);
            if (e.SelectedChoiceActionItem.ParentItem == setPriorityItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    DemoTask objInNewObjectSpace = (DemoTask)objectSpace.GetObject(obj);
                    objInNewObjectSpace.Priority = (Priority)e.SelectedChoiceActionItem.Data;
                }
            }
            else
                if (e.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach (Object obj in objectsToProcess)
                {
                    DemoTask objInNewObjectSpace = (DemoTask)objectSpace.GetObject(obj);
                    objInNewObjectSpace.Status = (BusinessObjects.TaskStatus)e.SelectedChoiceActionItem.Data;
                }
            }
            objectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
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
