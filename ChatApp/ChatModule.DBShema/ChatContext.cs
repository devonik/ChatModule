using ChatModule.DBShema.Models;
using System.Data.Entity;

namespace ChatModule.DBShema
{
    // represents a session with the database, allowing us to query and save data.We define a context that derives from System.Data.Entity.DbContext and exposes a typed DbSet<TEntity> for each class in our model.
    public class ChatContext : DbContext
    {
        public ChatContext() : base("ChatContext")
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Chatlog> Chatlog { get; set; }
        public DbSet<Category2User> Category2User { get; set; }
    }
}
