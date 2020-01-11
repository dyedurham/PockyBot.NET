using System.Collections.Generic;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Pegs
{
    public static class PegResultsHelperTestData
    {
        public static IEnumerable<object[]> MapUsersToPegRecipientsTestData()
        {
            yield return new object[]
            {
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
                            },
                            new Peg
                            {
                                Comment = "This is a peg keyword1",
                                Sender = new PockyUser
                                {
                                    UserId = "User3Id",
                                    Username = "User 3",
                                    Location = new UserLocation { Location = "Location2" }
                                }
                            }
                        },
                        PegsGiven = new List<Peg>
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
                                    Username = "User 1",
                                    Location = new UserLocation { Location = "Location1" }
                                }
                            }
                        }
                    },
                    new PockyUser
                    {
                        Username = "User 3",
                        UserId = "User3Id",
                        Roles = new List<Role>(),
                        Location = new UserLocation { Location = "Location2" },
                        PegsReceived = new List<Peg>(),
                        PegsGiven = new List<Peg>
                        {
                            new Peg
                            {
                                Comment = "This is a peg keyword1",
                                Receiver = new PockyUser
                                {
                                    Username = "User 1",
                                    Location = new UserLocation { Location = "Location1" }
                                }
                            }
                        }
                    }
                },
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        UserId = "User1Id",
                        Name = "User 1",
                        Location = "Location1",
                        TotalPoints = 2,
                        PegCount = 2,
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
                            },
                            new PegDetails
                            {
                                SenderName = "User 3",
                                Weight = 2,
                                Comment = "This is a peg keyword1",
                                Keywords = new List<string> { "keyword1" },
                                SenderLocation = "Location2"
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
            };
        }

        public static IEnumerable<object[]> GetWinnersTestData()
        {
            // no eligible winners
            yield return new object[]
            {
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        UserId = "User1Id",
                        Name = "User 1",
                        Location = "Location1",
                        TotalPoints = 2,
                        PegCount = 2,
                        PenaltyCount = 1,
                        PegsGivenCount = 0,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "User 2",
                                Weight = 1,
                                Comment = "This is a peg keyword1 keyword2",
                                Keywords = new List<string> {"keyword1", "keyword2"},
                                SenderLocation = "Location1"
                            },
                            new PegDetails
                            {
                                SenderName = "User 3",
                                Weight = 2,
                                Comment = "This is a peg keyword1",
                                Keywords = new List<string> {"keyword1"},
                                SenderLocation = "Location2"
                            }
                        },
                        Penalties = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "OtherBot",
                                Weight = 1,
                                Comment = "This is a shame peg penaltyKeyword",
                                Keywords = new List<string> {"penaltyKeyword"},
                                SenderLocation = "Location1"
                            }
                        }
                    }
                },
                1, // minimum
                1, // winners
                new List<PegRecipient>()
            };
            // single winner
            var winner1 = new PegRecipient
            {
                UserId = "Winner1Id",
                Name = "Winner 1",
                Location = "Location1",
                TotalPoints = 2,
                PegCount = 2,
                PenaltyCount = 1,
                PegsGivenCount = 2,
                Pegs = new List<PegDetails>
                {
                    new PegDetails
                    {
                        SenderName = "User 2",
                        Weight = 1,
                        Comment = "This is a peg keyword1 keyword2",
                        Keywords = new List<string> {"keyword1", "keyword2"},
                        SenderLocation = "Location1"
                    },
                    new PegDetails
                    {
                        SenderName = "User 3",
                        Weight = 2,
                        Comment = "This is a peg keyword1",
                        Keywords = new List<string> {"keyword1"},
                        SenderLocation = "Location2"
                    }
                },
                Penalties = new List<PegDetails>
                {
                    new PegDetails
                    {
                        SenderName = "OtherBot",
                        Weight = 1,
                        Comment = "This is a shame peg penaltyKeyword",
                        Keywords = new List<string> {"penaltyKeyword"},
                        SenderLocation = "Location1"
                    }
                }
            };
            var notEnoughPegsGiven = new PegRecipient
            {
                UserId = "NotEnoughPegs",
                Name = "Not Enough Pegs",
                Location = "Location2",
                TotalPoints = 2,
                PegCount = 1,
                PenaltyCount = 0,
                PegsGivenCount = 1,
                Pegs = new List<PegDetails>
                {
                    new PegDetails
                    {
                        SenderName = "User 2",
                        Weight = 1,
                        Comment = "This is an inter location peg keyword2",
                        Keywords = new List<string> { "keyword2" },
                        SenderLocation = "Location1"
                    }
                },
                Penalties = new List<PegDetails>()
            };
            var cutoff = new PegRecipient
            {
                UserId = "Cutoff",
                Name = "Cutoff",
                Location = "Location1",
                TotalPoints = 1,
                PegCount = 1,
                PenaltyCount = 0,
                PegsGivenCount = 2,
                Pegs = new List<PegDetails>
                {
                    new PegDetails
                    {
                        SenderName = "User 2",
                        Weight = 1,
                        Comment = "This is another peg keyword1",
                        Keywords = new List<string> { "keyword1" },
                        SenderLocation = "Location1"
                    }
                },
                Penalties = new List<PegDetails>()
            };

            yield return new object[]
            {
                new List<PegRecipient>
                {
                    winner1,
                    notEnoughPegsGiven,
                    cutoff
                },
                2, // minimum
                1, // winners
                new List<PegRecipient>
                {
                    winner1
                }
            };

            yield return new object[]
            {
                new List<PegRecipient>
                {
                    winner1,
                    notEnoughPegsGiven,
                    cutoff
                },
                2, // minimum
                2, // winners
                new List<PegRecipient>
                {
                    winner1,
                    cutoff
                }
            };
        }

        public static IEnumerable<object[]> GetCategoriesTestData()
        {
            yield return new object[]
            {
                new List<PegRecipient>
                {
                    new PegRecipient
                    {
                        UserId = "Winner1Id",
                        Name = "Winner 1",
                        Location = "Location1",
                        TotalPoints = 3,
                        PegCount = 3,
                        PenaltyCount = 1,
                        PegsGivenCount = 2,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "User 2",
                                Weight = 1,
                                Comment = "This is a peg keyword1 keyword2",
                                Keywords = new List<string> {"keyword1", "keyword2"},
                                SenderLocation = "Location1"
                            },
                            new PegDetails
                            {
                                SenderName = "User 3",
                                Weight = 1,
                                Comment = "This is a peg keyword1",
                                Keywords = new List<string> {"keyword1"},
                                SenderLocation = "Location2"
                            },
                            new PegDetails
                            {
                                SenderName = "User 4",
                                Weight = 1,
                                Comment = "This is a peg keyword2",
                                Keywords = new List<string> {"keyword2"},
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
                                Keywords = new List<string> {"penaltyKeyword"},
                                SenderLocation = "Location1"
                            }
                        }
                    },
                    new PegRecipient
                    {
                        UserId = "NotEnoughPegs",
                        Name = "Not Enough Pegs",
                        Location = "Location2",
                        TotalPoints = 2,
                        PegCount = 1,
                        PenaltyCount = 0,
                        PegsGivenCount = 1,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "User 2",
                                Weight = 1,
                                Comment = "This is an inter location peg keyword2",
                                Keywords = new List<string> {"keyword2"},
                                SenderLocation = "Location1"
                            }
                        },
                        Penalties = new List<PegDetails>()
                    },
                    new PegRecipient
                    {
                        UserId = "Cutoff",
                        Name = "Cutoff",
                        Location = "Location1",
                        TotalPoints = 1,
                        PegCount = 1,
                        PenaltyCount = 0,
                        PegsGivenCount = 2,
                        Pegs = new List<PegDetails>
                        {
                            new PegDetails
                            {
                                SenderName = "User 2",
                                Weight = 1,
                                Comment = "This is another peg keyword1",
                                Keywords = new List<string> {"keyword1"},
                                SenderLocation = "Location1"
                            }
                        },
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
                                UserId = "Winner1Id",
                                Name = "Winner 1",
                                Location = "Location1",
                                TotalPoints = 2,
                                PegCount = 2,
                                PenaltyCount = 0,
                                PegsGivenCount = 2,
                                Pegs = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "User 2",
                                        Weight = 1,
                                        Comment = "This is a peg keyword1 keyword2",
                                        Keywords = new List<string> {"keyword1", "keyword2"},
                                        SenderLocation = "Location1"
                                    },
                                    new PegDetails
                                    {
                                        SenderName = "User 3",
                                        Weight = 1,
                                        Comment = "This is a peg keyword1",
                                        Keywords = new List<string> {"keyword1"},
                                        SenderLocation = "Location2"
                                    }
                                },
                                Penalties = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "OtherBot",
                                        Weight = 1,
                                        Comment = "This is a shame peg penaltyKeyword",
                                        Keywords = new List<string> {"penaltyKeyword"},
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
                                UserId = "Winner1Id",
                                Name = "Winner 1",
                                Location = "Location1",
                                TotalPoints = 2,
                                PegCount = 2,
                                PenaltyCount = 0,
                                PegsGivenCount = 2,
                                Pegs = new List<PegDetails>
                                {
                                    new PegDetails
                                    {
                                        SenderName = "User 2",
                                        Weight = 1,
                                        Comment = "This is a peg keyword1 keyword2",
                                        Keywords = new List<string> {"keyword1", "keyword2"},
                                        SenderLocation = "Location1"
                                    },
                                    new PegDetails
                                    {
                                        SenderName = "User 4",
                                        Weight = 1,
                                        Comment = "This is a peg keyword2",
                                        Keywords = new List<string> {"keyword2"},
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
                                        Keywords = new List<string> {"penaltyKeyword"},
                                        SenderLocation = "Location1"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
