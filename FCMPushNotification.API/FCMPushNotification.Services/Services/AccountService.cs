using AutoMapper;
using FCMPushNotification.Repository.Interfaces;
using FCMPushNotification.Services.Interfaces;
using FCMPushNotification.Services.Models.Account;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCMPushNotification.Services.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository accountRepository,
            IMapper mapper,
            ILogger<AccountService> logger) : base(mapper)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<AccountResponse> AddAccountAsync(AddAccountRequest request)
        {
            var existAccount = await _accountRepository.FindByUserName(request.UserName);
            if (existAccount != null)
            {
                throw new Exception("Tài khoản này đã tồn tại");
            }

            var account = new Entities.Account()
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                DeviceToken = request.DeviceToken,
                IsOnline = false,
            };

            _accountRepository.Add(account);
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<IEnumerable<AccountResponse>> GetAccountAsync()
        {
            var accounts = await _accountRepository.GetAccountsAsync();
            return _mapper.Map<IEnumerable<AccountResponse>>(accounts);
        }

        public async Task<IEnumerable<AccountResponse>> GetOnlineAccountAsync()
        {
            var accounts = await _accountRepository.GetOnlineAccountsAsync();
            return _mapper.Map<IEnumerable<AccountResponse>>(accounts);
        }

        public async Task<bool> VerifyAccountAsync(LoginRequest request)
        {
            var account = await _accountRepository.FindByUserName(request.UserName);
            if (account != null)
            {
                account.IsOnline = true;
                UpdateDeviceToken(account, request.DeviceToken);
                return true;
            }
            return false;
        }

        public async Task LogoutAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.FindById(accountId);
            if (account != null)
            {
                account.IsOnline = false;
                _accountRepository.Update(account);
            }
        }

        public async Task LogoutAccountAsync(string username)
        {
            var account = await _accountRepository.FindByUserName(username);
            if (account != null)
            {
                account.IsOnline = false;
                _accountRepository.Update(account);
            }
        }

        private void UpdateDeviceToken(Entities.Account account, string deviceToken)
        {
            if (account != null && account.DeviceToken != deviceToken)
            {
                account.DeviceToken = deviceToken;
                _accountRepository.Update(account);
            }
        }
    }
}
