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
    public class ThongTinHocSinhViewModel : BaseViewModel
    {
        public bool everLoaded { get; set; }
        public ThongTinHocSinh ThongTinHocSinhWD { get; set; }
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public bool IsLoadAll { get; set; } = false;
        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachHocSinh;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachHocSinh { get => _danhSachHocSinh; set { _danhSachHocSinh = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Khoi> _khoiCmb;
        public ObservableCollection<StudentManagement.Model.Khoi> KhoiCmb { get => _khoiCmb; set { _khoiCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.Lop> _lopCmb;
        public ObservableCollection<StudentManagement.Model.Lop> LopCmb { get => _lopCmb; set { _lopCmb = value; OnPropertyChanged(); } }
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
        public ThongTinHocSinhViewModel()
        {
            everLoaded = false;
            NienKhoaQueries = "";
            KhoiQueries = "";
            LopQueries = "";
            LoadThongTinCmb();
            LoadThongTinHocSinh();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (everLoaded == false )
                {
                    ThongTinHocSinhWD = parameter as ThongTinHocSinh;
                    ThongTinHocSinhWD.cmbNienKhoa.SelectedIndex = 0;
                    ThongTinHocSinhWD.cmbKhoi.SelectedIndex = 0;
                    ThongTinHocSinhWD.cmbLop.SelectedIndex = 0;
                    everLoaded = true;
                }
            });
            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                IsLoadAll = false;
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    string item = cmb.SelectedItem as string;
                    if (item != null)
                    {
                        NienKhoaQueries = item.ToString();
                        FilterLopFromSelection();
                        if (ThongTinHocSinhWD.cmbLop.SelectedIndex >= 0)
                            LoadThongTinHocSinh();
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
                        FilterLopFromSelection();
                    }
                }
            });
            FilterLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
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
                            LoadThongTinHocSinh();
                        else DanhSachHocSinh.Clear();
                    }
                }
            });
            StudentSearch = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachHocSinh.Clear();
                IsLoadAll = false;
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try 
                        { 
                            con.Open(); 
                        } catch (Exception) 
                        { 
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                            return; 
                        }
                        string CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh " +
                                            " where hs.MaLop = " + LopQueries + " and TenHocSinh like N'%" + parameter.Text + "%'";
                        if (IsLoadAll)
                        {
                            CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh " +
                                            " where TenHocSinh like N'%" + parameter.Text + "%'";
                        }
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        bool stillLoad = false;
                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (stillLoad == true)
                                {
                                    if (reader.GetInt32(5) == 1)
                                    {
                                        try
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = false;
                                        }
                                        catch (Exception)
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = "Chưa có thông tin";
                                        }
                                    }
                                    else if (reader.GetInt32(5) == 2)
                                    {
                                        try
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = false;
                                        }
                                        catch (Exception)
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = "Chưa có thông tin";
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
                                    if (reader.GetInt32(5) == 1)
                                    {
                                        try
                                        {
                                            student.DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = true;
                                        }
                                        catch (Exception)
                                        {
                                            student.DiemTB1 = "Chưa có thông tin";
                                        }
                                    }
                                    else if (reader.GetInt32(5) == 2)
                                    {
                                        try
                                        {
                                            student.DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = true;
                                        }
                                        catch (Exception)
                                        {
                                            student.DiemTB2 = "Chưa có thông tin";
                                        }
                                    }
                                    DanhSachHocSinh.Add(student);
                                    stillLoad = true;
                                }
                            }
                            reader.NextResult();
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            });
            StudentSearchAll = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachHocSinh.Clear();
                IsLoadAll = true;
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try
                        {
                            con.Open();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                            return;
                        }
                        string CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        bool stillLoad = false;
                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (stillLoad == true)
                                {
                                    if (reader.GetInt32(5) == 1)
                                    {
                                        try
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = false;
                                        }
                                        catch (Exception)
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = "Chưa có thông tin";
                                            stillLoad = false;
                                        }
                                    }
                                    else if (reader.GetInt32(5) == 2)
                                    {
                                        try
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = false;
                                        }
                                        catch (Exception)
                                        {
                                            DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 =  "Chưa có thông tin";
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
                                    if (reader.GetInt32(5) == 1)
                                    {
                                        try
                                        {
                                            student.DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = true;
                                        }
                                        catch (Exception)
                                        {
                                            student.DiemTB1 = "Chưa có thông tin";
                                            stillLoad = true;
                                        }
                                    }
                                    else if (reader.GetInt32(5) == 2)
                                    {
                                        try
                                        {
                                            student.DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                            stillLoad = true;
                                        }
                                        catch (Exception)
                                        {
                                            student.DiemTB2 = "Chưa có thông tin";
                                            stillLoad = true;
                                        }
                                    }
                                    DanhSachHocSinh.Add(student);
                                    stillLoad = true;
                                }
                            }
                            reader.NextResult();
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            });
            AddStudent = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemHocSinhMoi window = new ThemHocSinhMoi();
                ThemHocSinhMoiViewModel data = window.DataContext as ThemHocSinhMoiViewModel;
                window.ShowDialog();
                LoadThongTinHocSinh();
            });
            UpdateStudent = new RelayCommand<Model.HocSinh>((parameter) => { return true; }, (parameter) =>
            {
                SuaThongTinHocSinh window = new SuaThongTinHocSinh();
                SuaThongTinHocSinhViewModel data = window.DataContext as SuaThongTinHocSinhViewModel;
                data.HocSinhHienTai = parameter;
                window.ShowDialog();
                LoadThongTinHocSinh();
            });
            DeleteStudent = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Model.HocSinh item = parameter as Model.HocSinh;
                XoaHocSinh(item);
                LoadThongTinHocSinh();
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
        public void LoadThongTinHocSinh()
        {
            DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    { 
                        con.Open();
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }

                    string CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh where hs.MaLop = " + LopQueries;
                    if (IsLoadAll)
                    {
                        CmdString = "select hs.MaHocSinh, TenHocSinh, NgaySinh, GioiTinh, TrungBinhHocKy,HocKy,AnhThe from HocSinh hs join ThanhTich tt on hs.MaHocSinh = tt.MaHocSinh";

                    }
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    bool stillLoad = false;
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (stillLoad == true)
                            {
                                if (reader.GetInt32(5) == 1)
                                {
                                    try
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = false;
                                    }
                                    catch (Exception)
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB1 = "Chưa có thông tin";
                                        stillLoad = false;
                                    }
                                }
                                else if (reader.GetInt32(5) == 2)
                                {
                                    try
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = false;
                                    }
                                    catch (Exception)
                                    {
                                        DanhSachHocSinh[DanhSachHocSinh.Count - 1].DiemTB2 = "Chưa có thông tin";
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
                                if (reader.GetInt32(5) == 1)
                                {
                                    try
                                    {
                                        student.DiemTB1 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = true;
                                    }
                                    catch (Exception)
                                    {
                                        student.DiemTB1 = "Chưa có thông tin";
                                        stillLoad = true;
                                    }
                                }
                                else if (reader.GetInt32(5) == 2)
                                {
                                    try
                                    {
                                        student.DiemTB2 = ((decimal)reader.GetDecimal(4)).ToString();
                                        stillLoad = true;
                                    }
                                    catch (Exception)
                                    {
                                        student.DiemTB2 = "Chưa có thông tin";
                                        stillLoad = true;
                                    }
                                }
                                DanhSachHocSinh.Add(student);
                                stillLoad = true;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }
        public void LoadThongTinCmb()
        {
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<StudentManagement.Model.Khoi>();
            LopCmb = new ObservableCollection<StudentManagement.Model.Lop>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        con.Open();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }
                    //đọc niên khóa
                    string CmdString = "select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                    cmd = new SqlCommand(CmdString, con);
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
                    cmd = new SqlCommand(CmdString, con);
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
                    con.Close();
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void FilterLopFromSelection()
        {
            LopCmb.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    { 
                        con.Open(); 
                    } catch (Exception) 
                    { 
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return; 
                    }
                    string CmdString = "select Malop, TenLop from Lop where MaKhoi = " + KhoiQueries + " and NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                
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
        public void XoaHocSinh(Model.HocSinh item)
        {
            MessageBoxResult ConfirmDelete = System.Windows.MessageBox.Show("Bạn có chắc chắn xóa học sinh này không?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (ConfirmDelete == MessageBoxResult.Yes)
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try
                        { 
                            con.Open();
                        } catch (Exception)
                        { 
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                            return; 
                        }
                        SqlCommand cmd;
                        string CmdString = "Delete from HeThongDiem where MaHocSinh = " + item.MaHocSinh;
                        cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();

                        CmdString = "Delete from ThanhTich where MaHocSinh = " + item.MaHocSinh;
                        cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();

                        CmdString = "Delete from HocSinh where MaHocSinh = " + item.MaHocSinh;
                        cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Đã xóa thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
        }
    }
}
