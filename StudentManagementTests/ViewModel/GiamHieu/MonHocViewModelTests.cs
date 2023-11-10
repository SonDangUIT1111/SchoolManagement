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
    public class MonHocViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private MonHocViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new MonHocViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.everLoaded = true;
            Assert.AreEqual(true, viewModel.everLoaded); 
            viewModel.MonHocWD = null;
            Assert.IsNull(viewModel.MonHocWD);
            viewModel.everLoaded = true;
            Assert.AreEqual(true, viewModel.everLoaded);
            viewModel.DanhSachMonHoc = null;
            Assert.IsNull(viewModel.DanhSachMonHoc);
            viewModel.MonHocEditting = null;
            Assert.IsNull(viewModel.MonHocEditting);
            viewModel.DataGridVisibility = false;
            Assert.AreEqual(false, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = false;
            Assert.AreEqual(false, viewModel.ProgressBarVisibility);
        }

        [TestMethod]
        public async Task LoadThongTinMonHoc_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new MonHocViewModel(fakeSqlConnection.Object);
            try
            {
                await sut.LoadThongTinMonHoc();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ThemMonHocMoiTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.ThemMonHocMoi("Trà đạo");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }


        [TestMethod]
        public void DeleteMonHoc_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel(fakeSqlConnection.Object);

            try
            {
                StudentManagement.Model.MonHoc mh = new StudentManagement.Model.MonHoc()
                {
                    MaMon = 1,
                };
                sut.DeleteMonHoc(mh);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        
        [TestMethod]
        public void TraCuuMonHoc_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.DanhSachMonHoc = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.MonHoc> { new StudentManagement.Model.MonHoc() };
                sut.TraCuuMonHoc("Toán");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        [TestMethod]
        public void EditTenMonTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.EditTenMon("Thanh lịch", "1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        [TestMethod]
        [DataRow("Toán")]
        [DataRow("Đi chơi")]
        public void KiemTraTonTaiMonHocTest(string search)
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel(fakeSqlConnection.Object);

            try
            {
                var result = sut.KiemTraTonTaiMonHoc(search);
                Console.WriteLine(result + search);
                if (search == "Toán")
                {
                    Assert.AreEqual(result,120);
                }
                else Assert.AreEqual(result,0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }  
        }


    }
}
