using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyResourceHacker
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelectedValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Return )
            {
                this.Close();
            }
        }
    }
}
