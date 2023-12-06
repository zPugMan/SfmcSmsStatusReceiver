using SfmcSmsStatusReceiver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Services
{
    public interface IDataLoader
    {
        Task<bool> UpsertDataAsync(IAuth authCache, SfmcDataExtension data);
    }
}
