namespace StudentManagement.Model
{
    public class ThanhTich
    {
        private int _maThanhTich;
        public int MaThanhTich { get { return _maThanhTich; } set { _maThanhTich = value; } }
        private string _nienKhoa;
        public string NienKhoa { get { return _nienKhoa; } set { _nienKhoa = value; } }
        private int _hocKy;
        public int HocKy { get { return _hocKy; } set { _hocKy = value; } }
        private int _maLop;
        public int MaLop { get { return _maLop; } set { _maLop = value; } }
        private string _tenLop;
        public string TenLop { get { return _tenLop; } set { _tenLop = value; } }
        private int _maHocSinh;
        public int MaHocSinh { get { return _maHocSinh; } set { _maHocSinh = value; } }
        private string _tenHocSinh;
        public string TenHocSinh { get { return _tenHocSinh; } set { _tenHocSinh = value; } }
        private bool _xepLoai;
        public bool XepLoai { get { return _xepLoai; } set { _xepLoai = value; } }
        private string _nhanXet;
        public string NhanXet { get { return _nhanXet; } set { _nhanXet = value; } }
        private float _tbhk;
        public float TBHK { get { return _tbhk; } set { _tbhk = value; } }
    }
}
