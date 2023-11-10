using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class ThongTinHocSinhViewModelTest
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThongTinHocSinhViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThongTinHocSinhViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.ThongTinHocSinhWD = null;
            Assert.IsNull(viewModel.ThongTinHocSinhWD);
            viewModel.DanhSachHocSinh = null;
            Assert.IsNull(viewModel.DanhSachHocSinh);
            viewModel.NienKhoaCmb = null;
            Assert.IsNull(viewModel.NienKhoaCmb);
            viewModel.KhoiCmb = null;
            Assert.IsNull(viewModel.KhoiCmb);
            viewModel.LopCmb = null;
            Assert.IsNull(viewModel.LopCmb);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.IsLoadAll = true;
            Assert.AreEqual(true, viewModel.IsLoadAll);
            viewModel.LopQueries = "ABC";
            Assert.AreEqual("ABC", viewModel.LopQueries);
            viewModel.KhoiQueries = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries);
            viewModel.NienKhoaQueries = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries);
            viewModel.everLoaded = false;
            Assert.AreEqual(false, viewModel.everLoaded);
        }

        [TestMethod]
        public void LoadThongTinComboBox_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThongTinHocSinhViewModel(fakeSqlConnection.Object);

            try
            {
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LoadThongTinCmb(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        [TestMethod]
        public async Task LoadThongTinHocSinh_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                sut.LopQueries = "151";
                await sut.LoadThongTinHocSinh();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SearchHocSinhTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                sut.LopQueries = "151";
                sut.SearchHocSinh("");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SearchHocSinhAllTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                sut.SearchHocSinhAll();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
        [TestMethod]
        public void FilterLopFromSelectionTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel(fakeSqlConnection.Object);
            try
            {
                sut.LopCmb = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.KhoiQueries = "1";
                sut.NienKhoaQueries = "2023-2024";
                sut.FilterLopFromSelection();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void XoaHocSinhTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThongTinHocSinhViewModel(fakeSqlConnection.Object);
            try
            {
                HocSinh hs = new HocSinh()
                {
                    MaHocSinh = 1
                };
                sut.XoaHocSinh(hs);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
    }
}
