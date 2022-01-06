﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BatchRenamer.Controls
{
    public class CustomListBox : HeaderedItemsControl 
    {
        public static readonly DependencyProperty ToolBarProperty =
            DependencyProperty.Register("ToolBar", typeof(StackPanel), typeof(CustomListBox));
        public StackPanel ToolBar
        {
            get { return (StackPanel)GetValue(ToolBarProperty); }
            set { SetValue(ToolBarProperty, value); }
        }
    }
}