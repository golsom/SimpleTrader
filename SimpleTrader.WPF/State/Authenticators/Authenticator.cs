using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.AuthenticationServices;
using SimpleTrader.WPF.State.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.WPF.State.Authenticators
{
    public class Authenticator :  IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStroe _accountStroe;

        public Authenticator(IAuthenticationService authenticationService, IAccountStroe accountStroe)
        {
            _authenticationService = authenticationService;
            _accountStroe = accountStroe;
        }
        
        public Account CurrentAccount
        {
            get
            {
                return _accountStroe.CurrentAccount;
            }
            private set
            {
                _accountStroe.CurrentAccount = value;
                StateChange?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;

        public event Action StateChange;

        public async Task Login(string username, string password)
        {
            CurrentAccount = await _authenticationService.Login(username, password);
        }

        public void Logout()
        {
            CurrentAccount = null;
        }

        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword)
        {
            return await _authenticationService.Register(email, username, password, confirmPassword);
        }
    }
}
