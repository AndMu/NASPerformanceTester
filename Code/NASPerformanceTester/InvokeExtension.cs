using System;
using System.Windows.Forms;

namespace NASPerformanceTester
{
    public static class InvokeExtension
    {
        public static void Execute(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
