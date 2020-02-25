using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeMyTime.WPF.Utility
{
    public class PagingManager<T> where T : class
    {
        public PagingManager(int pageSize = 20)
        {
            this.PageSize = pageSize;
        }

        public int GetAllPages()
        {
            int allPagesCount = 0;

            if (this.Data.Count > 0)
            {
                allPagesCount = (int)Math.Ceiling(((double)this.Data.Count / (double)this.PageSize));
            }

            return allPagesCount;
        }

        public IList<T> Page(int page)
        {
            if (page > MaxPage) page -= 1;
            if (page <= 0) page = 1;
            this.CurrentPage = page;
            return this.Data.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        }

        public IList<T> Data { get; set; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int MaxPage 
        { 
            get
            {
                int result = 1;
                var maxPage = this.GetAllPages();
                if (maxPage <= 0) return result;
                else return maxPage;
            } 
        }
        public bool CanPageBack { get => this.CurrentPage > 1; }
        public bool CanPageForward { get => this.CurrentPage < MaxPage; }
    }
}
