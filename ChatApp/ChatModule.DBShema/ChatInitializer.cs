using ChatModule.DBShema.Models;
using System;
using System.Collections.Generic;

namespace ChatModule.DBShema
{
    class ChatInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ChatContext>
    {
        protected override void Seed(ChatContext context)
        {
            var user = new List<User>
            {
            new User{first_name="Niklas",last_name="Grieger",avatarlink="Chat/img/grieger.bmp", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Till",last_name="Schreiber",avatarlink="", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Mounir",last_name="Ben-Sabeur",avatarlink="", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Martin",last_name="Schulz",avatarlink="", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Peter",last_name="Helfer",avatarlink="", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Max",last_name="Mustermann",avatarlink="", status="inactive", last_logout=DateTime.Now},
            new User{first_name="TestUser1",last_name="Olivetto",avatarlink="http://vignette3.wikia.nocookie.net/men-in-black/images/3/3d/Jeebs_SS_01.jpg/revision/latest?cb=20120521133931", status="inactive", last_logout=DateTime.Now},
            new User{first_name="TestUser2",last_name="Olivetto",avatarlink="http://vignette3.wikia.nocookie.net/men-in-black/images/0/07/Edgar_skin.png/revision/latest?cb=20120525100343", status="inactive", last_logout=DateTime.Now},
            new User{first_name="TestUser3",last_name="Olivetto",avatarlink="http://vignette1.wikia.nocookie.net/men-in-black/images/2/29/Arquilian.jpg/revision/latest?cb=20110719170543", status="inactive", last_logout=DateTime.Now}
            };

            user.ForEach(s => context.User.Add(s));
            context.SaveChanges();

            var supportGroup = new List<SupportGroup>
            {
            new SupportGroup{email="profile@berenberg.de",subject="Profile"},
            new SupportGroup{email="kalender@berenberg.de",subject="Kalender"},
            new SupportGroup{email="termine@berenberg.de",subject="Termine"},
            new SupportGroup{email="sport@berenberg.de",subject="Sport"}
            };
            supportGroup.ForEach(s => context.SupportGroup.Add(s));
            context.SaveChanges();

            var user2SupportGroup = new List<User2SupportGroup>
            {
                new User2SupportGroup {supportgroup_id=1,user_id=1 },
                new User2SupportGroup {supportgroup_id=1,user_id=2 },
                new User2SupportGroup {supportgroup_id=2,user_id=3 },
                new User2SupportGroup {supportgroup_id=2,user_id=4 },
                new User2SupportGroup {supportgroup_id=3,user_id=5 },
                new User2SupportGroup {supportgroup_id=4,user_id=6 }
            };
            user2SupportGroup.ForEach(s => context.User2SupportGroup.Add(s));
            context.SaveChanges();
        }
    }
}

