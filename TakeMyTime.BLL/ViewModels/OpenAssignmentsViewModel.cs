using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.Biz.Logic;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class OpenAssignmentsViewModel
    {

        private List<string> AssignmentNames;

        public OpenAssignmentsViewModel(int project_Id)
        {
            if(project_Id != 0)
            {
                AssignmentLogic assignmentLogic = new AssignmentLogic();

                var assignments = assignmentLogic.GetAssignmentsByProjectId(project_Id)
                    .Where(a => a.AssignmentStatus == Common.Enums.EnumDefinition.AssignmentStatus.Active ||
                                a.AssignmentStatus == Common.Enums.EnumDefinition.AssignmentStatus.Future ||
                                a.AssignmentStatus == Common.Enums.EnumDefinition.AssignmentStatus.Postponed)
                                .ToList();
                if(assignments.Any())
                {
                    AssignmentNames = new List<string>();

                    foreach (var assignment in assignments)
                    {
                        AssignmentNames.Add(assignment.Name);
                    }
                }
                
            }
            
        }


        public string MessageBoxOutput {
            get
            {
                return GetDisplayText();
            }
        }


        public string GetDisplayText() 
        {
            string displayText = String.Empty;
            if(AssignmentNames != null && AssignmentNames.Any())
            {
                foreach(var assignmentName in AssignmentNames)
                {
                    displayText += string.Format("- {0}", assignmentName) + System.Environment.NewLine;
                }
                
            }
            else
            {
                displayText = "No open assignments available for selected project!";
            }

            return displayText;

        }
    }
}
