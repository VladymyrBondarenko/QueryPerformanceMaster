using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxStarter.Core.Models
{
    public class LoadProfilersModel
    {
		private ObservableCollection<LoadProfilerModel> _loadProfilerModels;

		public ObservableCollection<LoadProfilerModel> LoadProfilerModels
        {
			get { return _loadProfilerModels; }
			set { _loadProfilerModels = value; }
		}
	}
}
