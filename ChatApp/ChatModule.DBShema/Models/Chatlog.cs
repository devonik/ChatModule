using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatModule.DBShema.Models
{
    public class Chatlog
    {
        [Key]
        public int chatlog_id { get; set; }
        //[Required]
        public int sender_id { get; set; }
        [Required]
        public int empfaenger_id { get; set; }
        //[Required]
        public string message { get; set; }
        [Required]
        public DateTime timestamp { get; set; }
        public virtual ICollection<SupportGroup> SupportGroup { get; set; }
    }
}
