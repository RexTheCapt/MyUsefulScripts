using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageDifference
{
    public partial class FormSetPath : Form
    {
        public string[] path => textBox1.Text.Split(';');

        public FormSetPath()
        {
            InitializeComponent();
            buttonAccept.Enabled = false;

            textBox1.Text = LastPath.Default.Path;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            LastPath.Default.Path = textBox1.Text;
            LastPath.Default.Save();
            
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (AllPathsExists())
            {
                buttonAccept.Enabled = true;
            }
            else
                buttonAccept.Enabled = false;
        }

        private bool AllPathsExists()
        {
            foreach (string path in textBox1.Text.Split(';'))
            {
                if (!Directory.Exists(path))
                    return false;
            }
            return true;
        }
    }
}
