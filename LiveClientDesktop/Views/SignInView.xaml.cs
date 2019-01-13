﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveClientDesktop.Views
{
    /// <summary>
    /// SignInView.xaml 的交互逻辑
    /// </summary>
    public partial class SignInView : UserControl
    {
        public SignInView(string domain, string liveId, string guid)
        {
            InitializeComponent();
            WebPageViewer viewer = new WebPageViewer(domain + "/miyun/clientTeaSign.aspx?liveid=" + liveId, domain, guid);
            MainGrid.Children.Insert(0, viewer);
        }
    }
}
