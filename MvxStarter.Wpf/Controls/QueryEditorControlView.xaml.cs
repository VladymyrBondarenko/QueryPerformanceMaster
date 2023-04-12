using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using MvvmCross.Platforms.Wpf.Views;
using System.IO;
using System.Reflection;

namespace MvxStarter.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for QueryEditorControlView.xaml
    /// </summary>
    public partial class QueryEditorControlView : MvxWpfView
    {
        public QueryEditorControlView()
        {
            InitializeComponent();

            initQueryTextEditor();
        }

        private void initQueryTextEditor()
        {
            Stream? stream = null;
            try
            {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MvxStarter.Wpf.Resources.SQL.xshd");
                if (stream == null) return;
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    #pragma warning disable CA1416 // Validate platform compatibility
                    AvalonEdit.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    #pragma warning restore CA1416 // Validate platform compatibility
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }
    }
}
