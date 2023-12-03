using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class SuaLopHocViewModel : BaseViewModel
    {
        public string GiaoVienQueries;
        public int MaGiaoVien { get; set; }
        public int MaKhoi { get; set; }
        public int Khoi { get; set; }


        public SuaThongTinLopHoc SuaLopWD { get; set; }
        private StudentManagement.Model.Lop _lopHocHienTai;
        public StudentManagement.Model.Lop LopHocHienTai
        {
            get => _lopHocHienTai;
            set { _lopHocHienTai = value;  }
        }

        private ObservableCollection<Model.GiaoVien> _giaoVienComboBox;
        public ObservableCollection<Model.GiaoVien> GiaoVienComboBox
        {
            get => _giaoVienComboBox;
            set { _giaoVienComboBox = value;  }
        }

        private ObservableCollection<string> _nienKhoaComboBox;
        public ObservableCollection<string> NienKhoaComboBox
        {
            get => _nienKhoaComboBox;
            set { _nienKhoaComboBox = value;  }
        }

       
        public ICommand LoadWindow { get; set; }
        public ICommand EditClass { get; set; }
        public ICommand CancelEditClass { get; set; }


        public void LoadGVCNCombobox()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string cmdString = "SELECT DISTINCT MaGiaoVien,TenGiaoVien FROM GiaoVien WHERE NOT EXISTS (Select DISTINCT MAGVCN from LOP where MaGiaoVien = MAGVCN)";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            GiaoVienComboBox.Add(new Model.GiaoVien()
                            {
                                MaGiaoVien = reader.GetInt32(0),
                                TenGiaoVien = reader.GetString(1)
                            });
                            //if (string.IsNullOrEmpty(GiaoVienQueries))
                            //{
                            //    GiaoVienQueries = reader.GetString(0);
                            //}
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


        public int TienHanhSuaLopHoc(string tenlop, string nienkhoa, string malop,string magv)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrap.Open();

                    string cmdText = "Select * from Lop where TenLop = '" + tenlop + "' and NienKhoa = '" + nienkhoa + "' and MaLop <>  " + malop;
                    SqlCommand cmdTest = new SqlCommand(cmdText, sqlConnectionWrap.GetSqlConnection());
                    int checkExists = Convert.ToInt32(cmdTest.ExecuteScalar());
                    sqlConnectionWrap.Close();
                    if (checkExists > 0)
                    {
                        return 0;
                    }

                    sqlConnectionWrap.Open();
                    string cmdString = "UPDATE Lop Set TenLop = '" + tenlop + "', NienKhoa = '" + nienkhoa + "', " +
                        "MaGVCN = " + magv + " where MaLop = " + malop;
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                    return cmd.ExecuteNonQuery();

            }
        }
        public SuaLopHocViewModel()
        {
            // Stryker disable all
            LopHocHienTai = new StudentManagement.Model.Lop() { };
            LoadWindow = new RelayCommand<SuaThongTinLopHoc>((parameter) => { return true; }, (parameter) =>
            {
                SuaLopWD = parameter;
                GiaoVienComboBox = new ObservableCollection<Model.GiaoVien>();
                LoadGVCNCombobox();
                NienKhoaComboBox = new ObservableCollection<string>();
                using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
                {
                    try
                    {
                        try
                        {
                            sqlConnectionWrap.Open();
                        }
                        catch (Exception)
                        {
                            //MessageBoxFail messageBoxFail = new MessageBoxFail();
                            //messageBoxFail.ShowDialog();
                            return;
                        }
                        string cmdString = "SELECT DISTINCT NienKhoa FROM Lop";
                        SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                NienKhoaComboBox.Add(reader.GetString(0));
                            }
                            reader.NextResult();
                        }
                        sqlConnectionWrap.Close();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return;
                    }
                }
            });

            EditClass = new RelayCommand<SuaThongTinLopHoc>((parameter) => { return true; }, (parameter) =>
            {
                if (SuaLopWD.EditFormTeacher != null && SuaLopWD.EditFormTeacher.SelectedItem != null)
                {

                    Model.GiaoVien item = SuaLopWD.EditFormTeacher.SelectedItem as Model.GiaoVien;
                    GiaoVienQueries = item.MaGiaoVien.ToString();
                }
                if (String.IsNullOrEmpty(SuaLopWD.EditClassName.Text) || String.IsNullOrEmpty(GiaoVienQueries))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng nhập đầy đủ thông tin";
                    MB.ShowDialog();
                    return;
                }
                else
                {
                    int result = TienHanhSuaLopHoc(SuaLopWD.EditClassName.Text, SuaLopWD.NienKhoaCmB.Text, LopHocHienTai.MaLop.ToString(), GiaoVienQueries);
                    if (result == 0)
                    {
                        MessageBoxOK messageBoxOK = new MessageBoxOK();
                        MessageBoxOKViewModel data = messageBoxOK.DataContext as MessageBoxOKViewModel;
                        data.Content = "Đã tồn tại tên lớp và niên khóa lớp này, vui lòng xem xét lại";
                        messageBoxOK.ShowDialog();
                    }
                    else if (result == 1)
                    {
                        MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        messageBoxSuccessful.ShowDialog();
                        SuaLopWD.Close();
                    }
                    else
                    {
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                    }
                }

            });

            CancelEditClass = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaLopWD.Close();
            });
        }
    }
}
