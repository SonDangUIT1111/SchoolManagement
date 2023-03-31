using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace StudentManagement.ViewModel.Menu 
{
    internal class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //}
        private string _tex="123123";

		public string Tex
		{
			get { return _tex; }
			set { _tex = value;
                OnPropertyChanged(nameof(Tex));
            }
		}
        class RelayCommand<T> : ICommand
        {
            private readonly Predicate<T> _canExecute;
            private readonly Action<T> _execute;

            public RelayCommand(Predicate<T> canExecute, Action<T> execute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");
                _canExecute = canExecute;
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
        public ICommand NavigatePage { get ; set; }

        public TestViewModel() {
            NavigatePage = new RelayCommand<StudentManagement.Views.Menu.Menu>((paramater) => { return true; }, (paramater) =>
            {
                //_PageContent = new StudentManagement.Views.Menu.BaoCao();
                //this._PageContent = new StudentManagement.Views.Menu.BaoCao();
                //paramater.RPage.Content = new StudentManagement.Views.Menu.BaoCao();
                MessageBox.Show(_tex);
                Tex = "seckk";

            });
            //Task.Run(() =>
            //{
            //while (true)
            //{
            //Random r = new Random();    
            //Tex=r.Next(1,1000).ToString();
            //MessageBox.Show(Tex);
            //Trace.WriteLine($"Tex: {Tex}");
            //Thread.Sleep(1000);
            // }
            // });

        }
    }
}
