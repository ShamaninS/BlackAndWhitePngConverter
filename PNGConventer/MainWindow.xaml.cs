using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PNGConventer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Uri _currentUriFile;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Событие нажатия на кнопку Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //инициализация и определение параметров диалогового окна для открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "*.png|*.png";
            if (openFileDialog.ShowDialog() == true) // показываем окно
            {
                try
                {
                    _currentUriFile = new Uri(openFileDialog.FileName); // пытаемся прочитать файл
                    CurrentImage.Source = new BitmapImage(_currentUriFile); // помещаем изображение в элемент CurrentImage
                }
                catch
                {
                    MessageBox.Show("Не удалось открыть файл");
                }
            }
        }
        
        /// <summary>
        /// Событие нажатия на кнопку Save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //инициализация и определение параметров диалогового окна для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "*.png|*.png";
            if (ConvertedImage.Source != null) // проверяем наличие конвертированного изображения
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (saveFileDialog.FileName != null ) // проверяем наличие пути к файлу
                    {
                        var encoder = new PngBitmapEncoder(); 
                        encoder.Frames.Add(BitmapFrame.Create(ConvertedImage.Source as BitmapSource));
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            encoder.Save(fileStream); // сохраняем конвертированное изображение
                        }
                    }
                }
            }
        }
            
        /// <summary>
        /// Событие нажатия на кнопку >>>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentImage.Source != null) // проверяем наличие исходного изображения
            {
                BitmapSource bitmapSource = new BitmapImage(_currentUriFile);
                ConvertedImage.Source = new FormatConvertedBitmap(bitmapSource, PixelFormats.Gray16, BitmapPalettes.Gray16, 0); // конвертируем и помещаем ч/б изображение в ConvertedImage
            }
        }
    }
}
