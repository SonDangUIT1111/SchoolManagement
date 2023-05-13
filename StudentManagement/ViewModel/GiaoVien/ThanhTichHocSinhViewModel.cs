using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    internal class ThanhTichHocSinhViewModel : BaseViewModel
    {
        public ThanhTichHocSinh ThanhTichWD;

        public string NienKhoaQueries;
        public string KhoiQueries;
        public string LopQueries;
        public string HocKyQueries;


        public ObservableCollection<string> _nienKhoaCombobox;
        public ObservableCollection<string> NienKhoaCombobox
        {
            get => _nienKhoaCombobox;
            set { _nienKhoaCombobox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Model.Khoi> _khoiCombobox;
        public ObservableCollection<Model.Khoi> KhoiCombobox
        {
            get => _khoiCombobox;
            set { _khoiCombobox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Model.Lop> _lopCombobox;
        public ObservableCollection<Model.Lop> LopCombobox
        {
            get => _lopCombobox;
            set { _lopCombobox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> _hocKyCombobox;
        public ObservableCollection<string> HocKyCombobox
        {
            get => _hocKyCombobox;
            set { _hocKyCombobox = value; OnPropertyChanged(); }
        }

        private ObservableCollection<StudentManagement.Model.ThanhTich> _danhSachThanhTichHocSinh;
        public ObservableCollection<StudentManagement.Model.ThanhTich> DanhSachThanhTichHocSinh
        {
            get => _danhSachThanhTichHocSinh;
            set { _danhSachThanhTichHocSinh = value; OnPropertyChanged(); }
        }


        public bool _nhanXetTextBoxVisibility;
        public bool NhanXetTextBoxVisibility
        {
            get { return _nhanXetTextBoxVisibility; }
            set
            {
                _nhanXetTextBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool _nhanXetTextBlockVisibility;
        public bool NhanXetTextBlockVisibility
        {
            get { return _nhanXetTextBlockVisibility; }
            set
            {
                _nhanXetTextBlockVisibility = value;
                OnPropertyChanged();
            }
        }


        public bool _editNhanXetVisibility;
        public bool EditNhanXetVisibility
        {
            get { return _editNhanXetVisibility; }
            set
            {
                _editNhanXetVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool _completeNhanXetVisibility;
        public bool CompleteNhanXetVisibility
        {
            get { return _completeNhanXetVisibility; ; }
            set
            {
                _completeNhanXetVisibility = value;
                OnPropertyChanged();
            }
        }


        public bool _nhanXetTextBoxIsEnabled;
        public bool NhanXetTextBoxIsEnabled
        {
            get { return _nhanXetTextBoxIsEnabled; ; }
            set
            {
                _nhanXetTextBoxIsEnabled = value;
                OnPropertyChanged();
            }
        }


        public ICommand LoadThanhTich { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand EditNhanXet { get; set; }
        public ICommand CompleteNhanXet { get; set; }

        public ThanhTichHocSinhViewModel()
        {

            DanhSachThanhTichHocSinh = new ObservableCollection<Model.ThanhTich>();
            LoadThanhTich = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThanhTichWD = parameter as ThanhTichHocSinh;
                LoadComboBox();
                LoadDanhSachThanhTichHocSinh();
                //NhanXetTextBoxVisibility = false;
                //NhanXetTextBlockVisibility = true;
                NhanXetTextBoxVisibility = true;
                CompleteNhanXetVisibility = false;
                EditNhanXetVisibility = true;
            });

            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterKhoiFromNienKhoa();
                }
            });

            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    FilterLopFromKhoi();
                }
            });

            FilterLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                MessageBox.Show(parameter.ToString());
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    LopQueries = item.MaLop.ToString();
                    FilterHocKyFromLop();
                }
            });

            EditNhanXet = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                //NhanXetTextBoxVisibility = true;
                //NhanXetTextBlockVisibility = false;
                EditNhanXetVisibility = false;
                CompleteNhanXetVisibility = true;
                NhanXetTextBoxIsEnabled = true;
            });

            CompleteNhanXet = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                //NhanXetTextBoxVisibility = false;
                //NhanXetTextBlockVisibility = true;
                EditNhanXetVisibility = true;
                CompleteNhanXetVisibility = false;
                NhanXetTextBoxIsEnabled = false;
                UpdateNhanXet();
            });
        }


        public void LoadComboBox()
        {
            NienKhoaCombobox = new ObservableCollection<string>();
            KhoiCombobox = new ObservableCollection<Khoi>();
            HocKyCombobox = new ObservableCollection<string>();
            LopCombobox = new ObservableCollection<Lop>();



            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
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
                    string cmdString = "select distinct NienKhoa from ThanhTich";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NienKhoaCombobox.Add(reader.GetString(0));
                            if (string.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                                ThanhTichWD.cmbNienKhoa.SelectedIndex = 0;
                                FilterKhoiFromNienKhoa();
                                ThanhTichWD.cmbKhoi.SelectedIndex = 0;
                                FilterLopFromKhoi();
                                ThanhTichWD.cmbLop.SelectedIndex = 0;
                                FilterHocKyFromLop();
                                ThanhTichWD.cmbHocky.SelectedIndex = 0;

                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public void FilterKhoiFromNienKhoa()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
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
                    string cmdString = "select distinct k.MaKhoi,Khoi from ThanhTich tt join Lop l on tt.MaLop = l.MaLop join Khoi k on k.MaKhoi = l.MaKhoi where NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            KhoiCombobox.Add(new Khoi()
                            {
                                MaKhoi = reader.GetInt32(0),
                                TenKhoi = reader.GetString(1)
                            });
                            if (string.IsNullOrEmpty(KhoiQueries))
                            {
                                KhoiQueries = reader.GetInt32(0).ToString();
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void FilterLopFromKhoi()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
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
                    string cmdString = "select distinct tt.MaLop,TenLop from ThanhTich tt join Lop l on tt.MaLop = l.MaLop " +
                                        " where NienKhoa = '" + NienKhoaQueries + "' l.MaKhoi = " + KhoiQueries + " ";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            LopCombobox.Add(new Lop()
                            {
                                MaLop = reader.GetInt32(0),
                                TenLop = reader.GetString(1)
                            });
                            if (string.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        public void FilterHocKyFromLop()
        {
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
                    string cmdString = "select distinct HocKy from ThanhTich tt join Lop l on tt.MaKhoi = l.MaKhoi " +
                                        " where NienKhoa = '" + NienKhoaQueries + "' and l.MaKhoi = " + KhoiQueries + " and l.MaLop = " + LopQueries + " ";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            HocKyCombobox.Add("Học kỳ " + reader.GetInt32(0).ToString());
                            if (string.IsNullOrEmpty(HocKyQueries))
                            {
                                HocKyQueries = reader.GetInt32(0).ToString();
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public void LoadDanhSachThanhTichHocSinh()
        {
            DanhSachThanhTichHocSinh.Clear();
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
                    string CmdString = "select tt.MaThanhTich,TenHocSinh,l.TenLop,tt.XepLoai,tt.TrungBinhHocKy,tt.NhanXet from ThanhTich tt join Lop l on tt.MaLop = l.MaLop join HocSinh hs on tt.MaHocSinh = hs.MaHocSinh " +
                                        " where NienKhoa ='" + NienKhoaQueries + "' and l.MaKhoi = " + KhoiQueries + " and tt.MaLop = " + LopQueries + " and HocKy = " + HocKyQueries + " ";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.ThanhTich thanhtich = new StudentManagement.Model.ThanhTich();
                            thanhtich.MaThanhTich = reader.GetInt32(0);
                            thanhtich.TenHocSinh = reader.GetString(1);
                            thanhtich.TenLop = reader.GetString(2);
                            thanhtich.XepLoai = reader.GetBoolean(3);
                            thanhtich.TBHK = (float)reader.GetDecimal(4);

                            if (reader.IsDBNull(5))
                            {
                                thanhtich.NhanXet = " ";

                            }
                            else
                            {
                                thanhtich.NhanXet = reader.GetString(5);
                            }

                            DanhSachThanhTichHocSinh.Add(thanhtich);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void UpdateNhanXet()
        {
            foreach (var item in DanhSachThanhTichHocSinh)
            {
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
                        string CmdString = "update ThanhTich Set NhanXet='" + item.NhanXet + "' where MaThanhTich = '" + item.MaThanhTich + "'";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
