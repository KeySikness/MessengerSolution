using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChatClient.Models;

namespace ChatClient.Controls
{
    public class ContactListBox : ListBox
    {
        private const int ITEM_HEIGHT = 60;
        private const int AVATAR_SIZE = 48;
        private const int PADDING = 6;
        private const int STATUS_DOT_SIZE = 12;

        public ContactListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = ITEM_HEIGHT;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= Items.Count)
                return;

            ContactItem contact = Items[e.Index] as ContactItem;
            if (contact == null)
            {
                base.OnDrawItem(e);
                return;
            }

            e.DrawBackground();

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Фон для выбранного элемента
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(230, 240, 255)))
                {
                    g.FillRectangle(brush, e.Bounds);
                }
            }

            // Позиции
            int x = e.Bounds.X + PADDING;
            int y = e.Bounds.Y + (e.Bounds.Height - AVATAR_SIZE) / 2;

            // Рисуем аватар
            Rectangle avatarRect = new Rectangle(x, y, AVATAR_SIZE, AVATAR_SIZE);
            DrawAvatar(g, contact, avatarRect);

            // Рисуем статус (кружок)
            int statusX = x + AVATAR_SIZE - STATUS_DOT_SIZE - 2;
            int statusY = y + AVATAR_SIZE - STATUS_DOT_SIZE - 2;
            DrawStatusDot(g, contact.GetStatusColor(), statusX, statusY);

            // Рисуем имя пользователя
            int textX = x + AVATAR_SIZE + PADDING * 2;
            int textY = y + 10;

            using (Font font = new Font("Segoe UI", 11, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                g.DrawString(contact.Username, font, brush, textX, textY);
            }

            // Рисуем статус текстом
            int statusTextY = textY + 20;
            using (Font font = new Font("Segoe UI", 9, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.Gray))
            {
                g.DrawString(contact.GetStatusText(), font, brush, textX, statusTextY);
            }

            e.DrawFocusRectangle();
        }

        private void DrawAvatar(Graphics g, ContactItem contact, Rectangle rect)
        {
            // Создаем круглый аватар
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                g.SetClip(path);

                if (contact.Avatar != null)
                {
                    // Рисуем изображение
                    g.DrawImage(contact.Avatar, rect);
                }
                else
                {
                    // Рисуем заглушку с инициалами
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, 150, 200)))
                    {
                        g.FillEllipse(brush, rect);
                    }

                    // Инициалы
                    string initials = GetInitials(contact.Username);
                    using (Font font = new Font("Segoe UI", 16, FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        SizeF size = g.MeasureString(initials, font);
                        float initialsX = rect.X + (rect.Width - size.Width) / 2;
                        float initialsY = rect.Y + (rect.Height - size.Height) / 2;
                        g.DrawString(initials, font, brush, initialsX, initialsY);
                    }
                }

                g.ResetClip();
            }

            // Рамка аватара
            using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 2))
            {
                g.DrawEllipse(pen, rect);
            }
        }

        private void DrawStatusDot(Graphics g, Color color, int x, int y)
        {
            // Белая подложка
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, x - 2, y - 2, STATUS_DOT_SIZE + 4, STATUS_DOT_SIZE + 4);
            }

            // Цветной кружок статуса
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, x, y, STATUS_DOT_SIZE, STATUS_DOT_SIZE);
            }

            // Рамка
            using (Pen pen = new Pen(Color.White, 2))
            {
                g.DrawEllipse(pen, x, y, STATUS_DOT_SIZE, STATUS_DOT_SIZE);
            }
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