using PowerCreator.LiveClient.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.Record
{
    public interface IRecord : IDisposable
    {
        bool IsRecord { get; }
        RecAndLiveState State { get; }

        bool StartRecord(string recordFileSavePath);

        bool StopRecord();

        bool PauseRecord();

        bool ResumeRecord();

    }
}
