using System;
using System.Windows;
using System.Windows.Threading;

namespace XamlTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application app = new Application();
            app.Run(new XamlWindow());
        }
    }
}