using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using StudentManagement.Model;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class BaoCaoViewModel:BaseViewModel
    {
        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string MonHocQueries { get; set; }
        public string LopQueries { get; set; }
        private ObservableCollection<StudentManagement.Model.BaoCaoMon> _danhSachBaoCaoMon;
        public ObservableCollection<StudentManagement.Model.BaoCaoMon> DanhSachBaoCaoMon
        {
            get => _danhSachBaoCaoMon;
            set { _danhSachBaoCaoMon = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _nienKhoaComboBox;
        public ObservableCollection<string> NienKhoaComboBox
        {
            get => _nienKhoaComboBox;
            set { _nienKhoaComboBox = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> _monComboBox;
        public ObservableCollection<string> MonComboBox
        {
            get => _monComboBox;
            set { _monComboBox = value; OnPropertyChanged(); }
        }

        public BaoCaoViewModel()
        {
            LoadComboboxData();
        }

        public void LoadComboboxData()
        {
            NienKhoaComboBox = new ObservableCollection<string>();
            MonComboBox = new ObservableCollection<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                con.Open();
                string cmdString = "select distinct NienKhoa from BaoCaoMon";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NienKhoaComboBox.Add(reader.GetString(0)); 
                        if(string.IsNullOrEmpty(NienKhoaQueries))
                        {
                            NienKhoaQueries = reader.GetString(0);                          
                        }
                    }
                    reader.NextResult();
                }
                con.Close();

                con.Open();
                cmdString = "select distinct TenMon from BaoCaoMon";
                cmd = new SqlCommand(cmdString, con);
                reader = cmd.ExecuteReader();
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MonComboBox.Add(reader.GetString(0));
                        if (string.IsNullOrEmpty(MonHocQueries))
                        {
                            MonHocQueries = reader.GetString(0);
                        }
                    }
                    reader.NextResult();
                }
                con.Close();
            }
        }
    }
}
