using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MvxStarter.Core.ViewModels.Controls
{
    public class TemplateNumericUpDown : MvxViewModel
    {
        public TemplateNumericUpDown()
        {
            CmdUpCommand = new MvxCommand(() => CmdUp_Click());
            CmdDownCommand = new MvxCommand(() => CmdDown_Click());
            ControlVisible = "Visible";
        }

        private int _numValue = 0;
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                if (value >= 0)
                {
                    SetProperty(ref _numValue, value);
                    TxtNum = value.ToString();
                }
            }
        }

        private string _txtNum;

        public string TxtNum
        {
            get { return _txtNum; }
            set { SetProperty(ref _txtNum, value); }
        }

        public IMvxCommand CmdUpCommand { get; set; }

        public IMvxCommand CmdDownCommand { get; set; }

        private string _controlVisible;
        public string ControlVisible
        {
            get { return _controlVisible; }
            set { SetProperty(ref _controlVisible, value); }
        }

        private void CmdUp_Click()
        {
            NumValue++;
        }

        private void CmdDown_Click()
        {
            NumValue--;
        }
    }
}
