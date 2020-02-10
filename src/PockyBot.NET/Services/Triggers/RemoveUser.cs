using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class RemoveUser : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        public string Command => Commands.RemoveUser;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => true;
        public string[] Permissions => new []{Roles.Admin, Roles.RemoveUser};

        public RemoveUser(IPockyUserRepository pockyUserRepository)
        {
            _pockyUserRepository = pockyUserRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            var messageParts = message.MessageParts.Skip(1).ToList();
            messageParts[0].Text = messageParts[0].Text.TrimStart().Remove(Commands.RemoveUser.Length);
            messageParts = messageParts.Where(x => !string.IsNullOrWhiteSpace(x.Text)).ToList();

            if (messageParts.Count < 1)
            {
                return new Message
                {
                    Text = "Please mention or provide the name of the person you want to remove."
                };
            }

            string messageText;

            if (messageParts.Count == 1)
            {
                switch (messageParts[0].MessageType)
                {
                    case MessageType.Text:
                        messageText = await RemoveUserByUsername(messageParts[0].Text).ConfigureAwait(false);
                        break;
                    case MessageType.PersonMention:
                        messageText = await RemoveUserByUserId(messageParts[0].UserId).ConfigureAwait(false);
                        break;
                    default:
                        messageText = "Invalid message type: cannot remove a group mention.";
                        break;
                }
            }
            else
            {
                messageText = "Can only remove one user at a time.";
            }
            return new Message
            {
                Text = messageText
            };
        }

        private async Task<string> RemoveUserByUsername(string username)
        {
            var pockyUsers = _pockyUserRepository.GetUsersByUsername(username);
            if (pockyUsers.Count() > 1)
            {
                return $"More than one user with the username '{username}' found, cannot remove them.";
            }
            if (!pockyUsers.Any())
            {
                return $"User with username '{username}', not found, cannot be removed";
            }
            var pockyUser = pockyUsers[0];

            if (UserHasOutstandingPegs(pockyUser))
            {
                return $"Cannot remove user '{username}', they still have outstanding pegs";
            }

            await _pockyUserRepository.RemoveUser(pockyUser);
            return $"User '{pockyUser.Username}' has been removed.";
        }

        private async Task<string> RemoveUserByUserId(string userId)
        {
            var pockyUser = _pockyUserRepository.GetUser(userId);

            if (UserHasOutstandingPegs(pockyUser))
            {
                return $"Cannot remove user '{pockyUser.Username}', they still have outstanding pegs";
            }

            await _pockyUserRepository.RemoveUser(pockyUser);
            return $"User '{pockyUser.Username}' has been removed.";
        }

        private static bool UserHasOutstandingPegs(PockyUser user)
        {
            return user.PegsGiven.Any() || user.PegsReceived.Any();
        }
    }
}
