using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using StudentManagement.Model;
using StudentManagement.ViewModel.MessageBox;
using StudentManagement.Views.GiaoVien;
using StudentManagement.Views.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace StudentManagement.ViewModel.GiaoVien
{
    public class HeThongBangDiemViewModel : BaseViewModel
    {

        // khai báo biến
        public bool everLoaded { get; set; }
        private int _idUser;
        public int IdUser { get { return _idUser; } set { _idUser = value; } }
        private bool _justReadOnly;
        public bool JustReadOnly { get { return _justReadOnly; } set { _justReadOnly = value; OnPropertyChanged(); } }
        private bool _canUserEdit;
        public bool CanUserEdit { get { return _canUserEdit; } set { _canUserEdit = value; OnPropertyChanged(); } }
        public string NienKhoaQueries { get; set; }
        public int HocKyQueries { get; set; }
        public string KhoiQueries { get; set; }
        public string LopQueries { get; set; }
        public string MonHocQueries { get; set; }

        public int DiemDat { get; set; }

        private Visibility _linevisibility;
        public Visibility LineVisibility { get { return _linevisibility; } set { _linevisibility = value; OnPropertyChanged(); } }
        private Visibility _colvisibility;
        public Visibility ColVisibility { get { return _colvisibility; } set { _colvisibility = value; OnPropertyChanged(); } }

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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        public HeThongBangDiem HeThongBangDiemWD { get; set; }
        private ObservableCollection<StudentManagement.Model.HeThongDiem> _danhSachDiem;
        public ObservableCollection<StudentManagement.Model.HeThongDiem> DanhSachDiem { get => _danhSachDiem; set { _danhSachDiem = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _nienKhoaCmb;
        public ObservableCollection<string> NienKhoaCmb { get => _nienKhoaCmb; set { _nienKhoaCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Lop> _lopDataCmb;
        public ObservableCollection<Lop> LopDataCmb { get => _lopDataCmb; set { _lopDataCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<Khoi> _khoiDataCmb;
        public ObservableCollection<Khoi> KhoiDataCmb { get => _khoiDataCmb; set { _khoiDataCmb = value; OnPropertyChanged(); } }
        private ObservableCollection<MonHoc> _monDataCmb;
        public ObservableCollection<MonHoc> MonDataCmb { get => _monDataCmb; set { _monDataCmb = value; OnPropertyChanged(); } }

        // khai báo ICommand
        public ICommand LoadWindow { get; set; }
        public ICommand MouseEnterComboBox { get; set; }
        public ICommand MouseLeaveComboBox { get; set; }
        public ICommand FilterNienKhoa { get; set; }
        public ICommand FilterHocKy { get; set; }
        public ICommand FilterKhoi { get; set; }
        public ICommand FilterLop { get; set; }
        public ICommand FilterMonHoc { get; set; }
        public ICommand LuuDiem { get; set; }
        public ICommand XuatDiem { get; set; }
        public HeThongBangDiemViewModel()
        {
            everLoaded = false;
            IdUser = 100000;
            JustReadOnly = true;
            CanUserEdit = false;
            DanhSachDiem = new ObservableCollection<HeThongDiem>();
            NienKhoaCmb = new ObservableCollection<string>();
            MonDataCmb = new ObservableCollection<MonHoc>();
            LopDataCmb = new ObservableCollection<Lop>();
            KhoiDataCmb = new ObservableCollection<Khoi>();
            LoadWindow = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                if (everLoaded == false)
                {
                    HeThongBangDiemWD = parameter as HeThongBangDiem;
                    LoadDuLieuComboBox();
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    await LoadDanhSachBangDiem();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                    XacDinhQuyenHan();
                    everLoaded = true;
                }
            });
            MouseEnterComboBox = new RelayCommand<ComboBox>((parameter) => { return true; }, (parameter) =>
            {
                parameter.Focus();
            });
            MouseLeaveComboBox = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                HeThongBangDiemWD.btnTrick.Focus();
            });
            FilterNienKhoa = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    NienKhoaQueries = cmb.SelectedItem.ToString();
                    FilterLopFromSelection();
                }
            });
            FilterHocKy = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    if (cmb.SelectedItem.ToString().Contains("1"))
                        HocKyQueries = 1;
                    else
                        HocKyQueries = 2;
                    XacDinhQuyenHan();
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    await LoadDanhSachBangDiem();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                }
            });
            FilterKhoi = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Khoi item = cmb.SelectedItem as Khoi;
                    KhoiQueries = item.MaKhoi.ToString();
                    FilterLopFromSelection();
                }
            });
            FilterLop = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    Lop item = cmb.SelectedItem as Lop;
                    if (item != null)
                    {
                        LopQueries = item.MaLop.ToString();
                        XacDinhQuyenHan();
                        ProgressBarVisibility = true;
                        DataGridVisibility = false;
                        await LoadDanhSachBangDiem();
                        ProgressBarVisibility = false;
                        DataGridVisibility = true;
                    }
                }
            });
            FilterMonHoc = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                ComboBox cmb = parameter as ComboBox;
                if (cmb != null && cmb.SelectedItem != null)
                {
                    MonHoc item = cmb.SelectedItem as MonHoc;
                    MonHocQueries = item.MaMon.ToString();
                    XacDinhQuyenHan();
                    ProgressBarVisibility = true;
                    DataGridVisibility = false;
                    await LoadDanhSachBangDiem();
                    ProgressBarVisibility = false;
                    DataGridVisibility = true;
                }
            });
            LuuDiem = new RelayCommand<object>((parameter) => { return true; }, async (parameter) =>
            {
                MessageBoxYesNo wd = new MessageBoxYesNo();

                var data = wd.DataContext as MessageBoxYesNoViewModel;
                data.Title = "Xác nhận!";
                data.Question = "Bạn có chắc chắn muốn lưu. (Lưu ý, những học sinh có điểm ở trạng thái chốt điểm sẽ không được lưu?";
                wd.ShowDialog();

                var result = wd.DataContext as MessageBoxYesNoViewModel;
                if (result.IsYes == true)
                {
                    if (DanhSachDiem[0].TrangThai == true)
                    {
                        MessageBoxOK MB = new MessageBoxOK();
                        var datamb = MB.DataContext as MessageBoxOKViewModel;
                        datamb.Content = "Danh sách điểm này đã được chốt, không thể sửa.";
                        MB.ShowDialog();
                        ProgressBarVisibility = true;
                        DataGridVisibility = false;
                        await LoadDanhSachBangDiem();
                        ProgressBarVisibility = false;
                        DataGridVisibility = true;
                        return;
                    }
                    if (KiemTraDiemHopLe() == false)
                    {
                        MessageBoxOK MB = new MessageBoxOK();
                        var datamb = MB.DataContext as MessageBoxOKViewModel;
                        datamb.Content = "Điểm nhập không hợp lệ, vui lòng kiểm tra lại.";
                        MB.ShowDialog();
                        return;
                    }
                    LuuBangDiem();
                }
            });
            XuatDiem = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                Khoi khoi = HeThongBangDiemWD.cmbKhoi.SelectedItem as Khoi;
                Lop lop = HeThongBangDiemWD.cmbLop.SelectedItem as Lop;
                MonHoc monHoc = HeThongBangDiemWD.cmbMonHoc.SelectedItem as MonHoc;
                if (HeThongBangDiemWD.cmbNienKhoa == null || HeThongBangDiemWD.cmbHocKy == null || khoi == null || lop == null || monHoc == null)
                {
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Vui lòng chọn đủ thông tin Niên khóa, Học kỳ, Khối, Lớp và Môn học";
                    MB.ShowDialog();
                    return;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                // Set the file dialog properties
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save Excel File";
                saveFileDialog.FileName = "BangDiem";

                // Show the SaveFileDialog and check if the user clicked the Save button
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Create a new Excel application
                    Excel.Application excelApp = new Excel.Application();

                    // Create a new workbook
                    Excel.Workbook workbook = excelApp.Workbooks.Add();

                    // Create a new worksheet
                    Excel.Worksheet worksheet = workbook.ActiveSheet;

                    //Set header kèm thông tin lớp
                    worksheet.Cells[1, 1] = "BẢNG ĐIỂM LỚP " + lop.TenLop;
                    Excel.Range headerRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 8]];
                    headerRange.Merge(true);
                    worksheet.Cells[2, 1] = "Niên khóa: ";
                    worksheet.Cells[2, 2] = HeThongBangDiemWD.cmbNienKhoa.SelectedValue;
                    worksheet.Cells[3, 1] = "Học kỳ: ";
                    worksheet.Cells[3, 2] = HeThongBangDiemWD.cmbHocKy.SelectedValue;
                    worksheet.Cells[4, 1] = "Khối: ";
                    worksheet.Cells[4, 2] = "'" + khoi.TenKhoi.ToString();
                    worksheet.Cells[5, 1] = "Lớp: ";
                    worksheet.Cells[5, 2] = lop.TenLop;
                    worksheet.Cells[5, 3] = "Mã lớp: ";
                    worksheet.Cells[5, 4] = "'" + lop.MaLop.ToString();
                    worksheet.Cells[6, 1] = "Môn học: ";
                    worksheet.Cells[6, 2] = monHoc.TenMon;
                    worksheet.Cells[6, 3] = "Mã môn học: ";
                    worksheet.Cells[6, 4] = "'" + monHoc.MaMon.ToString();

                    // Set the column headers
                    worksheet.Cells[7, 1] = "STT";
                    worksheet.Cells[7, 2] = "Mã học sinh";
                    worksheet.Cells[7, 3] = "Họ và tên";
                    worksheet.Cells[7, 4] = "Điểm 15 phút";
                    worksheet.Cells[7, 5] = "Điểm 1 tiết";
                    worksheet.Cells[7, 6] = "Điểm TB";
                    worksheet.Cells[7, 7] = "Xếp loại";
                    worksheet.Cells[7, 8] = "Trạng thái";


                    int row = 8;
                    int idx = 1;
                    Excel.Range dataRange = worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 8]];
                    // Iterate through the danhSachDiem and populate the worksheet
                    foreach (var diem in DanhSachDiem)
                    {
                        worksheet.Cells[row, 1] = idx;
                        worksheet.Cells[row, 2] = "'" + diem.MaHocSinh.ToString();
                        worksheet.Cells[row, 3] = diem.TenHocSinh;
                        worksheet.Cells[row, 4] = diem.Diem15Phut;
                        worksheet.Cells[row, 5] = diem.Diem1Tiet;
                        worksheet.Cells[row, 6] = diem.DiemTB;
                        worksheet.Cells[row, 7] = (diem.XepLoai == true) ? "Đạt" : "Không đạt";
                        worksheet.Cells[row, 8] = (diem.TrangThai == true) ? "Đã chốt" : "Chưa chốt";
                        dataRange = worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 8]];
                        dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        dataRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
                        row++; idx++;
                    }
                    //Format chhung
                    worksheet.Columns.ColumnWidth = 12;
                    worksheet.Rows.RowHeight = 20;
                    worksheet.Columns[3].ColumnWidth = 20;
                    Excel.Range cellsRange = worksheet.UsedRange;
                    cellsRange.Font.Name = "Times New Roman";
                    cellsRange.Font.Size = 12;
                    //Format header
                    headerRange.Font.Bold = true;
                    headerRange.Font.Size = 20;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    worksheet.Rows[1].RowHeight = 40;
                    //Format Bang
                    Excel.Range classRange = worksheet.Range[worksheet.Cells[7, 1], worksheet.Cells[7, 8]];
                    classRange.Font.Bold = true;
                    classRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    classRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    classRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception)
                        {
                            MessageBoxOK MB1 = new MessageBoxOK();
                            var datamb = MB1.DataContext as MessageBoxOKViewModel;
                            datamb.Content = "File được lưu mới đang được bật, vui lòng tắt file và thử lại";
                            MB1.ShowDialog();
                            return;
                        }
                    }
                    // Save the workbook at the chosen path
                    workbook.SaveAs(filePath);

                    // Close the workbook and release the resources
                    workbook.Close();
                    excelApp.Quit();

                    // Release COM objects to avoid memory leaks
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(headerRange);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(classRange);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataRange);

                    // Display a message box indicating the export is complete
                    MessageBoxOK MB = new MessageBoxOK();
                    var data = MB.DataContext as MessageBoxOKViewModel;
                    data.Content = "Xuất danh sách điểm thành công!";
                    MB.ShowDialog();
                }
            });
        }
        public void LoadDuLieuComboBox()
        {
            NienKhoaQueries = KhoiQueries = LopQueries = MonHocQueries = "";
            HocKyQueries = 1;
            KhoiDataCmb.Clear();
            LopDataCmb.Clear();
            MonDataCmb.Clear();
            NienKhoaCmb.Clear();
            HeThongBangDiemWD.cmbHocKy.Items.Add("Học kỳ 1");
            HeThongBangDiemWD.cmbHocKy.Items.Add("Học kỳ 2");
            HeThongBangDiemWD.cmbHocKy.SelectedIndex = 0;
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
                    string CmdString = "select distinct NienKhoa from Lop";
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NienKhoaCmb.Add(reader.GetString(0));
                            if (String.IsNullOrEmpty(NienKhoaQueries))
                            {
                                NienKhoaQueries = reader.GetString(0);
                                HeThongBangDiemWD.cmbNienKhoa.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();

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
                    CmdString = "select distinct MaKhoi,Khoi from Khoi";
                    cmd = new SqlCommand(CmdString, con);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Khoi item = new Khoi();
                            item.MaKhoi = reader.GetInt32(0);
                            item.TenKhoi = reader.GetString(1);
                            KhoiDataCmb.Add(item);
                            if (String.IsNullOrEmpty(KhoiQueries))
                            {
                                KhoiQueries = reader.GetInt32(0).ToString();
                                HeThongBangDiemWD.cmbKhoi.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();

                    if (!String.IsNullOrEmpty(NienKhoaQueries))
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
                        CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
                        cmd = new SqlCommand(CmdString, con);
                        reader = cmd.ExecuteReader();

                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Lop item = new Lop();
                                item.MaLop = reader.GetInt32(0);
                                item.TenLop = reader.GetString(1);
                                LopDataCmb.Add(item);
                                if (String.IsNullOrEmpty(LopQueries))
                                {
                                    LopQueries = reader.GetInt32(0).ToString();
                                    HeThongBangDiemWD.cmbLop.SelectedIndex = 0;
                                }
                            }
                            reader.NextResult();
                        }
                        con.Close();
                    }

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
                    CmdString = "select MaMon,TenMon from MonHoc";
                    cmd = new SqlCommand(CmdString, con);
                    reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MonHoc item = new MonHoc();
                            item.MaMon = reader.GetInt32(0);
                            item.TenMon = reader.GetString(1);
                            MonDataCmb.Add(item);
                            if (String.IsNullOrEmpty(MonHocQueries))
                            {
                                MonHocQueries = reader.GetInt32(0).ToString();
                                HeThongBangDiemWD.cmbMonHoc.SelectedIndex = 0;
                            }
                        }
                        reader.NextResult();
                    }
                    con.Close();
                }
                catch (Exception)
                {
                }
            }

        }

        public async Task LoadDanhSachBangDiem()
        {
            DanhSachDiem.Clear();
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    try
                    {
                        await con.OpenAsync();
                    }
                    catch (Exception)
                    {
                        MessageBoxFail messageBoxFail = new MessageBoxFail();
                        messageBoxFail.ShowDialog();
                        return;
                    }

                    string wherecommand = "";
                    if (!String.IsNullOrEmpty(NienKhoaQueries))
                    {
                        wherecommand = wherecommand + " where NienKhoa = '" + NienKhoaQueries +
                                        "' and ht.MaLop = " + LopQueries + " and ht.HocKy = " + HocKyQueries.ToString();
                    }
                    if (!String.IsNullOrEmpty(MonHocQueries))
                    {
                        wherecommand = wherecommand + " and MaMon = " + MonHocQueries;
                    }
                    string CmdString = "select ht.MaDiem,ht.HocKy,ht.MaLop,MaMon,ht.MaHocSinh,Diem15Phut,Diem1Tiet,DiemTrungBinh,XepLoai,TrangThai,TenHocSinh " +
                                        " from HeThongDiem ht join Lop l on ht.MaLop = l.MaLop join HocSinh hs on ht.MaHocSinh = hs.MaHocSinh " + wherecommand;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem
                            {
                                MaDiem = reader.GetInt32(0),
                                HocKy = reader.GetInt32(1),
                                MaLop = reader.GetInt32(2),
                                MaMon = reader.GetInt32(3),
                                MaHocSinh = reader.GetInt32(4),
                                TrangThai = reader.GetBoolean(9),
                                TenHocSinh = reader.GetString(10),
                            };
                            try
                            {
                                diem.Diem15Phut = (decimal)reader.GetDecimal(5);
                                diem.Diem1Tiet = (decimal)reader.GetDecimal(6);
                                diem.DiemTB = (decimal)reader.GetDecimal(7);
                                diem.XepLoai = reader.GetBoolean(8);
                            }
                            catch (Exception)
                            {

                            }
                            DanhSachDiem.Add(diem);
                        }
                        await reader.NextResultAsync();
                    }
                    con.Close();
                }
                catch (Exception)
                {
                }
            }
            HeThongBangDiemWD.cmbLop.Focus();
            HeThongBangDiemWD.btnTrick.Focus();
        }

        public void FilterLopFromSelection()
        {
            LopDataCmb.Clear();
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
                    string CmdString = "select MaLop,TenLop from Lop where NienKhoa = '" + NienKhoaQueries + "' and MaKhoi = " + KhoiQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StudentManagement.Model.Lop item = new StudentManagement.Model.Lop
                            {
                                MaLop = reader.GetInt32(0),
                                TenLop = reader.GetString(1),
                            };
                            LopDataCmb.Add(item);
                        }
                        reader.NextResult();
                    }
                    con.Close();
                    HeThongBangDiemWD.cmbLop.SelectedIndex = 0;
                }
                catch (Exception)
                {
                }
            }
        }
        public void XacDinhQuyenHan()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {
                try
                {
                    string CmdString = "";
                    int checkUser = 0;
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
                    CmdString = "Select count(*) from PhanCongGiangDay where MaGiaoVienPhuTrach = " + IdUser.ToString()
                            + " and MaLop = " + LopQueries + " and MaMon = " + MonHocQueries;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    checkUser = Convert.ToInt32(cmd.ExecuteScalar());

                    if (checkUser > 0)
                    {
                        HeThongBangDiemWD.tbThongBaoChan.Visibility = Visibility.Hidden;
                        HeThongBangDiemWD.tbThongBaoQuyen.Visibility = Visibility.Visible;
                        JustReadOnly = false;
                        CanUserEdit = true;
                        LineVisibility = Visibility.Visible;
                        ColVisibility = Visibility.Hidden;
                    }
                    else
                    {
                        HeThongBangDiemWD.tbThongBaoChan.Visibility = Visibility.Visible;
                        HeThongBangDiemWD.tbThongBaoQuyen.Visibility = Visibility.Hidden;
                        JustReadOnly = true;
                        CanUserEdit = false;
                        LineVisibility = Visibility.Hidden;
                        ColVisibility = Visibility.Visible;
                    }
                    con.Close();
                }
                catch (Exception)
                {
                }
            }

        }
        public void LuuBangDiem()
        {

            using (SqlConnection con = new SqlConnection(ConnectionString.connectionString))
            {   //Lay diem dat tu Qui Dinh
                try
                {
                    string CmdString = "select GiaTri from QuiDinh where MaQuiDinh = 4";
                    SqlCommand cmd;
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
                    cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DiemDat = reader.GetInt32(0);
                        }
                        reader.NextResult();
                    }
                    con.Close();

                }
                catch (Exception)
                {
                    MessageBoxFail messageboxfail = new MessageBoxFail();
                    messageboxfail.ShowDialog();
                }

                //Update diem tren HeThongDiem
                try
                {
                    string CmdString = "";
                    SqlCommand cmd;
                    int madiem = 0;
                    decimal diem15phut, diem1tiet, dtb = 0;
                    int xeploai = 0;
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
                    for (int i = 0; i < DanhSachDiem.Count; i++)
                    {
                        madiem = DanhSachDiem[i].MaDiem;
                        diem15phut = (decimal)DanhSachDiem[i].Diem15Phut;
                        diem1tiet = (decimal)DanhSachDiem[i].Diem1Tiet;
                        dtb = (diem15phut + diem1tiet) / 2;
                        if (dtb >= DiemDat)
                        {
                            xeploai = 1;
                        }
                        else xeploai = 0;
                        CmdString = "update HeThongDiem "
                                   + "set Diem15Phut = " + diem15phut.ToString() + ", Diem1Tiet = " + diem1tiet.ToString()
                                   + ", DiemTrungBinh = " + dtb.ToString() + " ,XepLoai = " + xeploai.ToString()
                                   + " where MaDiem = " + madiem.ToString();
                        cmd = new SqlCommand(CmdString, con);
                        try
                        {
                            cmd.ExecuteScalar();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    //MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                    //messageBoxSuccessful.ShowDialog();
                    //LoadDanhSachBangDiem();
                    con.Close();
                }
                catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }


                //Tinh dtb hoc sinh de Update ThanhTich
                for (int i = 0; i < DanhSachDiem.Count; i++)
                {
                    try
                    {
                    string CmdString = "select DiemTrungBinh from HeThongDiem where MaHocSinh = " + DanhSachDiem[i].MaHocSinh.ToString();
                    SqlCommand cmd;
                    
                    int madiem = 0, xeploai = 0, soluong = 0;
                    decimal dtbhk = 0;
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

                    cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    dtbhk += reader.GetDecimal(0);
                                    soluong += 1;
                                }
                                catch (Exception)
                                { }
                            }
                            reader.NextResult();
                        }
                        dtbhk /= soluong;
                        if (dtbhk > DiemDat) xeploai = 1; else xeploai = 0;
                        con.Close();
                //Update thanh tich
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

                            CmdString = "Update ThanhTich set TrungBinhHocKy = "+ Math.Round(dtbhk,2).ToString()+", XepLoai = "+xeploai.ToString() 
                                + " where MaHocSinh = " + DanhSachDiem[i].MaHocSinh.ToString() + " and MaLop = " + DanhSachDiem[i].MaLop.ToString()
                                + " and HocKy = " + DanhSachDiem[i].HocKy.ToString();
                            cmd = new SqlCommand(CmdString, con);
                            try
                            {
                                cmd.ExecuteScalar();
                            }
                            catch (Exception)
                            { }

                            con.Close() ;
                        }
                        catch (Exception)
                        {
                            MessageBoxFail messageBoxFail = new MessageBoxFail();
                            messageBoxFail.ShowDialog();
                            return;
                        }

                        MessageBoxSuccessful messageBoxSuccessful = new MessageBoxSuccessful();
                        messageBoxSuccessful.ShowDialog();
                        LoadDanhSachBangDiem();
                        //con.Close();
                    }
                    catch (Exception)
                {
                    MessageBoxFail messageBoxFail = new MessageBoxFail();
                    messageBoxFail.ShowDialog();
                }
            }


            }

    }
        public bool KiemTraDiemHopLe()
        {
            for (int i = 0; i < DanhSachDiem.Count; i++)
            {
                if (DanhSachDiem[i].Diem15Phut < 0 || DanhSachDiem[i].Diem15Phut > 10)
                    return false;
                if (DanhSachDiem[i].Diem1Tiet < 0 || DanhSachDiem[i].Diem1Tiet > 10)
                    return false;
            }
            return true;
        }
    }
}