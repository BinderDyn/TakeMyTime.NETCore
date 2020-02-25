using System.Collections.Generic;

namespace BinderDynamics.TakeMyTime.Biz.ViewModels
{
    public class EntriesViewModel
    {
        public EntriesViewModel()
        {
            entryViews = new List<EntryViewModel>();
        }

        public List<EntryViewModel> entryViews { get; set; }
    }
}
