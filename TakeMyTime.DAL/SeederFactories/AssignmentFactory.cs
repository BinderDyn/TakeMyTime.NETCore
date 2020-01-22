using System;
using System.Collections.Generic;
using System.Text;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL.SeederFactories
{
    public class AssignmentFactory
    {
        private AssignmentFactory()
        {
        }

        public static IEnumerable<Assignment> CreateAssignments(Project[] projects)
        {
            return new List<Assignment>()
            {
                Assignment.Create(CreateAssignmentCreateParam(projects[0], "Task 1", "Got stuff to do for this!")),
                Assignment.Create(CreateAssignmentCreateParam(projects[1], "Task 2", "Got stuff to do for this!")),
                Assignment.Create(CreateAssignmentCreateParam(projects[2], "Task 3", "Got stuff to do for this!")),
                Assignment.Create(CreateAssignmentCreateParam(projects[3], "Task 4", "Got stuff to do for this!")),
                Assignment.Create(CreateAssignmentCreateParam(projects[4], "Task 5", "Got stuff to do for this!")),
                Assignment.Create(CreateAssignmentCreateParam(projects[5], "Task 6", "Got stuff to do for this!")),
            };
        }

        private static AssignmentCreateParam CreateAssignmentCreateParam(Project project, string name, string description)
        {
            var random = new Random();
            AssignmentCreateParam param = null;

            if(random.Next(1, 7) > 3)
            {
                param = new AssignmentCreateParam
                {
                    Project = project,
                    Name = name,
                    Description = description,
                    DateDue = new DateTime(2020, random.Next(1, 13), random.Next(1, 28)),
                    DatePlanned = new DateTime(2020, random.Next(1, 13), random.Next(1, 28))
                };

                if (param.DateDue.Value < param.DatePlanned.Value)
                {
                    param.DateDue = param.DatePlanned.Value;
                }
            }
            else
            {
                param = new AssignmentCreateParam
                {
                    Project = project,
                    Name = name,
                    Description = description,
                    DateDue = null,
                    DatePlanned = null
                };
            }

            return param;
        }

        private class AssignmentCreateParam : Assignment.ICreateParam
        {
            public Project Project { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? DatePlanned { get; set; }
            public DateTime? DateDue { get; set; }
            public long? DurationPlannedAsTicks { get; set; }
        }
    }
}
