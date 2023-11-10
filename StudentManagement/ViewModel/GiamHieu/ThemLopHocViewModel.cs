using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThemLopHocViewModel : BaseViewModel
    {
        public ThemLopHoc ThemLopHocWD { get; set; }
        private string _maKhoi;
        public string MaKhoi { get { return _maKhoi; } set { _maKhoi = value;} }
        public string NienKhoa;
        private ObservableCollection<Khoi> _khoiCmb;
        public ObservableCollection<Khoi> KhoiCmb { get { return _khoiCmb;}  set { _khoiCmb = value;OnPropertyChanged(); } }

        private readonly ISqlConnectionWrapper sqlConnection;

        public ThemLopHocViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public ICommand AddClass { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand CancelAddClass { get; set; }

        public ThemLopHocViewModel()
        {
            KhoiCmb = new ObservableCollection<Khoi>();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemLopHocWD = parameter as ThemLopHoc;
                LoadKhoiCmb();
                NienKhoa = LoadNienKhoa(DateTime.Today);
            });

            AddClass = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (String.IsNullOrEmpty(ThemLopHocWD.ClassName.Text) || ThemLopHocWD.KhoiCmb.SelectedIndex == -1)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng điền đầy đủ thông tin";
                    MB.ShowDialog();
                }
                else 
                {
                    try
                    {
                        int result = ThemLopMoi(ThemLopHocWD.ClassName.Text, ThemLopHocWD.KhoiCmb.SelectedItem as Khoi);
                        if (result == -2)
                        {
                            MessageBoxOK messageBoxOK = new MessageBoxOK();
                            MessageBoxOKViewModel data = messageBoxOK.DataContext as MessageBoxOKViewModel;
                            data.Content = "Đã tồn tại tên lớp và niên khóa lớp này, vui lòng xem xét lại";
                            messageBoxOK.ShowDialog();
                            return;
                        }
                        else
                        {
                            MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                            messageBoxSuccessful.ShowDialog();
                        }
                        ThemLopHocWD.Close();
                    }
                    catch (Exception)
                    {
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return;
                    }
                }
            });

            CancelAddClass = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThemLopHocWD.Close();
            });
        }
  
        public void LoadKhoiCmb()
        {
            KhoiCmb.Clear();
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        sqlConnectionWrap.Open();
                    }
                    catch (Exception)
                    {
                        //MessageBoxFail messageBoxFail = new MessageBoxFail();
                        //messageBoxFail.ShowDialog();
                        return;
                    }
                    string cmdString = "SELECT DISTINCT MaKhoi,Khoi FROM Khoi";
                    SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            KhoiCmb.Add(new Khoi()
                            {
                                MaKhoi = reader.GetInt32(0),
                                TenKhoi = reader.GetString(1),
                            });
                            if (String.IsNullOrEmpty(MaKhoi))
                            {
                                MaKhoi = reader.GetInt32(0).ToString();
                                try
                                {
                                    ThemLopHocWD.KhoiCmb.SelectedIndex = 0;
                                } catch (Exception) { }
                            }
                        }
                        reader.NextResult();
                    }
                    sqlConnectionWrap.Close();

                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                    return;
                };
            }
        }
        public int ThemLopMoi(string tenlop, Khoi item)
        {
            using (var sqlConnectionWrap = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                try
                {
                    sqlConnectionWrap.Open();
                }
                catch (Exception)
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                    return -2;
                }
                string cmdText = "Select * from Lop where TenLop = '" + tenlop + "' and NienKhoa = '" + NienKhoa + "' and MaKhoi = " + MaKhoi;
                SqlCommand cmdTest = new SqlCommand(cmdText, sqlConnectionWrap.GetSqlConnection());
                int checkExists = Convert.ToInt32(cmdTest.ExecuteScalar());
                if (checkExists > 0)
                {
                    return -1;
                }
                MaKhoi = item.MaKhoi.ToString();
                string cmdString = "INSERT INTO Lop(TenLop, MaKhoi,NienKhoa) VALUES ('"
                                    + tenlop + "', " + MaKhoi + ", '"
                                    + NienKhoa + "')";
                SqlCommand cmd = new SqlCommand(cmdString, sqlConnectionWrap.GetSqlConnection());
                cmd.ExecuteNonQuery();
                sqlConnectionWrap.Close();
                return 1;
            }
        }
        public string LoadNienKhoa(DateTime dateTime)
        {
            int Month = dateTime.Month;
            int Year = dateTime.Year;

            if (Month < 6)
            {
                int PreviousYear = Year - 1;
                return PreviousYear + "-" + Year;
            }
            else
            {
                int NextYear = Year + 1;
                return Year + "-" + NextYear;
            }
        }
    }
}
