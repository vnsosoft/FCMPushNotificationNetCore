using FCMPushNotification.Entities;
using FCMPushNotification.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCMPushNotification.Repository.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(SampleDbContext context) : base(context)
        {
        }

        public async Task<Account> FindById(Guid id)
        {
            var account = await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return account;
        }

        public async Task<Account> FindByUserName(string username)
        {
            var account = await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == username);

            return account;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            var accounts = await _entities
                .AsNoTracking()
                .ToListAsync();

            return accounts;
        }

        public async Task<IEnumerable<Account>> GetOnlineAccountsAsync()
        {
            var accounts = await _entities
                .AsNoTracking()
                .Where(x=>x.IsOnline == true)
                .ToListAsync();

            return accounts;
        }
    }
}
