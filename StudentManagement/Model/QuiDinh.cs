using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class QuiDinh
    {
        private int _maQuiDinh;
        public int MaQuiDinh { get { return _maQuiDinh; } set { _maQuiDinh = value; } }
        private string _tenQuiDinh;
        public string TenQuiDinh { get { return _tenQuiDinh; } set { _tenQuiDinh = value; } }
        private int _giaTri;
        public int GiaTri { get { return _giaTri; } set { _giaTri = value; } }
    }
}
