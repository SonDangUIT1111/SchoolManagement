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

            var sut = new ThemPhanCongViewModel(fakeSqlConnection.Object);

            string _testNienKhoaQueries = "2023-2024";
            string _testKhoiQueries = "1";
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
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
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

            var sut = new ThemPhanCongViewModel(fakeSqlConnection.Object);

            ObservableCollection<MonHoc> _testMonHocCmb = new ObservableCollection<MonHoc>();
            ObservableCollection<StudentManagement.Model.GiaoVien> _testGiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();

            try
            {
                sut.MonHocCmb = _testMonHocCmb;
                sut.GiaoVienCmb = _testGiaoVienCmb;
                sut.LopQueries = _testLopQueries;
                sut.LoadOptionFromSelection();
                Assert.IsTrue(true);
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

            var sut = new ThemPhanCongViewModel(fakeSqlConnection.Object);

            string _testNienKhoaQueries = "2023-2024";
            string _testKhoiQueries = "1";

            ObservableCollection<Lop> _testLopCmb = new ObservableCollection<Lop>();

            try
            {
                sut.NienKhoaQueries = _testNienKhoaQueries;
                sut.KhoiQueries = _testKhoiQueries;
                sut.LopCmb = _testLopCmb;

                sut.FilterLopFromSelection();
                Assert.IsTrue(true);
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

            var sut = new ThemPhanCongViewModel(fakeSqlConnection.Object);

            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();

            try
            {
                sut.KhoiCmb = _testKhoiCmb;
                sut.FilterKhoiFromSelection(); 
                Assert.IsTrue(true);
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }


    }
}
