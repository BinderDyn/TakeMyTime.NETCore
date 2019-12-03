using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using TakeMyTime.DOM.Interfaces;

namespace TakeMyTime.DOM.Models
{
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        object IEntity.Id
        {
            get { return this.Id; }
        }

        public string Name { get; set; }

        private DateTime? created;
        [DataType(DataType.DateTime)]
        public DateTime Created
        {
            get { return created ?? DateTime.Now; }
            set { created = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? Edited { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        


        [Timestamp]
        public byte[] Version { get; set; }
    }
}
