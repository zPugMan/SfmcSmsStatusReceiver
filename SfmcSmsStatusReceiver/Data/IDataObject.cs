using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmcSmsStatusReceiver.Data
{
    public interface IDataObject
    {
        void LoadData(QueueMessage data);

        bool IsValidForUpload();
    }
}
