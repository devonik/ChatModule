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
            new User{first_name="Janina",last_name="Weiss",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Martina",last_name="Eiffel",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="David",last_name="Osterhagen",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Peter",last_name="Helfer",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="Max",last_name="Mustermann",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="TestUser1",last_name="Olivetto",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="TestUser2",last_name="Olivetto",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now},
            new User{first_name="TestUser3",last_name="Olivetto",avatarlink="Chat/img/unknown.jpg", status="inactive", last_logout=DateTime.Now}
            };

            user.ForEach(s => context.User.Add(s));
            context.SaveChanges();

            var supportGroup = new List<SupportGroup>
            {
            new SupportGroup{email="support@berenberg.de",subject="Controlling"},
            new SupportGroup{email="support@berenberg.de",subject="EMS"},
            new SupportGroup{email="support@berenberg.de",subject="Freigaben"},
            new SupportGroup{email="support@berenberg.de",subject="Private Banking"}
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

