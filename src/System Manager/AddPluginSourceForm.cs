using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireBlade.SystemManager
{
    public partial class AddPluginSourceForm : Form
    {
        public AddPluginSourceForm()
        {
            InitializeComponent();
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                button1.Enabled = false;
                return;
            }

            // Validate that the string the user provided is a valid repository.
            // First, split by a slash to retrieve both sides of the string.
            string[] sides = textBox2.Text.Split('/');

            // If there are more or less than 2 sides, this is an invalid format.
            if (sides.Length != 2)
            {
                e.Cancel = true;

                // if we specify null, windows will make the caption localized "Error"
                MessageBox.Show("The repository must be in the format: User/Repository.", null, MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // For each of the sides, check if they only use URL-safe characters.
            foreach (var s in sides)
                if (!Regex.IsMatch(s, @"^[A-Za-z0-9\-._~]+$"))
                {
                    e.Cancel = true;

                    MessageBox.Show("The repository you provided is not a valid repository name.", null,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            // If both tests fail, then we count it as a valid repository name.
            button1.Enabled = true;
        }
    }
}
