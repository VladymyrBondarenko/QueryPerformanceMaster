using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Messages
{
    public class CollapseWindowMessage : MvxMessage
    {
        public CollapseWindowMessage(object sender)
          : base(sender)
        {
        }
    }
}
