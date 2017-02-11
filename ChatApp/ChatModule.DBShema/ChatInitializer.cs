using ChatModule.DBShema.Models;
using System.Collections.Generic;

namespace ChatModule.DBShema
{
    class ChatInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ChatContext>
    {
        protected override void Seed(ChatContext context)
        {
            var user = new List<User>
            {
            new User{first_name="Niklas",last_name="Grieger",avatarlink=""},
            new User{first_name="Till",last_name="Schreiber",avatarlink=""},
            new User{first_name="Mounir",last_name="Ben-Sabeur",avatarlink=""},
            new User{first_name="Gytis",last_name="Barzdukas",avatarlink=""},
            new User{first_name="Yan",last_name="Li",avatarlink=""},
            new User{first_name="Peggy",last_name="Justice",avatarlink=""},
            new User{first_name="Laura",last_name="Norman",avatarlink=""},
            new User{first_name="Nino",last_name="Olivetto",avatarlink=""},
            new User{first_name="TestUser1",last_name="Olivetto",avatarlink="http://vignette3.wikia.nocookie.net/men-in-black/images/3/3d/Jeebs_SS_01.jpg/revision/latest?cb=20120521133931"},
            new User{first_name="TestUser2",last_name="Olivetto",avatarlink="http://vignette3.wikia.nocookie.net/men-in-black/images/0/07/Edgar_skin.png/revision/latest?cb=20120525100343"},
            new User{first_name="TestUser3",last_name="Olivetto",avatarlink="http://vignette1.wikia.nocookie.net/men-in-black/images/2/29/Arquilian.jpg/revision/latest?cb=20110719170543"}
            };

            user.ForEach(s => context.User.Add(s));
            context.SaveChanges();
            var category = new List<Category>
            {
            new Category{bezeichnung="Profile"},
            new Category{bezeichnung="Kalender"},
            new Category{bezeichnung="Termine"},
            new Category{bezeichnung="Calculus"},
            new Category{bezeichnung="Trigonometry"},
            new Category{bezeichnung="Composition"},
            new Category{bezeichnung="Literature"},
            new Category{bezeichnung="Sport"}
            };
            category.ForEach(s => context.Category.Add(s));
            context.SaveChanges();
            var category2User = new List<Category2User>
            {
            new Category2User{category_id=1,user_id=1},
            new Category2User{category_id=2,user_id=2},
            new Category2User{category_id=3,user_id=3},
            new Category2User{category_id=4,user_id=4},
            new Category2User{category_id=5,user_id=5},
            new Category2User{category_id=6,user_id=6},
            new Category2User{category_id=7,user_id=7},
            new Category2User{category_id=8,user_id=8}
            };
            category2User.ForEach(s => context.Category2User.Add(s));
            context.SaveChanges();
        }
    }
}

