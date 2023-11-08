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
            // Create the LopHocViewModel instance with the mock service
            viewModel = new LopHocViewModel();
        }
        //[AssemblyInitialize]
        //public static void AssemblyInit(TestContext context)
        //{
        //    // Initialize the WPF application for UI tests
        //    Application app = new Application();
        //}
        [TestMethod]
        public async Task LoadDanhSachHocSinh_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new LopHocViewModel(fakeSqlConnection.Object);

            await sut.LoadDanhSachHocSinh();
       
            Assert.IsTrue(true);
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

            var sut = new LopHocViewModel(fakeSqlConnection.Object);

            sut.LoadDanhSachLop();

            Assert.IsTrue(true);
        }
    }
}
