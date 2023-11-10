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

            var sut = new DanhSachLopViewModel(fakeSqlConnection.Object);
            try
            {
                sut.MaLop = 151;
                await sut.LoadDanhSachHocSinh();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void XoaHocSinhKhoiLop_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new DanhSachLopViewModel(fakeSqlConnection.Object);

            try
            {
                StudentManagement.Model.HocSinh hocsinh = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1,
                };
                sut.XoaHocSinh(hocsinh);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        [TestMethod]
        public void HoanTac_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new DanhSachLopViewModel(fakeSqlConnection.Object);

            try
            {
                StudentManagement.Model.HocSinh hocsinh = new StudentManagement.Model.HocSinh()
                {
                    MaHocSinh = 1,
                };
                sut.MaLop = 1;
                sut.HoanTac(hocsinh);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        [TestMethod]
        [DataRow("Thanh")]
        [DataRow("Linh")]
        public void LoadHocSinhTheoTen_Test(string searchWord)
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new DanhSachLopViewModel(fakeSqlConnection.Object);

            try
            { 
                sut.DanhSachLop = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { new StudentManagement.Model.HocSinh() };
                sut.MaLop = 151;
                sut.LocHocSinhTheoTen(searchWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

    }
}
