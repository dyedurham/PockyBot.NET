using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Models;

namespace PockyBot.NET.Tests.TestData
{
    internal static class DirectResultsMessageSenderTestData
    {
        public static IEnumerable<object[]> SendDirectMessagesTestData()
        {
            yield return new object[]
            {
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        PegCount = 1,
                        PenaltyCount = 0,
                        TotalPoints = 1,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                Comment = "This is a comment keyword1",
                                Keywords = new List<string> { "keyword1" },
                                SenderLocation = "Location1",
                                SenderName = "Sender Name",
                                Weight = 1
                            }
                        }
                    }
                },
                new List<Message>
                {
                    new Message
                    {
                        Text = "You have received 1 peg and 0 penalties this cycle, for a total of 1 point:\n\n* **Sender Name** (Location1, 1 point) — \"_This is a comment keyword1_\""
                    }
                }
            };

            yield return new object[]
            {
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        PegCount = 2,
                        PenaltyCount = 0,
                        TotalPoints = 3,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                Comment = "This is a comment keyword1",
                                Keywords = new List<string> { "keyword1" },
                                SenderLocation = "Location1",
                                SenderName = "Sender Name",
                                Weight = 1
                            },
                            new PegDetails
                            {
                                Comment = "This is another comment keyword2",
                                Keywords = new List<string> { "keyword2" },
                                SenderLocation = "Location2",
                                SenderName = "Other Sender",
                                Weight = 2
                            }
                        }
                    }
                },
                new List<Message>
                {
                    new Message
                    {
                        Text = "You have received 2 pegs and 0 penalties this cycle, for a total of 3 points:\n\n* **Sender Name** (Location1, 1 point) — \"_This is a comment keyword1_\"\n* **Other Sender** (Location2, 2 points) — \"_This is another comment keyword2_\""
                    }
                }
            };

            yield return new object[]
            {
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        PegCount = 1,
                        PenaltyCount = 0,
                        TotalPoints = 1,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                Comment = "This is a comment keyword1",
                                Keywords = new List<string> { "keyword1" },
                                SenderLocation = "Location1",
                                SenderName = "Sender Name",
                                Weight = 1
                            }
                        }
                    },
                    new PegRecipient
                    {
                        PegCount = 2,
                        PenaltyCount = 0,
                        TotalPoints = 3,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                Comment = "This is a comment keyword1",
                                Keywords = new List<string> { "keyword1" },
                                SenderLocation = "Location1",
                                SenderName = "Sender Name",
                                Weight = 1
                            },
                            new PegDetails
                            {
                                Comment = "This is another comment keyword2",
                                Keywords = new List<string> { "keyword2" },
                                SenderLocation = "Location2",
                                SenderName = "Other Sender",
                                Weight = 2
                            }
                        }
                    },
                    new PegRecipient
                    {
                        PegCount = 0,
                        PenaltyCount = 1,
                        TotalPoints = 0,
                        Pegs = new List<PegDetails>()
                    },
                    new PegRecipient
                    {
                        PegCount = 1,
                        PenaltyCount = 1,
                        TotalPoints = 0,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                Comment = "This is a comment keyword1",
                                Keywords = new List<string> { "keyword1" },
                                SenderLocation = "Location1",
                                SenderName = "Sender Name",
                                Weight = 1
                            }
                        }
                    }
                },
                new List<Message>
                {
                    new Message
                    {
                        Text = "You have received 1 peg and 0 penalties this cycle, for a total of 1 point:\n\n* **Sender Name** (Location1, 1 point) — \"_This is a comment keyword1_\""
                    },
                    new Message
                    {
                        Text = "You have received 2 pegs and 0 penalties this cycle, for a total of 3 points:\n\n* **Sender Name** (Location1, 1 point) — \"_This is a comment keyword1_\"\n* **Other Sender** (Location2, 2 points) — \"_This is another comment keyword2_\""
                    },
                    new Message
                    {
                        Text = "You have received 0 pegs and 1 penalty this cycle, for a total of 0 points:\n\n"
                    },
                    new Message
                    {
                        Text = "You have received 1 peg and 1 penalty this cycle, for a total of 0 points:\n\n* **Sender Name** (Location1, 1 point) — \"_This is a comment keyword1_\""
                    }
                }
            };
        }
    }
}
