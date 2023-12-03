using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.HocSinh;
using StudentManagement.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.HocSinh
{
    [TestClass]
    public class DiemSoViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private DiemSoViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new DiemSoViewModel();
            viewModel.NhanXet1 = "abc";
            Assert.AreEqual("abc", viewModel.NhanXet1);
            viewModel.NhanXet2 = "abc";
            Assert.AreEqual("abc", viewModel.NhanXet2);
            viewModel.XepLoai1 = "abc";
            Assert.AreEqual("abc", viewModel.XepLoai1);
            viewModel.XepLoai2 = "abc";
            Assert.AreEqual("abc", viewModel.XepLoai2);
            viewModel.TBHK1 = "abc";
            Assert.AreEqual("abc", viewModel.TBHK1);
            viewModel.TBHK2 = "abc";
            Assert.AreEqual("abc", viewModel.TBHK2);
        }

        [TestMethod]
        public void LoadDanhSachDiem()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new DiemSoViewModel();

            sut.DiemSoWD = new StudentManagement.Views.HocSinh.DiemSo();

            try
            {
                sut.IdHocSinh = 100046;
                var res = sut.LoadDanhSachDiem();
                Assert.IsTrue(sut.DanhSachDiemHK1.Count > 0);
                Assert.IsTrue(sut.DanhSachDiemHK2.Count > 0);
                Assert.IsTrue(res > 0);
                Assert.AreEqual(sut.XepLoai1, "Đạt");
                Assert.AreEqual(sut.XepLoai2, "Đạt");
                Assert.AreNotEqual(sut.TBHK1, "");
                Assert.AreNotEqual(sut.TBHK2, "");
                sut.IdHocSinh = 100055;
                res = sut.LoadDanhSachDiem();
                Assert.AreEqual(sut.XepLoai1, "Không đạt");
                Assert.AreEqual(sut.XepLoai2, "Không đạt");
                sut.IdHocSinh = 100096;
                sut.LoadDanhSachDiem();
                Assert.AreEqual(sut.XepLoai1, "Chưa có dữ liệu");
                Assert.AreEqual(sut.XepLoai2, "Chưa có dữ liệu");
                Assert.AreEqual(sut.TBHK1, "Chưa có dữ liệu");
                Assert.AreEqual(sut.TBHK2, "Chưa có dữ liệu");
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }
    }
}
