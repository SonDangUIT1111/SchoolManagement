using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagement.ViewModel.GiamHieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class TrangChuViewModelTest
    {
        private TrangChuViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new TrangChuViewModel();
        }

        [TestMethod]
        public void PropertiesTest()
        {
            Assert.IsNotNull(viewModel);
            viewModel.IdGiamHieu = 1;
            Assert.AreEqual(1, viewModel.IdGiamHieu);
            viewModel.BaoCaoPage = null;
            Assert.IsNull(viewModel.BaoCaoPage);
            viewModel.LopHocPage = null;
            Assert.IsNull(viewModel.LopHocPage);
            viewModel.ThayDoiQuyDinhPage = null;
            Assert.IsNull(viewModel.ThayDoiQuyDinhPage);
            viewModel.ThongTinGiaoVienPage = null;
            Assert.IsNull(viewModel.ThongTinGiaoVienPage);
            viewModel.ThongTinHocSinhPage = null;
            Assert.IsNull(viewModel.ThongTinHocSinhPage);
            viewModel.ThongTinTruongPage = null;
            Assert.IsNull(viewModel.ThongTinTruongPage);
            viewModel.QuanLiDiemSoPage = null;
            Assert.IsNull(viewModel.QuanLiDiemSoPage);
            viewModel.BaoCaoTongKetHocKyPage = null;
            Assert.IsNull(viewModel.BaoCaoTongKetHocKyPage);
            viewModel.MonHocPage = null;
            Assert.IsNull(viewModel.MonHocPage);
            viewModel.PhanCongGiangDayPage = null;
            Assert.IsNull(viewModel.PhanCongGiangDayPage);

        }
    }
}
