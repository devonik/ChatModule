using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule.DBShema.Models
{
    public class SupportGroup
    {
        [Key]
        public int supportgroup_id { get; set; }
        [Required]
        public string subject { get; set; }
        public string email { get; set; }
    }
}
