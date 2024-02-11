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

namespace project_GUI
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        public Action<PauseAction>? ClickedAction;
        public PauseMenu()
        {
            InitializeComponent();
        }

        private void Quit_Btn_Click(object sender, RoutedEventArgs e)
        {
            ClickedAction?.Invoke(PauseAction.Quit);
        }

        private void Continue_Btn_Click(object sender, RoutedEventArgs e)
        {
            ClickedAction?.Invoke(PauseAction.Continue);
        }
    }
}
