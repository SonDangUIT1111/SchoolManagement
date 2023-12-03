using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagement.ViewModel.HocSinh;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.ViewModel.Services;
using System.Collections.ObjectModel;
using StudentManagement.Model;
using System.Windows.Controls;
using StudentManagement.Views.HocSinh;

namespace StudentManagementTests.ViewModel.HocSinh
{
    [TestClass]
    public class TrangChuViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private TrangChuViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new TrangChuViewModel();
            viewModel.SayHello = "abc";
            Assert.AreEqual(viewModel.SayHello, "abc");
            viewModel.IdHocSinh = 1;
            Assert.AreEqual(viewModel.IdHocSinh, 1);
            viewModel.IdHocSinhstring = "abc";
            Assert.AreEqual(viewModel.IdHocSinhstring, "abc");
            viewModel.HocSinhHienTai = null;
            Assert.IsNull(viewModel.HocSinhHienTai);
        }
        [TestMethod]
        public void LoadThongTinCaNhan()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new TrangChuViewModel();

            StudentManagement.Model.HocSinh _testHocSinh = new StudentManagement.Model.HocSinh();


            try
            {
                sut.IdHocSinh = 100046;
                sut.HocSinhHienTai = _testHocSinh;
                sut.LoadThongTinCaNhan();
                Assert.AreEqual(sut.HocSinhHienTai.TenHocSinh, "Nguyễn Thiện Thanh 123");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LoadSayHello()
        {

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new TrangChuViewModel();

            StudentManagement.Model.HocSinh _testHocSinh = new StudentManagement.Model.HocSinh();

            

            Border _testBorder = new Border();

            try
            {
                sut.IdHocSinh = 100046;
                sut.HocSinhHienTai = _testHocSinh;
                sut.HocSinhWD = new HocSinhWindow();
                sut.LoadThongTinCaNhan();
                sut.LoadSayHello(_testBorder);
                Assert.IsTrue(true);
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }
    }
}
