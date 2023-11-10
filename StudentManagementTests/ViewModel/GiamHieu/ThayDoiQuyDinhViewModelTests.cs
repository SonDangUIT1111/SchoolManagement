﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class ThayDoiQuyDinhViewModelTests
    {
        private Mock<IDatabaseService> mockDatabaseService;
        private ThayDoiQuyDinhViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThayDoiQuyDinhViewModel();
        }
        [TestMethod]
        public void LoadThongTinCmb_GetValue()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            ObservableCollection<StudentManagement.Model.QuiDinh> _testDanhSachQuyDinh = new ObservableCollection<StudentManagement.Model.QuiDinh>();

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThayDoiQuyDinhViewModel(fakeSqlConnection.Object);

            try
            {
                sut.DanhSachQuyDinh = _testDanhSachQuyDinh;
                sut.LoadThongTinCmb();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void LoadQuyDinhFromSelection()
        {
            var fakeSqlConnection = new Mock<ISqlConnectionWrapper>();

            ObservableCollection<StudentManagement.Model.QuiDinh> _testDanhSachQuyDinh = new ObservableCollection<StudentManagement.Model.QuiDinh>();

            string _testQuyDinhQueries = "Sĩ số tối đa";

            fakeSqlConnection.Setup(wrapper => wrapper.Open()).Callback(() =>
            {
                // Custom logic to simulate opening the connection
                // You can add code here for your test scenario
            });

            var sut = new ThayDoiQuyDinhViewModel(fakeSqlConnection.Object);

            try
            {
                sut.QuyDinhQueries = _testQuyDinhQueries;
                sut.LoadQuyDinhFromSelection();
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }



        }
    }    
}