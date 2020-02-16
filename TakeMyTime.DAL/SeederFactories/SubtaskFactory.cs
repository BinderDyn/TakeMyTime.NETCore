using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.Models.Models;

namespace TakeMyTime.DAL.SeederFactories
{
    public class SubtaskFactory
    {
        public static IEnumerable<Subtask> Create(Assignment assignment)
        {
            var rnd = new Random();

            var subtasks = new List<Subtask>
            {
                Subtask.Create(new SubtaskCreateParam
                {
                    Name = subtaskNames[rnd.Next(0,5)],
                    Priority = EnumDefinition.SubtaskPriority.High
                }),
                Subtask.Create(new SubtaskCreateParam
                {
                    Name = subtaskNames[rnd.Next(0,5)],
                    Priority = EnumDefinition.SubtaskPriority.Low
                }),
                Subtask.Create(new SubtaskCreateParam
                {
                    Name = subtaskNames[rnd.Next(0,5)],
                    Priority = EnumDefinition.SubtaskPriority.Medium
                }),
                Subtask.Create(new SubtaskCreateParam
                {
                    Name = subtaskNames[rnd.Next(0,5)],
                    Priority = EnumDefinition.SubtaskPriority.Lowest
                })
            }.Take(rnd.Next(0,4));

            foreach (var s in subtasks)
            {
                s.Assignment = assignment;
                s.Assignment_Id = assignment.Id;
            }

            return subtasks;
        }

        public class SubtaskCreateParam : Subtask.ICreateParam
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public EnumDefinition.SubtaskPriority Priority { get; set; }
        }

        static string[] subtaskNames = { "Planning", "Structuring the workload", "Testing", "Documentation", "Getting the rest done" };
    }
}
