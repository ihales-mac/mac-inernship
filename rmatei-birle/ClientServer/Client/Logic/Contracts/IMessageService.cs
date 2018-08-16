using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.EventArguments;

namespace Client.Logic.Contracts
{
    interface IMessageService
    {
        event LoginEventHandler LoginEvent;
        event OnlineUserEventHandler OnlineUserEvent;
        event MessageEventHandler MessageEvent;
    }
}
