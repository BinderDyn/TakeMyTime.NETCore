using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums.EnumDefinition;

namespace TakeMyTime.Biz.ViewModels
{
    public class EntityCollectionItem
    {
        public EntityCollectionItem(int id, BackupEntityType entityType, string name)
        {
            this.Id = id;
            this.Name = name;
            this.EntityType = entityType;
        }

        public int Id { get; set; }
        public BackupEntityType EntityType { get; set; }
        public string Name { get; set; }
    }
}
