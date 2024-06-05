using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace 班级点名器
{
    /// <summary>
    /// TopModeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TopModeWindow : Window
    {

        Point pressedPosition;
        bool isDragMoved = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double Width = SystemParameters.PrimaryScreenWidth / 15;
            double Height = SystemParameters.PrimaryScreenHeight / 15;

            this.MinHeight = Height;
            this.MaxHeight = Height;
            this.MinWidth = Width;
            this.MaxWidth = Width;
            this.Width = Width;
            this.Height = Height;
            this.Left = SystemParameters.PrimaryScreenWidth / 1.3;
            this.Top = SystemParameters.PrimaryScreenHeight / 1.5;
        }

        //判断是点击操作还是拖动
        public TopModeWindow()
        {
            InitializeComponent();
            
        }

        void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            pressedPosition = e.GetPosition(this);
            
        }
        void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && pressedPosition != e.GetPosition(this))
            {
                isDragMoved = true;
                DragMove();
            }
        }
        void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragMoved)
            {
                isDragMoved = false;
                e.Handled = true;
            }
        }


        //点名事件
        private void start_Click(object sender, RoutedEventArgs e)
        {
            int WindowCount = 0;
            foreach (var Window in App.Current.Windows)
            {
                WindowCount += 1;
            
            }
            if(WindowCount>2) return;
            var window= new TopModeWindow_StartWindow();
            window.Owner = this;
            window.Topmost = true;
            window.Show();

            return;
        }

        //关闭事件
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();

            this.Close();
            return;
        }

        
    }
}
