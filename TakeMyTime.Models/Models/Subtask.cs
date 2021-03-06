﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using static Common.Enums.EnumDefinition;

namespace TakeMyTime.Models.Models
{
    public class Subtask : Entity<Subtask>
    {
        protected Subtask()
        {

        }

        public static Subtask Create(ICreateParam param)
        {
            var subtask = new Subtask();
            subtask.Init(param);
            return subtask;
        }

        public void SetStatus(SubtaskStatus status)
        {
            if (CanSetStatus(status))
            {
                this.Status = status;
            }
        }

        public void Update(IUpdateParam param)
        {
            this.Name = param.Name;
            this.Description = param.Description;
            this.Priority = param.Priority;
            this.SetEdited();
        }

        private bool CanSetStatus(SubtaskStatus status)
        {
            bool canSet = true;
            if (this.Status == status) canSet = false;
            if (this.Status == SubtaskStatus.Aborted) canSet = false;
            return canSet;
        }

        private void Init(ICreateParam param)
        {
            this.Name = param.Name;
            this.Description = param.Description;
            this.Priority = param.Priority;
            this.Status = SubtaskStatus.NotYetDone;
            this.Entries = new HashSet<Entry>();
            this.SetCreated();
        }

        public IEnumerable<Entry> ClearEntries()
        {
            IEnumerable<Entry> entries = new List<Entry>();
            if (this.Entries != null && this.Entries.Any())
            {
                entries = this.Entries.ToArray();
                this.Entries.Clear();
            }
            return entries;
        }

        public interface IUpdateParam : ICreateParam
        {
        }

        public interface ICreateParam
        {
            string Name { get; set; }
            string Description { get; set; }
            SubtaskPriority Priority { get; set; }
        }

        public virtual SubtaskStatus Status { get; set; }
        public virtual SubtaskPriority Priority { get; set; }
        public long? DurationTicks { get; set; }
        public string Description { get; set; }
        [ForeignKey("Assignment")]
        public virtual int? Assignment_Id { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
