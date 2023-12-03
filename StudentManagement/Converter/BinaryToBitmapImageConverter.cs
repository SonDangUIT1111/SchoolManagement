using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StudentManagement.Converter
{
    public class ByteArrayToBitmapImageConverter 
    {
        //public BitmapImage ConvertByteArrayToBitMapImage(byte[] imageByteArray)
        //{
        //    BitmapImage img = new BitmapImage();
        //    using (MemoryStream memStream = new MemoryStream(imageByteArray))
        //    {
        //        img.BeginInit();
        //        img.CacheOption = BitmapCacheOption.OnLoad;
        //        img.StreamSource = memStream;
        //        img.EndInit();
        //        img.Freeze();
        //    }
        //    return img;
        //}



        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    BitmapImage img = new BitmapImage();
        //    if (value != null)
        //    {
        //        img = this.ConvertByteArrayToBitMapImage(value as byte[]);
        //    }
        //    return img;
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return null;
        //}
        public byte[] ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }
    }
}
