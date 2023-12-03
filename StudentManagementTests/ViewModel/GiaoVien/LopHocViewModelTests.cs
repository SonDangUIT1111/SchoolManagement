using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.ViewModel.GiaoVien;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.ViewModel.Services;
using StudentManagement.Views.GiaoVien;
using System.Windows.Controls;

namespace StudentManagementTests.ViewModel.GiaoVien
{
    [TestClass]
    public class LopHocViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private LopHocViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new LopHocViewModel();
        }

        [TestMethod]
        public void PropertiesTest()
        {
            viewModel.everLoaded = true;
            Assert.IsTrue(viewModel.everLoaded);
            viewModel.IdGiaoVien = 1;
            Assert.AreEqual(1, viewModel.IdGiaoVien);
            viewModel.NienKhoaQueries = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries);
            viewModel.KhoiQueries = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries);
            viewModel.LopQueries = "abc";
            Assert.AreEqual("abc", viewModel.LopQueries);
            viewModel.LopDaChon = null;
            Assert.IsNull(viewModel.LopDaChon);
            viewModel.LopHocWD = null;
            Assert.IsNull(viewModel.LopHocWD);
            viewModel.DanhSachhs = null;
            Assert.IsNull(viewModel.DanhSachhs);
            viewModel.DanhSachKhoi = null;
            Assert.IsNull(viewModel.DanhSachKhoi);
            viewModel.DanhSachNienKhoa = null;
            Assert.IsNull(viewModel.DanhSachNienKhoa);
            viewModel.DanhSachLop = null;
            Assert.IsNull(viewModel.DanhSachLop);
            viewModel.DataGridVisibility = true;
            Assert.IsTrue(viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.IsTrue(viewModel.ProgressBarVisibility);
        }


        [TestMethod]
        public void LoadDanhSachLop_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel();

            try
            {
                sut.DanhSachLop = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.Lop> { };
                sut.DanhSachhs = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.KhoiQueries = "1";
                sut.NienKhoaQueries = "2023-2024";
                sut.LoadDanhSachLop();
                Assert.AreEqual(sut.LopQueries, "151");
                Assert.IsTrue(sut.DanhSachLop.Count > 0);
            }catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void XacDinhLaGiaoVienChuNhiem_KhongPhaiCNTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel();

            try
            {
                sut.IdGiaoVien = 100032;
                var result = sut.IsGiaoVienChuNhiem("100046");
                Assert.IsFalse(result);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void XacDinhLaGiaoVienChuNhiem_LaCNTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel();

            try
            {
                sut.IdGiaoVien = 100031;
                var result = sut.IsGiaoVienChuNhiem("100046");
                Assert.IsTrue(result);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InitComboBoxTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel();

            try
            {
                sut.DanhSachNienKhoa = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.DanhSachKhoi = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.Khoi> { };
                sut.DanhSachLop = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.Lop> { };
                sut.NienKhoaQueries = null;
                sut.KhoiQueries = null;
                sut.InitComboBox();
                sut.KhoiQueries = "1";
                sut.InitComboBox();
                Assert.AreEqual(sut.NienKhoaQueries,"2022-2023");
                Assert.IsTrue(sut.DanhSachNienKhoa.Count > 0);
                Assert.AreEqual(sut.DanhSachKhoi[0].MaKhoi,1);
                Assert.AreEqual(sut.KhoiQueries, "1");
                Assert.AreEqual(sut.LopQueries, "166");
                Assert.IsTrue(sut.DanhSachLop.Count > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LocHocSinhTheoTenTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel();

            try
            {
                sut.DanhSachhs = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.LopQueries = "151";
                TextBox textBox = new TextBox() { Text = "Thanh 123" };
                sut.LocHocSinhTheoTen(textBox);
                Assert.AreEqual(sut.DanhSachhs[0].MaHocSinh,100046);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task LoadDanhSachHocSinh()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel();
            try
            {
                sut.DanhSachhs = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HocSinh> { };
                sut.LopQueries = "151";
                await sut.LoadDanhSachHocSinh();
                Assert.IsTrue(sut.DanhSachhs.Count > 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
    }
}
