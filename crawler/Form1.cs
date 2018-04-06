using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pageInfo page = new pageInfo();
            page.FirstPageUrl = "https://dichvubds.vn/nha-dat-ban-ha-noi/page/1";
            page.PageURL = "https://dichvubds.vn/nha-dat-ban-ha-noi/page/{0}";
            //
            crawler robot = new crawler(page);
            robot.start();
        }
    }
}
