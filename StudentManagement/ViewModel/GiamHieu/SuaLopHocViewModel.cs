using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
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
            set { _lopHocHienTai = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Model.GiaoVien> _giaoVienComboBox;
        public ObservableCollection<Model.GiaoVien> GiaoVienComboBox
        {
            get => _giaoVienComboBox;
            set { _giaoVienComboBox = value; OnPropertyChanged(); }
        }


       
        public ICommand LoadWindow { get; set; }
        public ICommand EditClass { get; set; }
        public ICommand CancelEditClass { get; set; }

        public SuaLopHocViewModel()
        {
            LopHocHienTai = new StudentManagement.Model.Lop() { };
            LoadWindow = new RelayCommand<SuaThongTinLopHoc>((parameter) => { return true; }, (parameter) =>
            {
                SuaLopWD = parameter;
                LoadGVCNCombobox();
            });

            EditClass = new RelayCommand<SuaThongTinLopHoc>((parameter) => { return true; }, (parameter) =>
            {
                if (SuaLopWD.EditFormTeacher != null && SuaLopWD.EditFormTeacher.SelectedItem != null)
                {

                    Model.GiaoVien item = SuaLopWD.EditFormTeacher.SelectedItem as Model.GiaoVien;
                    GiaoVienQueries = item.MaGiaoVien.ToString();
                }
                if (String.IsNullOrEmpty(SuaLopWD.EditClassName.Text) || 
                    String.IsNullOrEmpty(SuaLopWD.EditAcademyYear.Text) 
                    || String.IsNullOrEmpty(GiaoVienQueries))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng nhập đầy đủ thông tin";
                    MB.ShowDialog();
                }
                else using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    try
                    {
                        try
                        {
                            con.Open();
                        }
                        catch (Exception)
                        {
                                MessageBoxFail messageBoxFail = new MessageBoxFail();
                                messageBoxFail.ShowDialog();
                                return;
                        }
                        string cmdString = "UPDATE Lop Set TenLop = '" + SuaLopWD.EditClassName.Text + "', NienKhoa = '" + SuaLopWD.EditAcademyYear.Text + "', " +
                                    "MaGVCN = " + GiaoVienQueries + " where MaLop = " + LopHocHienTai.MaLop.ToString();
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        messageBoxSuccessful.ShowDialog();
                        SuaLopWD.Close();
                    }
                    catch (Exception )
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

        public void LoadGVCNCombobox()
        {
            GiaoVienComboBox = new ObservableCollection<Model.GiaoVien>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    { 
                        con.Open(); 
                    } catch (Exception) 
                    {
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return; 
                    }
                    string cmdString = "SELECT DISTINCT MaGiaoVien,TenGiaoVien FROM GiaoVien WHERE NOT EXISTS (Select DISTINCT MAGVCN from LOP where MaGiaoVien = MAGVCN)";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
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
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                    return;
                }
            }

        }
    }
}
