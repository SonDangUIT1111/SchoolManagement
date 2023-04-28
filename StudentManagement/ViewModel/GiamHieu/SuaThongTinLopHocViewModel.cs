using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class SuaThongTinLopHocViewModel:BaseViewModel
    {
        public string _tenLop;
        public string TenLop
        {
            get => _tenLop;
            set
            {
                _tenLop = value;
                OnPropertyChanged();
            }
        }
        public SuaThongTinLopHocViewModel()
        {
            MessageBox.Show(TenLop.ToString());
        }
    }
}
