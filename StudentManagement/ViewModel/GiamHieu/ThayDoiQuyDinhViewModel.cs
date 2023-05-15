using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThayDoiQuyDinhViewModel : BaseViewModel
    {
        public bool everLoaded { get; set; }
        public ThayDoiQuyDinh ThayDoiQuyDinhWD { get; set; }
        public string QuyDinhQueries { get; set; }
        private ObservableCollection<StudentManagement.Model.QuiDinh> _danhSachQuyDinh;
        public ObservableCollection<StudentManagement.Model.QuiDinh> DanhSachQuyDinh { get => _danhSachQuyDinh; set { _danhSachQuyDinh = value; OnPropertyChanged(); } }
        public ICommand LoadData { get; set; }
        public ICommand FilterQuyDinh { get; set; }
        public ICommand EnableChange { get; set; }
        public ICommand ChangeRule { get; set; }
        public ThayDoiQuyDinhViewModel()
        {
            everLoaded = false;
            QuyDinhQueries = "";
            LoadThongTinCmb();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (everLoaded == false)
                {
                    ThayDoiQuyDinhWD = parameter as ThayDoiQuyDinh;
                    everLoaded = true;
                }

            });
            FilterQuyDinh = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null)
                {
                    QuiDinh selected = cmb.SelectedItem as QuiDinh;
                    string item = selected.TenQuiDinh;
                    if (item != null)
                    {
                        QuyDinhQueries = item.ToString();
                        LoadQuyDinhFromSelection();
                    }
                }
            });
            EnableChange = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThayDoiQuyDinhWD.tbGiaTri.IsEnabled = true;
                ThayDoiQuyDinhWD.btnXacNhan.IsEnabled = true;

            });
            ChangeRule = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb.SelectedItem != null)
                {
                    QuiDinh rule = cmb.SelectedItem as QuiDinh;
                    string tenqd = rule.TenQuiDinh;
                    string strvalue = ThayDoiQuyDinhWD.tbGiaTri.Text;
                    int value;
                    bool isInt = int.TryParse(strvalue, out value);
                    if (isInt)
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
                                string CmdString = "update QuiDinh SET GiaTri =" + value + " where TenQuiDinh = N'" + tenqd + "'";
                                SqlCommand cmd = new SqlCommand(CmdString, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            
                        }
                        ThayDoiQuyDinhWD.btnXacNhan.IsEnabled = false;
                        ThayDoiQuyDinhWD.tbGiaTri.IsEnabled = false;
                        MessageBox.Show("Thay đổi thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Giá trị không được rỗng và phải là một số nguyên!");
                    }

                }
                else
                {
                    MessageBox.Show("Hãy chọn quy định trước!");
                }

            });
        }
        public void LoadThongTinCmb()
        {
            DanhSachQuyDinh = new ObservableCollection<StudentManagement.Model.QuiDinh>();
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
                    string CmdString = "select * from QuiDinh";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            QuiDinh item = new QuiDinh();
                            item.MaQuiDinh = reader.GetInt32(0);
                            item.TenQuiDinh = reader.GetString(1);
                            item.GiaTri = reader.GetInt32(2);
                            DanhSachQuyDinh.Add(item);
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
        public void LoadQuyDinhFromSelection()
        {
            QuiDinh item = new QuiDinh();
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
                    string CmdString = "select * from QuiDinh where TenQuiDinh = N'" + QuyDinhQueries + "'";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item.MaQuiDinh = reader.GetInt32(0);
                            item.TenQuiDinh = reader.GetString(1);
                            item.GiaTri = reader.GetInt32(2);
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
            ThayDoiQuyDinhWD.tbGiaTri.Text = item.GiaTri.ToString();
        }
    }

}
