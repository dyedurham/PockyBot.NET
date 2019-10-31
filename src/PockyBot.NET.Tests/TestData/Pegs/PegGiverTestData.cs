using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Pegs
{
    public static class PegGiverTestData
    {
        public static IEnumerable<object[]> ValidTestData()
        {
            yield return new object[]
            {
                "this is a comment",
                new PockyUser
                {
                    UserId = "TestUser1",
                    Username = "Test User 1"
                },
                new PockyUser
                {
                    UserId = "TestUser2",
                    Username = "Test User 2"
                },
                1,
                new Peg
                {
                    Comment = "this is a comment",
                    SenderId = "TestUser1",
                    ReceiverId = "TestUser2"
                },
                new Message
                {
                    Text = "Peg given to Test User 2. You have given 1 peg this fortnight.",
                    RoomId = "TestUser1"
                },
                new Message
                {
                    Text = "You have received a new peg from Test User 1 with message: \"this is a comment\".",
                    RoomId = "TestUser2"
                }
            };

            yield return new object[]
            {
                "for a reason",
                new PockyUser
                {
                    UserId = "TestUser1",
                    Username = "Test User 1"
                },
                new PockyUser
                {
                    UserId = "TestUser2",
                    Username = "Test User 2"
                },
                1,
                new Peg
                {
                    Comment = "for a reason",
                    SenderId = "TestUser1",
                    ReceiverId = "TestUser2"
                },
                new Message
                {
                    Text = "Peg given to Test User 2. You have given 1 peg this fortnight.",
                    RoomId = "TestUser1"
                },
                new Message
                {
                    Text = "You have received a new peg from Test User 1 with message: \"for a reason\".",
                    RoomId = "TestUser2"
                }
            };

            yield return new object[]
            {
                "this is a comment",
                new PockyUser
                {
                    UserId = "TestUser1",
                    Username = "Test User 1"
                },
                new PockyUser
                {
                    UserId = "TestUser2",
                    Username = "Test User 2"
                },
                3,
                new Peg
                {
                    Comment = "this is a comment",
                    SenderId = "TestUser1",
                    ReceiverId = "TestUser2"
                },
                new Message
                {
                    Text = "Peg given to Test User 2. You have given 3 pegs this fortnight.",
                    RoomId = "TestUser1"
                },
                new Message
                {
                    Text = "You have received a new peg from Test User 1 with message: \"this is a comment\".",
                    RoomId = "TestUser2"
                }
            };
        }
    }
}
