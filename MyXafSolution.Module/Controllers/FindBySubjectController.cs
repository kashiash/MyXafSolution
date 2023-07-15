using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using MyXafSolution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyXafSolution.Module.Controllers
{
    public class FindBySubjectController : ViewController
    {
        ParametrizedAction findBySubjectAction;
        public FindBySubjectController() : base()
        {
            // Target required Views (use the TargetXXX properties) and create their Actions.
            findBySubjectAction = new ParametrizedAction(this, $"{GetType().FullName}{nameof(findBySubjectAction)}", PredefinedCategory.View , typeof(string));
            findBySubjectAction.Execute += findBySubjectAction_Execute;
            
        }
private void findBySubjectAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
{
    var objectType = ((ListView)View).ObjectTypeInfo.Type;
    IObjectSpace objectSpace = Application.CreateObjectSpace(objectType);
    string paramValue = e.ParameterCurrentValue as string;
    object obj = objectSpace.FirstOrDefault<DemoTask>(task => task.Subject.Contains(paramValue));
    if (obj != null)
    {
        DetailView detailView = Application.CreateDetailView(objectSpace, obj);
        detailView.ViewEditMode = ViewEditMode.Edit;
        e.ShowViewParameters.CreatedView = detailView;
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
