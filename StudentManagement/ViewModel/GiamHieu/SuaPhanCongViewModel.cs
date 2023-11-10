using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
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
        private ObservableCollection<StudentManagement.Model.GiaoVien> _giaoVienCmb;
        public ObservableCollection<StudentManagement.Model.GiaoVien> GiaoVienCmb { get => _giaoVienCmb; set { _giaoVienCmb = value; OnPropertyChanged(); } }

        public ICommand LoadData { get; set; }
        public ICommand SuaPhanCong { get; set; }
        public ICommand HuySuaPC { get; set; }

        private readonly ISqlConnectionWrapper sqlConnection;

        public SuaPhanCongViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public SuaPhanCongViewModel()
        {

            LoadData = new RelayCommand<SuaPhanCong>((parameter) => { return true; }, (parameter) =>
            {
                SuaPhanCongWD = parameter;
                LoadThongTinCmb();
                SuaPhanCongWD.cmbGiaoVien.SelectedIndex = GiaoVienCmb.ToList().FindIndex(x => x.TenGiaoVien == PhanCongHienTai.TenGiaoVien); ;
            });

            SuaPhanCong = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Model.GiaoVien giaovien = SuaPhanCongWD.cmbGiaoVien.SelectedItem as Model.GiaoVien;
                if (giaovien == null)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng chọn giáo viên giảng dạy";
                    MB.ShowDialog();
                }
                else
                {
                    MessageBoxYesNo wd = new MessageBoxYesNo();

                    var data = wd.DataContext as MessageBoxYesNoViewModel;
                    data.Title = "Xác nhận!";
                    data.Question = "Bạn có chắc chắn muốn thay đổi không?";
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
                                string CmdString = "update PhanCongGiangDay set  MaGiaoVienPhuTrach=" + giaovien.MaGiaoVien + " where MaPhanCong = " + PhanCongHienTai.MaPhanCong + "";
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                                messageBoxSuccessful.ShowDialog();
                                SuaPhanCongWD.Close();
                            }
                            catch (Exception)
                            {
                                MessageBoxFail messageBoxFail = new MessageBoxFail();
                                messageBoxFail.ShowDialog();
                            }
                        }
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
            GiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try 
                    {
                        sqlConnectionWrapper.Open();
                    }
                    catch (Exception) 
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return; 
                    }
                    string CmdString = "select MaGiaoVien, TenGiaoVien from GiaoVien";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
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
                    sqlConnectionWrapper.Close();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
    }
}
