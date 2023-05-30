using StudentManagement.Views.GiamHieu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StudentManagement.ViewModel.GiamHieu
{
    internal class ThongTinTruongViewModel:BaseViewModel
    {

        private string _thongTinTruongImageSource;

        public string ThongTinTruongImageSource {

            get => _thongTinTruongImageSource;
            set
            {
                _thongTinTruongImageSource = value;
                OnPropertyChanged();
            }

        }

        private int _imageNum;

        public int ImageNum
        {
            get => _imageNum;
            set
            {
                _imageNum = value;
                OnPropertyChanged();
            }
        }

        public ICommand PreviousImage { get; set; }
        public ICommand NextImage { get; set; }

        public ThongTinTruongViewModel()
        {
            GetImage();


            PreviousImage = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (ImageNum > 1)
                {
                    ImageNum--;
                    ThongTinTruongImageSource = @"D:\StudentManagement\StudentManagement\Resources\Images\ThongTinTruong" + ImageNum + ".png";
                }
            });

            NextImage = new RelayCommand<object>((parameter) => { return true; }, (parameter) =>
            {
                if (ImageNum < 2)
                {
                    ImageNum++;
                    ThongTinTruongImageSource = @"D:\StudentManagement\StudentManagement\Resources\Images\ThongTinTruong" + ImageNum + ".png";
                }
            });
        }
            

        public void GetImage()
        {
            ImageNum = 1;
            ThongTinTruongImageSource = @"D:\StudentManagement\StudentManagement\Resources\Images\ThongTinTruong1.png";
        }

    }
}
