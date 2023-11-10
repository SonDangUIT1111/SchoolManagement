using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Model;
using System.Collections.ObjectModel;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class PhanCongGiangDayViewModelTest
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private PhanCongGiangDayViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new PhanCongGiangDayViewModel();
        }

        [TestMethod]
        public void DataGridVisibility()
        {
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
        }

        [TestMethod]
        public void ProgressBarVisibility()
        {
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
        }
        [TestMethod]
        public void LoadThongTinCmb_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            ObservableCollection<string> _testNienKhoaCmb = new ObservableCollection<string>();
            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();
            ObservableCollection<Lop> _testLopCmb = new ObservableCollection<Lop>();


            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new PhanCongGiangDayViewModel(fakeSqlConnection.Object);

            try
            {
                sut.NienKhoaQueries = null;
                sut.KhoiQueries = null;
                sut.LopQueries = null;
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
        public async Task LoadThongTinPhanCong_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });

            var sut = new PhanCongGiangDayViewModel(fakeSqlConnection.Object);

            try
            {
                sut.LopQueries = "151";
                await sut.LoadThongTinPhanCong();
                Assert.IsTrue(true);
            }
            catch (Exception)   
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void FilterLopFromSelection()
        {
            ObservableCollection<Lop> _testLopCmb = new ObservableCollection<Lop>();
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });
            var sut = new PhanCongGiangDayViewModel(fakeSqlConnection.Object);

            try
            {
                sut.LopCmb = _testLopCmb;
                sut.KhoiQueries = "1";
                sut.NienKhoaQueries = "2023-2024";
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
            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });
            var sut = new PhanCongGiangDayViewModel(fakeSqlConnection.Object);

            try
            {
                sut.KhoiCmb = _testKhoiCmb;
                sut.FilterKhoiFromSelection();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();  
            }
        }
        [TestMethod]
        public void XoaPhanCong()
        {
            PhanCongGiangDay _testItem = new PhanCongGiangDay();
            _testItem.MaPhanCong = 100021;
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });
            var sut = new PhanCongGiangDayViewModel(fakeSqlConnection.Object);
            try
            {
                sut.XoaPhanCong(_testItem);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
