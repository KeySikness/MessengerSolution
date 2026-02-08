using System;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class AddContactForm : Form
    {
        // Email или номер, введённый пользователем
        public string Login => txtLogin.Text.Trim();

        // Ник выбранного пользователя
        public string Nickname { get; private set; } = "";

        public AddContactForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите email или номер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Nickname = txtLogin.Text.Trim(); // временно, сервер вернёт настоящий ник
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AddContactForm_Load(object sender, EventArgs e)
        {
            txtLogin.Focus();
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
