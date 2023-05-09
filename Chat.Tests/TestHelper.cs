using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Bogus;
using Chat.BLL.Automapper;
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Tests
{
    public static class TestHelper
    {
        public static DbContextOptions<ChatDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new ChatDbContext(options))
            {
                SeedData(context);
                context.SaveChanges();
            }
            return options;
        }

        public static void SeedData(ChatDbContext context)
        {
            context.Groups.AddRange(GetfakeGroupList());
            context.Users.AddRange(GetfakeUserList());
            context.GroupsInfo.AddRange(GetfakeGroupInfoViewList());
            context.UserGroup.AddRange(GetfakeUserGroupList());
            context.Messages.AddRange(GetfakeMessageListTry());
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }



        public static List<Group> GetfakeGroupList()
        {
            return new List<Group>()
            {
                new Group
                {
                    Id= 1,
                    Name = "First Group",
                    AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                    CreationDate = DateTime.Parse("2023-04-24"),
                    Description = "First group description",
                },
                new Group
                {
                    Id= 2,
                    Name = "Second Group",
                    AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                    CreationDate = DateTime.Parse("2023-04-25"),
                    Description = "Second group description",
                },
                new Group
                {
                    Id= 3,
                    Name = "Third Group",
                    AdminId = "361ca39f-a39e-46b1-921c-a38a9803fceb",
                    CreationDate = DateTime.Parse("2023-04-25"),
                    Description = "Third group description",
                },
            };
        }


        public static List<User> GetfakeUserList() 
        {
            return new List<User>() 
            {
                new User
                {
                    Id = "57f9b20f-3d14-4f48-be5f-90084218b437",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "+380993899092",
                    UserName = "admin@example.com",
                    BirthDate = DateTime.Parse("1993-04-25")
                },
                new User
                {
                    Id = "361ca39f-a39e-46b1-921c-a38a9803fceb",
                    Email = "test2@example.com",
                    FirstName = "SecondUser",
                    LastName = "User",
                    PhoneNumber = "+380993899093",
                    UserName = "test2@example.com",
                    BirthDate = DateTime.Parse("1994-04-25")
                },
                new User
                {
                    Id = "772aef6c-24fa-41f9-af4d-7aa907eb2356",
                    Email = "test22@example.com",
                    FirstName = "ThirdUser",
                    LastName = "User",
                    PhoneNumber = "+380993899092",
                    UserName = "test22@example.com",
                    BirthDate = DateTime.Parse("1993-04-25")
                },
            };
        }

        public static List<UserGroup> GetfakeUserGroupList() 
        {
            return new List<UserGroup>() 
            {
                new UserGroup {Id = 1, GroupId = 1, UserId = "57f9b20f-3d14-4f48-be5f-90084218b437", JoinDate = DateTime.Parse("2023-05-09")},
                new UserGroup {Id = 2, GroupId = 1, UserId = "361ca39f-a39e-46b1-921c-a38a9803fceb", JoinDate = DateTime.Parse("2023-05-09")},
                new UserGroup {Id = 3, GroupId = 1, UserId = "772aef6c-24fa-41f9-af4d-7aa907eb2356", JoinDate = DateTime.Parse("2023-05-09")},
                new UserGroup {Id = 4, GroupId = 2, UserId = "57f9b20f-3d14-4f48-be5f-90084218b437", JoinDate = DateTime.Parse("2023-05-09")},
                new UserGroup {Id = 5, GroupId = 2, UserId = "772aef6c-24fa-41f9-af4d-7aa907eb2356", JoinDate = DateTime.Parse("2023-05-09")},
                new UserGroup {Id = 6, GroupId = 3, UserId = "361ca39f-a39e-46b1-921c-a38a9803fceb", JoinDate = DateTime.Parse("2023-05-09")},
                new UserGroup {Id = 7, GroupId = 3, UserId = "57f9b20f-3d14-4f48-be5f-90084218b437", JoinDate = DateTime.Parse("2023-05-09")},
            };
        }

        public static List<Message> GetfakeMessageListTry()
        {
            return new List<Message>()
            {
                new Message {Id = 1, GroupId = 1, SenderId = "57f9b20f-3d14-4f48-be5f-90084218b437", SendDate = DateTime.Parse("2023-05-09")},
                new Message {Id = 2, GroupId = 1, SenderId = "361ca39f-a39e-46b1-921c-a38a9803fceb", SendDate = DateTime.Parse("2023-05-09")},
                new Message {Id = 3, GroupId = 1, SenderId = "772aef6c-24fa-41f9-af4d-7aa907eb2356", SendDate = DateTime.Parse("2023-05-09")},
                new Message {Id = 4, GroupId = 2, SenderId = "57f9b20f-3d14-4f48-be5f-90084218b437", SendDate = DateTime.Parse("2023-05-09")},
                new Message {Id = 5, GroupId = 2, SenderId = "772aef6c-24fa-41f9-af4d-7aa907eb2356", SendDate = DateTime.Parse("2023-05-09")},
                new Message {Id = 6, GroupId = 3, SenderId = "361ca39f-a39e-46b1-921c-a38a9803fceb", SendDate = DateTime.Parse("2023-05-09")},
                new Message {Id = 7, GroupId = 3, SenderId = "57f9b20f-3d14-4f48-be5f-90084218b437", SendDate = DateTime.Parse("2023-05-09")},
            };
        }

        public static List<Message> GetfakeMessageList() 
        {
            var ids = 1;
            var groupId = 1;
            var faker = new Faker<Message>()
                .RuleFor(m => m.Id, f => ids++)
                .RuleFor(m => m.SenderId, f => f.Random.Guid().ToString())
                .RuleFor(m => m.GroupId, f => groupId)
                .RuleFor(m => m.Text, f => f.Lorem.Sentence(1));

            var messages = faker.Generate(10);
            
            return messages;
        }

        public static List<GroupInfoView> GetfakeGroupInfoViewList() 
        { 
            return new List<GroupInfoView>()
            { 
                new GroupInfoView
                {
                    Id= 1,
                    Name = "First Group",
                    AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                    CreationDate = DateTime.Parse("2023-04-24"),
                    Description = "First group description",
                    AdminEmail = "admin@example.com",
                    Members = 3,
                    TotalMessages = 15
                },
                new GroupInfoView
                {
                    Id= 2,
                    Name = "Second Group",
                    AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                    CreationDate = DateTime.Parse("2023-04-25"),
                    Description = "Second group description",
                    AdminEmail = "admin@example.com",
                    Members = 2,
                    TotalMessages = 5
                },
                new GroupInfoView
                {
                    Id= 3,
                    Name = "Third Group",
                    AdminId = "361ca39f-a39e-46b1-921c-a38a9803fceb",
                    CreationDate = DateTime.Parse("2023-04-25"),
                    Description = "Third group description",
                    AdminEmail = "test2@example.com",
                    Members = 2,
                    TotalMessages = 5
                },
            };
        }

}
}
