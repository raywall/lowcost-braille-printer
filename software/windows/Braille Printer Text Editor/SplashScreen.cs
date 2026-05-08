using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Braille_Printer_Editor
{
    public partial class SplashScreen : Form
    {
        bool Splash;
        Timer timer;

        public SplashScreen(bool splash = true)
        {
            InitializeComponent();
            Splash = splash;
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            closeButton.Visible = !Splash;

            try
            {
                versionLabel.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
            }
            catch { }
        }

        private void SplashScreen_Shown(object sender, EventArgs e)
        {
            if (Splash)
            {
                timer = new Timer();
                timer.Interval = 4000;
                timer.Start();

                timer.Tick += Timer_Tick;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            var editor = new EditorForm();
            editor.Show();

            Hide();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
