using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TakeMyTime.DAL.uow;
using Common.Enums;
using TakeMyTime.Models.Models;

namespace TakeMyTime.BLL.Logic
{
    public class EntryLogic
    {
        private readonly UnitOfWork unitOfWork;

        public EntryLogic(UnitOfWork uow = null)
        {
            if (uow != null)
            {
                this.unitOfWork = uow;
            }
            else
            {
                this.unitOfWork = new UnitOfWork();
            }
        }

        public IEnumerable<Entry> GetAllEntries()
        {
            var entries = unitOfWork.Entries.LoadAll();
            
            return entries;
        }

        public Entry GetEntryById(int id)
        {
            return unitOfWork.Entries.Get(id);
        }

        public void UpdateEntry(int entry_id, Entry.IUpdateParam param)
        {
            var edit = unitOfWork.Entries.Get(entry_id);
            edit.Update(param);
            unitOfWork.Complete();
        }

        public void DeleteEntry(int entryId)
        {
            var toBeDeleted = GetEntryById(entryId);
            unitOfWork.Entries.Remove(toBeDeleted);
            unitOfWork.Complete();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
