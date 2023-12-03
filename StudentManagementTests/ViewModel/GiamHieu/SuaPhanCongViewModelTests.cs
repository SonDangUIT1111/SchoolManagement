using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class SuaPhanCongViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private SuaPhanCongViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new SuaPhanCongViewModel();
            PhanCongGiangDay pc = new StudentManagement.Model.PhanCongGiangDay() { MaPhanCong = 1 };
            viewModel.PhanCongHienTai = pc;
            Assert.AreEqual(viewModel.PhanCongHienTai, pc);
        }
        [TestMethod]
        public void LoadThongTinCmb_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            ObservableCollection<StudentManagement.Model.GiaoVien> _testGiaoVienCmb = new ObservableCollection<StudentManagement.Model.GiaoVien>();


            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new SuaPhanCongViewModel();

            try
            {
                sut.GiaoVienCmb = _testGiaoVienCmb;
                sut.LoadThongTinCmb();
                Assert.AreEqual(sut.GiaoVienCmb[0].MaGiaoVien,100031);
            } catch (Exception)
            {
                Assert.Fail();
            }


        }
    }   
}
