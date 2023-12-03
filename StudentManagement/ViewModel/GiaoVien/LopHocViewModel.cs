using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class LopHocViewModel : BaseViewModel
    {
        public bool everLoaded { get; set; }
        private int _idGiaoVien;
        public int IdGiaoVien { get { return _idGiaoVien; } set { _idGiaoVien = value; } }
        public string NienKhoaQueries;
        public string KhoiQueries;
        public string LopQueries;
        public StudentManagement.Model.Lop LopDaChon { get; set; }
        public LopHoc LopHocWD { get; set; }

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

        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachhs;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachhs { get => _danhSachhs; set { _danhSachhs = value;  } }

        private ObservableCollection<Model.Khoi> _danhsachkhoi;
        public ObservableCollection<Model.Khoi> DanhSachKhoi { get => _danhsachkhoi; set { _danhsachkhoi = value;  } }

        private ObservableCollection<string> _danhsachnienkhoa;
        public ObservableCollection<string> DanhSachNienKhoa { get => _danhsachnienkhoa; set { _danhsachnienkhoa = value;  } }
        private ObservableCollection<StudentManagement.Model.Lop> _danhsachlop;
        public ObservableCollection<StudentManagement.Model.Lop> DanhSachLop { get => _danhsachlop; set { _danhsachlop = value;  } }
        public ICommand LoadWindow { get; set; }
        public ICommand LocHocSinh { get; set; }
        public ICommand LoadLop { get; set; }
        public ICommand LoadKhoi { get; set; }

        public ICommand LoadHocSinh { get; set; }
        public ICommand UpdateHocSinh { get; set; }

        public bool IsGiaoVienChuNhiem(string ID)
        {
            int IdGVCN;
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrap.Open();
                    string CmdString = "select MaGVCN from HocSinh,Lop where HocSinh.MaLop = Lop.MaLop and MaHocSinh = " + ID;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    IdGVCN = reader.GetInt32(0);
                    if (IdGVCN != IdGiaoVien) return false;
                    return true;

            }
        }
        public async Task LoadDanhSachHocSinh()
        {
            
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select MaHocSinh,TenHocSinh,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from HocSinh where MaLop = \'" + LopQueries + "\'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                            student.MaHocSinh = reader.GetInt32(0);
                            student.TenHocSinh = reader.GetString(1);
                            student.NgaySinh = reader.GetDateTime(2);
                            student.GioiTinh = reader.GetBoolean(3);
                            student.DiaChi = reader.GetString(4);
                            student.Email = reader.GetString(5);
                            student.Avatar = (byte[])reader.GetValue(6);
                            DanhSachhs.Add(student);
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

        public void InitComboBox()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "Select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string nienkhoa = reader.GetString(0);
                            DanhSachNienKhoa.Add(nienkhoa);
                            if (String.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                                //try
                                //{
                                //    LopHocWD.ChonKhoa.SelectedIndex = 0;
                                //}catch (Exception) { }
                            }
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrap.Close();
                    reader.Close();

                    sqlConnectionWrap.Open();
                    CmdString = "select MaKhoi,Khoi from Khoi";
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DanhSachKhoi.Add(new Khoi()
                            {
                                MaKhoi = reader.GetInt32(0),
                                TenKhoi = reader.GetString(1),
                            });
                            if (String.IsNullOrEmpty(KhoiQueries))
                            {
                                KhoiQueries = reader.GetInt32(0).ToString();
                                //try
                                //{
                                //    LopHocWD.ChonKhoi.SelectedIndex = 0;
                                //} catch (Exception) { }
                            }
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrap.Close();
                    reader.Close();

                    sqlConnectionWrap.Open();
                    if (String.IsNullOrEmpty(KhoiQueries))
                        KhoiQueries = "0";
                    CmdString = "select MaLop,TenLop from Lop where MaKhoi = " + KhoiQueries +
                        " and NienKhoa = '" + NienKhoaQueries + "'";
                    cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop lop = new StudentManagement.Model.Lop();
                            lop.MaLop = reader.GetInt32(0);
                            lop.TenLop = reader.GetString(1);
                            DanhSachLop.Add(lop);
                            if (String.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
                                //try
                                //{
                                //    LopHocWD.ChonLop.SelectedIndex = 0;
                                //} catch (Exception) { }
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
        public void LoadDanhSachLop()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    if (String.IsNullOrEmpty(KhoiQueries))
                        KhoiQueries = "0";
                    string CmdString = "select MaLop,TenLop from Lop where MaKhoi = " + KhoiQueries +
                        " and NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop lop = new StudentManagement.Model.Lop();
                            lop.MaLop = reader.GetInt32(0);
                            lop.TenLop = reader.GetString(1);
                            DanhSachLop.Add(lop);
                            if (String.IsNullOrEmpty(LopQueries))
                                LopQueries = reader.GetInt32(0).ToString();
                        }
                        reader.NextResult();
                    }
                    //if (LopHocWD.ChonLop != null && LopHocWD.ChonLop.Items.Count > 0 && LopHocWD.ChonLop.SelectedIndex != 0)
                    //{
                    //    LopHocWD.ChonLop.SelectedIndex = 0;
                    //}
                }
                catch (Exception )
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public void LocHocSinhTheoTen(TextBox tb)
        {
            string value = tb.Text;
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select MaHocSinh,TenHocSinh,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from HocSinh where MaLop = " + LopQueries + " and TenHocSinh like N'%"+value+"%'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh();
                            student.MaHocSinh = reader.GetInt32(0);
                            student.TenHocSinh = reader.GetString(1);
                            student.NgaySinh = reader.GetDateTime(2);
                            student.GioiTinh = reader.GetBoolean(3);
                            student.DiaChi = reader.GetString(4);
                            student.Email = reader.GetString(5);
                            student.Avatar = (byte[])reader.GetValue(6);
                            DanhSachhs.Add(student);
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


        public LopHocViewModel()
        {
            // Stryker disable all
            everLoaded = false;
            DanhSachKhoi = new ObservableCollection<Model.Khoi>();
            DanhSachhs = new ObservableCollection<Model.HocSinh>();
            DanhSachNienKhoa = new ObservableCollection<string>();
            DanhSachLop = new ObservableCollection<Model.Lop>();
            LoadWindow = new RelayCommand<LopHoc>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    LopHocWD = parameter;
                    DanhSachNienKhoa.Clear();
                    DanhSachKhoi.Clear();
                    InitComboBox();
                    DataGridVisibility = false;
                    ProgressBarVisibility = true;
                    DanhSachhs.Clear();
                    await LoadDanhSachHocSinh();
                    DataGridVisibility = true;
                    ProgressBarVisibility = false;
                    everLoaded = true;
                }
            });
            LocHocSinh = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                TextBox tb = parameter;
                DanhSachhs.Clear();
                LocHocSinhTheoTen(tb);
            });
            LoadKhoi = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                if (LopHocWD.ChonKhoa != null && LopHocWD.ChonKhoa.SelectedItem != null)
                {
                    NienKhoaQueries = LopHocWD.ChonKhoa.SelectedItem.ToString();
                    LopQueries = null;
                    DanhSachhs.Clear();
                    DanhSachLop.Clear();
                    LoadDanhSachLop();
                }
            });
            LoadLop = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                if (LopHocWD.ChonKhoi != null && LopHocWD.ChonKhoi.SelectedItem != null)
                {
                    Khoi item = LopHocWD.ChonKhoi.SelectedItem as Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    LopQueries = null;
                    DanhSachhs.Clear();
                    DanhSachLop.Clear();
                    LoadDanhSachLop();
                }
            });
            LoadHocSinh = new RelayCommand<String>((parameter) => { return true; }, async (parameter) =>
            {
                if (LopHocWD.ChonLop != null && LopHocWD.ChonLop.SelectedItem != null)
                {
                    Lop item = LopHocWD.ChonLop.SelectedItem as Lop;
                    LopQueries = item.MaLop.ToString();
                }
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachhs.Clear();
                await LoadDanhSachHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
            UpdateHocSinh = new RelayCommand<Model.HocSinh>((parameter) => { return true; }, async (parameter) =>
            {
                if (!IsGiaoVienChuNhiem(parameter.MaHocSinh.ToString()))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var datamb = MB.DataContext as MessageBoxOKViewModel;
                    datamb.Content = "Bạn không thể chỉnh sửa đối tượng này";
                    MB.ShowDialog();
                    return;
                }
                StudentManagement.Views.GiamHieu.SuaThongTinHocSinh window = new StudentManagement.Views.GiamHieu.SuaThongTinHocSinh();
                StudentManagement.ViewModel.GiamHieu.SuaThongTinHocSinhViewModel data = window.DataContext as StudentManagement.ViewModel.GiamHieu.SuaThongTinHocSinhViewModel;
                data.HocSinhHienTai = parameter;
                window.ShowDialog();
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachhs.Clear();
                await LoadDanhSachHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
        }
    }
}
