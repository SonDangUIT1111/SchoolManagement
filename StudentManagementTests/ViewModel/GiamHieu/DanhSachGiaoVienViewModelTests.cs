using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using StudentManagement.Model;
using System.Windows;
using System.Collections.ObjectModel;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class DanhSachGiaoVienViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private DanhSachGiaoVienViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new DanhSachGiaoVienViewModel();
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
        public async Task LoadDanhSachGiaoVien_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new DanhSachGiaoVienViewModel();
            try
            {
                sut.DanhSachGiaoVien = new ObservableCollection<StudentManagement.Model.GiaoVien>();
                await sut.LoadDanhSachGiaoVien();
                Assert.AreEqual(sut.DanhSachGiaoVien[0].MaGiaoVien,100031);
            } catch (Exception)
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public async Task LocGiaoVienTheoTen_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new DanhSachGiaoVienViewModel();
            try
            {
                sut.DanhSachGiaoVien = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.GiaoVien>() { };
                await sut.LocGiaoVienTheoTen("Nguyễn Thủy Hằng");
                Assert.AreEqual(sut.DanhSachGiaoVien[0].MaGiaoVien, 100031);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void XoaGiaoVien()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new DanhSachGiaoVienViewModel();


            StudentManagement.Model.GiaoVien giaoVien = new StudentManagement.Model.GiaoVien();
            giaoVien.MaGiaoVien = '1';
            
            try
            {
                var res = sut.XoaGiaoVien(giaoVien);
                Assert.AreEqual(res, 0);
            } catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
