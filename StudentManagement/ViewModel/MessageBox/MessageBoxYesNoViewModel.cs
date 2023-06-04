using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
namespace StudentManagement.ViewModel.MessageBox
{
    public class MessageBoxYesNoViewModel : BaseViewModel
    {
        #region commands
        public ICommand Yes { get; set; }

        public ICommand No { get; set; }

        public ICommand Loaded { get; set; }

        #endregion

        private string _Title;
        public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }

        private string _Question;
        public string Question { get => _Question; set { _Question = value; OnPropertyChanged(); } }

        public bool IsYes = false;
        public MessageBoxYesNoViewModel()
        {
            Loaded = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                IsYes = false;
            }
            );

            No = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            }
            );

            Yes = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                IsYes = true;
                p.Close();
            }
            );
        }

    }
}
