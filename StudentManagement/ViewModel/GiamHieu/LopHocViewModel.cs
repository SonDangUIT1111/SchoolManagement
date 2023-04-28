using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class LopHocViewModel: BaseViewModel
    {

        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string TenLopQueries { get; set; }

        private ObservableCollection<StudentManagement.Model.Lop> _danhSachLopHoc;
        public ObservableCollection<StudentManagement.Model.Lop> DanhSachLopHoc { get => _danhSachLopHoc; set { _danhSachLopHoc = value; OnPropertyChanged(); } }
       
        public DanhSachLop DanhSachLopPage;
        public LopHoc LopHocWD { get; set; }

        public ObservableCollection<string> _nienKhoaCmb;
        
        public ObservableCollection<string> NienKhoaCmb
        {
            get => _nienKhoaCmb;
            set
            {
                _nienKhoaCmb= value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> _khoiCmb;

        public ObservableCollection<string> KhoiCmb
        {
            get => _khoiCmb;
            set
            {
                _khoiCmb= value;
                OnPropertyChanged();
            }
        }

        public Lop _gridSeletecdItem;
        
        public Lop GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                OnPropertyChanged();
            }
        }
            

        public ICommand SwitchDanhSachLop { get; set; }
        public ICommand ThemLop { get; set; }
        public ICommand LoadLopHoc { get; set; }        
        public ICommand FilterNienKhoa { get; set; }   
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterTenLop { get; set; }
        public ICommand XoaLop { get; set; }
        public ICommand SuaLop { get; set; }
        public LopHocViewModel()
        {

            DanhSachLopHoc = new ObservableCollection<Lop>();

            LoadDanhSachLopHoc();

            LoadLopHoc = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                LopHocWD = parameter as LopHoc;
                LoadComboBox();
            });

            ThemLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Window window = null;
                window = new ThemLopHoc();
                window.Show();
            });

            SuaLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaThongTinLopHocViewModel suaThongTinLopHocViewModel = new SuaThongTinLopHocViewModel();
                MessageBox.Show(GridSelectedItem.TenLop);
                suaThongTinLopHocViewModel.TenLop = GridSelectedItem.TenLop;
                Window window1 = null;
                window1 = new SuaThongTinLopHoc();
                window1.DataContext = suaThongTinLopHocViewModel;
                window1.Show();
            });

            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    LoadDanhSachLopHoc();
                    FilterKhoiFromNiemKhoa();
                }
            });

            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    KhoiQueries = cmb.SelectedItem.ToString();
                    LoadDanhSachLopHoc();                }
            });

            FilterTenLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                TextBox txt = parameter as TextBox;
                if (txt != null)
                {
                    TenLopQueries = txt.Text.ToString();
                    LoadDanhSachLopHoc();
                }
            });

            XoaLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (MessageBox.Show("Bạn có muốn xoá lớp không?", "Xoá lớp", MessageBoxButton.YesNo) == MessageBoxResult.Yes){
                    using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                    {
                        con.Open();
                        string cmdString = "DELETE FROM Lop WHERE TenLop = '" + GridSelectedItem.TenLop + "'";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        LoadDanhSachLopHoc();

                    }
                }
            });


        }

        public void LoadComboBox()
        {
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<string>();

            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "SELECT DISTINCT NienKhoa from Lop";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read()) 
                    {
                        NienKhoaCmb.Add(reader.GetString(0));
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                cmdString = "select Khoi from Khoi";
                cmd = new SqlCommand(cmdString, con);
                reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        KhoiCmb.Add(reader.GetString(0));
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LoadDanhSachLopHoc()
        {
            DanhSachLopHoc.Clear();
            DanhSachLopHoc = new ObservableCollection<Model.Lop>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();

                string WhereCmdString = "";

                if (!String.IsNullOrEmpty(NienKhoaQueries))
                {
                    WhereCmdString = WhereCmdString + " WHERE NienKhoa = '" + NienKhoaQueries + "'";
                }

                if (!String.IsNullOrEmpty(KhoiQueries))
                {
                    if (!String.IsNullOrEmpty(WhereCmdString))
                    {
                        WhereCmdString = WhereCmdString + " AND Khoi = '" + KhoiQueries + "'";
                    }
                    else
                    {
                        WhereCmdString = WhereCmdString + " WHERE Khoi = '" + KhoiQueries + "'";
                    }
                }

                if (!String.IsNullOrEmpty(TenLopQueries))
                {
                    if (!String.IsNullOrEmpty(WhereCmdString))
                    {
                        WhereCmdString = WhereCmdString + " AND TenLop = '" + TenLopQueries + "'";
                    }
                    else
                    {
                        WhereCmdString = WhereCmdString + " WHERE TenLop = '" + TenLopQueries + "'";
                    }
                }

                string CmdString = "SELECT * FROM Lop" + WhereCmdString;
                MessageBox.Show(CmdString);
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.Lop lophoc = new StudentManagement.Model.Lop();
                        lophoc.TenLop = reader.GetString(1);
                        DanhSachLopHoc.Add(lophoc);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void FilterKhoiFromNiemKhoa()
        {
          
        }
    }
}
