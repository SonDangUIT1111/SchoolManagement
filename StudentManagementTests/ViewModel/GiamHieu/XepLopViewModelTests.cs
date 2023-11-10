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
    public class XepLopViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private XepLopViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new XepLopViewModel();
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.LopHocDangChon = null;
            Assert.IsNull(viewModel.LopHocDangChon);
            viewModel.XepLopWD = null;
            Assert.IsNull(viewModel.XepLopWD);
            viewModel.DanhSachHocSinh = null;
            Assert.IsNull(viewModel.DanhSachHocSinh);
            viewModel.NamSinhCmb = null;
            Assert.IsNull(viewModel.NamSinhCmb);
            viewModel.SelectCheckBox = null;
            Assert.IsNull(viewModel.SelectCheckBox);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true,viewModel.ProgressBarVisibility);
        }

        [TestMethod]
        public void LoadNamSinh_Test()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new XepLopViewModel(fakeSqlConnection.Object);

            try
            {
                sut.NamSinhCmb = new System.Collections.ObjectModel.ObservableCollection<string> { };
                sut.LopHocDangChon = new Lop()
                {
                    MaLop = 151
                };
                sut.LoadNamSinh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
            Assert.IsTrue(true);


        }

        [TestMethod]
        public async Task LoadDanhSachHocSinh_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new XepLopViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                sut.LopHocDangChon = new Lop()
                {
                    MaLop = 151
                };
                await sut.LoadDanhSachHocSinh();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LoadDanhSachHocSinhTheoNamSinh_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new XepLopViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                sut.LopHocDangChon = new Lop()
                {
                    MaLop = 151
                };
                await sut.LoadDanhSachTheoNamSinh("2007","");
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task LocDanhSach_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new XepLopViewModel(fakeSqlConnection.Object);
            try
            {
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                sut.LopHocDangChon = new Lop()
                {
                    MaLop = 151
                };
                ComboBox cb =new ComboBox() { SelectedItem = "2007"};
                await sut.LocDanhSach("",cb);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ThemHocSinhMoiVaoLop_VuotQuiDinhSiSoTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new XepLopViewModel(fakeSqlConnection.Object);

            try
            {
                sut.SelectCheckBox = new bool[] {true, false,true,false};
                sut.LopHocDangChon = new Lop()
                {
                    MaLop = 151
                };
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { };
                var result = sut.ThemHocSinhVaoLop();
                Assert.AreEqual(-1, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public void ThemHocSinhMoiVaoLop_DuDieuKienTest()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() => {
            });

            var sut = new XepLopViewModel(fakeSqlConnection.Object);

            try
            {
                sut.SelectCheckBox = new bool[] { true };
                sut.LopHocDangChon = new Lop()
                {
                    MaLop = 152
                };
                sut.DanhSachHocSinh = new System.Collections.ObjectModel.ObservableCollection<HocSinh> { 
                };
                sut.DanhSachHocSinh.Add(new HocSinh()
                {
                    MaHocSinh = 100056
                });
                var result = sut.ThemHocSinhVaoLop();
                Assert.AreEqual(1, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.IsFalse(true);
            }
        }

        [TestMethod]
        public void ClearSelectArrayTest()
        {
            viewModel.SelectCheckBox = new bool[4] { true, true, true, true };
            viewModel.ClearSelectArray();
            for (int i = 0; i < viewModel.SelectCheckBox.Length; i++)
            {
                if (viewModel.SelectCheckBox[i] == true)
                {
                    Assert.Fail();
                    return;
                }
            }
            Assert.IsTrue(true);
        }
    }
}
