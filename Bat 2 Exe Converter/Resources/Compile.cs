using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Compile
{
    class Compile
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = "cmd.exe";
            process.WindowStyle = ProcessWindowStyle.%mode%;
            process.Arguments = @"/c %code%";
	        Process.Start(process);
        }
    }
}