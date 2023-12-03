using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LiveCharts;
using StudentManagement.Model;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiaoVien
{
    [TestClass]
    public class TranChuViewModelTest
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private TrangChuViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new TrangChuViewModel();
            viewModel.CurrentUser = null;
            Assert.IsNull(viewModel.CurrentUser);
            viewModel.GiaoVienWD = null;
            Assert.IsNull(viewModel.GiaoVienWD);
            viewModel.BaoCaoPage = null;
            Assert.IsNull (viewModel.BaoCaoPage);
            viewModel.BaoCaoHocKyPage = null;
            Assert.IsNull(viewModel.BaoCaoHocKyPage);
            viewModel.LopHocPage = null;
            Assert.IsNull(viewModel.LopHocPage);
            viewModel.ThanhTichHocSinhPage = null;
            Assert.IsNull(viewModel.ThanhTichHocSinhPage);
            viewModel.HeThongBangDiemPage = null;
            Assert.IsNull(viewModel.HeThongBangDiemPage);
            viewModel.ThongTinCaNhanPage = null;
            Assert.IsNull(viewModel.ThongTinCaNhanPage);
            viewModel.ThongTinTruongPage = null;
            Assert.IsNull(viewModel.ThongTinTruongPage);
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


            sut.CurrentUser = new StudentManagement.Model.GiaoVien();
            sut.CurrentUser.MaGiaoVien = 100031;



            try
            {
                sut.LoadThongTinCaNhan();
                Assert.AreEqual(sut.CurrentUser.TenGiaoVien, "Nguyễn Thủy Hằng");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

    }
}
