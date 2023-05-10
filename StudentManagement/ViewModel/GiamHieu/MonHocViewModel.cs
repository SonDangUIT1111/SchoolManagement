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
        public MonHoc MonHocWD { get; set; }

        private ObservableCollection<StudentManagement.Model.MonHoc> _danhSachMonHoc;
        public ObservableCollection<StudentManagement.Model.MonHoc> DanhSachMonHoc { get => _danhSachMonHoc; set { _danhSachMonHoc = value; OnPropertyChanged(); } }
        public MonHoc _gridSeletecdItem;
        public MonHoc GridSelectedItem
        {
            get { return _gridSeletecdItem; }
            set
            {
                _gridSeletecdItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadData { get; set; }
        public ICommand DeleteSubject { get; set; }
        public ICommand EditSubject { get; set; }
        public ICommand SubjectSearch { get; set; }
        public ICommand AddSubject { get; set; }
        public ICommand AddConfirm { get; set; }


        public MonHocViewModel()
        {
            LoadThongTinMonHoc();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                MonHocWD = parameter as MonHoc;
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
                                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                                string CmdString = "update MonHoc set ApDung = 0 where MaMon = " + mh.MaMon.ToString();
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            MessageBox.Show("Môn học này sẽ không được áp dụng trong dạy học nữa");
                            LoadThongTinMonHoc();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Đang có giáo viên được phân công giảng dạy môn này. Vui lòng đảm bảo môn không được giảng dạy mới có thể xóa được.");
                        }
                    }
                }
            });
            EditSubject = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Model.MonHoc mh = parameter as Model.MonHoc;
                if (mh != null )
                {
                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đổi tên", "Thông báo", MessageBoxButton.YesNo); 
                    if (result == MessageBoxResult.Yes)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            try
                            {
                                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
                                string CmdString = "update MonHoc set TenMon = "+mh.TenMon + " where MaMon = "+mh.MaMon.ToString();
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
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
                        try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                    }
                }
            });
            AddSubject = new RelayCommand<TextBox>((parameter) => { return true; }, (parameter) =>
            {
                MonHocWD.txtTenMH.IsEnabled = true;
                MonHocWD.btnThemMonHoc.Visibility = Visibility.Hidden;
                MonHocWD.btnXacNhan.Visibility = Visibility.Visible;
                MonHocWD.btnXacNhan.IsEnabled = true;
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
                                try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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
                                    MonHocWD.btnXacNhan.IsEnabled = false;
                                    return;
                                }


                                con.Close();
                                con.Open();
                                CmdString = "insert into MonHoc (TenMon, MaTruong, ApDung) values (N'" + monhoc + "', 1, 1)";
                                cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Thêm môn học thành công!");
                                con.Close();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                            }
                        }
                        MonHocWD.txtTenMH.Text = "";
                        MonHocWD.txtTenMH.IsEnabled = false;
                        MonHocWD.btnThemMonHoc.Visibility = Visibility.Visible;
                        MonHocWD.btnXacNhan.Visibility = Visibility.Hidden;
                        MonHocWD.btnXacNhan.IsEnabled = false;
                        LoadThongTinMonHoc();
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
                    try { con.Open(); } catch (Exception) { MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền"); return; }
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
                catch (Exception)
                {
                    MessageBox.Show("Lỗi mạng, vui lòng kiểm tra lại đường truyền");
                }
            }
        }
    }
}
