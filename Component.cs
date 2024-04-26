using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ritprogram1
{
    public class Penna
    {
        public Color FärgPenna;
        public float tjocklek;
        public Color FärgSudd;

        public Penna(Color färg, float tjocklek )
        {
            this.FärgPenna = färg;
            this.tjocklek = tjocklek;
            this.FärgSudd = Color.White;
        }

        // Metod som ändrar pennans färg genom colorDialog 
        public void ÄndraFärg()
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                FärgPenna = colorDialog.Color;
            }
        }

        // Metod som ändrar tjocklek
        public void ÄndraTjocklek(float nyTjocklek)
        {
            tjocklek = nyTjocklek;
        }

        public void Rita(Point StartPoint, Point EndPoint, Graphics g)
        {
            // Penna skapas med aktuell färg och tjocklek
            using (Pen pen = new Pen(FärgPenna, tjocklek))
            {
                g.DrawLine(pen, StartPoint, EndPoint);
            }
        }

        public void Sudda(Point startPoint, Point EndPoint, Graphics g)
        {
            //Sudd skapas med färgen vit och tjocklek som påverkas av nyTjocklek
            using(Pen p = new Pen(FärgSudd, tjocklek))
            {
                g.DrawLine(p, startPoint, EndPoint);
            }

        }
    }
}
