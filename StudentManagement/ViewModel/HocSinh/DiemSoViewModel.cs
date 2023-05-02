using StudentManagement.Model;
using StudentManagement.Views.HocSinh;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModel.HocSinh
{
    internal class DiemSoViewModel : BaseViewModel
    {
        public DiemSo DiemSoWD { get; set; }   
        private ObservableCollection<String> _danhsachnienkhoa;
        public ObservableCollection<String> DanhSachNienKhoa { get => _danhsachnienkhoa; set { _danhsachnienkhoa = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhsachdiemhk1;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiemHK1 { get => _danhsachdiemhk1; set { _danhsachdiemhk1 = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhsachdiemhk2;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiemHK2 { get => _danhsachdiemhk2; set { _danhsachdiemhk2 = value; OnPropertyChanged(); } }
        private decimal TbHk1;
        private decimal TbHk2;
        private int i;
        public ICommand LoadWindow { get; set; }
        public ICommand LoadDiem { get; set; }
        public DiemSoViewModel()
        {
            LoadWindow = new RelayCommand<DiemSo>((parameter) => { return true; }, (parameter) =>
            {
                DiemSoWD = parameter;
                DiemSoWD.BangDiem.Visibility = System.Windows.Visibility.Hidden;
            });
            LoadDiem = new RelayCommand<String>((parameter) => { return true; }, (parameter) =>
            {
                LoadDanhSachDiem();
            });
            LoadDanhSachNienKhoa();
            
        }
        public void LoadDanhSachNienKhoa()
        {
            DanhSachNienKhoa = new ObservableCollection<String>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "Select distinct NienKhoa from HeThongDiem";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string nienkhoa = reader.GetString(0);
                        DanhSachNienKhoa.Add(nienkhoa);
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
        public void LoadDanhSachDiem()
        {
            DiemSoWD.BangDiem.Visibility = System.Windows.Visibility.Visible;
            DanhSachDiemHK1 = new ObservableCollection<Model.HeThongDiem>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                //string CmdString = "select MaHocSinh,TenHocSinh from HocSinh where MaLop = '100'";
                string CmdString = "select TenMon,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai from HeThongDiem where HocKy = 1 and MaHocSinh = 100001";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                i = 0;
                TbHk1 = 0;
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem();
                        diem.TenMon= reader.GetString(0);
                        diem.Diem15Phut = reader.GetDecimal(1);
                        diem.Diem1Tiet= reader.GetDecimal(2);
                        diem.DiemTB = reader.GetDecimal(3);
                        diem.XepLoai = reader.GetBoolean(4);
                        DanhSachDiemHK1.Add(diem);
                        i++;
                        TbHk1 += diem.DiemTB;
                    }
                    reader.NextResult();
                }
                con.Close();
                TbHk1 /= i;
                DiemSoWD.DiemTbHK1.Text = TbHk1.ToString();
            }
            DanhSachDiemHK2 = new ObservableCollection<Model.HeThongDiem>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                //string CmdString = "select MaHocSinh,TenHocSinh from HocSinh where MaLop = '100'";
                string CmdString = "select TenMon,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai from HeThongDiem where HocKy = 2 and MaHocSinh = 100001";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                i = 0;
                TbHk2 = 0;
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem();
                        diem.TenMon = reader.GetString(0);
                        diem.Diem15Phut = reader.GetDecimal(1);
                        diem.Diem1Tiet = reader.GetDecimal(2);
                        diem.DiemTB = reader.GetDecimal(3);
                        diem.XepLoai = reader.GetBoolean(4);
                        DanhSachDiemHK2.Add(diem);
                        i++;
                        TbHk2 += diem.DiemTB;
                    }
                    reader.NextResult();
                }
                con.Close();
                TbHk2 /= i;
                DiemSoWD.DiemTbHK2.Text = TbHk2.ToString();
            }
        }
    }

}
