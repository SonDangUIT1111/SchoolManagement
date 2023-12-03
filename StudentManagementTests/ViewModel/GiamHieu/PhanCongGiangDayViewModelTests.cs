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

            var sut = new PhanCongGiangDayViewModel();

            try
            {
                sut.NienKhoaQueries = null;
                sut.KhoiQueries = null;
                sut.LopQueries = null;
                sut.NienKhoaCmb = _testNienKhoaCmb;
                sut.KhoiCmb = _testKhoiCmb;
                sut.LopCmb = _testLopCmb;
                sut.LoadThongTinCmb();
                Assert.IsTrue(sut.NienKhoaCmb.Count > 0);
                Assert.AreEqual(sut.NienKhoaQueries, "2022-2023");
                Assert.IsTrue(sut.KhoiCmb.Count > 0);
                Assert.AreEqual(sut.KhoiQueries, "1");
                Assert.IsTrue(sut.LopCmb.Count > 0);
                Assert.AreEqual(sut.LopQueries, "166");
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

            var sut = new PhanCongGiangDayViewModel();

            try
            {
                sut.LopQueries = "151";
                sut.DanhSachPhanCong = new ObservableCollection<PhanCongGiangDay>();
                await sut.LoadThongTinPhanCong();
                Assert.AreEqual(sut.DanhSachPhanCong[0].MaPhanCong, 100022);
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
            var sut = new PhanCongGiangDayViewModel();

            try
            {
                sut.LopCmb = _testLopCmb;
                sut.KhoiQueries = "1";
                sut.NienKhoaQueries = "2023-2024";
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
            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });
            var sut = new PhanCongGiangDayViewModel();

            try
            {
                sut.KhoiCmb = _testKhoiCmb;
                sut.FilterKhoiFromSelection();
                Assert.AreEqual(sut.KhoiCmb[0].MaKhoi,1);
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
            _testItem.MaPhanCong = 1;
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
            });
            var sut = new PhanCongGiangDayViewModel();
            try
            {
                var res = sut.XoaPhanCong(_testItem);
                Assert.AreEqual(res,0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
