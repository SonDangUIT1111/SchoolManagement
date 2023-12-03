using System;

namespace StudentManagement.Model
{
    public class HocSinh
    {
        private int _maHocSinh;
        public int MaHocSinh { get { return _maHocSinh; } set { _maHocSinh = value; } }
        private string _tenHocSinh;
        public string TenHocSinh { get { return _tenHocSinh; } set { _tenHocSinh = value; } }
        private DateTime _ngaySinh;
        public DateTime NgaySinh { get { return _ngaySinh; } set { _ngaySinh = value; } }
        private bool _gioiTinh;
        public bool GioiTinh { get { return _gioiTinh; } set { _gioiTinh = value; } }
        private string _diaChi;
        public string DiaChi { get { return _diaChi; } set { _diaChi = value; } }
        private string _email;
        public string Email { get { return _email; } set { _email = value; } }
        private byte[] _avatar;
        public byte[] Avatar { get { return _avatar; } set { _avatar = value; } }

        private string _diemTB1;
        public string DiemTB1 { get { return _diemTB1; } set { _diemTB1 = value; } }
        private string _diemTB2;
        public string DiemTB2 { get { return _diemTB2; } set { _diemTB2 = value; } }
    }
}
