// Autor: Gael Zair Limon Rdz
using System;
using System.Windows.Forms;

namespace SistemaLogisticaAuditoria
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
