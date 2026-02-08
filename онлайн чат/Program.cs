using ChatClient;
using System;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;


namespace онлайн_чат
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
