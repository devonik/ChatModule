using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatModule.DBShema.Models
{
    public class Category2User
    {
        [Key]
        public int category2user_ID { get; set; }
        public int category_id { get; set; }
        public int user_id { get; set; }

        //For Entitys Framework's "Lazy Loading" ->  Lazy Loading means that the contents of these properties will be automatically loaded from the database when you try to access them
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
