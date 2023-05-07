using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class HeThongDiem
    {
        private int _soThuTu;
        public int SoThuTu { get { return _soThuTu; } set { _soThuTu = value; } }
        private int _maDiem;
        public int MaDiem { get { return _maDiem; } set { _maDiem = value; } }
        private string _nienKhoa;
        public string NienKhoa { get { return _nienKhoa; } set { _nienKhoa = value; } }
        private int _hocKy;
        public int HocKy { get { return _hocKy; } set { _hocKy = value; } }
        private int _maLop;
        public int MaLop { get { return _maLop; } set { _maLop = value; } }
        private string _tenLop;
        public string TenLop { get { return _tenLop; } set { _tenLop = value; } }
        private int _maMon;
        public int MaMon { get { return _maMon; } set { _maMon = value; } }
        private string _tenMon;
        public string TenMon { get { return _tenMon; } set { _tenMon = value; } }
        private int _maHocSinh;
        public int MaHocSinh { get { return _maHocSinh; } set { _maHocSinh = value;} }
        private string _tenHocSinh;
        public string TenHocSinh { get { return _tenHocSinh; } set { _tenHocSinh = value; } }
        private decimal _diem15Phut;
        public decimal Diem15Phut { get { return _diem15Phut; } set { _diem15Phut = value; } }
        private decimal _diem1Tiet;
        public decimal Diem1Tiet { get { return _diem1Tiet; } set { _diem1Tiet = value; } }
        private decimal _diemTB;
        public decimal DiemTB { get { return _diemTB; } set { _diemTB = value; } }
        private bool _xepLoai;
        public bool XepLoai { get { return _xepLoai; } set { _xepLoai = value;} }
        private bool _trangThai;
        public bool TrangThai { get { return _trangThai; } set { _trangThai = value;} }

    }
}
