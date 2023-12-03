using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class ThemPhanCongViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThemPhanCongViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThemPhanCongViewModel();
        }
        [TestMethod]
        public void LoadThongTinCmb()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThemPhanCongViewModel();

            string _testNienKhoaQueries =  null;
            string _testKhoiQueries = null;
            string _testLopQueries = null;
            ObservableCollection<string> _testNienKhoaCmb = new ObservableCollection<string>();
            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();
            ObservableCollection<Lop> _testLopCmb = new ObservableCollection<Lop>();

            try
            {
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.KhoiQueries = _testKhoiQueries;
                sut.LopQueries = _testLopQueries;
                sut.NienKhoaCmb = _testNienKhoaCmb;
                sut.KhoiCmb = _testKhoiCmb;
                sut.LopCmb = _testLopCmb;

                sut.LoadThongTinCmb();
                Assert.AreEqual(sut.NienKhoaQueries, "2022-2023");
                Assert.IsTrue(sut.NienKhoaCmb.Count > 0);
                Assert.AreEqual(sut.KhoiCmb[0].MaKhoi, 1);
                Assert.AreEqual(sut.KhoiQueries, "1");
                Assert.AreEqual(sut.LopQueries, "166");
                Assert.AreEqual(sut.LopCmb[0].MaLop, 166);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
        [TestMethod]
        public void LoadOptionFromSelection()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            string _testLopQueries = "151";

            var sut = new ThemPhanCongViewModel();

            ObservableCollection<MonHoc> _testMonHocCmb = new ObservableCollection<MonHoc>();
            ObservableCollection<StudentManagement.Model.GiaoVien> _testGiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();

            try
            {
                sut.MonHocCmb = _testMonHocCmb;
                sut.GiaoVienCmb = _testGiaoVienCmb;
                sut.LopQueries = _testLopQueries;
                sut.LoadOptionFromSelection();
                Assert.AreEqual(sut.MonHocCmb[0].MaMon,120);
                Assert.AreEqual(sut.GiaoVienCmb[0].MaGiaoVien, 100031);
            } catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void FilterLopFromSelection()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThemPhanCongViewModel();

            string _testNienKhoaQueries = "2023-2024";
            string _testKhoiQueries = "1";

            ObservableCollection<Lop> _testLopCmb = new ObservableCollection<Lop>();

            try
            {
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.KhoiQueries = _testKhoiQueries;
                sut.LopCmb = _testLopCmb;

                sut.FilterLopFromSelection();
                Assert.AreEqual(sut.LopCmb[0].MaLop,151);
            } catch (Exception)
            {
                Assert.Fail();
            }
            
        }
        [TestMethod]
        public void FilterKhoiFromSelection()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThemPhanCongViewModel();

            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();

            try
            {
                sut.KhoiCmb = _testKhoiCmb;
                sut.FilterKhoiFromSelection();
                Assert.AreEqual(sut.KhoiCmb[0].MaKhoi, 1);
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }


    }
}
