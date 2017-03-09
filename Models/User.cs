using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatModule.DBShema.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatarlink { get; set; }
        public DateTime? last_logout { get; set; }
        [DefaultValue("offline")]
        public string status { get; set; }
        //For Entitys Framework's "Lazy Loading" ->  Lazy Loading means that the contents of these properties will be automatically loaded from the database when you try to access them
        public virtual ICollection<User2SupportGroup> Category2User { get; set; }
    }
}
