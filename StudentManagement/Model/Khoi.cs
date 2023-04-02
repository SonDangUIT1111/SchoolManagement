using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class Khoi
    {
        private int _maKhoi;
        public int MaKhoi { get { return _maKhoi; } set { _maKhoi = value;} }
        private string _tenKhoi;
        public string TenKhoi { get { return _tenKhoi; } set { _tenKhoi = value; } }
    }
}
