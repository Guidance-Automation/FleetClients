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
using GACore.Controls;

namespace FleetClients.Controls
{
	/// <summary>
	/// Interaction logic for KingpinStateControl.xaml
	/// </summary>
	public partial class KingpinStateControl : UserControl
	{
		public KingpinStateControl()
		{
			InitializeComponent();
                        
            GACore.Controls.KingpinStatusControl foo = null;
		}
	}
}
