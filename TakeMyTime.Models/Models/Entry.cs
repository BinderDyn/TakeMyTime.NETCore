using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DOM.Interfaces;

namespace TakeMyTime.DOM.Models
{
    public class Entry : Entity<Entry>, ICreatable, IInitiable
    {
        [Key]
        new public int Id { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
        [ForeignKey("Assignment")]
        public int? AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        //public TimeSpan? Duration { get; set; }
        public long? DurationAsTicks { get; set; }
        public int? Pages { get; set; }
        public int? Words { get; set; }
        public string Comment { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public void Init()
        {
            
        }
    }
}
