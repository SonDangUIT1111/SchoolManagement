using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass()]
    public class QuanLiDiemSoViewModelTests
    {
        QuanLiDiemSoViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new QuanLiDiemSoViewModel();
        }
        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.IdUser = 1;
            Assert.AreEqual(1, viewModel.IdUser);
            viewModel.everLoaded = true;
            Assert.AreEqual(true, viewModel.everLoaded);
            viewModel.NienKhoaQueries = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries);
            viewModel.HocKyQueries = 1;
            Assert.AreEqual(1, viewModel.HocKyQueries);
            viewModel.KhoiQueries = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries);
            viewModel.LopQueries = "abc";
            Assert.AreEqual("abc", viewModel.LopQueries);
            viewModel.MonHocQueries = "abc";
            Assert.AreEqual("abc", viewModel.MonHocQueries);
            viewModel.NienKhoaQueries2 = "abc";
            Assert.AreEqual("abc", viewModel.NienKhoaQueries2);
            viewModel.HocKyQueries2 = 1;
            Assert.AreEqual(1, viewModel.HocKyQueries2);
            viewModel.KhoiQueries2 = "abc";
            Assert.AreEqual("abc", viewModel.KhoiQueries2);
            viewModel.LopQueries2 = "abc";
            Assert.AreEqual("abc", viewModel.LopQueries2);
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
            viewModel.QuanLiDiemSoWD = null;
            Assert.IsNull(viewModel.QuanLiDiemSoWD);
            viewModel.DanhSachDiem = null;
            Assert.IsNull(viewModel.DanhSachDiem);
            viewModel.DanhSachThanhTich = null;
            Assert.IsNull(viewModel.DanhSachThanhTich);
            viewModel.DanhSachBaoCaoMon = null;
            Assert.IsNull(viewModel.DanhSachBaoCaoMon);
            viewModel.DanhSachBaoCaoHocKy = null;
            Assert.IsNull(viewModel.DanhSachBaoCaoHocKy);
            viewModel.NienKhoaCmb = null;
            Assert.IsNull(viewModel.NienKhoaCmb);
            viewModel.LopDataCmb = null;
            Assert.IsNull(viewModel.LopDataCmb);
            viewModel.KhoiDataCmb = null;
            Assert.IsNull(viewModel.KhoiDataCmb);
            viewModel.MonDataCmb = null;
            Assert.IsNull(viewModel.MonDataCmb);
            viewModel.NienKhoaCmb2 = null;
            Assert.IsNull(viewModel.NienKhoaCmb2);
            viewModel.LopDataCmb2 = null;
            Assert.IsNull(viewModel.LopDataCmb2);
            viewModel.KhoiDataCmb2 = null;
            Assert.IsNull(viewModel.KhoiDataCmb2);
        }

        [TestMethod()]
        public void CountPercentageTest()
        {
            var result = viewModel.CountPercentage(1, 2);
            Assert.AreEqual("50%", result);
        }

    }
}
