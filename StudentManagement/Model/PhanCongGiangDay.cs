using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class PhanCongGiangDay
    {
        private int _maPhanCong;
        public int MaPhanCong { get { return _maPhanCong; } set { _maPhanCong = value; } }
        private string _nienKhoa;
        public string NienKhoa { get { return _nienKhoa; } set { _nienKhoa = value; } }

        private string _tenLop;
        public string TenLop { get { return _tenLop; } set { _tenLop = value; } }
        private int _siSo;
        public int SiSo { get { return _siSo; } set { _siSo = value; } }

        private string _tenMon;
        public string TenMon { get { return _tenMon; } set { _tenMon = value; } }

        private string _tenGiaoVien;
        public string TenGiaoVien { get { return _tenGiaoVien; } set { _tenGiaoVien = value; } }


    }
}