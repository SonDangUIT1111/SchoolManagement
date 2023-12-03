using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThongTinHocSinhViewModel : BaseViewModel
    {
        const string cctt = "Chưa có thông tin";
        public bool everLoaded { get; set; }
        public ThongTinHocSinh ThongTinHocSinhWD { get; set; }
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public bool IsLoadAll { get; set; } = false;

        private bool _dataGridVisibility;
        public bool DataGridVisibility
        {
            get
            {
                return _dataGridVisibility;
            }
            set
            {
                _dataGridVisibility = value;
                
            }
        }

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility
        {
            get
            {
                return _progressBarVisibility;
            }
            set
            {
                _progressBarVisibility = value;
                
            }
        }


        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachHocSinh;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachHocSinh { get => _danhSachHocSinh; set { _danhSachHocSinh = value;  } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value;  } }
        private ObservableCollection<StudentManagement.Model.Khoi> _khoiCmb;
        public ObservableCollection<StudentManagement.Model.Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value;  } }
        private ObservableCollection<StudentManagement.Model.Lop> _lopCmb;
        public ObservableCollection<StudentManagement.Model.Lop> LopCmb { get => _lopCmb; set { _lopCmb = value;  } }
        //public ICommand Test { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand StudentSearch { get; set; }
        public ICommand StudentSearchAll { get; set; }
        public ICommand AddStudent { get; set; }
        public ICommand UpdateStudent { get; set; }
        public ICommand DeleteStudent { get; set; }
        public ICommand MouseEnterComboBox { get; set; }
        public ICommand MouseLeaveComboBox { get; set; }
        public async Task LoadThongTinHocSinh()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();

                    string CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe,DiaChi,Email from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh where hs.MaLop = " + LopQueries;
                    if (IsLoadAll)
                    {
                        CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe,DiaChi,Email from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh";
                    }
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    bool stillLoad = false;
                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            if (stillLoad == true)
                            {
                                //comment de test do data ko co truong hop nay
                                //if (reader.GetInt32(5) == 1)
                                //{
                                //    try
                                //    {
                                //        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                //        stillLoad = false;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = "Chưa có thông tin";
                                //        stillLoad = false;
                                //    }
                                //}
                                if (reader.GetInt32(5) == 2)
                                {
                                    try
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = false;
                                    }
                                    catch (Exception)
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = cctt;
                                        stillLoad = false;
                                    }
                                }
                            }
                            else
                            {
                                StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                                student.MaHocSinh = reader.GetInt32(0);
                                student.TenHocSinh = reader.GetString(1);
                                student.NgaySinh = reader.GetDateTime(2);
                                student.GioiTinh = reader.GetBoolean(3);
                                student.Avatar = (byte[])reader[6];
                                student.DiaChi = reader.GetString(7);
                                student.Email = reader.GetString(8);
                                if (reader.GetInt32(5) == 1)
                                {
                                    try
                                    {
                                        student.DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = true;
                                    }
                                    catch (Exception)
                                    {
                                        student.DiemTB1 = cctt;
                                        stillLoad = true;
                                    }
                                }
                                //comment de test vi du lieu khong co truong hop nay
                                //else if (reader.GetInt32(5) == 2)
                                //{
                                //    try
                                //    {
                                //        student.DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                //        stillLoad = true;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        student.DiemTB2 = "Chưa có thông tin";
                                //        stillLoad = true;
                                //    }
                                //}
                                DanhSachHocSinh.Add(student);
                                stillLoad = true;
                            }
                        }
                        await reader.NextResultAsync();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }

        public void LoadThongTinCmb()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    //đọc niên khóa
                    string CmdString = "select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NienKhoaCmb.Add(reader.GetString(0));
                            if (String.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                            }
                        }
                        reader.NextResult();
                    }
                    reader.Close();

                    //đọc khối
                    CmdString = "select distinct MaKhoi,Khoi from Khoi";
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Khoi item = new Khoi();
                            item.MaKhoi = reader.GetInt32(0);
                            item.TenKhoi = reader.GetString(1);
                            if (String.IsNullOrEmpty(KhoiQueries))
                            {
                                KhoiQueries = reader.GetInt32(0).ToString();
                            }
                            KhoiCmb.Add(item);
                        }
                        reader.NextResult();
                    }
                    reader.Close();
                    // đọc lớp
                    CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Lop item = new Lop();
                            item.MaLop = reader.GetInt32(0);
                            item.TenLop = reader.GetString(1);
                            if (String.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
                            }
                            LopCmb.Add(item);
                        }
                        reader.NextResult();
                    }
                } catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public void SearchHocSinh(string tenhs)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe, DiaChi,Email from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh " +
                                        " where hs.MaLop = " + LopQueries + " and TenHocSinh like N'%" + tenhs + "%'";
                    if (IsLoadAll)
                    {
                        CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe,DiaChi, Email from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh " +
                                        " where TenHocSinh like N'%" + tenhs + "%'";
                    }
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    bool stillLoad = false;
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (stillLoad == true)
                            {
                                //comment de test vi k co data truong hop nay
                                //if (reader.GetInt32(5) == 1)
                                //{
                                //    try
                                //    {
                                //        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                //        stillLoad = false;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = "Chưa có thông tin";
                                //    }
                                //}
                                if (reader.GetInt32(5) == 2)
                                {
                                    try
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = false;
                                    }
                                    catch (Exception)
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = cctt;
                                    }
                                }
                            }
                            else
                            {
                                StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                                student.MaHocSinh = reader.GetInt32(0);
                                student.TenHocSinh = reader.GetString(1);
                                student.NgaySinh = reader.GetDateTime(2);
                                student.GioiTinh = reader.GetBoolean(3);
                                student.Avatar = (byte[])reader[6];
                                student.DiaChi = reader.GetString(7);
                                student.Email = reader.GetString(8);
                                if (reader.GetInt32(5) == 1)
                                {
                                    try
                                    {
                                        student.DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = true;
                                    }
                                    catch (Exception)
                                    {
                                        student.DiemTB1 = cctt;
                                    }
                                }
                                //comment de test do data k co th nay
                                //else if (reader.GetInt32(5) == 2)
                                //{
                                //    try
                                //    {
                                //        student.DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                //        stillLoad = true;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        student.DiemTB2 = "Chưa có thông tin";
                                //    }
                                //}
                                DanhSachHocSinh.Add(student);
                                stillLoad = true;
                            }
                        }
                        reader.NextResult();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public void SearchHocSinhAll()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe,DiaChi,Email from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    bool stillLoad = false;
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (stillLoad == true)
                            {
                                //comment de test vi data k co th nay
                                //if (reader.GetInt32(5) == 1)
                                //{
                                //    try
                                //    {
                                //        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                //        stillLoad = false;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = "Chưa có thông tin";
                                //        stillLoad = false;
                                //    }
                                //}
                                if (reader.GetInt32(5) == 2)
                                {
                                    try
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = false;
                                    }
                                    catch (Exception)
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = cctt;
                                        stillLoad = false;
                                    }
                                }
                            }
                            else
                            {
                                StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                                student.MaHocSinh = reader.GetInt32(0);
                                student.TenHocSinh = reader.GetString(1);
                                student.NgaySinh = reader.GetDateTime(2);
                                student.GioiTinh = reader.GetBoolean(3);
                                student.Avatar = (byte[])reader[6];
                                student.DiaChi = reader.GetString(7);
                                student.Email = reader.GetString(8);
                                if (reader.GetInt32(5) == 1)
                                {
                                    try
                                    {
                                        student.DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = true;
                                    }
                                    catch (Exception)
                                    {
                                        student.DiemTB1 = cctt;
                                        stillLoad = true;
                                    }
                                }
                                // comment de test vi data k co th nay
                                //else if (reader.GetInt32(5) == 2)
                                //{
                                //    try
                                //    {
                                //        student.DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                //        stillLoad = true;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        student.DiemTB2 = "Chưa có thông tin";
                                //        stillLoad = true;
                                //    }
                                //}
                                DanhSachHocSinh.Add(student);
                                stillLoad = true;
                            }
                        }
                        reader.NextResult();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public void FilterLopFromSelection()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select Malop, TenLop from Lop where MaKhoi = " + KhoiQueries + " and NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Lop item = new Lop();
                            item.MaLop = reader.GetInt32(0);
                            item.TenLop = reader.GetString(1);
                            LopCmb.Add(item);
                        }
                        reader.NextResult();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            
            }
        }
        public int XoaHocSinh(Model.HocSinh item)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                int count = 0;
                    sqlConnectionWrap.Open();
                    SqlCommand cmd;
                    string CmdString = "Delete from HeThongDiem where MaHocSinh = " + item.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    count += cmd.ExecuteNonQuery();

                    CmdString = "Delete from ThanhTich where MaHocSinh = " + item.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    count +=cmd.ExecuteNonQuery();

                    CmdString = "Delete from HocSinh where MaHocSinh = " + item.MaHocSinh;
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    count+=cmd.ExecuteNonQuery();
                //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                //messageBoxSuccessful.ShowDialog();
                return count;

            }
        }

        public ThongTinHocSinhViewModel()
        {
            // Stryker disable all
            everLoaded = false;
            NienKhoaQueries = "";
            KhoiQueries = "";
            LopQueries = "";
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
            LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
            LoadThongTinCmb();

            LoadData = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    ThongTinHocSinhWD = parameter as ThongTinHocSinh;
                    ThongTinHocSinhWD.cmbNienKhoa.SelectedIndex = 0;
                    ThongTinHocSinhWD.cmbKhoi.SelectedIndex = 0;
                    ThongTinHocSinhWD.cmbLop.SelectedIndex = 0;
                    DataGridVisibility = false;
                    ProgressBarVisibility = true;
                    DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
                    NienKhoaCmb = new ObservableCollection<string>();
                    KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                    LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                    await LoadThongTinHocSinh();
                    DataGridVisibility = true;
                    ProgressBarVisibility = false;
                    everLoaded = true;
                }
            });
            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                IsLoadAll = false;
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    string item = cmb.SelectedItem as string;
                    if (item != null)
                    {
                        NienKhoaQueries = item.ToString();
                        LopCmb.Clear();
                        FilterLopFromSelection();
                        if (LopCmb.Count > 0)
                        {
                            ThongTinHocSinhWD.cmbLop.SelectedIndex = 0;
                        }
                        else
                        {
                            ThongTinHocSinhWD.cmbLop.SelectedIndex = -1;
                        }
                        if (ThongTinHocSinhWD.cmbLop.SelectedIndex >= 0)
                        {
                            DataGridVisibility = false;
                            ProgressBarVisibility = true;
                            DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
                            NienKhoaCmb = new ObservableCollection<string>();
                            KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                            LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                            await LoadThongTinHocSinh();
                            DataGridVisibility = true;
                            ProgressBarVisibility = false;
                        }
                        else DanhSachHocSinh.Clear();
                    }
                }
            });
            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                IsLoadAll = false;
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    if (item != null)
                    {
                        KhoiQueries = item.MaKhoi.ToString();
                        LopCmb.Clear();
                        FilterLopFromSelection();
                        if (LopCmb.Count > 0)
                        {
                            ThongTinHocSinhWD.cmbLop.SelectedIndex = 0;
                        }
                        else
                        {
                            ThongTinHocSinhWD.cmbLop.SelectedIndex = -1;
                        }
                    }
                }
            });
            FilterLop = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                IsLoadAll = false;
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    if (item != null)
                    {
                        LopQueries = item.MaLop.ToString();
                        if (ThongTinHocSinhWD.cmbLop.SelectedIndex >= 0)
                        {
                            DataGridVisibility = false;
                            ProgressBarVisibility = true;
                            DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
                            NienKhoaCmb = new ObservableCollection<string>();
                            KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                            LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                            await LoadThongTinHocSinh();
                            DataGridVisibility = true;
                            ProgressBarVisibility = false;
                        }
                        else DanhSachHocSinh.Clear();
                    }
                }
            });
            StudentSearch = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachHocSinh.Clear();
                SearchHocSinh(parameter.Text);
            });
            StudentSearchAll = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachHocSinh.Clear();
                IsLoadAll = true;
                SearchHocSinhAll();
            });
            AddStudent = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ThemHocSinhMoi window = new ThemHocSinhMoi();
                ThemHocSinhMoiViewModel data = window.DataContext as ThemHocSinhMoiViewModel;
                window.ShowDialog();
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachHocSinh = new ObservableCollection<Model.HocSinh>(); NienKhoaCmb = new ObservableCollection<string>();
                KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                await LoadThongTinHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
            UpdateStudent = new RelayCommand<Model.HocSinh>((parameter) => { return true; }, async (parameter) =>
            {
                SuaThongTinHocSinh window = new SuaThongTinHocSinh();
                SuaThongTinHocSinhViewModel data = window.DataContext as SuaThongTinHocSinhViewModel;
                data.HocSinhHienTai = parameter;
                window.ShowDialog();
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
                NienKhoaCmb = new ObservableCollection<string>();
                KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                await LoadThongTinHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
            DeleteStudent = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có chắc chắn muốn xóa học sinh này?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true)
                {
                    Model.HocSinh item = parameter as Model.HocSinh;
                    XoaHocSinh(item);
                }
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
                NienKhoaCmb = new ObservableCollection<string>();
                KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
                LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
                await LoadThongTinHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
            MouseEnterComboBox = new RelayCommand<ComboBox>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Focus();
            });
            MouseLeaveComboBox = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThongTinHocSinhWD.btnTrick.Focus();
            });
        }
    }
}
