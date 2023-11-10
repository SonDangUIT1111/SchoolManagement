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

            var sut = new DiemSoViewModel(fakeSqlConnection.Object);

            sut.DiemSoWD = new StudentManagement.Views.HocSinh.DiemSo();

            try
            {
                sut.IdHocSinh = 100046;
                sut.LoadDanhSachDiem();
                sut.IdHocSinh = 100096;
                sut.LoadDanhSachDiem();
                Assert.IsTrue(true);
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail();
            }
        }
    }
}
