using System;
using System.Reflection;
using System.Windows.Forms;

namespace CloudFlareDDNS
{


    /// <summary>
    /// About box form
    /// </summary>
    public partial class frmAbout : Form
    {
        /// <summary>
        /// Form entry point
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string copyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            string description = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;

            txtDescription.Text = description;
            txtVersion.Text = version;
            txtCopyright.Text = copyright;

        }//end frmAbout()


        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }//end btnClose_Click()


    }//end class
}//end namespace
