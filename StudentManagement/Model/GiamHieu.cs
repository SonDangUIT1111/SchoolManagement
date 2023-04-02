using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Model
{
    public class GiamHieu
    {
        private int _maTruong;
        public int MaTruong { get { return _maTruong; } set { _maTruong = value; } }
        private string _tenTruong;
        public string TenTruong { get { return _tenTruong; } set { _tenTruong = value; } }
    }
}
