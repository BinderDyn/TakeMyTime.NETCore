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

        protected void SetCreated()
        {
            this.Created = DateTime.Now;
        }

        protected void SetEdited()
        {
            this.Edited = DateTime.Now;
        }

        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Edited { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
    }
}
