namespace StudentManagement.Model
{
    public class BaoCaoMon
    {
        private int _maBaoCaoMon;
        public int MaBaoCaoMon { get { return _maBaoCaoMon; } set { _maBaoCaoMon = value; } }
        private int _maLop;
        public int MaLop { get { return _maLop; } set { _maLop = value; } }
       
        private int _maMon;
        public int MaMon { get { return _maMon; } set { _maMon = value; } }
        private int _hocKy;
        public int HocKy { get { return _hocKy; } set { _hocKy = value; } }
        private int _soLuongDat;
        public int SoLuongDat { get { return _soLuongDat; } set { _soLuongDat = value; } }
        private string _tiLe;
        public string TiLe { get { return _tiLe; } set { _tiLe = value; } }
    }
}
