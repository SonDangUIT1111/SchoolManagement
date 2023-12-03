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
            StudentManagement.Model.MonHoc mh = new StudentManagement.Model.MonHoc() { MaMon = 1, TenMon = "a",ApDung = true };
            viewModel.MonHocEditting = mh;
            Assert.AreEqual(viewModel.MonHocEditting,mh);
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);

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

            var sut = new MonHocViewModel();
            try
            {
                sut.DanhSachMonHoc = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.MonHoc>();
                await sut.LoadThongTinMonHoc();
                Assert.AreEqual(sut.DanhSachMonHoc[0].MaMon, 120);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void ThemMonHocMoiTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel();

            try
            {
                var res = sut.ThemMonHocMoi("Trà đạo");
                Assert.AreEqual(1, res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }


        [TestMethod]
        public void DeleteMonHoc_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel();

            try
            {
                StudentManagement.Model.MonHoc mh = new StudentManagement.Model.MonHoc()
                {
                    MaMon = 1,
                };
                var res = sut.DeleteMonHoc(mh);
                Assert.AreEqual(res, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }

        
        [TestMethod]
        public void TraCuuMonHoc_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel();

            try
            {
                sut.DanhSachMonHoc = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.MonHoc> ();
                sut.TraCuuMonHoc("Toán");
                Assert.AreEqual(sut.DanhSachMonHoc[0].MaMon, 120);
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

            var sut = new MonHocViewModel();

            try
            {
                var res = sut.EditTenMon("Thanh lịch", "1");
                Assert.AreEqual(0, res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }

        [TestMethod]
        [DataRow("Toán")]
        [DataRow("Đi chơi")]
        public void KiemTraTonTaiMonHocTest(string search)
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new MonHocViewModel();

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
