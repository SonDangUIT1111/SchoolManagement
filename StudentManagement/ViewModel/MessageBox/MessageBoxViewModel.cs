using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StudentManagement.ViewModel.MessageBox
{
    public class MessageBoxViewModel : BaseViewModel
    {
        #region commands
        public ICommand Loaded { get; set; }

        #endregion

        DispatcherTimer timer;

        int second = 2500;


        public MessageBoxViewModel()
        {
            Loaded = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                timer.Tick += Timer_Tick;
                timer.Start();

                void Timer_Tick(object sender, EventArgs e)
                {
                    second -= 500;
                    if (second < 1)
                    {
                        second = 2500;
                        timer.Stop();
                        p.Close();
                    }
                }
            }
            );
        }
    }
}

