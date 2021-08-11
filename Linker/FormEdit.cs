using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Linker
{
    internal partial class FormEdit : Form
    {
        internal LinkInfo LinkInfo;

        internal FormEdit(LinkInfo linkInfo)
        {
            InitializeComponent();

            LinkInfo = linkInfo;

            textBoxFrom.TextChanged += TextBox_TextChanged;
            textBoxTo.TextChanged += TextBox_TextChanged;

            buttonAccept.Click += ButtonAccept_Click;
            buttonCancel.Click += ButtonCancel_Click;

            textBoxFrom.Text = linkInfo.From;
            textBoxTo.Text = linkInfo.To;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            LinkInfo = new LinkInfo(textBoxFrom.Text, textBoxTo.Text);
            DialogResult = DialogResult.OK;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (Directory.Exists(tb.Text))
            {
                buttonAccept.Enabled = true;
                tb.BackColor = Color.FromName("Window");
            }
            else
            {
                buttonAccept.Enabled = false;
                tb.BackColor = Color.Red;
            }
        }
    }
}
