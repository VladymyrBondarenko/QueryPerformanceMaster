using MvvmCross.Plugin.Messenger;
using MvxStarter.Core.ViewModels;
using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Messages
{
    public class ClosedQueryEditorTabMessage : MvxMessage
    {
        public ClosedQueryEditorTabMessage(object sender, QueryEditorTabViewModel editorTabViewModel)
            : base(sender)
        {
            EditorTabViewModel = editorTabViewModel;
        }

        public QueryEditorTabViewModel EditorTabViewModel { get; private set; }
    }
}
