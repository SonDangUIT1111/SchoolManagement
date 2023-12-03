using StudentManagement.Model;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class ThanhTichHocSinhViewModel : BaseViewModel
    {
        const string whereconst = "' where MaThanhTich = '";
        const string ccnx = "Chưa có nhận xét";
        const bool t = true;
        const bool f = false;
        public ThanhTichHocSinh ThanhTichWD;

        public string NienKhoaQueries;
        public string KhoiQueries;
        public string LopQueries;
        public string HocKyQueries;
        private int idUser;
        public int IdUser { get { return idUser; } set { idUser = value; } }
        public bool everLoaded { get; set; }

        public ObservableCollection<string> _nienKhoaCombobox;
        public ObservableCollection<string> NienKhoaCombobox
        {
            get => _nienKhoaCombobox;
            set { _nienKhoaCombobox = value;  }
        }

        public ObservableCollection<Model.Khoi> _khoiCombobox;
        public ObservableCollection<Model.Khoi> KhoiCombobox
        {
            get => _khoiCombobox;
            set { _khoiCombobox = value;  }
        }

        public ObservableCollection<Model.Lop> _lopCombobox;
        public ObservableCollection<Model.Lop> LopCombobox
        {
            get => _lopCombobox;
            set { _lopCombobox = value;  }
        }

        public ObservableCollection<string> _hocKyCombobox;
        public ObservableCollection<string> HocKyCombobox
        {
            get => _hocKyCombobox;
            set { _hocKyCombobox = value;  }
        }

        private ObservableCollection<StudentManagement.Model.ThanhTich> _danhSachThanhTichHocSinh;
        public ObservableCollection<StudentManagement.Model.ThanhTich> DanhSachThanhTichHocSinh
        {
            get => _danhSachThanhTichHocSinh;
            set { _danhSachThanhTichHocSinh = value;  }
        }


        public bool _nhanXetTextBoxVisibility;
        public bool NhanXetTextBoxVisibility
        {
            get { return _nhanXetTextBoxVisibility; }
            set
            {
                _nhanXetTextBoxVisibility = value;
                
            }
        }

        public bool _nhanXetTextBlockVisibility;
        public bool NhanXetTextBlockVisibility
        {
            get { return _nhanXetTextBlockVisibility; }
            set
            {
                _nhanXetTextBlockVisibility = value;
                            }
        }


        public bool _editNhanXetVisibility;
        public bool EditNhanXetVisibility
        {
            get { return _editNhanXetVisibility; }
            set
            {
                _editNhanXetVisibility = value;
                
            }
        }

        public bool _completeNhanXetVisibility;
        public bool CompleteNhanXetVisibility
        {
            get { return _completeNhanXetVisibility; ; }
            set
            {
                _completeNhanXetVisibility = value;
                
            }
        }


        public bool _nhanXetTextBoxIsEnabled;
        public bool NhanXetTextBoxIsEnabled
        {
            get { return _nhanXetTextBoxIsEnabled; ; }
            set
            {
                _nhanXetTextBoxIsEnabled = value;
                
            }
        }


        private bool _dataGridVisibility;
        public bool DataGridVisibility
        {
            get
            {
                return _dataGridVisibility;
            }
            set
            {
                _dataGridVisibility = value;
                
            }
        }

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility
        {
            get
            {
                return _progressBarVisibility;
            }
            set
            {
                _progressBarVisibility = value;
                
            }
        }


        public ICommand LoadThanhTich { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand EditNhanXet { get; set; }
        public ICommand CompleteNhanXet { get; set; }



        public void LoadComboBox()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string cmdString = "select distinct NienKhoa from ThanhTich tt join Lop l on tt.MaLop = l.MaLop";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NienKhoaCombobox.Add(reader.GetString(0));
                            if (string.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                                //try
                                //{
                                //    ThanhTichWD.cmbNienKhoa.SelectedIndex = 0;
                                //}
                                //catch (Exception) { }
                                //FilterKhoiFromNienKhoa();
                                //try
                                //{
                                //    ThanhTichWD.cmbKhoi.SelectedIndex = 0;
                                //} catch (Exception) { }
                                //FilterLopFromKhoi();
                                //try
                                //{
                                //    ThanhTichWD.cmbLop.SelectedIndex = 0;
                                //} catch (Exception) { }
                                //FilterHocKyFromLop();
                                //try
                                //{
                                //    ThanhTichWD.cmbHocky.SelectedIndex = 0;
                                //} catch (Exception) { }

                            }
                        }
                        reader.NextResult();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail msgBoxFail = new MessageBoxFail();
                    //msgBoxFail.ShowDialog();
                }
            }
        }


        public void FilterKhoiFromNienKhoa()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string cmdString = "select distinct k.MaKhoi,Khoi from ThanhTich tt join Lop l on tt.MaLop = l.MaLop join Khoi k on k.MaKhoi = l.MaKhoi where NienKhoa = '" + NienKhoaQueries + "'";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
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
                            //try
                            //{
                            //    ThanhTichWD.cmbKhoi.SelectedIndex = 0;
                            //} catch (Exception) { }   
                            if (string.IsNullOrEmpty(KhoiQueries))
                            {
                                KhoiQueries = reader.GetInt32(0).ToString();
                            }
                        }
                        reader.NextResult();
                    }
                } catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }

        public void FilterLopFromKhoi()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string cmdString = "select distinct tt.MaLop,TenLop from ThanhTich tt join Lop l on tt.MaLop = l.MaLop " +
                                        " where NienKhoa = '" + NienKhoaQueries + "' and  l.MaKhoi = " + KhoiQueries;
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
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
                            //try
                            //{
                            //    ThanhTichWD.cmbLop.SelectedIndex = 0;
                            //} catch (Exception) { }
                            if (string.IsNullOrEmpty(LopQueries))
                            {
                                LopQueries = reader.GetInt32(0).ToString();
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
        }



        public void FilterHocKyFromLop()
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string cmdString = "select distinct HocKy from ThanhTich tt join Lop l on tt.MaLop = l.MaLop " +
                                        " where NienKhoa = '" + NienKhoaQueries + "' and l.MaKhoi = " + KhoiQueries + " and l.MaLop = " + LopQueries;
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            HocKyCombobox.Add("Học kỳ " + reader.GetInt32(0).ToString());
                            //try
                            //{
                            //    ThanhTichWD.cmbHocky.SelectedIndex = 0;
                            //} catch (Exception) { }
                            if (string.IsNullOrEmpty(HocKyQueries))
                            {
                                HocKyQueries = reader.GetInt32(0).ToString();
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
        }


        public async Task LoadDanhSachThanhTichHocSinh()
        {
            bool verify = f;
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                    string CmdString = "select tt.MaThanhTich,TenHocSinh,l.TenLop,tt.XepLoai,tt.TrungBinhHocKy,tt.NhanXet,MaGVCN from ThanhTich tt join Lop l on tt.MaLop = l.MaLop join HocSinh hs on tt.MaHocSinh = hs.MaHocSinh " +
                                        " where NienKhoa ='" + NienKhoaQueries + "' and l.MaKhoi = " + KhoiQueries + " and tt.MaLop = " + LopQueries + " and HocKy = " + HocKyQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.ThanhTich thanhtich = new StudentManagement.Model.ThanhTich();
                            thanhtich.MaThanhTich = reader.GetInt32(0);
                            thanhtich.TenHocSinh = reader.GetString(1);
                            thanhtich.TenLop = reader.GetString(2);
                            try
                            {
                                thanhtich.XepLoai = reader.GetBoolean(3);
                            }
                            catch (Exception) { }
                            try
                            {
                                thanhtich.TBHK = (float)reader.GetDecimal(4);
                            }
                            catch (Exception) { }
                            try
                            {
                                thanhtich.NhanXet = reader.GetString(5);
                            }
                            catch (Exception)
                            {
                                thanhtich.NhanXet = ccnx;
                            }

                           

                            DanhSachThanhTichHocSinh.Add(thanhtich);
                            if (verify == f)
                            {
                                try
                                {
                                    if (IdUser == reader.GetInt32(6))
                                    {
                                        EditNhanXetVisibility = t;
                                        CompleteNhanXetVisibility = f;
                                        NhanXetTextBoxIsEnabled = f;
                                        verify = t;
                                    }
                                    else
                                    {
                                        EditNhanXetVisibility = f;
                                        CompleteNhanXetVisibility = f;
                                        NhanXetTextBoxIsEnabled = f;
                                        verify = t;
                                    }
                                }
                                catch (Exception)
                                {
                                    EditNhanXetVisibility = f;
                                    CompleteNhanXetVisibility = f;
                                    NhanXetTextBoxIsEnabled = f;
                                    verify = t;
                                }
                                
                            }
                        }
                        await reader.NextResultAsync();
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }

         public int UpdateNhanXet()
            {
            int count = 0;
                foreach (var item in DanhSachThanhTichHocSinh)
                {
                    using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
                    {
                            sqlConnectionWrap.Open();
                            string CmdString = "update ThanhTich Set NhanXet=N'" + item.NhanXet + whereconst + item.MaThanhTich + "'";
                            SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrap.GetSqlConnection());
                            count += cmd.ExecuteNonQuery();

                    }
                }
            return count;
            }
        public ThanhTichHocSinhViewModel()
        {
            // Stryker disable all
            IdUser = 100000;
            everLoaded = false;
            DanhSachThanhTichHocSinh = new ObservableCollection<Model.ThanhTich>();
            NienKhoaCombobox = new ObservableCollection<string>();
            KhoiCombobox = new ObservableCollection<Khoi>();
            HocKyCombobox = new ObservableCollection<string>();
            LopCombobox = new ObservableCollection<Lop>();
            LoadThanhTich = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                //if (everLoaded == false)
                //{
                ThanhTichWD = parameter as ThanhTichHocSinh;
                KhoiCombobox.Clear();
                LopCombobox.Clear();
                HocKyCombobox.Clear();
                LoadComboBox();
                FilterKhoiFromNienKhoa();
                FilterLopFromKhoi();
                FilterHocKyFromLop();
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                DanhSachThanhTichHocSinh.Clear();
                await LoadDanhSachThanhTichHocSinh();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
                //    everLoaded = true;
                //}
            });

            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    KhoiCombobox.Clear();
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
                    LopCombobox.Clear();
                    FilterLopFromKhoi();
                }
            });

            FilterLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    LopQueries = item.MaLop.ToString();
                    HocKyCombobox.Clear();
                    FilterHocKyFromLop();
                }
            });
            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries = "1";
                    else
                        HocKyQueries = "2";
                    DataGridVisibility = false;
                    ProgressBarVisibility = true;
                    DanhSachThanhTichHocSinh.Clear();
                    await LoadDanhSachThanhTichHocSinh();
                    DataGridVisibility = true;
                    ProgressBarVisibility = false;
                }
            });

            EditNhanXet = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                EditNhanXetVisibility = false;
                CompleteNhanXetVisibility = true;
                NhanXetTextBoxIsEnabled = true;
            });

            CompleteNhanXet = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                EditNhanXetVisibility = true;
                CompleteNhanXetVisibility = false;
                NhanXetTextBoxIsEnabled = false;
                UpdateNhanXet();
            });
        }
    }
    }

 
