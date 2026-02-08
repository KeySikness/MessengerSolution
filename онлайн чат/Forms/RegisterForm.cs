using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace онлайн_чат
{
    public partial class RegisterForm : Form
    {
        private HubConnection _connection;

        public RegisterForm()
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
                MessageBox.Show(
                    "Не удалось подключиться к серверу:\n" + ex.Message,
                    "Ошибка подключения",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            try
            {
                await _connection.InvokeAsync<int>(
                    "Register",
                    txtFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    txtMiddleName.Text.Trim(),
                    txtUsername.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtPhone.Text.Trim(),
                    txtPassword.Text.Trim()
                );

                MessageBox.Show("Регистрация успешна");
                new LoginForm().Show();
                Hide();
            }
            catch (HubException ex) // ✅ КЛИЕНТСКИЙ
            {
                MessageBox.Show(ex.Message, "Ошибка регистрации");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            await RegisterAsync();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            Hide();
        }
    }
}
