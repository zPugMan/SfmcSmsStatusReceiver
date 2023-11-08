using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Utils
{
    public static class EnvironmentUtils
    {
        public static string GetEnvironmentVariable(string name, string defaultValue) 
        {
            var val = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            return String.IsNullOrEmpty( val ) ? defaultValue : val;
        }
    }
}
