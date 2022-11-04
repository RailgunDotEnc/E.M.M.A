using Emma.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Emma
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static BaseModel? basemodel;
        protected override void OnStartup(StartupEventArgs e)
        {
            basemodel = new();
            base.OnStartup(e);
        }
        
    }
}
