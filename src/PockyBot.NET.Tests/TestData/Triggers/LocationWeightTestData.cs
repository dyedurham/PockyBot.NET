using System;
using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Tests.TestData.Triggers
{
    public static class LocationWeightTestData
    {
        public static IEnumerable<object[]> RespondTestData()
        {
            // no command
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight "
                        }
                    }
                },
                Array.Empty<string>(),
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Please specify a command. Possible values are get, set, and delete."
                }
            };

            // invalid command
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight blah"
                        }
                    }
                },
                Array.Empty<string>(),
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Unknown command. Possible values are get, set, and delete."
                }
            };

            // get command, no location weights
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight get"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "No location weights set."
                }
            };

            // get command, location weights set
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight get"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>
                {
                    new GeneralConfig { Name = "NotALocationWeight", Value = 0 },
                    new GeneralConfig { Name = "locationWeightLocation1toLocation2", Value = 66 },
                    new GeneralConfig { Name = "locationWeightlocation3ToLocation1", Value = 3 }
                },
                new Message
                {
                    Text = "Here are the current location weights:\n\n* Location1 <-> Location2: 66\n* Location1 <-> Location3: 3"
                }
            };

            // set command, not enough arguments
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set"
                        }
                    }
                },
                Array.Empty<string>(),
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Please specify two locations and a weight."
                }
            };

            // set command, invalid first location
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set blah Location2 3"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Location value \"blah\" is invalid."
                }
            };

            // set command, invalid second location
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set Location1 blah2 3"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Location value \"blah2\" is invalid."
                }
            };

            // set command, non integer
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set Location1 Location2 3.5"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Weight must be set to a whole number."
                }
            };

            // set command, less than zero
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set Location1 Location2 -3"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Weight must be greater than or equal to zero."
                }
            };

            // delete command, not enough arguments
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight delete"
                        }
                    }
                },
                Array.Empty<string>(),
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "You must specify a two location names to delete the weighting for."
                }
            };

            // delete command, invalid first location
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight delete blah Location2"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Location value \"blah\" is invalid."
                }
            };

            // delete command, invalid second location
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight delete Location1 blah2"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Location value \"blah2\" is invalid."
                }
            };

            // delete command, no existing config
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight delete Location1 Location2"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>
                {
                    new GeneralConfig { Name = "NotALocationWeight", Value = 0 },
                    new GeneralConfig { Name = "locationWeightlocation3ToLocation1", Value = 3 }
                },
                new Message
                {
                    Text = "No weighting found for locations Location1 and Location2."
                }
            };
        }

        public static IEnumerable<object[]> SetLocationWeightTestData()
        {
            // successful
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set Location1 Location2 3"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>(),
                new Message
                {
                    Text = "Location weight has been set."
                },
                "locationWeightLocation1toLocation2",
                3
            };

            // override existing config
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight set Location1 Location2 3"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>
                {
                    new GeneralConfig { Name = "NotALocationWeight", Value = 0 },
                    new GeneralConfig { Name = "locationWeightLocation1toLocation2", Value = 66 },
                    new GeneralConfig { Name = "locationWeightlocation3ToLocation1", Value = 3 }
                },
                new Message
                {
                    Text = "Location weight has been set."
                },
                "locationWeightLocation1toLocation2",
                3
            };
        }

        public static IEnumerable<object[]> DeleteLocationWeightTestData()
        {
            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight delete Location1 Location2"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>
                {
                    new GeneralConfig { Name = "NotALocationWeight", Value = 0 },
                    new GeneralConfig { Name = "locationWeightLocation1toLocation2", Value = 66 },
                    new GeneralConfig { Name = "locationWeightlocation3ToLocation1", Value = 3 }
                },
                new Message
                {
                    Text = "Location weight has been deleted."
                },
                new GeneralConfig { Name = "locationWeightLocation1toLocation2", Value = 66 }
            };

            yield return new object[]
            {
                new Message
                {
                    Sender = new Person
                    {
                        UserId = "testUserId",
                        Username = "Test User"
                    },
                    MessageParts = new []
                    {
                        new MessagePart
                        {
                            MessageType = MessageType.PersonMention,
                            Text = "TestBot",
                            UserId = "testBotId"
                        },
                        new MessagePart
                        {
                            MessageType = MessageType.Text,
                            Text = " locationweight delete Location1 Location2"
                        }
                    }
                },
                new[] { "Location1", "Location2", "Location3" },
                new List<GeneralConfig>
                {
                    new GeneralConfig { Name = "NotALocationWeight", Value = 0 },
                    new GeneralConfig { Name = "locationWeightLocation2toLocation1", Value = 66 },
                    new GeneralConfig { Name = "locationWeightlocation3ToLocation1", Value = 3 }
                },
                new Message
                {
                    Text = "Location weight has been deleted."
                },
                new GeneralConfig { Name = "locationWeightLocation2toLocation1", Value = 66 }
            };
        }
    }
}
