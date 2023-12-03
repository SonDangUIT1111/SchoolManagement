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
    public class BaoCaoHocKyViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private BaoCaoHocKyViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new BaoCaoHocKyViewModel();
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

            var sut = new BaoCaoHocKyViewModel();


            ObservableCollection<string> _testNienKhoaComboBox = new ObservableCollection<string>();
            string _testNienKhoaQueries = "";


            try
            {

                sut.NienKhoaComboBox = _testNienKhoaComboBox;
                sut.NienKhoaQueries = _testNienKhoaQueries;
                var result = sut.LoadComboboxData() > 0;

                Assert.IsTrue(result);
                result = sut.NienKhoaComboBox.Count > 0;

                Assert.IsTrue(result);
                sut.NienKhoaQueries = null;
                sut.LoadComboboxData();
                Assert.AreEqual(sut.NienKhoaQueries, "2022-2023");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void FilterKhoiFomNienKhoa()
        {

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new BaoCaoHocKyViewModel();
            ObservableCollection<Khoi> _testKhoiComboBox = new ObservableCollection<Khoi>();
            string _testHocKyQueries = "1";
            string _testNienKhoaQueries = "2023-2024";
            string _testKhoiQueries = "1";

            try
            {
                sut.KhoiComboBox = _testKhoiComboBox;
                sut.HocKyQueries = _testHocKyQueries;
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.KhoiQueries = _testKhoiQueries;
                var result = sut.FilterKhoiFromNienKhoa() > 0;
                Assert.IsTrue(result);
                Assert.AreEqual(sut.KhoiComboBox[0].MaKhoi, 1);
                Assert.AreEqual(sut.KhoiQueries, "1");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }

        }
        [TestMethod]
        public async Task LoadDanhSachBaoCaoHocKyAndChart()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            ObservableCollection<BaoCaoHocKy> _testDanhSachBaoCaoHocKy = new ObservableCollection<BaoCaoHocKy>();


            string _testHocKyQueries = "1";
            string _testNienKhoaQueries = "2023-2024";
            string _testKhoiQueries = "1";
            int _testDat = 0;
            int _testKhongDat = 0;
            int _testTongSiSoLop = 0;
            bool _IsTeting = true;
            List<string> _testTenLop = new List<string>();
            List<int> _testSoLuongDatChartVal = new List<int>();
            LiveCharts.SeriesCollection _testSoLuongDat = new SeriesCollection();
            LiveCharts.SeriesCollection _testTiLeDat = new SeriesCollection();



            var sut = new BaoCaoHocKyViewModel();

            try
            {
                sut.DanhSachBaoCaoHocKy = _testDanhSachBaoCaoHocKy;
                sut.HocKyQueries = _testHocKyQueries;
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.KhoiQueries = _testKhoiQueries;
                sut.TenLop = _testTenLop;
                sut.SoLuongDat = _testSoLuongDat;
                sut.SoLuongDatChartVal = _testSoLuongDatChartVal;
                sut.Dat = _testDat;
                sut.TongSiSoLop = _testTongSiSoLop;
                sut.KhongDat = _testKhongDat;
                sut.TiLeDat = _testTiLeDat;
                sut.IsTesting = _IsTeting;
                await sut.LoadDanhSachBaoCaoHocKy();
                var res = sut.DanhSachBaoCaoHocKy.Count > 0;
                Assert.IsTrue(res);
                sut.LoadCartesianChart();
                sut.LoadPieChart();
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
            viewModel.Dat = 1;
            viewModel.KhongDat = 1;
            Assert.AreEqual(1, viewModel.Dat);
            Assert.AreEqual(1, viewModel.KhongDat);
            Assert.AreEqual(true, viewModel.CartersianChartVisibility);
            Assert.AreEqual(true, viewModel.PieChartVisibility);
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
        }
    }
}
