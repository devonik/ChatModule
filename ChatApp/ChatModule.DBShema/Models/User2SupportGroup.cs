using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule.DBShema.Models
{
    public class User2SupportGroup
    {
        [Key]
        public int user2supportgroup_id { get; set; }
        [Required]
        public int supportgroup_id { get; set; }
        [Required]
        public int user_id { get; set; }
        public virtual SupportGroup SupportGroup { get; set; }
        public virtual User User { get; set; }
    }
}
