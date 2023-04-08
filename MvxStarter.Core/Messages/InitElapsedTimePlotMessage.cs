using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Messages
{
    public class InitElapsedTimePlotMessage : MvxMessage
    {
        public InitElapsedTimePlotMessage(object sender, double[] dataX, double[] dataY)
          : base(sender)
        {
            DataX = dataX;
            DataY = dataY;
        }

        public double[] DataX { get; }

        public double[] DataY { get; }
    }
}
