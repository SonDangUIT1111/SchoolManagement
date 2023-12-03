using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class XepLopViewModel : BaseViewModel
    {

        // khai báo biến
        private Lop _lopHocDangChon;
        public Lop LopHocDangChon { get { return _lopHocDangChon; } set { _lopHocDangChon = value;  } }
        public XepLopChoHocSinh XepLopWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HocSinh> _danhSachHocSinh;
        public ObservableCollection<StudentManagement.Model.HocSinh> DanhSachHocSinh { get => _danhSachHocSinh; set { _danhSachHocSinh = value;  } }
        private ObservableCollection<string> _namSinhCmb;
        public ObservableCollection<string> NamSinhCmb { get => _namSinhCmb; set { _namSinhCmb = value;  } }
        private bool[] _selectCheckBox;
        public bool[] SelectCheckBox { get => _selectCheckBox; set { _selectCheckBox = value;  } }

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


        // khai báo ICommand
        public ICommand FindTheoNamSinh { get; set; }
        public ICommand Filter { get; set; }
        public ICommand DanhDau { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand XepLop { get; set; }
        public ICommand LoadWindow { get; set; }
        public void LoadNamSinh()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open(); 
                    string CmdString = "select distinct Year(NgaySinh) from HocSinh where TenHocSinh is not null and (MaLop <> " + LopHocDangChon.MaLop.ToString()
                                   + " or MaLop is null)";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NamSinhCmb.Add(reader.GetInt32(0).ToString());
                        }
                        reader.NextResult();
                    }

                }
                catch (Exception)
                {
                    //MessageBoxFail msgBoxFail = new MessageBoxFail();
                    //msgBoxFail.ShowDialog();
                }
            }
        }
        public async Task LoadDanhSachHocSinh()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select * from HocSinh where TenHocSinh is not null and (MaLop <> " + LopHocDangChon.MaLop.ToString()
                                    + " or MaLop is null)";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                            {
                                MaHocSinh = reader.GetInt32(0),
                                TenHocSinh = reader.GetString(1),
                                NgaySinh = reader.GetDateTime(2),
                                GioiTinh = reader.GetBoolean(3),
                                DiaChi = reader.GetString(4),
                                Email = reader.GetString(5),
                                Avatar = (byte[])reader[6],
                            };
                            DanhSachHocSinh.Add(student);
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


        public async Task LoadDanhSachTheoNamSinh(string value,string searchWord)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select * from HocSinh where TenHocSinh is not null and Year(NgaySinh) = " + value + " and (MaLop <> " + LopHocDangChon.MaLop.ToString()
                                    + " or MaLop is null)" + " and TenHocSinh like '%" + searchWord + "%'";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                            {
                                MaHocSinh = reader.GetInt32(0),
                                TenHocSinh = reader.GetString(1),
                                NgaySinh = reader.GetDateTime(2),
                                GioiTinh = reader.GetBoolean(3),
                                DiaChi = reader.GetString(4),
                                Email = reader.GetString(5),
                                Avatar = (byte[])reader[6]
                            };
                            DanhSachHocSinh.Add(student);
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
        public async Task LocDanhSach(string value,ComboBox namsinh)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select * from HocSinh where TenHocSinh is not null and TenHocSinh like N'%" + value + "%' and (MaLop <>" + LopHocDangChon.MaLop.ToString()
                                    + " or MaLop is null)";

                    if (namsinh.SelectedItem != null)
                    {
                        CmdString = "select * from HocSinh where TenHocSinh is not null and TenHocSinh like N'%" + value + "%' and (MaLop <>" + LopHocDangChon.MaLop.ToString()
                                        + " or MaLop is null) and Year(NgaySinh) = " + namsinh.SelectedItem.ToString();
                    }
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.HocSinh student = new StudentManagement.Model.HocSinh
                            {
                                MaHocSinh = reader.GetInt32(0),
                                TenHocSinh = reader.GetString(1),
                                NgaySinh = reader.GetDateTime(2),
                                GioiTinh = reader.GetBoolean(3),
                                DiaChi = reader.GetString(4),
                                Email = reader.GetString(5),
                                Avatar = (byte[])reader[6]
                            };
                            DanhSachHocSinh.Add(student);
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

        public int ThemHocSinhVaoLop()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrap.Open();
                    int numberOfStudents = 0;
                    for (int i = 0; i < SelectCheckBox.Length; i++)
                    {
                        if (SelectCheckBox[i] == true)
                        {
                            numberOfStudents++;
                        }
                    }
                    List<int> quiDinh = new List<int>();
                    string cmdTest = "select GiaTri,SiSo from QuiDinh,Lop where MaQuiDinh = 1 and MaLop = " + LopHocDangChon.MaLop;
                    SqlCommand cmd1 = new SqlCommand(cmdTest, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader readerTest = cmd1.ExecuteReader();
                    readerTest.Read();
                    quiDinh.Add(readerTest.GetInt32(0));
                    quiDinh.Add(readerTest.GetInt32(1));
                    if (numberOfStudents + quiDinh[1] > quiDinh[0])
                    {
                        return -1;
                    }
                    readerTest.Close();

                    string CmdString;
                    SqlCommand cmd;
                int count = 0;
                    for (int i = 0; i < SelectCheckBox.Length; i++)
                    {
                        if (SelectCheckBox[i] == true)
                        {
                            CmdString = "Update HocSinh set MaLop = " + LopHocDangChon.MaLop +
                                        " where MaHocSinh = " + DanhSachHocSinh[i].MaHocSinh;
                            cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                            count += cmd.ExecuteNonQuery();

                            CmdString = "Update HeThongDiem set MaLop = " + LopHocDangChon.MaLop +
                                        " where MaHocSinh = " + DanhSachHocSinh[i].MaHocSinh;
                            cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                            count+=cmd.ExecuteNonQuery();

                            CmdString = "Update ThanhTich set MaLop = " + LopHocDangChon.MaLop +
                                        " where MaHocSinh = " + DanhSachHocSinh[i].MaHocSinh;
                            cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                            count+=cmd.ExecuteNonQuery();
                        }
                    }
                    return count;

            }
        }
        public void ClearSelectArray()
        {
            for (int i = 0; i < SelectCheckBox.Length; i++)
            {
                SelectCheckBox[i] = false;
            }
        }
        public XepLopViewModel()
        {
            // Stryker disable all
            LopHocDangChon = new Lop();
            NamSinhCmb = new ObservableCollection<string>();
            LoadNamSinh();
            // define command
            LoadWindow = new RelayCommand<XepLopChoHocSinh>((parameter) => { return true; }, async (parameter) =>
            {
                XepLopWD = parameter;
                ProgressBarVisibility = true;
                DanhSachHocSinh = new ObservableCollection<Model.HocSinh>();
                await LoadDanhSachHocSinh();
                ProgressBarVisibility = false;
                SelectCheckBox = new bool[DanhSachHocSinh.Count];
            });
            FindTheoNamSinh = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                string value = cmb.SelectedItem as string;
                ProgressBarVisibility = true;
                DanhSachHocSinh.Clear();
                await LoadDanhSachTheoNamSinh(value, XepLopWD.tbSearch.Text);
                ProgressBarVisibility = false;
                ClearSelectArray();
            });
            Filter = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                TextBox textBox = parameter as TextBox;
                string text = textBox.Text;
                ProgressBarVisibility = true;
                DanhSachHocSinh.Clear();
                await LocDanhSach(text, XepLopWD.cmbNamSinh);
                ClearSelectArray();
                ProgressBarVisibility = false;
            });
            DanhDau = new RelayCommand<DataGrid>((parameter) => { return true; }, (parameter) =>
            {
                int location = parameter.SelectedIndex;
                SelectCheckBox[location] = !SelectCheckBox[location];
            });
            CancelCommand = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                XepLopWD.Close();
            });
            XepLop = new RelayCommand<DataGrid>((parameter) => { return true; }, (parameter) =>
            {
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có chắc chắn muốn thêm những học sinh này vào lớp "
                                                            + LopHocDangChon.TenLop;
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;

                if (result.IsYes == true)
                {
                    int rel = ThemHocSinhVaoLop();
                    if (rel == -2)
                    {
                        MessageBoxOK messageBoxOK = new MessageBoxOK();
                        MessageBoxOKViewModel datamb = messageBoxOK.DataContext as MessageBoxOKViewModel;
                        datamb.Content = "Đã có lỗi xảy ra";
                        messageBoxOK.ShowDialog();
                    }
                    else if (rel == -1)
                    {
                        MessageBoxOK messageBoxOK = new MessageBoxOK();
                        MessageBoxOKViewModel datamb = messageBoxOK.DataContext as MessageBoxOKViewModel;
                        datamb.Content = "Bị vượt quá sĩ số tối đa của 1 lớp. Vui lòng kiểm tra lại qui định với giám hiệu";
                        messageBoxOK.ShowDialog();
                    }
                    else
                    {
                        MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        messageBoxSuccessful.ShowDialog();
                        XepLopWD.Close();
                    }
                }

            });
        }
    }
}
