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
    public class DanhSachLopViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private DanhSachLopViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new DanhSachLopViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.DanhSachLopWindow = null;
            Assert.IsNull(viewModel.DanhSachLopWindow);
            viewModel.DataGridVisibility = false;
            Assert.AreEqual(false, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = false;
            Assert.AreEqual(false, viewModel.ProgressBarVisibility);
            viewModel.TenLop = "abc";
            Assert.AreEqual("abc", viewModel.TenLop);
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(viewModel.ProgressBarVisibility, true);
        }

        [TestMethod]
        public async Task LoadDanhSachHocSinhTrongLop_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new DanhSachLopViewModel();
            try
            {
                sut.MaLop = 151;
                await sut.LoadDanhSachHocSinh();
                Assert.AreEqual(sut.DanhSachLop[0].MaHocSinh, 100046);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void XoaHocSinhKhoiLop_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new DanhSachLopViewModel();

            try
            {
                StudentManagement.Model.HocSinh hocsinh = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1,
                };
                var res = sut.XoaHocSinh(hocsinh);
                Assert.IsTrue(res[0] == 0);
                Assert.IsTrue(res[1] == 0);
                Assert.IsTrue(res[2] == 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }

        }

        [TestMethod]
        public void HoanTac_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new DanhSachLopViewModel();

            try
            {
                StudentManagement.Model.HocSinh hocsinh = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1,
                };
                sut.MaLop = 1;
                var res = sut.HoanTac(hocsinh);
                Assert.IsTrue(res[0] == 0);
                Assert.IsTrue(res[1] == 0);
                Assert.IsTrue(res[2] == 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }

        [TestMethod]
        [DataRow("Thanh")]
        public void LoadHocSinhTheoTen_Test(string searchWord)
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new DanhSachLopViewModel();

            try
            { 
                sut.DanhSachLop = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.MaLop = 151;
                sut.LocHocSinhTheoTen(searchWord);
                Assert.AreEqual(sut.DanhSachLop[0].MaHocSinh,100046);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }

    }
}
