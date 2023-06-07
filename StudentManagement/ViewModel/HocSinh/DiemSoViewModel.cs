using StudentManagement.Model;
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
        public bool everLoaded { get; set; }
        private int _idHocSinh;
        public int IdHocSinh { get { return _idHocSinh; } set { _idHocSinh = value; } }
        public DiemSo DiemSoWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhsachdiemhk1;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiemHK1 { get => _danhsachdiemhk1; set { _danhsachdiemhk1 = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhsachdiemhk2;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiemHK2 { get => _danhsachdiemhk2; set { _danhsachdiemhk2 = value; OnPropertyChanged(); } }
        public ICommand LoadWindow { get; set; }
        public DiemSoViewModel()
        {
            everLoaded = false;
            LoadWindow = new RelayCommand<DiemSo>((parameter) => { return true; }, (parameter) =>
            {
                if (everLoaded == false)
                {
                    DiemSoWD = parameter;
                    LoadDanhSachDiem();
                    everLoaded = true;
                }
            });

        }
        public void LoadDanhSachDiem()
        {
            //load ten
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    string TenHs;
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
                    string CmdString = "select TenHocSinh,TenLop from HocSinh hs join Lop l on hs.MaLop = l.MaLop where MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) reader.Read();
                    TenHs = reader.GetString(0);
                    DiemSoWD.Ten.Text = TenHs;
                    DiemSoWD.Ten2.Text = TenHs;
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            }
            // load diem
            DanhSachDiemHK1 = new ObservableCollection<Model.HeThongDiem>();
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
                    string CmdString = "select TenMon,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai from HeThongDiem ht join MonHoc mh on ht.MaMon = mh.MaMon where HocKy = 1 and MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                    con.Close();
                } catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            }
            DanhSachDiemHK2 = new ObservableCollection<Model.HeThongDiem>();
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
                    string CmdString = "select TenMon,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai from HeThongDiem ht join MonHoc mh on ht.MaMon = mh.MaMon where HocKy = 2 and MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            }
            //load xep loai
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
                    string CmdString = "select XepLoai,NhanXet,TrungBinhHocky from ThanhTich where MaHocSinh = " + IdHocSinh.ToString();
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
                                    if (XepLoai) DiemSoWD.XepLoai.Text = "Đạt"; else DiemSoWD.XepLoai.Text = "Không đạt";
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.XepLoai.Text = "Chưa có dữ liệu";
                                }
                                
                                
                                try
                                {
                                    DiemSoWD.NhanXet.Text = reader.GetString(1);
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.NhanXet.Text = "Chưa có dữ liệu";
                                }
                                try
                                {
                                    DiemSoWD.DiemTbHK1.Text = reader.GetDecimal(2).ToString();
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.DiemTbHK1.Text = "Chưa có dữ liệu";
                                }
                                stt++;
                            }
                            else
                                {
                                //if (reader.GetBoolean(0)) DiemSoWD.XepLoai2.Text = "Đạt"; else DiemSoWD.XepLoai2.Text = "Không đạt";
                                
                                try
                                {
                                    bool XepLoai = reader.GetBoolean(0); 
                                    if (XepLoai) DiemSoWD.XepLoai2.Text = "Đạt"; else DiemSoWD.XepLoai.Text = "Không đạt";
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.XepLoai2.Text = "Chưa có dữ liệu";
                                }
                            
                                try
                                {
                                    DiemSoWD.NhanXet2.Text = reader.GetString(1);
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.NhanXet2.Text = "Chưa có dữ liệu";
                                }
                                try
                                {
                                    DiemSoWD.DiemTbHK2.Text = reader.GetDecimal(2).ToString();
                                }
                                catch (Exception)
                                {
                                    DiemSoWD.DiemTbHK2.Text = "Chưa có dữ liệu";
                                }
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                } catch (Exception)
                {
                    System.Windows.MessageBox.Show("l2");
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            }
        }
    }

}
