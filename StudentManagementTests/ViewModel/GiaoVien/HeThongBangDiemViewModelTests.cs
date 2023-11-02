using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagement.ViewModel.GiaoVien;
using StudentManagement.ViewModel.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementTests.ViewModel.GiaoVien
{
    [TestClass()]
    public class HeThongBangDiemViewModelTests
    {
        HeThongBangDiemViewModel viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new HeThongBangDiemViewModel();
        }
        [TestMethod()]
        public void PropertiesTest()
        {
            viewModel.IdUser = 1;
            Assert.AreEqual(1, viewModel.IdUser);
            viewModel.everLoaded = true;
            Assert.AreEqual(true, viewModel.everLoaded);
            viewModel.JustReadOnly = true;
            Assert.AreEqual(true, viewModel.JustReadOnly);
            viewModel.CanUserEdit = true;
            Assert.AreEqual(true, viewModel.CanUserEdit);
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
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
            viewModel.HeThongBangDiemWD = null;
            Assert.IsNull(viewModel.HeThongBangDiemWD);
            viewModel.DanhSachDiem = null;
            Assert.IsNull(viewModel.DanhSachDiem);
            viewModel.NienKhoaCmb = null;
            Assert.IsNull(viewModel.NienKhoaCmb);
            viewModel.LopDataCmb = null;
            Assert.IsNull(viewModel.LopDataCmb);
            viewModel.KhoiDataCmb = null;
            Assert.IsNull(viewModel.KhoiDataCmb);
            viewModel.MonDataCmb = null;
            Assert.IsNull(viewModel.MonDataCmb);
        }

        [TestMethod()]
        public void KiemTraDiemHopLeTest()
        {
            viewModel.DanhSachDiem = new System.Collections.ObjectModel.ObservableCollection<StudentManagement.Model.HeThongDiem>()
            {
         
            };
            StudentManagement.Model.HeThongDiem diem = new StudentManagement.Model.HeThongDiem
            {
               Diem15Phut = 10,
               Diem1Tiet = 9,
               DiemTB = 8,
            };
            viewModel.DanhSachDiem.Add(diem);
            var result = viewModel.KiemTraDiemHopLe();
            Assert.IsTrue(result);
            StudentManagement.Model.HeThongDiem diem2 = new StudentManagement.Model.HeThongDiem
            {
                Diem15Phut = 12,
                Diem1Tiet = -4,
                DiemTB = 8,
            };
            viewModel.DanhSachDiem.Add(diem2);
            var result2 = viewModel.KiemTraDiemHopLe();
            Assert.IsFalse(result2);
        }

    }
}
