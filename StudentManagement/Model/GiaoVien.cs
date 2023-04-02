using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class GiaoVien
    {
        private int _maGiaoVien;
        public int MaGiaoVien { get { return _maGiaoVien; } set { _maGiaoVien = value; } }
        private string _tenGiaoVien;
        public string TenGiaoVien { get { return _tenGiaoVien; } set { _tenGiaoVien = value; } }
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
    }
}
