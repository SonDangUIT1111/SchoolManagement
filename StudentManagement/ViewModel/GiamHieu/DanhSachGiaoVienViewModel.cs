using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class DanhSachGiaoVienViewModel : BaseViewModel
    {
        private ObservableCollection<StudentManagement.Model.GiaoVien> _danhSachGiaoVien;
        public ObservableCollection<StudentManagement.Model.GiaoVien> DanhSachGiaoVien { get => _danhSachGiaoVien; set { _danhSachGiaoVien = value; OnPropertyChanged(); } }

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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }


        //declare ICommand
        public ICommand LoadGiaoVien { get; set; }
        public ICommand LocGiaoVien { get; set; }
        public ICommand ThemGiaoVien { get; set; }
        public ICommand UpdateGiaoVien { get; set; }
        public ICommand RemoveGiaoVien { get; set; }

        public DanhSachGiaoVienViewModel()
        {
            LoadGiaoVien = new RelayCommand<TextBox>((parameter) => { return true; }, async (parameter) =>
            {
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadDanhSachGiaoVien();
                ProgressBarVisibility = false;
                DataGridVisibility = true;

            });
            LocGiaoVien = new RelayCommand<TextBox>((parameter) => { return true; }, async (parameter) =>
            {
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                TextBox tb = parameter;
                await LocGiaoVienTheoTen(tb.Text);
                ProgressBarVisibility = false;
                DataGridVisibility = true;

            });
            ThemGiaoVien = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ThemGiaoVien window = new ThemGiaoVien();
                ThemGiaoVienViewModel data = window.DataContext as ThemGiaoVienViewModel;
                window.ShowDialog();
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadDanhSachGiaoVien();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
            });
            UpdateGiaoVien = new RelayCommand<Model.GiaoVien>((parameter) => { return true; }, async (parameter) =>
            {
                SuaGiaoVien window = new SuaGiaoVien();
                SuaGiaoVienViewModel data = window.DataContext as SuaGiaoVienViewModel;
                data.GiaoVienHienTai = parameter;
                window.ShowDialog();
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadDanhSachGiaoVien();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
            });
            RemoveGiaoVien = new RelayCommand<Model.GiaoVien>((parameter) => { return true; }, async (parameter) =>
            {
                Model.GiaoVien item = parameter;
                XoaGiaoVien(item);
                ProgressBarVisibility = true;
                DataGridVisibility = false;
                await LoadDanhSachGiaoVien();
                ProgressBarVisibility = false;
                DataGridVisibility = true;
            });
        }
        public async Task LoadDanhSachGiaoVien()
        {
            DanhSachGiaoVien = new ObservableCollection<Model.GiaoVien>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        await con.OpenAsync();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }

                    string CmdString = "select MaGiaoVien,TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from GiaoVien";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.GiaoVien teacher = new StudentManagement.Model.GiaoVien();
                            teacher.MaGiaoVien = reader.GetInt32(0);
                            teacher.TenGiaoVien = reader.GetString(1);
                            teacher.NgaySinh = reader.GetDateTime(2);
                            teacher.GioiTinh = reader.GetBoolean(3);
                            teacher.DiaChi = reader.GetString(4);
                            teacher.Email = reader.GetString(5);
                            teacher.Avatar = (byte[])reader[6];
                            DanhSachGiaoVien.Add(teacher);
                        }
                        await reader.NextResultAsync();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public async Task LocGiaoVienTheoTen(string value)
        {
            DanhSachGiaoVien.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        await con.OpenAsync();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        return;
                    }

                    string CmdString = "select MaGiaoVien,TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from GiaoVien where TenGiaoVien is not null and TenGiaoVien like N'%" + value + "%'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.GiaoVien teacher = new StudentManagement.Model.GiaoVien
                            {
                                MaGiaoVien = reader.GetInt32(0),
                                TenGiaoVien = reader.GetString(1),
                                NgaySinh = reader.GetDateTime(2),
                                GioiTinh = reader.GetBoolean(3),
                                DiaChi = reader.GetString(4),
                                Email = reader.GetString(5),
                                Avatar = (byte[])reader[6],
                            };
                            DanhSachGiaoVien.Add(teacher);
                        }
                        await reader.NextResultAsync();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void XoaGiaoVien(Model.GiaoVien value)
        {
            MessageBoxResult ConfirmDelete = System.Windows.MessageBox.Show("Bạn có chắc chắn xóa giáo viên?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
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
                        string CmdString = "Delete From GiaoVien where MaGiaoVien = " + value.MaGiaoVien;
                        cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteScalar();
                        MessageBox.Show("Đã xóa " + value.TenGiaoVien);
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
        }
    }
}
