﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string s = Sercurity.Encrypt.DecryptConn("7qgZIaEIJKPOSnfkg1n5OA==");
            //MessageBox.Show(s);
            var o = lunadate.Solar2Lunar(DateTime.Now);
        }
    }
}
