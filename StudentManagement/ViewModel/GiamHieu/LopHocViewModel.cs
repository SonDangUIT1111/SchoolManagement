﻿using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class LopHocViewModel : BaseViewModel
    {
        const string ccgvcn = "Chưa có GVCN";
        public string NienKhoaQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string TenLopQueries { get; set; }

        private ObservableCollection<StudentManagement.Model.Lop> _danhSachLopHoc;
        public ObservableCollection<StudentManagement.Model.Lop> DanhSachLopHoc { get => _danhSachLopHoc; set { _danhSachLopHoc = value;  } }

        public DanhSachLop DanhSachLopPage;
        public LopHoc LopHocWD { get; set; }

        public ObservableCollection<string> _nienKhoaCmb;

        public ObservableCollection<string> NienKhoaCmb
        {
            get => _nienKhoaCmb;
            set
            {
                _nienKhoaCmb = value;
                
            }
        }

        public ObservableCollection<Model.Khoi> _khoiCmb;

        public ObservableCollection<Model.Khoi> KhoiCmb
        {
            get => _khoiCmb;
            set
            {
                _khoiCmb = value;
                
            }
        }

        public Lop _gridSeletecdItem;

        public Lop GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                
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


        public ICommand SwitchDanhSachLop { get; set; }
        public ICommand ThemLop { get; set; }
        public ICommand LoadLopHoc { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterTenLop { get; set; }
        public ICommand XoaLop { get; set; }
        public ICommand SuaLop { get; set; }
        public ICommand XemLop { get; set; }

        public void LoadComboBox()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();
                    string cmdString = "SELECT DISTINCT NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                NienKhoaCmb.Add(reader.GetString(0));
                                if (String.IsNullOrEmpty(NienKhoaQueries))
                                    NienKhoaQueries = reader.GetString(0);
                            } catch (Exception)
                            {
                            }
 
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrapper.Close();

                    sqlConnectionWrapper.Open();
                    cmdString = "select MaKhoi,Khoi from Khoi";
                    cmd = new SqlCommand(cmdString, sqlConnectionWrapper.GetSqlConnection());
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Model.Khoi item = new Model.Khoi()
                            {
                                MaKhoi = reader.GetInt32(0),
                                TenKhoi = reader.GetString(1),
                           
                            };
                            try
                            {
                                KhoiCmb.Add(item);
                                if (String.IsNullOrEmpty(KhoiQueries))
                                    KhoiQueries = reader.GetInt32(0).ToString();
                            } catch (Exception)
                            {

                            }
                        }
                        reader.NextResult();
                    }


                    //await LoadDanhSachLopHoc();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public async Task LoadDanhSachLopHoc()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();

                    string WhereCmdString = "";

                    if (!String.IsNullOrEmpty(NienKhoaQueries))
                    {
                        WhereCmdString = WhereCmdString + " WHERE NienKhoa = '" + NienKhoaQueries + "'";
                    }

                    if (!String.IsNullOrEmpty(KhoiQueries))
                    {
                        if (!String.IsNullOrEmpty(WhereCmdString))
                        {
                            WhereCmdString = WhereCmdString + " AND l.MaKhoi = " + KhoiQueries;
                        }
                        else
                        {
                            WhereCmdString = WhereCmdString + " WHERE l.MaKhoi = " + KhoiQueries;
                        }
                    }

                    string CmdString = "SELECT l.MaLop, TenLop, SiSo, NienKhoa, TenGiaoVien " +
                        " FROM Lop l join Khoi k on l.MaKhoi = k.MaKhoi " +
                        " left join GiaoVien gv on l.MaGVCN = gv.MaGiaoVien " + WhereCmdString;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        StudentManagement.Model.Lop lophoc = new StudentManagement.Model.Lop();
                        lophoc.MaLop = reader.GetInt32(0);
                        lophoc.TenLop = reader.GetString(1);
                        lophoc.SiSo = reader.GetInt32(2);
                        lophoc.NienKhoa = reader.GetString(3);
                        try
                        {
                            lophoc.TenGVCN = reader.GetString(4);
                        }
                        catch (Exception)
                        {
                            lophoc.TenGVCN = ccgvcn;
                        }
                        DanhSachLopHoc.Add(lophoc);
                    }
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();

                }
            }
        }
        public void FilterFromTenLop()
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrapper.Open();

                    string WhereCmdString = "";

                    if (!String.IsNullOrEmpty(NienKhoaQueries))
                    {
                        WhereCmdString = WhereCmdString + " WHERE NienKhoa = '" + NienKhoaQueries + "'";
                    }

                    if (!String.IsNullOrEmpty(KhoiQueries))
                    {
                        if (!String.IsNullOrEmpty(WhereCmdString))
                        {
                            WhereCmdString = WhereCmdString + " AND l.MaKhoi = " + KhoiQueries ;
                        }
                        else
                        {
                            WhereCmdString = WhereCmdString + " WHERE l.MaKhoi = " + KhoiQueries ;
                        }
                    }

                    if (!String.IsNullOrEmpty(TenLopQueries))
                    {
                        if (!String.IsNullOrEmpty(WhereCmdString))
                        {
                            WhereCmdString = WhereCmdString + " AND TenLop like '%" + TenLopQueries + "%'";
                        }
                        else
                        {
                            WhereCmdString = WhereCmdString + " WHERE TenLop like '%" + TenLopQueries + "%'";
                        }
                    }

                    string CmdString = "SELECT l.MaLop,TenLop,SiSo,NienKhoa,TenGiaoVien " +
                                        " FROM Lop l join Khoi k on l.MaKhoi = k.MaKhoi " +
                                        " left join GiaoVien gv on l.MaGVCN = gv.MaGiaoVien" + WhereCmdString;
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop lophoc = new StudentManagement.Model.Lop();
                            lophoc.MaLop = reader.GetInt32(0);
                            lophoc.TenLop = reader.GetString(1);
                            lophoc.SiSo = reader.GetInt32(2);
                            lophoc.NienKhoa = reader.GetString(3);
                            try
                            {
                                lophoc.TenGVCN = reader.GetString(4);
                            }
                            catch (Exception)
                            {
                                lophoc.TenGVCN = "Chưa có GVCN";
                            }
                            DanhSachLopHoc.Add(lophoc);
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

        public LopHocViewModel()
        {
            // Stryker disable all

            DanhSachLopHoc = new ObservableCollection<Lop>();
            NienKhoaCmb = new ObservableCollection<string>();
            KhoiCmb = new ObservableCollection<Model.Khoi>();
            DanhSachLopHoc = new ObservableCollection<Model.Lop>();

            LoadLopHoc = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                LopHocWD = parameter as LopHoc;
                DanhSachLopHoc.Clear();
                NienKhoaCmb.Clear();
                KhoiCmb.Clear();
                LoadComboBox();
                if (NienKhoaCmb.Count > 0)
                {
                    LopHocWD.cmbNienKhoa.SelectedIndex = 0;
                    NienKhoaQueries = NienKhoaCmb[0];
                }
                if (KhoiCmb.Count > 0)
                {
                    LopHocWD.cmbKhoi.SelectedIndex = 0;
                    KhoiQueries = KhoiCmb[0].MaKhoi.ToString();
                }
                DanhSachLopHoc.Clear();
                await LoadDanhSachLopHoc();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });

            ThemLop = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ThemLopHoc window = new ThemLopHoc();
                ThemLopHocViewModel data = window.DataContext as ThemLopHocViewModel;
                window.ShowDialog();
                DanhSachLopHoc.Clear();
                await LoadDanhSachLopHoc();
            });

            SuaLop = new RelayCommand<Model.Lop>((parameter) => { return true; }, async (parameter) =>
            {
                SuaThongTinLopHoc window = new SuaThongTinLopHoc();
                SuaLopHocViewModel data = window.DataContext as SuaLopHocViewModel;
                data.LopHocHienTai = parameter;
                window.ShowDialog();
                DanhSachLopHoc.Clear();
                await LoadDanhSachLopHoc();
            });
            XemLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachLop danhSachLopWd = new DanhSachLop();
                DanhSachLopViewModel vm = danhSachLopWd.DataContext as DanhSachLopViewModel;
                Model.Lop lop = parameter as Model.Lop;
                vm.MaLop = lop.MaLop;
                vm.TenLop = lop.TenLop;
                LopHocWD.NavigationService.Navigate(danhSachLopWd);
            });

            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    DataGridVisibility = false;
                    ProgressBarVisibility = true;
                    if (cmb.SelectedIndex >= 0)
                    {
                        NienKhoaQueries = cmb.SelectedItem.ToString();
                    }
                    if (LopHocWD.cmbKhoi.Items.Count > 0)
                    {
                        LopHocWD.cmbKhoi.SelectedIndex = 0;
                    }
                    DanhSachLopHoc.Clear();
                    await LoadDanhSachLopHoc();
                    DataGridVisibility = true;
                    ProgressBarVisibility = false;
                }
            });

            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    if (cmb.SelectedIndex >= 0)
                    {
                        DataGridVisibility = false;
                        ProgressBarVisibility = true;
                        Model.Khoi item = cmb.SelectedItem as Model.Khoi;
                        KhoiQueries = item.MaKhoi.ToString();
                        DanhSachLopHoc.Clear();
                        await LoadDanhSachLopHoc();
                        DataGridVisibility = true;
                        ProgressBarVisibility = false;
                    }
                }
            });

            FilterTenLop = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                TextBox txt = parameter as TextBox;
                if (txt != null)
                {
                    TenLopQueries = txt.Text.ToString();
                    DanhSachLopHoc.Clear();
                    FilterFromTenLop();
                }
            });

            XoaLop = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                Model.Lop item = parameter as Model.Lop;
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có muốn xóa lớp không?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true)
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
                                MessageBoxFail messageBoxFail = new MessageBoxFail();
                                messageBoxFail.ShowDialog();
                                return;
                            }
                            string cmdString = "DELETE FROM Lop WHERE MaLop = " + item.MaLop.ToString() + "";
                            SqlCommand cmd = new SqlCommand(cmdString, con);
                            cmd.ExecuteNonQuery();
                            MessageBoxOK MB = new MessageBoxOK();
                            var datamb = MB.DataContext as MessageBoxOKViewModel;
                            datamb.Content = "Xóa thành công";
                            MB.ShowDialog();
                            DataGridVisibility = false;
                            ProgressBarVisibility = true;
                            DanhSachLopHoc.Clear();
                            await LoadDanhSachLopHoc();
                            DataGridVisibility = true;
                            ProgressBarVisibility = false;
                            con.Close();
                        }
                        catch (Exception)
                        {
                            MessageBoxFail messageBoxFail = new MessageBoxFail();
                            messageBoxFail.ShowDialog();
                        }
                    }
                }
            });


        }



    }
}
