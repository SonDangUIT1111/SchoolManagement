using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class LopHocViewModel : BaseViewModel
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
                _nienKhoaCmb = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> _khoiCmb;

        public ObservableCollection<string> KhoiCmb
        {
            get => _khoiCmb;
            set
            {
                _khoiCmb = value;
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
        public ICommand XemLop { get; set; }
        public LopHocViewModel()
        {

            DanhSachLopHoc = new ObservableCollection<Lop>();
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<string>();
            DanhSachLopHoc = new ObservableCollection<Model.Lop>();
            LoadDanhSachLopHoc();

            LoadLopHoc = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                LopHocWD = parameter as LopHoc;
                LoadComboBox();
            });

            ThemLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemLopHoc window = new ThemLopHoc();
                ThemLopHocViewModel data = window.DataContext as ThemLopHocViewModel;
                window.ShowDialog();
                LoadDanhSachLopHoc();
            });

            SuaLop = new RelayCommand<Model.Lop>((parameter) => { return true; }, (parameter) =>
            {
                SuaThongTinLopHoc window = new SuaThongTinLopHoc();
                SuaLopHocViewModel data = window.DataContext as SuaLopHocViewModel;
                data.LopHocHienTai = parameter;
                window.ShowDialog();
            });
            XemLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
             {
                 DanhSachLop danhSachLopWd = new DanhSachLop();
                 DanhSachLopViewModel vm = danhSachLopWd.DataContext as DanhSachLopViewModel;
                 Model.Lop lop = parameter as Model.Lop;
                 vm.MaLop = lop.MaLop;
                 vm.TenLop = lop.TenLop;
                 LopHocWD.NavigationService.Navigate(danhSachLopWd);
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
                    LoadDanhSachLopHoc();
                }
            });

            FilterTenLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                TextBox txt = parameter as TextBox;
                if (txt != null)
                {
                    TenLopQueries = txt.Text.ToString();
                    FilterFromTenLop();
                }
            });

            XoaLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Model.Lop item = parameter as Model.Lop;
                if (MessageBox.Show("Bạn có muốn xoá lớp không?", "Xoá lớp", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                    {
                        try
                        {
                            try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                            string cmdString = "DELETE FROM Lop WHERE TenLop = '" + item.TenLop + "'";
                            SqlCommand cmd = new SqlCommand(cmdString, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        }
                        LoadDanhSachLopHoc();
                    }
                }
            });


        }

        public void LoadComboBox()
        {

            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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

                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
            }
        }
        public void LoadDanhSachLopHoc()
        {
            DanhSachLopHoc.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }

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
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop lophoc = new StudentManagement.Model.Lop();
                            lophoc.TenLop = reader.GetString(1);
                            lophoc.SiSo = reader.GetInt32(2);
                            lophoc.NienKhoa = reader.GetString(3);
                            lophoc.TenGVCN = reader.GetString(7);
                            DanhSachLopHoc.Add(lophoc);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
            }
        }
        public void FilterFromTenLop()
        {
            DanhSachLopHoc.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }

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
                            WhereCmdString = WhereCmdString + " AND TenLop like '%" + TenLopQueries + "%'";
                        }
                        else
                        {
                            WhereCmdString = WhereCmdString + " WHERE TenLop like '%" + TenLopQueries + "%'";
                        }
                    }

                    string CmdString = "SELECT * FROM Lop" + WhereCmdString;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop lophoc = new StudentManagement.Model.Lop();
                            lophoc.TenLop = reader.GetString(1);
                            lophoc.SiSo = reader.GetInt32(2);
                            lophoc.NienKhoa = reader.GetString(3);
                            lophoc.TenGVCN = reader.GetString(7);
                            DanhSachLopHoc.Add(lophoc);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
            }
        }
        public void FilterKhoiFromNiemKhoa()
        {

        }
    }
}
