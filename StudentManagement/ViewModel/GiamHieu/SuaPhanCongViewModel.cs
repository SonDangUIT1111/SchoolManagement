using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class SuaPhanCongViewModel : BaseViewModel
    {
        public SuaPhanCong SuaPhanCongWD { get; set; }
        public int idxMon = -1;
        public int idxGV = -1;
        private StudentManagement.Model.PhanCongGiangDay _phanCongHienTai;
        public StudentManagement.Model.PhanCongGiangDay PhanCongHienTai { get => _phanCongHienTai; set { _phanCongHienTai = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.MonHoc> _monHocCmb;
        public ObservableCollection<StudentManagement.Model.MonHoc> MonHocCmb { get => _monHocCmb; set { _monHocCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.GiaoVien> _giaoVienCmb;
        public ObservableCollection<StudentManagement.Model.GiaoVien> GiaoVienCmb { get => _giaoVienCmb; set { _giaoVienCmb = value; OnPropertyChanged(); } }

        public ICommand LoadData { get; set; }
        public ICommand SuaPhanCong { get; set; }
        public ICommand HuySuaPC { get; set; }

        public SuaPhanCongViewModel()
        {

            //PhanCongHienTai = new StudentManagement.Model.PhanCongGiangDay();
            LoadData = new RelayCommand<SuaPhanCong>((parameter) => { return true; }, (parameter) =>
            {
                SuaPhanCongWD = parameter;
                LoadThongTinCmb();
            });

            SuaPhanCong = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Model.MonHoc monhoc = SuaPhanCongWD.cmbMonHoc.SelectedItem as Model.MonHoc;
                Model.GiaoVien giaovien = SuaPhanCongWD.cmbGiaoVien.SelectedItem as Model.GiaoVien;
                if (monhoc == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học!");
                }
                else if (giaovien == null)
                {
                    MessageBox.Show("Vui lòng chọn giáo viên giảng dạy!");
                }
                else
                {
                    MessageBoxResult ConfirmChange = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn thay đổi không?", "Change Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (ConfirmChange == MessageBoxResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                            string CmdString = "update PhanCongGiangDay set MaMon=" + monhoc.MaMon + ", TenMon=N'" + monhoc.TenMon + "', MaGiaoVienPhuTrach=" + giaovien.MaGiaoVien + ", TenGiaoVien=N'" + giaovien.TenGiaoVien + "' where MaPhanCong = " + PhanCongHienTai.MaPhanCong + "";
                            //MessageBox.Show(CmdString);
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        MessageBox.Show("Sửa phân công giảng dạy thành công!");
                        SuaPhanCongWD.Close();
                    }

                }
            });
            HuySuaPC = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                SuaPhanCongWD.Close();
            });
        }
        public void LoadThongTinCmb()
        {
            MonHocCmb = new ObservableCollection<StudentManagement.Model.MonHoc>();
            GiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
                string CmdString = "select * from MonHoc where TenMon=N'" + PhanCongHienTai.TenMon + "' or TenMon not in (select TenMon from PhanCongGiangDay where TenLop=N'" + PhanCongHienTai.TenLop.ToString() + "' and NienKhoa=N'" + PhanCongHienTai.NienKhoa.ToString() + "')";
                //MessageBox.Show(CmdString);
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.MonHoc item = new Model.MonHoc();
                        item.MaMon = reader.GetInt32(0);
                        item.TenMon = reader.GetString(1);
                        MonHocCmb.Add(item);


                    }
                    reader.NextResult();
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
                string CmdString = "select MaGiaoVien, TenGiaoVien from GiaoVien";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.GiaoVien item = new Model.GiaoVien();
                        item.MaGiaoVien = reader.GetInt32(0);
                        item.TenGiaoVien = reader.GetString(1);
                        GiaoVienCmb.Add(item);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
            SuaPhanCongWD.cmbMonHoc.SelectedIndex = MonHocCmb.ToList().FindIndex(x => x.TenMon == PhanCongHienTai.TenMon); ;
            SuaPhanCongWD.cmbGiaoVien.SelectedIndex = GiaoVienCmb.ToList().FindIndex(x => x.TenGiaoVien == PhanCongHienTai.TenGiaoVien); ;

        }
    }
}
