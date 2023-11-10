using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MonHoc = StudentManagement.Views.GiamHieu.MonHoc;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class MonHocViewModel : BaseViewModel
    {
        public bool everLoaded { get; set; }
        public MonHoc MonHocWD { get; set; }
        public bool IsLoadAll { get; set; } = false;

        private ObservableCollection<StudentManagement.Model.MonHoc> _danhSachMonHoc;
        public ObservableCollection<StudentManagement.Model.MonHoc> DanhSachMonHoc { get => _danhSachMonHoc; set { _danhSachMonHoc = value; OnPropertyChanged(); } }

        private Model.MonHoc _monHocEditting;
        public Model.MonHoc MonHocEditting { get => _monHocEditting; set { _monHocEditting = value; OnPropertyChanged(); } }

        private bool _dataGridVisibility;

        public bool DataGridVisibility{get{return _dataGridVisibility;}set { _dataGridVisibility = value; OnPropertyChanged(); }
        }

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility{get{return _progressBarVisibility;}set { _progressBarVisibility = value; OnPropertyChanged(); }}

        private readonly ISqlConnectionWrapper sqlConnection;

        public MonHocViewModel(ISqlConnectionWrapper sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public ICommand LoadData { get; set; }
        public ICommand DeleteSubject { get; set; }
        public ICommand EditSubject { get; set; }
        public ICommand SubjectSearch { get; set; }
        public ICommand AddSubject { get; set; }
        public ICommand AddConfirm { get; set; }
        public ICommand SubjectSearchAll { get; set; }

        public ICommand EditEnable { get; set; }
        public ICommand LostFocusTxt { get; set; }


        public MonHocViewModel()
        {
            everLoaded = false;
            MonHocEditting = new Model.MonHoc();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    MonHocWD = parameter as MonHoc;
                    DataGridVisibility = false;
                    ProgressBarVisibility = true;
                    await LoadThongTinMonHoc();
                    DataGridVisibility = true;
                    ProgressBarVisibility = false;
                    everLoaded = true;
                }
            });
            DeleteSubject = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có muốn xóa môn học này không?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true)
                {
                    Model.MonHoc mh = parameter as Model.MonHoc;
                    if (mh != null)
                    {
                        try
                        {
                            DeleteMonHoc(mh);
                        }
                        catch (Exception)
                        {
                            MessageBoxFail messageBoxFail = new MessageBoxFail();
                            messageBoxFail.ShowDialog();
                            return;
                        }
                        MessageBoxOK MB = new MessageBoxOK();
                        var datamb = MB.DataContext as MessageBoxOKViewModel;
                        datamb.Content = "Môn học này sẽ không được áp dụng trong dạy học nữa";
                        MB.ShowDialog();
                        DataGridVisibility = false;
                        ProgressBarVisibility = true;
                        await LoadThongTinMonHoc();
                        DataGridVisibility = true;
                        ProgressBarVisibility = false;
                    }
                }
            });
            EditEnable = new RelayCommand<object>((parameter) => { return true; },  (parameter) =>
            {
                MonHocEditting = parameter as Model.MonHoc;
                MonHocWD.txtTenMH.Text = "";
                MonHocWD.txtTenMH.IsEnabled = false;
                MonHocWD.txtTenMH.IsEnabled = true;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Visible;
            });
            EditSubject = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (MonHocEditting != null )
                {
                    MessageBoxYesNo wd = new MessageBoxYesNo();

                    var data = wd.DataContext as MessageBoxYesNoViewModel;
                    data.Title = "Xác nhận!";
                    data.Question = "Bạn có chắc chắn muốn đổi tên";
                    wd.ShowDialog();

                    var result = wd.DataContext as MessageBoxYesNoViewModel;
                    if (result.IsYes == true)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            try
                            {
                                int count = KiemTraTonTaiMonHoc(MonHocWD.txtTenMH.Text);
                                if (count > 0)
                                {
                                    MessageBoxOK MB = new MessageBoxOK();
                                    var datamb = MB.DataContext as MessageBoxOKViewModel;
                                    datamb.Content = "Tên môn học đã tồn tại, vui lòng chọn tên khác ";
                                    MB.ShowDialog();
                                    con.Close();
                                    MonHocWD.txtTenMH.Text = "";
                                    MonHocWD.txtTenMH.IsEnabled = false;
                                    MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                    MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                    MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                                    return;
                                }
                                EditTenMon(MonHocWD.txtTenMH.Text,MonHocEditting.MaMon.ToString());
                                MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                                messageBoxSuccessful.ShowDialog();
                                MonHocWD.txtTenMH.Text = "";
                                MonHocWD.txtTenMH.IsEnabled = false;
                                MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                            }
                            catch (Exception)
                            {
                                MessageBoxFail messageBoxFail = new MessageBoxFail();
                                messageBoxFail.ShowDialog();
                                return;
                            }
                            DataGridVisibility = false;
                            ProgressBarVisibility = true;
                            await LoadThongTinMonHoc();
                            DataGridVisibility = true;
                            ProgressBarVisibility = false;
                        }
                    }    
                }
            });
            SubjectSearch = new RelayCommand<TextBox>((parameter) => { return true; },async (parameter) =>
            {
                DanhSachMonHoc.Clear();
                ProgressBarVisibility = true;
                try
                {
                    TraCuuMonHoc(parameter.Text);
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                    return;
                }

                ProgressBarVisibility = false;
            });
            AddSubject = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                MonHocWD.txtTenMH.IsEnabled = true;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnXacNhan.Visibility = Visibility.Visible;
                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
            });
            AddConfirm = new RelayCommand<TextBox>((parameter) => { return true; },async (parameter) =>
            {
                string monhoc = parameter.Text;
                if (string.IsNullOrEmpty(monhoc))
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var datamb = MB.DataContext as MessageBoxOKViewModel;
                    datamb.Content = "Vui lòng nhập tên môn học.";
                    MB.ShowDialog();
                    return;
                }
                else
                {
                    MessageBoxYesNo wd = new MessageBoxYesNo();

                    var data = wd.DataContext as MessageBoxYesNoViewModel;
                    data.Title = "Xác nhận!";
                    data.Question = "Bạn có muốn thêm môn học này không?";
                    wd.ShowDialog();

                    var result = wd.DataContext as MessageBoxYesNoViewModel;
                    if (result.IsYes == true)
                    {
                        try
                        {
                            int count = KiemTraTonTaiMonHoc(monhoc);   
                            if (count > 0 )
                            {
                                MessageBoxOK MB = new MessageBoxOK();
                                var datamb = MB.DataContext as MessageBoxOKViewModel;
                                datamb.Content = "Tên môn học đã tồn tại, vui lòng chọn tên khác";
                                MB.ShowDialog();
                                MonHocWD.txtTenMH.Text = "";
                                MonHocWD.txtTenMH.IsEnabled = false;
                                MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                                return;
                            }

                            ThemMonHocMoi(monhoc);
                            MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                            messageBoxSuccessful.ShowDialog();
                            MonHocWD.txtTenMH.Text = "";
                            MonHocWD.txtTenMH.IsEnabled = false;
                            MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                            MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                            MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                            DataGridVisibility = false;
                            ProgressBarVisibility = true;
                            await LoadThongTinMonHoc();
                            DataGridVisibility = true;
                            ProgressBarVisibility = false;
                        }
                        catch (Exception)
                        {
                            MessageBoxFail messageBoxFail = new MessageBoxFail();
                            messageBoxFail.ShowDialog();
                        }
                    }
                }
            });
            SubjectSearchAll = new RelayCommand<TextBox>((parameter) => { return true; }, async (parameter) =>
            {
                DanhSachMonHoc.Clear();
                IsLoadAll = true;
                DataGridVisibility = false;
                ProgressBarVisibility = true;
                await LoadThongTinMonHoc();
                DataGridVisibility = true;
                ProgressBarVisibility = false;
            });
        }
        public async Task LoadThongTinMonHoc()
        {
            DanhSachMonHoc = new ObservableCollection<Model.MonHoc>();

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
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return;
                    }

                    string CmdString = "select * from MonHoc where ApDung = 1";
                    SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        StudentManagement.Model.MonHoc monhoc = new StudentManagement.Model.MonHoc();
                        monhoc.MaMon = reader.GetInt32(0);
                        monhoc.TenMon = reader.GetString(1);
                        DanhSachMonHoc.Add(monhoc);
                    }

                    sqlConnectionWrapper.Close();
                }
                catch (Exception )
                {
                    //MessageBoxFail messageBoxFail = new MessageBoxFail();
                    //messageBoxFail.ShowDialog();
                }
            }
        }
        public void DeleteMonHoc(Model.MonHoc mh)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
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
                string CmdString = "update MonHoc set ApDung = 0 where MaMon = " + mh.MaMon.ToString();
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                cmd.ExecuteNonQuery();
                sqlConnectionWrapper.Close();
            }
        }

        public int KiemTraTonTaiMonHoc(string tenmon)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrapper.Open();
                string CmdString1 = "select * from MonHoc where TenMon = N'" + tenmon + "'";
                SqlCommand cmd1 = new SqlCommand(CmdString1, sqlConnectionWrapper.GetSqlConnection());
                int count = Convert.ToInt32(cmd1.ExecuteScalar());
                sqlConnectionWrapper.Close();
                return count;
            }
        }

        public void EditTenMon(string tenmon,string mamon)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
            {
                sqlConnectionWrapper.Open();
                string CmdString = "update MonHoc set TenMon = N'" + tenmon + "' where MaMon = " + mamon;
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                cmd.ExecuteNonQuery();
                sqlConnectionWrapper.Close();
            }
        }

        public void TraCuuMonHoc(string searchWord)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
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
                string CmdString = "select * from MonHoc where TenMon like N'%" + searchWord + "%' and ApDung = 1";
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentManagement.Model.MonHoc monhoc = new StudentManagement.Model.MonHoc();
                        monhoc.MaMon = reader.GetInt32(0);
                        monhoc.TenMon = reader.GetString(1);
                        DanhSachMonHoc.Add(monhoc);
                    }
                    reader.NextResult();
                }
                sqlConnectionWrapper.Close();
            }
        }

        public void ThemMonHocMoi(string monhoc)
        {
            using (var sqlConnectionWrapper = new SqlConnectionWrapper(ConnectionString.connectionString))
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
                string CmdString = "insert into MonHoc (TenMon, MaTruong, ApDung) values (N'" + monhoc + "', 1, 1)";
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnectionWrapper.GetSqlConnection());
                cmd.ExecuteNonQuery();
                sqlConnectionWrapper.Close();
            }
        }
    }
}
