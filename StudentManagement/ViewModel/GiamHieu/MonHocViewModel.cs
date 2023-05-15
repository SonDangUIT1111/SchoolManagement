using StudentManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

        private ObservableCollection<StudentManagement.Model.MonHoc> _danhSachMonHoc;
        public ObservableCollection<StudentManagement.Model.MonHoc> DanhSachMonHoc { get => _danhSachMonHoc; set { _danhSachMonHoc = value; OnPropertyChanged(); } }

        private Model.MonHoc _monHocEditting;
        public Model.MonHoc MonHocEditting { get => _monHocEditting; set { _monHocEditting = value; OnPropertyChanged(); } }
        public ICommand LoadData { get; set; }
        public ICommand DeleteSubject { get; set; }
        public ICommand EditSubject { get; set; }
        public ICommand SubjectSearch { get; set; }
        public ICommand AddSubject { get; set; }
        public ICommand AddConfirm { get; set; }
        public ICommand EditEnable { get; set; }
        public ICommand LostFocusTxt { get; set; }


        public MonHocViewModel()
        {
            everLoaded = false;
            MonHocEditting = new Model.MonHoc();
            LoadThongTinMonHoc();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (everLoaded == false)
                {
                    MonHocWD = parameter as MonHoc;
                    everLoaded = true;
                }
            });
            DeleteSubject = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (MessageBox.Show("Bạn có muốn xoá môn học này không?", "Xoá môn học", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Model.MonHoc mh = parameter as Model.MonHoc;
                    if (mh != null)
                    {
                        try
                        {
                            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                            {
                                try 
                                { 
                                    con.Open(); 
                                } catch (Exception) 
                                { 
                                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); 
                                    return; 
                                }
                                string CmdString = "update MonHoc set ApDung = 0 where MaMon = " + mh.MaMon.ToString();
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            MessageBox.Show("Môn học này sẽ không được áp dụng trong dạy học nữa");
                            LoadThongTinMonHoc();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            });
            EditEnable = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                MonHocEditting = parameter as Model.MonHoc;
                MonHocWD.txtTenMH.Text = "";
                MonHocWD.txtTenMH.IsEnabled = false;
                MonHocWD.txtTenMH.IsEnabled = true;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Visible;
            });
            EditSubject = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (MonHocEditting != null )
                {
                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đổi tên", "Thông báo", MessageBoxButton.YesNo); 
                    if (result == MessageBoxResult.Yes)
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
                                string CmdString1 = "select * from MonHoc where TenMon = N'" + MonHocEditting.TenMon + "'";
                                SqlCommand cmd1 = new SqlCommand(CmdString1, con);
                                int count = Convert.ToInt32(cmd1.ExecuteScalar());
                                if (count > 0)
                                {
                                    MessageBox.Show("Tên môn học đã tồn tại, vui lòng chọn tên khác");
                                    con.Close();
                                    MonHocWD.txtTenMH.Text = "";
                                    MonHocWD.txtTenMH.IsEnabled = false;
                                    MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                    MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                    MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                                    return;
                                }

                                string CmdString = "update MonHoc set TenMon = N'"+MonHocEditting.TenMon + "' where MaMon = "+MonHocEditting.MaMon.ToString();
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MessageBox.Show("Đổi tên môn học thành công");
                                MonHocWD.txtTenMH.Text = "";
                                MonHocWD.txtTenMH.IsEnabled = false;
                                MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        LoadThongTinMonHoc();
                    }    
                }
            });
            SubjectSearch = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                DanhSachMonHoc.Clear();
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
                        string CmdString = "select * from MonHoc where TenMon like '%" + parameter.Text + "%' and ApDung = 1";
                        SqlCommand cmd = new SqlCommand(CmdString, con);
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
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
            AddSubject = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                MonHocWD.txtTenMH.IsEnabled = true;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnXacNhan.Visibility = Visibility.Visible;
                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
            });
            AddConfirm = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                string monhoc = parameter.Text;
                if (string.IsNullOrEmpty(monhoc))
                {
                    MessageBox.Show("Vui lòng nhập tên môn học!");
                    return;
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn thêm môn học này không?", "Thêm môn học", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                                string CmdString = "select * from MonHoc where TenMon = N'" + monhoc+"'";
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                int count = Convert.ToInt32(cmd.ExecuteScalar());   
                                if (count > 0 )
                                {
                                    MessageBox.Show("Tên môn học đã tồn tại, vui lòng chọn tên khác");
                                    con.Close();
                                    MonHocWD.txtTenMH.Text = "";
                                    MonHocWD.txtTenMH.IsEnabled = false;
                                    MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                    MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                    MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                                    return;
                                }

                                CmdString = "insert into MonHoc (TenMon, MaTruong, ApDung) values (N'" + monhoc + "', 1, 1)";
                                cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Thêm môn học thành công!");
                                con.Close();
                                MonHocWD.txtTenMH.Text = "";
                                MonHocWD.txtTenMH.IsEnabled = false;
                                MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                                MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                                MonHocWD.btnXacNhanDoiTen.Visibility = Visibility.Hidden;
                                LoadThongTinMonHoc();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            });
        }
        public void LoadThongTinMonHoc()
        {
            DanhSachMonHoc = new ObservableCollection<Model.MonHoc>();
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
                    string CmdString = "select * from MonHoc where ApDung = 1";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
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
