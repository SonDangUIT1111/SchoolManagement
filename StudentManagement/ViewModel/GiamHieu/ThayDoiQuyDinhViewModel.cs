using Microsoft.Win32;
using StudentManagement.Model;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace StudentManagement.ViewModel.GiamHieu
{
    public class ThayDoiQuyDinhViewModel: BaseViewModel
    {
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
            QuyDinhQueries = "";
            LoadThongTinCmb();
            LoadData = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ThayDoiQuyDinhWD = parameter as ThayDoiQuyDinh;
                //ThayDoiQuyDinhWD.cmbQuyDinh.SelectedIndex = 0;

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
                    //MessageBox.Show("Co quy dinh");
                    QuiDinh rule = cmb.SelectedItem as QuiDinh;
                    string tenqd = rule.TenQuiDinh;
                    //MessageBox.Show(tenqd);
                    string strvalue = ThayDoiQuyDinhWD.tbGiaTri.Text;
                    //MessageBox.Show(strvalue);
                    int value;
                    bool isInt = int.TryParse(strvalue, out value);
                    if (isInt)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
                        {
                            con.Open();
                            string CmdString = "update QuiDinh SET GiaTri =" + value + " where TenQuiDinh = N'" + tenqd + "'";
                            //MessageBox.Show(CmdString);
                            SqlCommand cmd = new SqlCommand(CmdString, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        ThayDoiQuyDinhWD.btnXacNhan.IsEnabled = false;
                        MessageBox.Show("Thay đổi thành công!");
                    } else
                    {
                        MessageBox.Show("Giá trị không được rỗng và phải là một số nguyên!");
                    }

                } else
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
                con.Open();
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
        }
        public void LoadQuyDinhFromSelection()
        {
            QuiDinh item = new QuiDinh();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string CmdString = "select * from QuiDinh where TenQuiDinh = N'" + QuyDinhQueries +"'";
                //MessageBox.Show(CmdString);
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
            ThayDoiQuyDinhWD.tbGiaTri.Text= item.GiaTri.ToString();
        }
    }
    
}
