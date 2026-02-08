using ChatClient;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace онлайн_чат
{
    public partial class LoginForm : Form
    {
        private HubConnection _connection;

        public LoginForm()
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
                MessageBox.Show("Ошибка подключения:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите email/номер и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int userId = await _connection.InvokeAsync<int>(
                    "Login",
                    txtEmail.Text.Trim(),
                    txtPassword.Text.Trim()
                );

                Hide();
                new ChatClient.ChatForm(userId).ShowDialog();
                Close();
            }
            catch (HubException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            await LoginAsync();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void btnForgot_Click(object sender, EventArgs e)
        {
            new ForgotPasswordForm().Show();
            Hide();
        }
    }
}