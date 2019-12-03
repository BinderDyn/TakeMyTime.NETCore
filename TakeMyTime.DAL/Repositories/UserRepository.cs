using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        TakeMyTimeDbContext context;

        public UserRepository(TakeMyTimeDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
