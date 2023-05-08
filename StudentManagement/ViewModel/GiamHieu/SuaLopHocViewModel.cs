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

        private ObservableCollection<string> _giaoVienComboBox;
        public ObservableCollection<string> GiaoVienComboBox
        {
            get => _giaoVienComboBox;
            set { _giaoVienComboBox = value; OnPropertyChanged(); }
        }

        private string selectedGiaoVien;

        public string SelectedGiaoVien
        {
            get { return selectedGiaoVien; }
            set
            {
                if (selectedGiaoVien != value)
                {
                    selectedGiaoVien = value;
                    LayMaGiaoVien();
                }
            }
        }

        public void LayMaGiaoVien()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    return;
                }
                string cmdString = "SELECT MaGiaoVien FROM GiaoVien WHERE TenGiaoVien = '" + selectedGiaoVien + "'";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MaGiaoVien = reader.GetInt32(0);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
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
                GiaoVienComboBox.Add(LopHocHienTai.TenGVCN);
                SuaLopWD.EditFormTeacher.SelectedItem = LopHocHienTai.TenGVCN;
            });

            EditClass = new RelayCommand<SuaThongTinLopHoc>((parameter) => { return true; }, (parameter) =>
            {
                if (String.IsNullOrEmpty(SuaLopWD.EditClassName.Text) || String.IsNullOrEmpty(SuaLopWD.EditNumberOfStudent.Text) || String.IsNullOrEmpty(SuaLopWD.EditAcademyYear.Text) || String.IsNullOrEmpty(selectedGiaoVien))
                {
                    MessageBox.Show("Nhập đầy đủ thông tin");
                }
                else using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                    {
                        try
                        {
                            try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                        }


                        string cmdString = "UPDATE Lop Set TenLop = '" + SuaLopWD.EditClassName.Text + "' , SiSo = '" + SuaLopWD.EditNumberOfStudent.Text + "', NienKhoa = '" + SuaLopWD.EditAcademyYear.Text + "',";

                        switch (SuaLopWD.EditClassName.Text.Substring(0, 2))
                        {
                            case "10":
                                MaKhoi = 1;
                                Khoi = 10;
                                break;
                            case "11":
                                MaKhoi = 2;
                                Khoi = 11;
                                break;
                            case "12":
                                MaKhoi = 3;
                                Khoi = 12;
                                break;
                        }

                        cmdString = cmdString + " MaKhoi = '" + MaKhoi + "', Khoi = '" + Khoi + "', MaGVCN ='" + MaGiaoVien + "', TenGVCN = '" + selectedGiaoVien + "' where TenLop = '" + LopHocHienTai.TenLop + "'";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Sửa lớp học thành công");
                        SuaLopWD.Close();
                    }

            });

            CancelEditClass = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaLopWD.Close();
            });
        }

        public void LoadGVCNCombobox()
        {
            GiaoVienComboBox = new ObservableCollection<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    return;
                }
                string cmdString = "SELECT DISTINCT TenGiaoVien FROM GiaoVien WHERE TenGiaoVien NOT IN (SELECT DISTINCT TenGVCN FROM Lop)";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GiaoVienComboBox.Add(reader.GetString(0));
                        if (string.IsNullOrEmpty(GiaoVienQueries))
                        {
                            GiaoVienQueries = reader.GetString(0);
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }

        }
    }
}
