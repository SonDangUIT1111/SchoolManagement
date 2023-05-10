namespace StudentManagement.Model
{
    public class MonHoc
    {
        private int _maMon;
        public int MaMon { get { return _maMon; } set { _maMon = value; } }
        private string _tenMon;
        public string TenMon { get { return _tenMon; } set { _tenMon = value; } }
        private bool _apDung;
        public bool ApDung { get { return _apDung; } set { _apDung = value; } }
    }
}
