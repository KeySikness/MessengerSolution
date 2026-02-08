using System;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace онлайн_чат
{
    public partial class ConfirmEmailForm : Form
    {
        private string _code;
        public bool Confirmed { get; private set; } = false;

        public ConfirmEmailForm(string code)
        {
            InitializeComponent();
            _code = code;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == _code)
            {
                Confirmed = true;
                MessageBox.Show("Email подтвержден!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный код!");
            }
        }

        private void ConfirmEmailForm_Load(object sender, EventArgs e)
        {

        }
    }
}
