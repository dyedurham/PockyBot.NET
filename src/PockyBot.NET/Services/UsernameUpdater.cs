using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.People;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services
{
    internal class UsernameUpdater : IUsernameUpdater
    {
        private readonly IPersonHandler _personHandler;
        private readonly IPockyUserRepository _pockyUserRepository;

        public UsernameUpdater(IPersonHandler personHandler, IPockyUserRepository pockyUserRepository)
        {
            _personHandler = personHandler;
            _pockyUserRepository = pockyUserRepository;
        }

        public async Task<List<PockyUser>> UpdateUsernames(List<PockyUser> users)
        {
            var usersDetails = await Task.WhenAll(users.Select(x => _personHandler.GetPersonAsync(x.UserId)));
            var dbUpdates = new List<Task>();

            var updatedUsers = users.Select(u =>
            {
                var userDetails = usersDetails.FirstOrDefault(x => x.UserId == u.UserId);
                if (userDetails?.Username != u.Username)
                {
                    u.Username = userDetails?.Username;
                    dbUpdates.Add(_pockyUserRepository.UpdateUsernameAsync(u.UserId, u.Username));
                }

                return u;
            }).ToList();

            await Task.WhenAll(dbUpdates).ConfigureAwait(false);
            return updatedUsers;
        }
    }
}
