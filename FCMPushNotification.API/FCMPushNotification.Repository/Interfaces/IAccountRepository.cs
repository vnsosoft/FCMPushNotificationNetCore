using FCMPushNotification.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCMPushNotification.Repository.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetAccountsAsync();

        Task<IEnumerable<Account>> GetOnlineAccountsAsync();

        Task<Account> FindById(Guid id);

        Task<Account> FindByUserName(string username);
    }
}
