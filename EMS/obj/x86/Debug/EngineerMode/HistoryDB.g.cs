﻿#pragma checksum "..\..\..\..\EngineerMode\HistoryDB.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1ED83CC1487E8BD8A04C3BAB8DEB3739EE4C4380"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace EMS.EngineerMode {
    
    
    /// <summary>
    /// HistoryDB
    /// </summary>
    public partial class HistoryDB : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.DataGrid dg_list;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_search;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_save;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.DatePicker dp_to;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Windows.Controls.DatePicker dp_from;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_partID;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_equipID;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ck_returnWithExpiry;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\EngineerMode\HistoryDB.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ck_summaryReport;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/EMS;component/engineermode/historydb.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\EngineerMode\HistoryDB.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.dg_list = ((Microsoft.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            this.btn_search = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\..\EngineerMode\HistoryDB.xaml"
            this.btn_search.Click += new System.Windows.RoutedEventHandler(this.btn_search_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_save = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\..\EngineerMode\HistoryDB.xaml"
            this.btn_save.Click += new System.Windows.RoutedEventHandler(this.btn_save_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dp_to = ((Microsoft.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.dp_from = ((Microsoft.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.txt_partID = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.txt_equipID = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.ck_returnWithExpiry = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.ck_summaryReport = ((System.Windows.Controls.CheckBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

