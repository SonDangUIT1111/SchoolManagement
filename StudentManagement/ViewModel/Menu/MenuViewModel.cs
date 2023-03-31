using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Menu
{
    public class MenuViewModel : ObservableObject
    {

        public Page _PageContent;

        public Page PageBaoCao=new StudentManagement.Views.Menu.BaoCao();
        public Page PageLopHoc= new StudentManagement.Views.Menu.LopHoc();
        public Page PageThayDoiQuyDinh= new StudentManagement.Views.Menu.ThayDoiQuyDinh();
        public Page PageThongTinGiaoVien= new StudentManagement.Views.Menu.ThongTinGiaoVien();
        public Page PageThongTinHocSinh= new StudentManagement.Views.Menu.ThongTinHocSinh();
        public Page PageThongTinTruong= new StudentManagement.Views.Menu.ThongTinTruong();
        private string _Tex="sek";
        public string Tex
        {
            get => _Tex;
            set {
                _Tex = value;
                OnPropertyChanged(nameof(Tex));
            }
        }

        public Page PageContent
        {
            get { return _PageContent; }
            set { _PageContent = value;
                OnPropertyChanged(nameof(PageContent));
            }
        }
         
        public ICommand NavigatePageBaoCao { get; set; }
        public ICommand NavigatePageLopHoc { get; set; }
        public ICommand NavigatePageThayDoiQuyDinh { get; set; }
        public ICommand NavigatePageThongTinGiaoVien { get; set; }
        public ICommand NavigatePageThongTinHocSinh { get; set; }
        public ICommand NavigatePageThongTinTruong { get; set; }

        public MenuViewModel()
        {
            NavigatePageBaoCao = new RelayCommand<Page>((paramater) => { return true; }, (paramater) =>{PageContent = PageBaoCao;}); 
            NavigatePageLopHoc = new RelayCommand<Page>((paramater) => { return true; }, (paramater) => { PageContent = PageLopHoc; });
            NavigatePageThayDoiQuyDinh = new RelayCommand<Page>((paramater) => { return true; }, (paramater) => { PageContent = PageThayDoiQuyDinh; });
            NavigatePageThongTinGiaoVien = new RelayCommand<Page>((paramater) => { return true; }, (paramater) => { PageContent = PageThongTinGiaoVien; });
            NavigatePageThongTinHocSinh = new RelayCommand<Page>((paramater) => { return true; }, (paramater) => { PageContent = PageThongTinHocSinh; });
            NavigatePageThongTinTruong = new RelayCommand<Page>((paramater) => { return true; }, (paramater) => { PageContent = PageThongTinTruong; });

        }
    }
}
