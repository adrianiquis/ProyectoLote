﻿#pragma checksum "..\..\..\..\View\InicioView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5E9663DD905C4E19B2B1C178A31999668126138A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CProyectoLote.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using System.Windows.Shell;


namespace ProyectoLote.View {
    
    
    /// <summary>
    /// InicioMenu
    /// </summary>
    public partial class InicioMenu : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 79 "..\..\..\..\View\InicioView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl Titulo;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\View\InicioView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnInventario;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\View\InicioView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMenuVentas;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\..\View\InicioView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMenuInicio;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\..\View\InicioView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinimizar;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\View\InicioView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCerrar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProyectoLote;component/view/inicioview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\InicioView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Titulo = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 2:
            this.btnInventario = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\..\View\InicioView.xaml"
            this.btnInventario.Click += new System.Windows.RoutedEventHandler(this.btnInventario_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnMenuVentas = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\..\..\View\InicioView.xaml"
            this.btnMenuVentas.Click += new System.Windows.RoutedEventHandler(this.btnMenuVentas_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnMenuInicio = ((System.Windows.Controls.Button)(target));
            
            #line 110 "..\..\..\..\View\InicioView.xaml"
            this.btnMenuInicio.Click += new System.Windows.RoutedEventHandler(this.btnMenuInicio_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnMinimizar = ((System.Windows.Controls.Button)(target));
            
            #line 118 "..\..\..\..\View\InicioView.xaml"
            this.btnMinimizar.Click += new System.Windows.RoutedEventHandler(this.btnMinimizar_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnCerrar = ((System.Windows.Controls.Button)(target));
            
            #line 125 "..\..\..\..\View\InicioView.xaml"
            this.btnCerrar.Click += new System.Windows.RoutedEventHandler(this.btnCerrar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

