using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LiveCharts;
using StudentManagement.Model;
using StudentManagement.ViewModel.HocSinh;
using StudentManagement.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentManagementTests.ViewModel.HocSinh
{
    [TestClass]
    public class BaoCaoMonHocViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private BaoCaoMonHocViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new BaoCaoMonHocViewModel();
        }

        [TestMethod]
        public void LoadComboboxData()
        {

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new BaoCaoMonHocViewModel(fakeSqlConnection.Object);


            ObservableCollection<string> _testNienKhoaComboBox = new ObservableCollection<string>();
            ObservableCollection<MonHoc> _testMonHocComboBox = new ObservableCollection<MonHoc>();
            string _testNienKhoaQueries = "";
            string _testMonHocQueries = "";
            string _testHocKyQueries = "1";



            try
            {
                sut.NienKhoaComboBox = _testNienKhoaComboBox;
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.MonHocQueries = _testMonHocQueries;
                sut.MonHocComboBox = _testMonHocComboBox;
                sut.HocKyQueries = _testHocKyQueries;
 
                sut.LoadComboboxData();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task LoadDanhSachMonHocAndChart()
        {

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new BaoCaoMonHocViewModel(fakeSqlConnection.Object);


            ObservableCollection<string> _testNienKhoaComboBox = new ObservableCollection<string>();
            ObservableCollection<MonHoc> _testMonHocComboBox = new ObservableCollection<MonHoc>();
            ObservableCollection<BaoCaoMon> _testDanhSachBaoCaoMon = new ObservableCollection<BaoCaoMon>();
            string _testNienKhoaQueries = "2023-2024";
            string _testMonHocQueries = "120";
            string _testHocKyQueries = "1";
            int _testDat = 0;
            int _testKhongDat = 0;
            int _testTongSiSoLop = 0;
            List<string> _testTenLop = new List<string>();
            List<int> _testSoLuongDatChartVal = new List<int>();
            LiveCharts.SeriesCollection _testSoLuongDat = new SeriesCollection();
            LiveCharts.SeriesCollection _testTiLeDat = new SeriesCollection();



            try
            {
                sut.NienKhoaComboBox = _testNienKhoaComboBox;
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.MonHocQueries = _testMonHocQueries;
                sut.MonHocComboBox = _testMonHocComboBox;
                sut.HocKyQueries = _testHocKyQueries;
                sut.DanhSachBaoCaoMon = _testDanhSachBaoCaoMon;
                sut.TenLop = _testTenLop;
                sut.SoLuongDatChartVal = _testSoLuongDatChartVal;
                sut.SoLuongDat = _testSoLuongDat;
                sut.Dat = _testDat;
                sut.TongSiSoLop = _testTongSiSoLop;
                sut.KhongDat = _testKhongDat;
                sut.TiLeDat = _testTiLeDat;
                await sut.LoadDanhSachBaoCaoMon();
                sut.LoadCartesianChart();
                sut.LoadPieChart();
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestVisibility()
        {
            viewModel.CartersianChartVisibility = true;
            viewModel.PieChartVisibility = true;
            viewModel.DataGridVisibility = true;
            viewModel.ProgressBarVisibility = true;

            Assert.AreEqual(true, viewModel.CartersianChartVisibility);
            Assert.AreEqual(true, viewModel.PieChartVisibility);
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
        }
    }
}
