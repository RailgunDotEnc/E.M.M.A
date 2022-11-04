using Emma.Model.Main;
using System.Windows;

namespace Emma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.basemodel.memory.CreateRunData();
            App.basemodel.runtimedata.window = Application.Current.MainWindow.WindowState;
            new MainProgram(App.basemodel);
        }
    }
}
