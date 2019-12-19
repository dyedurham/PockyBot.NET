using System;
using System.Collections.Generic;
using System.Text;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class FinishTestData
    {
        public static IEnumerable<object[]> RespondTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Text = "TestBot finish",
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            Text = "TestBot",
                            MessageType = MessageType.PersonMention,
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            Text = " finish",
                            MessageType = MessageType.Text
                        }
                    }
                },
                new List<PockyUser>
                {
                    new PockyUser
                    {
                        Username = "User 1",
                        UserId = "User1Id",
                        Roles = new List<Role>(),
                        Location = new UserLocation{ Location = "Location1" },
                        PegsReceived = new List<Peg>
                        {
                            new Peg
                            {
                                Comment = "This is a peg keyword1 keyword2",
                                Sender = new PockyUser
                                {
                                    UserId = "User2Id",
                                    Username = "User 2",
                                    Location = new UserLocation { Location = "Location1" }
                                }
                            }
                        },
                        PegsGiven = new List<Peg>()
                        {
                            new Peg()
                            {
                                Comment = "This is a shame peg penaltyKeyword",
                                Receiver = new PockyUser
                                {
                                    Username = "OtherBot",
                                    Location = new UserLocation { Location = "Location1" }
                                }
                            }
                        }
                    },
                    new PockyUser
                    {
                        Username = "User 2",
                        UserId = "User2Id",
                        Roles = new List<Role>(),
                        Location = new UserLocation { Location = "Location1" },
                        PegsReceived = new List<Peg>(),
                        PegsGiven = new List<Peg>()
                        {
                            new Peg
                            {
                                Comment = "This is a peg keyword1 keyword2",
                                Receiver = new PockyUser
                                {
                                    Username = "User 2",
                                    Location = new UserLocation { Location = "Location1" }
                                }
                            }
                        }
                    },
                },
                1, // minimum
                1, // winners
                "http://fakelocation.com",
                new Message
                {
                    Text = "[Here are all pegs given this cycle](http://fakelocation.com)"
                }
            };
        }
    }
}
