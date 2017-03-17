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
        public int mitarbieter_id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        public string avatarlink { get; set; }
        public DateTime? last_logout { get; set; }
        [DefaultValue("offline")]
        public string status { get; set; }
    }
}
