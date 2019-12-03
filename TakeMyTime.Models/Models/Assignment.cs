using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Interfaces;
using static Common.Enums.EnumDefinition;

namespace TakeMyTime.DOM.Models
{
    public class Assignment : Entity<Assignment>, IInitiable, ICreatable<Assignment>
    {
        [Key]
        new public int Id { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public DateTime? DatePlanned { get; set; }
        public DateTime? DateDue { get; set; }
        public long? DurationPlannedAsTicks { get; set; }
        public AssignmentStatus AssignmentStatus { get; set; }
        public int? Pages { get; set; }
        public int? Words { get; set; }
        public string Comment { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int TimesPushed { get; set; }

        [ToDo]
        public Assignment Create()
        {
            Assignment assignment = new Assignment();
            return assignment;
        }

        public void Init()
        {
            this.Entries = new HashSet<Entry>();
        }
    }

    
}
