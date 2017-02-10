using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatModule.DBShema.Models
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }
        public string bezeichnung { get; set; }
        //For Entitys Framework's "Lazy Loading" ->  Lazy Loading means that the contents of these properties will be automatically loaded from the database when you try to access them
        public virtual ICollection<Category2User> Category2User { get; set; }
    }
}
