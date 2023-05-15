using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Title_Bar_ViewModels
{
    public class Title_Bar_Log_ViewModel
    {

        // Icommand declare
        public ICommand MinimalWindowCommandLG { get; set; }
        public ICommand CloseWindowCommandLG { get; set; }
        public ICommand MoveWindowCommand { get; set; }


        public Title_Bar_Log_ViewModel()
        {
            MinimalWindowCommandLG = new RelayCommand<UserControl>((parameter) => { return parameter == null ? false : true; }, (parameter) =>
            {
                FrameworkElement window = GetWindowParent(parameter);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Minimized)
                        w.WindowState = WindowState.Minimized;
                    else
                        w.WindowState = WindowState.Maximized;
                }
            }
            );
            CloseWindowCommandLG = new RelayCommand<UserControl>((parameter) => { return parameter == null ? false : true; }, (parameter) =>
            {
                FrameworkElement window = GetWindowParent(parameter);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
           );
            MoveWindowCommand = new RelayCommand<UserControl>((parameter) => { return parameter == null ? false : true; }, (parameter) =>
            {
                FrameworkElement window = GetWindowParent(parameter);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            }
           );
        }
        FrameworkElement GetWindowParent(UserControl parameter)
        {
            FrameworkElement parent = parameter;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
