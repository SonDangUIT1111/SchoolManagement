using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModel.MessageBox
{
    public class MessageBoxOKViewModel : BaseViewModel
    {
        #region commands
        public ICommand OK { get; set; }
        #endregion

        private string _Content;
        public string Content { get => _Content; set { _Content = value; OnPropertyChanged(); } }

        public MessageBoxOKViewModel()
        {
            OK = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            }
            );
        }
    }
}
