using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.SystemModule;

namespace MyXafSolution.Module.BusinessObjects
{
    [DefaultClassOptions]

    [ListViewFilter("Today", "GetDate([DueDate]) = LocalDateTimeToday()")]
    [ListViewFilter("In three days", @"[DueDate] >= ADDDAYS(LocalDateTimeToday(), -3) AND 
    [DueDate] < LocalDateTimeToday()")]
    [ListViewFilter("In two weeks", @"[DueDate] >= ADDDAYS(LocalDateTimeToday(), -14) AND 
    [DueDate] < LocalDateTimeToday()")]
    [ListViewFilter("The last week", @"GetDate([DueDate]) > LocalDateTimeLastWeek() AND 
    GetDate([DueDate]) <= ADDDAYS(LocalDateTimeLastWeek(), 5)")]
    [ListViewFilter("This week", @"GetDate([DueDate]) > LocalDateTimeThisWeek() AND 
    GetDate([DueDate]) <= ADDDAYS(LocalDateTimeThisWeek(), 5)")]
    public class Task : BaseObject
    {
        [ModelDefault("EditMask", "d")]
        public virtual DateTime DueDate { get; set; }
    }
    [ModelDefault("Caption", "Task")]
    public class DemoTask : BaseObject
    {
        public virtual DateTime? DateCompleted { get; set; }

        public virtual String Subject { get; set; }


        public virtual bool IsCompleted { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual String Description { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual int PercentCompleted { get; set; }
        public virtual Priority Priority { get; set; }

        public virtual DateTime? Deadline { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited), ModelDefault("AllowEdit", "False")]
        public virtual string Comments { get; set; }

        private TaskStatus status;

        public virtual TaskStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                if (isLoaded)
                {
                    if (value == TaskStatus.Completed)
                    {
                        DateCompleted = DateTime.Now;
                    }
                    else
                    {
                        DateCompleted = null;
                    }
                }
            }
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted()
        {
            Status = TaskStatus.Completed;
        }

        [StringLength(4096)]
        public virtual string Note { get; set; }

        private bool isLoaded = false;
        public override void OnLoaded()
        {
            isLoaded = true;
        }

        //    public virtual IList<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public virtual ObservableCollection<Employee> Employees { get; set; }

        public DemoTask()
        {
            Employees = new ObservableCollection<Employee>();
        }


        /* Use this attribute to display the Postpone button in the UI
            and call the Postpone() method when a user clicks this button*/
        [Action(ToolTip = "Postpone the task to the next day", Caption = "Postpone")]
        // Shift the task's due date forward by one day
        public void Postpone()
        {
            if (DueDate == DateTime.MinValue)
            {
                DueDate = DateTime.Now;
            }
            DueDate = DueDate + TimeSpan.FromDays(1);
        }

        [Action(Caption = "Postpone",
    TargetObjectsCriteria = "[Deadline] Is Not Null And Not [IsCompleted]")]
        public void Postpone(PostponeParametersObject parameters)
        {
            if (Deadline.HasValue && !IsCompleted && (parameters.PostponeForDays > 0))
            {
                Deadline += TimeSpan.FromDays(parameters.PostponeForDays);
                Comments += String.Format("Postponed for {0} days, new deadline is {1:d}\r\n{2}\r\n",
                parameters.PostponeForDays, Deadline, parameters.Comment);
            }
        }

        [Action(Caption = "Complete", TargetObjectsCriteria = "Not [IsCompleted]")]
        public void Complete()
        {
            IsCompleted = true;
        }

    }
    public enum TaskStatus
    {
        [ImageName("State_Task_NotStarted")]
        NotStarted,
        [ImageName("State_Task_InProgress")]
        InProgress,
        [ImageName("State_Task_WaitingForSomeoneElse")]
        WaitingForSomeoneElse,
        [ImageName("State_Task_Deferred")]
        Deferred,
        [ImageName("State_Task_Completed")]
        Completed
    }

    public enum Priority
    {
        [ImageName("State_Priority_Low")]
        Low,
        [ImageName("State_Priority_Normal")]
        Normal,
        [ImageName("State_Priority_High")]
        High
    }



[DomainComponent]
    public class PostponeParametersObject
    {
        public PostponeParametersObject() { PostponeForDays = 1; }
        public uint PostponeForDays { get; set; }
        [FieldSize(FieldSizeAttribute.Unlimited)]
        public string Comment { get; set; }
    }
}