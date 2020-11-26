using SimpleTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleTrader.WPF.State.Accounts
{
    public interface IAccountStroe
    {
        Account CurrentAccount { get; set; }
        event Action StateChanged;
    }
}
