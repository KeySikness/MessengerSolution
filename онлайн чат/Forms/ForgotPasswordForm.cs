using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace онлайн_чат
{
    public partial class ForgotPasswordForm : Form
    {
        private HubConnection _connection;

        public ForgotPasswordForm()
        {
            InitializeComponent();
            _ = InitConnectionAsync();
        }

        private async Task InitConnectionAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5072/chat")
                .WithAutomaticReconnect()
                .Build();

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения:\n" + ex.Message);
            }
        }

        private async Task ResetPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Введите email/телефон и новый пароль");
                return;
            }

            try
            {
                await _connection.InvokeAsync(
                    "ResetPassword",
                    txtEmail.Text.Trim(),
                    txtNewPassword.Text.Trim()
                );

                MessageBox.Show("Пароль изменён");
                new LoginForm().Show();
                Close();
            }
            catch (HubException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            await ResetPasswordAsync();
        }

        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {
            // ничего не делаем
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            Hide();
        }
    }
}
