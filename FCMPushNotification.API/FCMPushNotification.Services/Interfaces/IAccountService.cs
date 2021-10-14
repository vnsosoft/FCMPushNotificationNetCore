using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FCMPushNotification.Services.Models.Account;

namespace FCMPushNotification.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponse> AddAccountAsync(AddAccountRequest request);

        Task<IEnumerable<AccountResponse>> GetAccountAsync();

        Task<IEnumerable<AccountResponse>> GetOnlineAccountAsync();

        Task<bool> VerifyAccountAsync(LoginRequest request);

        Task LogoutAccountAsync(Guid accountId);

        Task LogoutAccountAsync(string username);
    }
}
