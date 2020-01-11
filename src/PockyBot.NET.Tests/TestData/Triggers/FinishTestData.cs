using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Models;
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
                            new Peg
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
                        PegsGiven = new List<Peg>
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
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        UserId = "User1Id",
                        Name = "User 1",
                        Location = "Location1",
                        TotalPoints = 0,
                        PegCount = 1,
                        PenaltyCount = 1,
                        PegsGivenCount = 0,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "User 2",
                                Weight = 1,
                                Comment = "This is a peg keyword1 keyword2",
                                Keywords = new List<string> { "keyword1", "keyword2" },
                                SenderLocation = "Location1"
                            }
                        },
                        Penalties = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "OtherBot",
                                Weight = 1,
                                Comment = "This is a shame peg penaltyKeyword",
                                Keywords = new List<string> { "penaltyKeyword" },
                                SenderLocation = "Location1"
                            }
                        }
                    },
                    new PegRecipient
                    {
                        UserId = "User2Id",
                        Name = "User 2",
                        TotalPoints = 0,
                        PegCount = 0,
                        PenaltyCount = 0,
                        PegsGivenCount = 1,
                        Pegs = new List<PegDetails>(),
                        Penalties = new List<PegDetails>()
                    }
                },
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        UserId = "User2Id",
                        Name = "User 2",
                        TotalPoints = 0,
                        PegCount = 0,
                        PenaltyCount = 0,
                        PegsGivenCount = 1,
                        Pegs = new List<PegDetails>(),
                        Penalties = new List<PegDetails>()
                    }
                },
                new List<PegCategory>
                {
                    new PegCategory
                    {
                        Name = "keyword1",
                        Recipients = new List<PegRecipient>
                        {
                            new PegRecipient
                            {
                                UserId = "User1Id",
                                Name = "User 1",
                                Location = "Location1",
                                TotalPoints = 0,
                                PegCount = 1,
                                PenaltyCount = 1,
                                PegsGivenCount = 0,
                                Pegs = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "User 2",
                                        Weight = 1,
                                        Comment = "This is a peg keyword1 keyword2",
                                        Keywords = new List<string> { "keyword1", "keyword2" },
                                        SenderLocation = "Location1"
                                    }
                                },
                                Penalties = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "OtherBot",
                                        Weight = 1,
                                        Comment = "This is a shame peg penaltyKeyword",
                                        Keywords = new List<string> { "penaltyKeyword" },
                                        SenderLocation = "Location1"
                                    }
                                }
                            }
                        }
                    },
                    new PegCategory
                    {
                        Name = "keyword2",
                        Recipients = new List<PegRecipient>
                        {
                            new PegRecipient
                            {
                                UserId = "User1Id",
                                Name = "User 1",
                                Location = "Location1",
                                TotalPoints = 0,
                                PegCount = 1,
                                PenaltyCount = 1,
                                PegsGivenCount = 0,
                                Pegs = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "User 2",
                                        Weight = 1,
                                        Comment = "This is a peg keyword1 keyword2",
                                        Keywords = new List<string> { "keyword1", "keyword2" },
                                        SenderLocation = "Location1"
                                    }
                                },
                                Penalties = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "OtherBot",
                                        Weight = 1,
                                        Comment = "This is a shame peg penaltyKeyword",
                                        Keywords = new List<string> { "penaltyKeyword" },
                                        SenderLocation = "Location1"
                                    }
                                }
                            }
                        }
                    }
                },
                "http://fakelocation.com",
                new Message
                {
                    Text = "[Here are all pegs given this cycle](http://fakelocation.com)"
                }
            };
        }
    }
}
