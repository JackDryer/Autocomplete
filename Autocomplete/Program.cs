using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Serilog;
namespace Autocomplete
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
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            try
            {
                Application.Run(new MainProgram());
            }
            catch (Exception e)
            {
                LogException(e,"In try catch");
                Application.Restart();
            }
        }
        private static void LogException(Exception e,string message)
        {
            Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("autocomplete.log", rollingInterval: RollingInterval.Infinite)
    .CreateLogger();
            Log.Error(e, message);
            Log.CloseAndFlush();
        }
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            LogException(e, "In thread");
            RestartApplication();
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogException(e.Exception, "in Application Thread");
            RestartApplication();
        }
        private static void RestartApplication()
        {
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }
    }
}