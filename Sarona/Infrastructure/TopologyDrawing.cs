using Sarona.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Sarona.Infrastructure
{
    public class TopologyDrawing
    {
        private Point origin = new Point(0, 0);

        private void DrawExchangeBox(IEnumerable<Exchange> exchanges, Graphics g, Point p)
        {
            foreach (var exch in exchanges)
            {

            }
        }
        public void Create(IEnumerable<Exchange> exchanges)
        {
            Point origin = new Point();

            var pageWidth = 1920;
            var pageHeight = 1080;
            Bitmap bitmap = new Bitmap(pageWidth, pageHeight);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, pageWidth + 10, pageHeight + 10));

            int exchangeWidth = (pageWidth - 20 * exchanges.Count() + 20) / exchanges.Count();
            int exchangeHeight = 1000;
            origin.X = 0;
            origin.Y = 7000;
            for (int i = 0; i < exchanges.Count(); i++)
            {

                origin.X = 20 + i * (exchangeWidth + 20);
                g.DrawRectangle(new Pen(Color.Black) { DashStyle = DashStyle.Dash, Width= 10}, origin.X, origin.Y, exchangeWidth, exchangeHeight);
            }







            bitmap.Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.png"));
        }
    }
}
