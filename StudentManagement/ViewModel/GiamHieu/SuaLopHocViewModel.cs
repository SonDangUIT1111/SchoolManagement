using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
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
                if (String.IsNullOrEmpty(SuaLopWD.EditClassName.Text) || 
                    String.IsNullOrEmpty(SuaLopWD.EditAcademyYear.Text) 
                    || String.IsNullOrEmpty(GiaoVienQueries))
                {
                    MessageBox.Show("Nhập đầy đủ thông tin");
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
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                            return;
                        }
                        string cmdString = "UPDATE Lop Set TenLop = '" + SuaLopWD.EditClassName.Text + "', NienKhoa = '" + SuaLopWD.EditAcademyYear.Text + "', " +
                                    "MaGVCN = " + GiaoVienQueries + " where MaLop = " + LopHocHienTai.MaLop.ToString();
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Sửa lớp học thành công");
                        SuaLopWD.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                        return; 
                    }
                    string cmdString = "SELECT DISTINCT MaGiaoVien,TenGiaoVien FROM GiaoVien WHERE MaGiaoVien NOT IN (SELECT DISTINCT MaGVCN FROM Lop)";
                    MessageBox.Show(cmdString);
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
                            if (string.IsNullOrEmpty(GiaoVienQueries))
                            {
                                GiaoVienQueries = reader.GetString(0);
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
    }
}
