using ChatModule.DBShema;
using ChatModule.DBShema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ChatModule.DataAccess.Repositories
{
    public class ChatRepository
    {
        private ChatContext db = new ChatContext();
        public IEnumerable<Category> GetAllCategories()
        {
            var categoryList = db.Category.ToList();
            return categoryList;
        }
    }
}
