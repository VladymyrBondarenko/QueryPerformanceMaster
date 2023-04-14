using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Messages
{
    public class DropBuffersAndCacheErrorMessage : MvxMessage
    {
        public DropBuffersAndCacheErrorMessage(object sender, string errorMessage, string errorCaption)
           : base(sender)
        {
            ErrorCaption = errorCaption;
            ErrorMessage = errorMessage;
        }

        public string ErrorCaption { get; set; }

        public string ErrorMessage { get; set; }
    }
}
