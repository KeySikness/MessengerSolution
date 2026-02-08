using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChatClient.Models;

namespace ChatClient.Controls
{
    public class DialogInfoPanel : Panel
    {
        private PictureBox _avatarBox;
        private Label _nameLabel;
        private Label _statusLabel;
        private Panel _statusDot;

        private ContactItem _currentContact;

        public DialogInfoPanel()
        {
            Height = 70;
            BackColor = Color.FromArgb(245, 245, 245);
            BorderStyle = BorderStyle.FixedSingle;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Аватар
            _avatarBox = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(50, 50),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.None
            };

            // Имя пользователя
            _nameLabel = new Label
            {
                Location = new Point(70, 15),
                Size = new Size(300, 22),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            // Статус (текст)
            _statusLabel = new Label
            {
                Location = new Point(90, 40),
                Size = new Size(200, 18),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent
            };

            // Статус (кружок)
            _statusDot = new Panel
            {
                Location = new Point(70, 43),
                Size = new Size(12, 12),
                BackColor = Color.Gray
            };

            Controls.Add(_avatarBox);
            Controls.Add(_nameLabel);
            Controls.Add(_statusLabel);
            Controls.Add(_statusDot);
        }

        public void SetContact(ContactItem contact)
        {
            _currentContact = contact;

            if (contact == null)
            {
                _nameLabel.Text = "";
                _statusLabel.Text = "";
                _avatarBox.Image = null;
                _statusDot.BackColor = Color.Gray;
                return;
            }

            _nameLabel.Text = contact.Username;
            _statusLabel.Text = contact.GetStatusText();
            _statusDot.BackColor = contact.GetStatusColor();

            // Устанавливаем аватар
            if (contact.Avatar != null)
            {
                _avatarBox.Image = CreateCircularImage(contact.Avatar, 50);
            }
            else
            {
                _avatarBox.Image = CreateDefaultAvatar(contact.Username, 50);
            }
        }

        public void UpdateStatus(UserStatus status)
        {
            if (_currentContact == null)
                return;

            _currentContact.Status = status;
            _statusLabel.Text = _currentContact.GetStatusText();
            _statusDot.BackColor = _currentContact.GetStatusColor();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Рисуем круглый статус dot
            if (_statusDot != null)
            {
                Graphics g = _statusDot.CreateGraphics();
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (SolidBrush brush = new SolidBrush(_statusDot.BackColor))
                {
                    g.FillEllipse(brush, 0, 0, _statusDot.Width, _statusDot.Height);
                }
            }
        }

        private Image CreateCircularImage(Image img, int size)
        {
            Bitmap result = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, size, size);
                    g.SetClip(path);
                    g.DrawImage(img, 0, 0, size, size);
                }
            }

            return result;
        }

        private Image CreateDefaultAvatar(string username, int size)
        {
            Bitmap result = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Фон
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, 150, 200)))
                {
                    g.FillEllipse(brush, 0, 0, size, size);
                }

                // Инициалы
                string initials = GetInitials(username);
                using (Font font = new Font("Segoe UI", size / 3, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    SizeF textSize = g.MeasureString(initials, font);
                    float x = (size - textSize.Width) / 2;
                    float y = (size - textSize.Height) / 2;
                    g.DrawString(initials, font, brush, x, y);
                }
            }

            return result;
        }

        private string GetInitials(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "?";

            string[] parts = username.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2)
                return (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();

            return username.Substring(0, Math.Min(2, username.Length)).ToUpper();
        }
    }
}