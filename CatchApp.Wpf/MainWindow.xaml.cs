using Abp.Authorization.Users;
using Abp.Dependency;
using CatchApp.Clubs;
using CatchApp.Users;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatchApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISingletonDependency
    {
        IClubAppService _clubAppService;
        UserManager _userManager;

        public MainWindow()
        {
            InitializeComponent();
            _clubAppService = IocManager.Instance.Resolve<IClubAppService>();
            _userManager = IocManager.Instance.Resolve<UserManager>();
            
        }

        private async void UebernahmeClubImages_Click(object sender, RoutedEventArgs e)
        {
            var loginResult = await _userManager.LoginAsync("hans", "hans", "default");
            if (loginResult.Result == AbpLoginResultType.Success)
            {
            }
            else
                MessageBox.Show("Anmeldung fehlgeschlagen!");
        }
    }
}
