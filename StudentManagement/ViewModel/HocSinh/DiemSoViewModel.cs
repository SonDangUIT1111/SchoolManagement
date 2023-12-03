using StudentManagement.Model;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.HocSinh;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace StudentManagement.ViewModel.HocSinh
{
    public class DiemSoViewModel : BaseViewModel
    {
        const string d = "Đạt";
        const string kd = "Không đạt";
        const string ccdl = "Chưa có dữ liệu";
        public bool everLoaded { get; set; }
        private int _idHocSinh;
        public int IdHocSinh { get { return _idHocSinh; } set { _idHocSinh = value; } }
        private string _xepLoai1;
        public string XepLoai1 { get { return _xepLoai1; } set { _xepLoai1 = value; } }
        private string _nhanXet1;
        public string NhanXet1 { get { return _nhanXet1; } set { _nhanXet1 = value; } }
        private string _tbhk1;
        public string TBHK1 { get { return _tbhk1; } set { _tbhk1 = value; } }
        private string _xepLoai2;
        public string XepLoai2 { get { return _xepLoai2; } set { _xepLoai2 = value; } }
        private string _nhanXet2;
        public string NhanXet2 { get { return _nhanXet2; } set { _nhanXet2 = value; } }
        private string _tbhk2;
        public string TBHK2 { get { return _tbhk2; } set { _tbhk2 = value; } }
        public DiemSo DiemSoWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhsachdiemhk1;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiemHK1 { get => _danhsachdiemhk1; set { _danhsachdiemhk1 = value;  } }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhsachdiemhk2;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiemHK2 { get => _danhsachdiemhk2; set { _danhsachdiemhk2 = value;  } }
        public ICommand LoadWindow { get; set; }

        public int LoadDanhSachDiem()
        {
            //load ten
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    string TenHs;
                    sqlConnectionWrapper.Open();
                    string CmdString = "select TenHocSinh,TenLop from HocSinh hs join Lop l on hs.MaLop = l.MaLop where MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    TenHs = reader.GetString(0);
                    DiemSoWD.Ten.Text = TenHs;
                    DiemSoWD.Ten2.Text = TenHs;

            }
            // load diem
            DanhSachDiemHK1 = new ObservableCollection<Model.HeThongDiem>();
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select TenMon,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai from HeThongDiem ht join MonHoc mh on ht.MaMon = mh.MaMon where HocKy = 1 and MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem();
                                diem.TenMon = reader.GetString(0);
                                diem.Diem15Phut = reader.GetDecimal(1);
                                diem.Diem1Tiet = reader.GetDecimal(2);
                                diem.DiemTB = reader.GetDecimal(3);
                                diem.XepLoai = reader.GetBoolean(4);
                                DanhSachDiemHK1.Add(diem);
                            }
                            catch (Exception)
                            {

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
            DanhSachDiemHK2 = new ObservableCollection<Model.HeThongDiem>();
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string CmdString = "select TenMon,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai from HeThongDiem ht join MonHoc mh on ht.MaMon = mh.MaMon where HocKy = 2 and MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem();
                                diem.TenMon = reader.GetString(0);
                                diem.Diem15Phut = reader.GetDecimal(1);
                                diem.Diem1Tiet = reader.GetDecimal(2);
                                diem.DiemTB = reader.GetDecimal(3);
                                diem.XepLoai = reader.GetBoolean(4);
                                DanhSachDiemHK2.Add(diem);
                            }
                            catch (Exception) { }
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
            //load xep loai
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {

                    sqlConnectionWrapper.Open();
                    string CmdString = "select XepLoai,NhanXet,TrungBinhHocky from ThanhTich where MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    int stt = 0;
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (stt == 0)
                            {
                                //if (reader.GetBoolean(0)) DiemSoWD.XepLoai.Text = "Đạt"; else DiemSoWD.XepLoai.Text = "Không đạt";

                                try
                                {
                                    bool XepLoai = reader.GetBoolean(0);
                                if (XepLoai)
                                {
                                    DiemSoWD.XepLoai.Text = d;
                                    XepLoai1 = d;
                                }
                                else
                                {
                                    DiemSoWD.XepLoai.Text = kd;
                                    XepLoai1 = kd;
                                }
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.XepLoai.Text = ccdl;
                                    XepLoai1 = ccdl;
                                }


                                try
                                {
                                    DiemSoWD.NhanXet.Text = reader.GetString(1);
                                    NhanXet1 = reader.GetString(1);
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.NhanXet.Text = ccdl;
                                    NhanXet1 = ccdl;
                                }
                                try
                                {
                                    DiemSoWD.DiemTbHK1.Text = reader.GetDecimal(2).ToString();
                                    TBHK1 = reader.GetString(2).ToString();
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.DiemTbHK1.Text = ccdl;
                                    TBHK1 = ccdl;
                                }
                                stt++;
                            }
                            else
                            {
                                //if (reader.GetBoolean(0)) DiemSoWD.XepLoai2.Text = "Đạt"; else DiemSoWD.XepLoai2.Text = "Không đạt";

                                try
                                {
                                    bool XepLoai = reader.GetBoolean(0);
                                if (XepLoai)
                                {
                                    DiemSoWD.XepLoai2.Text = d;
                                    XepLoai2 = d;
                                }
                                else
                                {
                                    DiemSoWD.XepLoai2.Text = kd;
                                    XepLoai2 = kd;
                                }
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.XepLoai2.Text = ccdl;
                                    XepLoai2 = ccdl;
                                }

                                try
                                {
                                    DiemSoWD.NhanXet2.Text = reader.GetString(1);
                                    NhanXet2 = reader.GetString(1);
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.NhanXet2.Text = ccdl;
                                    NhanXet2= ccdl;
                                }
                                try
                                {
                                    DiemSoWD.DiemTbHK2.Text = reader.GetDecimal(2).ToString();
                                    TBHK2 = reader.GetDecimal(2).ToString();
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.DiemTbHK2.Text = ccdl;
                                    TBHK2 = ccdl;
                                }
                            }
                        }
                        reader.NextResult();
                    }
                    return stt;

            }
        }

        public DiemSoViewModel()
        {
            // Stryker disable all
            everLoaded = false;
            LoadWindow = new RelayCommand<DiemSo>((parameter) => { return true; }, (parameter) =>
            {
                //if (everLoaded == false)
                //{
                DiemSoWD = parameter;
                LoadDanhSachDiem();
                //everLoaded = true;
                //}
            });

        }
    }

}