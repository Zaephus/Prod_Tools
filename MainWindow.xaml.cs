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

namespace Prod_Tools {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        public void Update() {
            if((Keyboard.GetKeyStates(Key.Enter) & KeyStates.Down) > 0) {
                ButtonAddName_Click(null, null);
            }
        }

        private void ButtonAddName_Click(object _sender, RoutedEventArgs _e) {
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !lstNames.Items.Contains(txtName.Text)) {
                lstNames.Items.Add(txtName.Text);
                txtName.Clear();
            }
        }

    }

}