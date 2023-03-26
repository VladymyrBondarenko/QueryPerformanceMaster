using MvvmCross.Plugin.Messenger;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Messages
{
    public class ConnectionErrorMessage : MvxMessage
    {
        public ConnectionErrorMessage(object sender, string errorMessage, string errorCaption)
           : base(sender)
        {
            ErrorCaption = errorCaption;
            ErrorMessage = errorMessage;
        }

        public string ErrorCaption { get; set; }

        public string ErrorMessage { get; set; }
    }
}
