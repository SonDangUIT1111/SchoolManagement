using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiaoVien
{
    [TestClass]
    public class ThanhTichHocSinhViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThanhTichHocSinhViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThanhTichHocSinhViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.everLoaded = true;
            Assert.IsTrue(viewModel.everLoaded);
            viewModel.IdUser = 1;
            Assert.AreEqual(1, viewModel.IdUser);
            viewModel.HocKyQueries = "abc";
            Assert.AreEqual("abc", viewModel.HocKyQueries);
            viewModel.LopQueries = "abc";
            Assert.AreEqual("abc", viewModel.LopQueries);
            viewModel.KhoiQueries = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries);
            viewModel.NienKhoaQueries = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries);
            viewModel.NienKhoaCombobox = null;
            Assert.IsNull(viewModel.NienKhoaCombobox);
            viewModel.KhoiCombobox = null;
            Assert.IsNull(viewModel.KhoiCombobox);
            viewModel.LopCombobox = null;
            Assert.IsNull(viewModel.LopCombobox);
            viewModel.HocKyCombobox= null;
            Assert.IsNull(viewModel.HocKyCombobox);
            viewModel.DanhSachThanhTichHocSinh = null;
            Assert.IsNull(viewModel.DanhSachThanhTichHocSinh);
            viewModel.NhanXetTextBoxVisibility = true;
            Assert.IsTrue(viewModel.NhanXetTextBoxVisibility);
            viewModel.NhanXetTextBlockVisibility = true;
            Assert.IsTrue(viewModel.NhanXetTextBlockVisibility);
            viewModel.EditNhanXetVisibility = true;
            Assert.IsTrue(viewModel.EditNhanXetVisibility);
            viewModel.CompleteNhanXetVisibility = true;
            Assert.IsTrue(viewModel.CompleteNhanXetVisibility);
            viewModel.NhanXetTextBoxIsEnabled = true;
            Assert.IsTrue(viewModel.NhanXetTextBoxIsEnabled);
            viewModel.DataGridVisibility = true;
            Assert.IsTrue(viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.IsTrue(viewModel.ProgressBarVisibility);
        }

        [TestMethod()]
        public void LoadComboBoxTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThanhTichHocSinhViewModel();

            try
            {
                sut.NienKhoaCombobox = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.KhoiCombobox = new System.Collections.ObjectModel.ObservableCollection<Khoi> { };
                sut.LopCombobox = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.HocKyCombobox = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.LoadComboBox();
                Assert.IsTrue(sut.NienKhoaCombobox.Count > 0);
                Assert.AreEqual(sut.NienKhoaQueries, "2023-2024");
                sut.NienKhoaQueries = null;
                sut.LoadComboBox();
                Assert.AreEqual(sut.NienKhoaQueries, "2023-2024");
            }
            catch (Exception )
            {
                Assert.Fail();
            }

        }
        [TestMethod()]
        public void FilterKhoiFromNienKhoaTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThanhTichHocSinhViewModel();

            try
            {
                sut.KhoiCombobox = new System.Collections.ObjectModel.ObservableCollection<Khoi> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.FilterKhoiFromNienKhoa();
                Assert.AreEqual(sut.KhoiCombobox[0].MaKhoi, 1);
                Assert.AreEqual(sut.KhoiQueries, "1");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        [TestMethod()]
        public void FilterLopFromKhoiTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThanhTichHocSinhViewModel();

            try
            {
                sut.LopCombobox = new System.Collections.ObjectModel.ObservableCollection<Lop> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LopQueries = null;
                sut.FilterLopFromKhoi();
                Assert.AreEqual(sut.LopQueries,"151");
                Assert.AreEqual(sut.LopCombobox[0].MaLop,151);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
        [TestMethod()]
        public void FilterHocKyFromLopTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThanhTichHocSinhViewModel();

            try
            {
                sut.HocKyCombobox = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LopQueries = "151";
                sut.FilterHocKyFromLop();
                //Assert.AreEqual(sut.HocKyCombobox[0],"Học kỳ 1");
                //Assert.AreEqual(sut.HocKyQueries, "1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }

        }


        [TestMethod]
        public async Task LoadDanhSachThanhTichTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThanhTichHocSinhViewModel();
            try
            {
                sut.DanhSachThanhTichHocSinh = new System.Collections.ObjectModel.ObservableCollection<ThanhTich> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LopQueries = "151";
                sut.HocKyQueries = "1";
                sut.IdUser = 100031;
                await sut.LoadDanhSachThanhTichHocSinh();
                Assert.IsTrue(sut.DanhSachThanhTichHocSinh.Count > 0);
                Assert.AreEqual(sut.DanhSachThanhTichHocSinh[0].XepLoai, true);
                //Assert.AreEqual(sut.DanhSachThanhTichHocSinh[0].TBHK, 9.97);
                Assert.AreEqual(sut.DanhSachThanhTichHocSinh[0].NhanXet, "");
                Assert.AreEqual(sut.EditNhanXetVisibility, true);
                Assert.AreEqual(sut.CompleteNhanXetVisibility, false);
                Assert.AreEqual(sut.NhanXetTextBoxIsEnabled, false);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public async Task UpdateNhanXetTest_CaseKhongPhaiGVCNVaCaseLaGVCN()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new ThanhTichHocSinhViewModel();

            try
            {
                sut.DanhSachThanhTichHocSinh = new System.Collections.ObjectModel.ObservableCollection<ThanhTich> { };
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.LopQueries = "151";
                sut.HocKyQueries = "1";
                sut.IdUser = 100031;
                await sut.LoadDanhSachThanhTichHocSinh();
                var result = sut.UpdateNhanXet();
                Assert.IsTrue(result > 0);
                sut.IdUser = 100032;
                sut.UpdateNhanXet();

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }



    }
}
