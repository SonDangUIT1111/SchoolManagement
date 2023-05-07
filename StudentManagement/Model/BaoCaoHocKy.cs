using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class BaoCaoHocKy
    {
        private int _maBaoCaoHocKy;
        public int MaBaoCaoHocKy { get { return _maBaoCaoHocKy; } set { _maBaoCaoHocKy = value; } }
        private int _maLop;
        public int MaLop { get { return _maLop; } set { _maLop = value; } }
        private string _tenLop;
        public string TenLop { get { return _tenLop; } set { _tenLop = value; } }
        private int _siSo;
        public int SiSo { get { return _siSo; } set { _siSo = value; } }
        private int _hocKy;
        public int HocKy { get { return _hocKy; } set { _hocKy = value; } }
        private string _nienKhoa;
        public string NienKhoa { get { return _nienKhoa; } set { _nienKhoa = value; } }
        private int _soLuongDat;
        public int SoLuongDat { get { return _soLuongDat; } set { _soLuongDat = value; } }
        private string _tiLe;
        public string TILe { get { return _tiLe; } set { _tiLe = value; } }
    }
}
