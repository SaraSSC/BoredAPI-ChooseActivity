using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTryTest
{
    public class DialogService
    {
        public void ShowMessage(string title, string message)
        {
            System.Windows.Forms.MessageBox.Show(message, title);
        }
    }
}
