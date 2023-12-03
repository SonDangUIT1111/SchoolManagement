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
    public class SuaLopHocViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private SuaLopHocViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new SuaLopHocViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.GiaoVienQueries = "abc";
            Assert.AreEqual("abc",viewModel.GiaoVienQueries);
            viewModel.MaGiaoVien = 1;
            Assert.AreEqual(1, viewModel.MaGiaoVien);
            viewModel.MaKhoi = 1;
            Assert.AreEqual(1, viewModel.MaKhoi);
            viewModel.Khoi = 1;
            Assert.AreEqual(1, viewModel.Khoi);
            viewModel.SuaLopWD = null;
            Assert.IsNull(viewModel.SuaLopWD);
            Lop l = new Lop() { MaLop = 1 };
            viewModel.LopHocHienTai = l;
            Assert.AreEqual(viewModel.LopHocHienTai,l);
            viewModel.GiaoVienComboBox = null;
            Assert.IsNull(viewModel.GiaoVienComboBox);
            viewModel.NienKhoaComboBox = null;
            Assert.IsNull(viewModel.NienKhoaComboBox);
        }

        [TestMethod]
        public void LoadGVCNComboBox_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaLopHocViewModel();

            try
            {
                sut.GiaoVienComboBox = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.GiaoVien> { };
                sut.LoadGVCNCombobox();
                Assert.AreEqual(sut.GiaoVienComboBox[0].MaGiaoVien, 100033);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }


        }

        [TestMethod]
        public void TienHanhSuaLopTest_ThongTinLopDaTonTai()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaLopHocViewModel();

            try
            {
                var result = sut.TienHanhSuaLopHoc("10A1","2023-2024","152","");
                Assert.AreEqual(result, 0);
                result = sut.TienHanhSuaLopHoc("10A1", "2023-2024", "1", "");
                Assert.AreEqual(result, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public void TienHanhSuaLopTest_ThongTinLopChuaTonTai()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new SuaLopHocViewModel();

            try
            {
                var result = sut.TienHanhSuaLopHoc("10AOther", "2022-2023", "164", "100040");
                Assert.AreEqual(result,1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
        }

    }
}
