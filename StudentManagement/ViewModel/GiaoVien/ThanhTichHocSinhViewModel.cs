using StudentManagement.Model;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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

        public ObservableCollection<string> _khoiCombobox;
        public ObservableCollection<string> KhoiCombobox
        {
            get => _khoiCombobox;
            set { _khoiCombobox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> _lopCombobox;
        public ObservableCollection<string> LopCombobox
        {
            get => _lopCombobox;
            set { _lopCombobox = value; OnPropertyChanged(); }
        }

        public ObservableCollection<int> _hocKyCombobox;
        public ObservableCollection<int> HocKyCombobox
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

        public ThanhTichHocSinhViewModel(){


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
                if (cmb != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterKhoiFromNienKhoa();
                }
            });

            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    KhoiQueries = cmb.SelectedItem.ToString();
                    FilterLopFromKhoi();
                }
            });

            FilterLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                MessageBox.Show(parameter.ToString());
                if (cmb != null)
                {
                    LopQueries = cmb.SelectedItem.ToString();
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
            KhoiCombobox = new ObservableCollection<string>();
            HocKyCombobox = new ObservableCollection<int>();
            LopCombobox = new ObservableCollection<string>();

            

            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
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
        }


        public void FilterKhoiFromNienKhoa()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct left(TenLop,2) from ThanhTich where NienKhoa = '" + NienKhoaQueries + "'";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        KhoiCombobox.Add(reader.GetString(0));
                        if (string.IsNullOrEmpty(KhoiQueries))
                        {
                            KhoiQueries = reader.GetString(0);
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }

        public void FilterLopFromKhoi()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct TenLop from ThanhTich where NienKhoa = '" + NienKhoaQueries + "' and left(TenLop,2) = '"+KhoiQueries+"'";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LopCombobox.Add(reader.GetString(0));
                        if (string.IsNullOrEmpty(LopQueries))
                        {
                            LopQueries = reader.GetString(0);
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }



        public void FilterHocKyFromLop()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct HocKy from ThanhTich where NienKhoa = '" + NienKhoaQueries + "' and left(TenLop,2) = '" + KhoiQueries + "' and TenLop = '"+LopQueries+"'";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HocKyCombobox.Add(reader.GetInt32(0));
                        if (string.IsNullOrEmpty(HocKyQueries))
                        {
                            HocKyQueries = reader.GetInt32(0).ToString();
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }


        public void LoadDanhSachThanhTichHocSinh()
        {
            DanhSachThanhTichHocSinh = new ObservableCollection<Model.ThanhTich>();
            DanhSachThanhTichHocSinh.Clear();
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "select * from ThanhTich where NienKhoa ='"+NienKhoaQueries+"' and left(TenLop,2) = '"+KhoiQueries+"' and TenLop = '"+LopQueries+"' and HocKy ='"+HocKyQueries+"'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.ThanhTich thanhtich = new StudentManagement.Model.ThanhTich();
                            thanhtich.MaThanhTich = reader.GetInt32(0);
                            thanhtich.TenHocSinh = reader.GetString(6);
                            thanhtich.TenLop = reader.GetString(4);
                            thanhtich.XepLoai = reader.GetBoolean(7);
                            thanhtich.TBHK = (float)reader.GetDecimal(9);

                            if (reader.IsDBNull(8))
                            {
                                thanhtich.NhanXet = " ";

                            }
                            else
                            {
                                thanhtich.NhanXet = reader.GetString(8);
                            }

                        DanhSachThanhTichHocSinh.Add(thanhtich);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
        }

        public void UpdateNhanXet()
        {
            foreach (var item in DanhSachThanhTichHocSinh)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                {
                    con.Open();
                    string CmdString = "update ThanhTich Set NhanXet='" + item.NhanXet + "' where MaThanhTich = '" + item.MaThanhTich + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
