using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class RoleConfig : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IChatHelper _chatHelper;

        public string Command => Commands.RoleConfig;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public string[] Permissions => new[] { Roles.Admin, Roles.Config };

        public RoleConfig(IPockyUserRepository pockyUserRepository, IChatHelper chatHelper)
        {
            _pockyUserRepository = pockyUserRepository;
            _chatHelper = chatHelper;
        }

        public async Task<Message> Respond(Message message)
        {
            var command = message.MessageParts[1].Text
                .Trim()
                .Remove(0, Command.Length)
                .Trim()
                .ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(command))
            {
                return new Message
                {
                    Text = "Please specify a command. Possible values are get, set, and delete."
                };
            }

            string responseText;
            switch (command)
            {
                case Actions.Get:
                    responseText = await GetRoleConfigMessage();
                    break;
                case Actions.Set:
                    responseText = await SetRoleConfigMessage(message);
                    break;
                case Actions.Delete:
                    responseText = await DeleteRoleConfigMessage(message);
                    break;
                default:
                    responseText = "Unknown command. Possible values are get, set, and delete.";
                    break;
            }

            return new Message
            {
                Text = responseText
            };
        }

        private async Task<string> GetRoleConfigMessage()
        {
            var users = await _pockyUserRepository.GetAllUserRolesAsync();

            if (users.Count == 0)
            {
                return "No roles have been set.";
            }

            var roleConfigMessageBuilder = new StringBuilder("Here is the current config:\n\n");
            foreach (var user in users)
            {
                roleConfigMessageBuilder.Append(
                    $"- {user.Username}: {string.Join(", ", user.Roles.Select(x => x.UserRole))}\n");
            }

            return roleConfigMessageBuilder.ToString();
        }

        private async Task<string> SetRoleConfigMessage(Message message)
        {
            if (message.MessageParts.Length != 4)
            {
                return "You must specify a user and a role to set.";
            }

            if (message.MessageParts[2].MessageType != MessageType.PersonMention)
            {
                return "Please mention a user you want to set a role for.";
            }

            var validRoles = Roles.All;
            var desiredRole = message.MessageParts[3].Text.Trim().ToUpperInvariant();
            if (!validRoles.Contains(desiredRole))
            {
                return $"Invalid role {desiredRole}. Valid values are: {string.Join(", ", validRoles)}.";
            }

            var userId = message.MessageParts[2].UserId;
            var user = await _chatHelper.People.GetPersonAsync(userId);
            var pockyUser = await _pockyUserRepository.AddOrUpdateUserAsync(userId, user.Username);

            if (pockyUser.HasRole(desiredRole))
            {
                return $"Role {desiredRole} is already set for user {pockyUser.Username}.";
            }

            await _pockyUserRepository.AddRoleAsync(userId, desiredRole);
            return "Role has been set.";
        }

        private async Task<string> DeleteRoleConfigMessage(Message message)
        {
            if (message.MessageParts.Length != 4)
            {
                return "You must specify a user and a role to delete.";
            }

            if (message.MessageParts[2].MessageType != MessageType.PersonMention)
            {
                return "Please mention a user you want to delete a role for.";
            }

            var validRoles = Roles.All;
            var roleToDelete = message.MessageParts[3].Text.Trim().ToUpperInvariant();
            if (!validRoles.Contains(roleToDelete))
            {
                return $"Invalid role {roleToDelete}. Valid values are: {string.Join(", ", validRoles)}.";
            }

            var userId = message.MessageParts[2].UserId;
            var user = await _chatHelper.People.GetPersonAsync(userId);
            var pockyUser = await _pockyUserRepository.AddOrUpdateUserAsync(userId, user.Username);

            if (!pockyUser.HasRole(roleToDelete))
            {
                return $"Role {roleToDelete} is not set for user {pockyUser.Username}.";
            }

            await _pockyUserRepository.RemoveRoleAsync(userId, roleToDelete);
            return "Role has been deleted.";
        }
    }
}
