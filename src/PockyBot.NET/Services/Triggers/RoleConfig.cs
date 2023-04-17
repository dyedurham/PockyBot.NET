using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Helpers;

namespace PockyBot.NET.Services.Triggers
{
    internal class RoleConfig : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IChatHelper _chatHelper;

        public string Command => Commands.RoleConfig;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public Role[] Permissions => new[] { Role.Admin, Role.Config };

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
                    responseText = await GetRoleConfigMessage().ConfigureAwait(false);
                    break;
                case Actions.Set:
                    responseText = await SetRoleConfigMessage(message).ConfigureAwait(false);
                    break;
                case Actions.Delete:
                    responseText = await DeleteRoleConfigMessage(message).ConfigureAwait(false);
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

        public string GetHelpMessage(string botName, PockyUser user)
        {
            return "### How to configure role config values 🗞️!\n" +
                   $"1. To get/set/delete user roles, type `@{botName} {Commands.RoleConfig} {Actions.Get}|{Actions.Set}|{Actions.Delete} {{@User}} {{role}}`\n" +
                   "1. I will respond in the room you messaged me in.";
        }

        private async Task<string> GetRoleConfigMessage()
        {
            var users = await _pockyUserRepository.GetAllUserRolesAsync();

            if (users.Count == 0)
            {
                return "No roles have been set.";
            }

            var roleConfigMessageBuilder = new StringBuilder("Here is the current config:\n\n");
            foreach (var user in users.OrderBy(x => x.Username))
            {
                roleConfigMessageBuilder.AppendLine(
                    $"* {user.Username}: {string.Join(", ", user.Roles.Select(x => x.Role).OrderBy(x => x))}");
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

            var desiredRole = message.MessageParts[3].Text.Trim().ToUpperInvariant();
            if (!EnumHelpers.IsEnumDefinedCaseInsensitive(typeof(Role), desiredRole))
            {
                return $"Invalid role {desiredRole}. Valid values are: {string.Join(", ", Enum.GetValues(typeof(Role)))}.";
            }

            var roleEnum = Enum.Parse<Role>(desiredRole, true);

            var userId = message.MessageParts[2].UserId;
            var user = await _chatHelper.People.GetPersonAsync(userId);
            var pockyUser = await _pockyUserRepository.AddOrUpdateUserAsync(userId, user.Username);

            if (pockyUser.HasRole(roleEnum))
            {
                return $"Role {desiredRole} is already set for user {pockyUser.Username}.";
            }

            await _pockyUserRepository.AddRoleAsync(userId, roleEnum);
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

            var roleToDelete = message.MessageParts[3].Text.Trim().ToUpperInvariant();
            if (!EnumHelpers.IsEnumDefinedCaseInsensitive(typeof(Role), roleToDelete))
            {
                return
                    $"Invalid role {roleToDelete}. Valid values are: {string.Join(", ", Enum.GetValues(typeof(Role)))}.";
            }

            var roleEnum = Enum.Parse<Role>(roleToDelete, true);

            var userId = message.MessageParts[2].UserId;
            var user = await _chatHelper.People.GetPersonAsync(userId);
            var pockyUser = await _pockyUserRepository.AddOrUpdateUserAsync(userId, user.Username);

            if (!pockyUser.HasRole(roleEnum))
            {
                return $"Role {roleToDelete} is not set for user {pockyUser.Username}.";
            }

            await _pockyUserRepository.RemoveRoleAsync(userId, roleEnum);
            return "Role has been deleted.";
        }
    }
}
