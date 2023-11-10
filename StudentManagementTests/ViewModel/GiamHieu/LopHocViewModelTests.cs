﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentManagement.ViewModel.GiamHieu;
using StudentManagement.ViewModel.Services;
using StudentManagement.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace StudentManagementTests.ViewModel.GiamHieu
{
    [TestClass]
    public class LopHocViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private LopHocViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new LopHocViewModel();
        }
        [TestMethod]
        public void NienKhoaCmb()
        {
            ObservableCollection<string> _testNienKhoa = new ObservableCollection<string>();
            _testNienKhoa.Add("2019-2020");
            viewModel.NienKhoaCmb = _testNienKhoa;
            Assert.AreEqual(_testNienKhoa, viewModel.NienKhoaCmb);  
        }
        [TestMethod]
        public void KhoiComb()
        {
            ObservableCollection<string> _testKhoi = new ObservableCollection<string>();
            _testKhoi.Add("10");
            viewModel.NienKhoaCmb = _testKhoi;
            Assert.AreEqual(_testKhoi, viewModel.NienKhoaCmb);
        }
        [TestMethod]
        public void GridSelectedItem()
        {
            StudentManagement.Model.Lop _testLop = new Lop();
            _testLop.TenLop = "10A1";
            viewModel.GridSelectedItem = _testLop;
            Assert.AreEqual(_testLop, viewModel.GridSelectedItem);
        }
        [TestMethod]
        public void DataGridVisibility()
        {
            viewModel.DataGridVisibility = true;
            Assert.AreEqual(true, viewModel.DataGridVisibility);
        }
        [TestMethod]
        public void ProgressBarVisibility()
        {
            viewModel.ProgressBarVisibility = true;
            Assert.AreEqual(true, viewModel.ProgressBarVisibility);
        }
        [TestMethod]
        public void LoadComboBox_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {});

            var sut = new LopHocViewModel(fakeSqlConnection.Object);
            try
            {
                sut.LoadComboBox();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public async Task LoadDanhSachLopHoc()
        {
            ObservableCollection<StudentManagement.Model.Lop> _testDanhSachLop = new ObservableCollection<StudentManagement.Model.Lop>();
            ObservableCollection<string> _testNienKhoaCmb = new ObservableCollection<string>();
            ObservableCollection<Khoi> _testKhoiCmb = new ObservableCollection<Khoi>();


            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {});

            var sut = new LopHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.DanhSachLopHoc = _testDanhSachLop;
                sut.NienKhoaCmb = _testNienKhoaCmb;
                sut.KhoiCmb = _testKhoiCmb;
                sut.NienKhoaQueries = null;
                sut.KhoiQueries = null;
                await sut.LoadDanhSachLopHoc();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task LoadDanhSachLopHoc_KhongGVCN()
        {
            ObservableCollection<StudentManagement.Model.Lop> _testDanhSachLop = new ObservableCollection<StudentManagement.Model.Lop>();

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            { });

            var sut = new LopHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.DanhSachLopHoc = _testDanhSachLop;
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "3";
                await sut.LoadDanhSachLopHoc();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void FilterFromTenLop_ValueInputed()
        {
            ObservableCollection<StudentManagement.Model.Lop> _testDanhSachLop = new ObservableCollection<StudentManagement.Model.Lop>();

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {});

            var sut = new LopHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.DanhSachLopHoc = _testDanhSachLop;
                sut.NienKhoaQueries = "2023-2024";
                sut.KhoiQueries = "1";
                sut.TenLopQueries = "10A1";
                sut.FilterFromTenLop();
            } catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void FilterFromTenLop_ValueEmpty()
        {
            ObservableCollection<StudentManagement.Model.Lop> _testDanhSachLop = new ObservableCollection<StudentManagement.Model.Lop>();

            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();
            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            { });

            var sut = new LopHocViewModel(fakeSqlConnection.Object);

            try
            {
                sut.DanhSachLopHoc = _testDanhSachLop;
                sut.NienKhoaQueries = "";
                sut.KhoiQueries = "";
                sut.TenLopQueries = "";
                sut.FilterFromTenLop();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}