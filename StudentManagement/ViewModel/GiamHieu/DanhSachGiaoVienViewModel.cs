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
        private readonly ISqlConnectionWrapper sqlConnection;

        public DanhSachGiaoVienViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
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
                DanhSachGiaoVien.Clear();
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
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có chắc chắn muốn xóa giáo viên này?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true) {
                    Model.GiaoVien item = parameter;
                    XoaGiaoVien(item);
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    await LoadDanhSachGiaoVien();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                }

            });
        }
        public async Task LoadDanhSachGiaoVien()
        {
            DanhSachGiaoVien = new ObservableCollection<Model.GiaoVien>();
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        sqlConnectionWrapper.Open();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog(); 
                        return;
                    }

                    string CmdString = "select MaGiaoVien,TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from GiaoVien";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                    sqlConnectionWrapper.Close();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }

        public async Task LocGiaoVienTheoTen(string value)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        sqlConnectionWrapper.Open();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return;
                    }

                    string CmdString = "select MaGiaoVien,TenGiaoVien,NgaySinh,GioiTinh,DiaChi,Email,AnhThe from GiaoVien where TenGiaoVien is not null and TenGiaoVien like N'%" + value + "%'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                    sqlConnectionWrapper.Close();
                }
                catch (Exception)
                {
                    //MessageBoxFail commandBoxFail = new MessageBoxFail();
                    //commandBoxFail.ShowDialog();
                }
            }
        }

        public void XoaGiaoVien(Model.GiaoVien value)
        {
                using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
                {
                    try
                    {
                        try 
                        {
                            sqlConnectionWrapper.Open();
                        }
                        catch (Exception) 
                        { 
                            //MessageBoxFail messageBoxFail = new MessageBoxFail();
                            //messageBoxFail.ShowDialog();
                            return; 
                        }
                        SqlCommand cmd;
                        string CmdString = "Delete From GiaoVien where MaGiaoVien = " + value.MaGiaoVien;
                        cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                        cmd.ExecuteScalar();
                        //MessageBoxOK MB = new MessageBoxOK();
                        //var datamb = MB.DataContext as MessageBoxOKViewModel;
                        //datamb.Content = "Đã xóa " + value.TenGiaoVien;
                        //MB.ShowDialog();
                        sqlConnectionWrapper.Close();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                    }
                }
        }
    }
}
