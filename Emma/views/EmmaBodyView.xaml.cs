using Emma.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Emma.views
{
    /// <summary>
    /// Interaction logic for EmmaBodyView.xaml
    /// </summary>
    public partial class EmmaBodyView : UserControl
    {
        public EmmaBodyView()
        {
            InitializeComponent();
            DataContext = new ViewModelQue();
            
        }

        private void Emma_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.basemodel.runtimedata.dragged=true;
            var window = Application.Current.MainWindow;
            window.DragMove();
            App.basemodel.runtimedata.dragged = false;
        }
    }
}
