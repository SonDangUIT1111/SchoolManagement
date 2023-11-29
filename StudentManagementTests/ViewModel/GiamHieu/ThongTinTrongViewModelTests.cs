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
    public class ThongTinTruongViewModelTests
    {
        private ThongTinTruongViewModel viewModel;
        [TestInitialize]
        public void TestInitialize()
        {
            viewModel = new ThongTinTruongViewModel();
        }
        [TestMethod]
        public void LoadThongTInTruong()
        {
            viewModel.ThongTinTruongImageSource = @"D:\StudentManagement\StudentManagement\Resources\Images\ThongTinTruong1.png";
            Assert.AreEqual(@"D:\StudentManagement\StudentManagement\Resources\Images\ThongTinTruong1.png", viewModel.ThongTinTruongImageSource);
            viewModel.ImageNum = 1;
            Assert.AreEqual(1, viewModel.ImageNum);
            viewModel.GetImage();
            Assert.AreEqual(@"D:\StudentManagement\StudentManagement\Resources\Images\ThongTinTruong1.png", viewModel.ThongTinTruongImageSource);
        }
    }
}
