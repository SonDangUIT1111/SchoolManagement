using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class BaoCaoMon
    {
        private int _maBaoCaoMon;
        public int MaBaoCaoMon { get { return _maBaoCaoMon; } set { _maBaoCaoMon = value; } }
        private string _tenLop;
        public string TenLop { get { return _tenLop; } set { _tenLop = value; } }
        private string _tenMon;
        public string TenMon { get { return _tenMon; } set { _tenMon = value; } }
        private int _hocKy;
        public int HocKy { get { return _hocKy; } set { _hocKy = value; } }
        private string _nienKhoa;
        public string NienKhoa { get { return _nienKhoa; } set { _nienKhoa = value; } }
        private int _soLuongDat;
        public int SoLuongDat { get { return _soLuongDat; } set { _soLuongDat = value; } }
        private string _tiLe;
        public string TiLe { get { return _tiLe; } set { _tiLe = value; } }
    }
}
