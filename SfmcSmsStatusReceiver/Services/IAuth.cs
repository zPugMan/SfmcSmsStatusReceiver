using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Services
{
    public interface IAuth
    {
        bool RequireAuth();
        string RestAPI { get; }
        string AuthToken { get; }
        DateTime? RefreshExpiry { get; }

        Task Initialize { get; }

        Task<bool> AuthorizeAsync();
    }
}
