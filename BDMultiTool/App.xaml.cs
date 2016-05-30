﻿using BDMultiTool.Macros;
using BDMultiTool.Utilities;
using BDMultiTool.Utilities.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BDMultiTool {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application {
        public const String version = "0.1";
        public static volatile bool appCoreIsInitialized = false;
        public static volatile WindowAttacher windowAttacher;
        public static volatile bool minimized;
        public static volatile Overlay overlay;

        public App() {
            minimized = false;

            overlay = new Overlay();
            windowAttacher = new WindowAttacher(WindowAttacher.getHandleByWindowTitleBeginningWith("Black Desert"), overlay);

            appCoreIsInitialized = true;

            letAllComponentsRegister();
        }


        private void letAllComponentsRegister() {
            Type multiToolType = typeof(MultiToolMarkUpThread);
            
            foreach (Assembly currentAssembly in AppDomain.CurrentDomain.GetAssemblies()) {
                Type[] currentType = currentAssembly.GetTypes();

                foreach(Type currentInnerType in currentType) {
                    if (multiToolType.IsAssignableFrom(currentInnerType) && !currentInnerType.IsInterface) {
                        Activator.CreateInstance(currentInnerType);
                    }
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e) {
            CustomNotifyIcon.getInstance();
            base.OnStartup(e);
        }

        public static void exit() {
            CustomNotifyIcon.dispose();
            Environment.Exit(0);
        }
    }
}