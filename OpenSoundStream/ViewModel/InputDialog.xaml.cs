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

namespace OpenSoundStream.Views
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class CostumInputDialog : Window
    {
			public CostumInputDialog(string question, string defaultAnswer = "")
			{
				InitializeComponent();
				txtUser.Text = question;
				txtPW.Text = defaultAnswer;
			}

			private void btnDialogOk_Click(object sender, RoutedEventArgs e)
			{
				this.DialogResult = true;
			}

			private void Window_ContentRendered(object sender, EventArgs e)
			{
				txtPW.SelectAll();
				txtPW.Focus();
			}

		public string Answer
		{
			get { return txtPW.Text; }
		}
	
    }
}
