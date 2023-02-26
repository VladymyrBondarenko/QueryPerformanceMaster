using QueryPerformanceMaster.Domain.SqlProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Models
{
    public class QueryEditorTabModel
    {
        public string TabTitle { get; set; }

        public string QueryEditorContent { get; set; }

        public SqlProvider SqlProvider { get; set; }

        public string Database { get; set; }

        public string ConnectionString { get; set; }

        public bool IsSelected { get; set; }
    }
}
