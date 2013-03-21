using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DailyPhebus
{
    public partial class TimeListBox : ListBox
    {

        LoadForm.LoadType type;

        public TimeListBox()
        {
            
            Size = new Size(454, 290);
            Location = new Point(12, 140);
            DoubleBuffered = true;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 20;
            Font = new Font("Trebuchet MS", 10.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            ForeColor = Color.FromArgb(199, 200, 229);
            BorderStyle = BorderStyle.None;
            if (Items.Count % 2 != 0)
                BackColor = Color.FromArgb(49, 49, 68);
            else
                BackColor = Color.FromArgb(72, 71, 99);

        }


        public void setType(LoadForm.LoadType type)
        {
            this.type = type;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                if (e.Index % 2 == 0)
                    e.Graphics.FillRectangle(
                        new SolidBrush(Color.FromArgb(49, 49, 68)),
                        new Rectangle(0, e.Index * ItemHeight, Width, ItemHeight)
                    );
                else
                    e.Graphics.FillRectangle(
                        new SolidBrush(Color.FromArgb(72, 71, 99)),
                        new Rectangle(0, e.Index * ItemHeight, Width, ItemHeight)
                    );

                e.DrawBackground();
                var textRect = e.Bounds;
                textRect.Y -= 1;

                int offset = 3;
                if (type == LoadForm.LoadType.HourlySchedules && !Items[e.Index].ToString().Trim().Equals("-"))
                    offset = 6;

                string itemText = Items[e.Index].ToString().Substring(offset, Items[e.Index].ToString().Length - offset);
                string itemHour = Items[e.Index].ToString().Substring(0, offset);
                TextRenderer.DrawText(e.Graphics, itemHour, new Font("Trebuchet MS", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))), textRect, Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
               
                var textRectBis = e.Bounds;
                textRectBis.X = textRect.X + 10 * offset;
                TextRenderer.DrawText(e.Graphics, itemText, e.Font, textRectBis, e.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                e.DrawFocusRectangle();
            }
        }
    }
}
