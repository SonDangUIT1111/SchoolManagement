using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
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
        private ObservableCollection<Khoi> _khoiCmb;
        public ObservableCollection<Khoi> KhoiCmb { get { return _khoiCmb;}  set { _khoiCmb = value;OnPropertyChanged(); } }
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
            });

            AddClass = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (String.IsNullOrEmpty(ThemLopHocWD.ClassName.Text) || String.IsNullOrEmpty(ThemLopHocWD.AcademyYear.Text) 
                    || ThemLopHocWD.KhoiCmb.SelectedIndex == -1)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng điền đầy đủ thông tin";
                    MB.ShowDialog();
                }
                else using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
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
                        Khoi item = ThemLopHocWD.KhoiCmb.SelectedItem as Khoi;
                        MaKhoi = item.MaKhoi.ToString();
                        string cmdString = "INSERT INTO Lop(TenLop, MaKhoi,NienKhoa) VALUES ('" 
                                            + ThemLopHocWD.ClassName.Text + "', " + MaKhoi + ", '" 
                                            + ThemLopHocWD.AcademyYear.Text + "')";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        messageBoxSuccessful.ShowDialog();
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
                    string cmdString = "SELECT DISTINCT MaKhoi,Khoi FROM Khoi";
                    SqlCommand cmd = new SqlCommand(cmdString, con);
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
                                ThemLopHocWD.KhoiCmb.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();

                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                    return;
                };
            }
        }
    }
}
