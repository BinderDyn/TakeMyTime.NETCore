using System.ComponentModel.DataAnnotations;
using TakeMyTime.DOM.Interfaces;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DOM.Models
{
    public class User : Entity<User>, IInitiable, ICreatable<User>
    {
        public void Init()
        {
            
        }

        public User Create()
        {
            User user = new User();
            return user;
        }


        [Key]
        new public int Id { get; set; }
    }
}
