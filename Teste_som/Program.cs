using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Teste_som
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form Form1 = new Form1();
            Application.Run(Form1);
        }
    }
}
