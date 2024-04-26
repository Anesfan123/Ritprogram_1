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
    public partial class Form1 : Form
    {

        private readonly Bitmap bm = new Bitmap(800, 600);//Skapar ny bitmap 
        public Penna Penna; // Skapa en instans av Penna
        private Point previousPoint;
        private Point startPoint; // För att hålla koll på linjens startpunkt
        private Point endPoint; // För att hålla koll på linjens slutpunkt
        //Deklarerar Privata bool variabler
        private bool Drawing = false;
        private bool Sudda = false;
        private bool DrawingLine = false; // Kollar vilken av ritfunktionerna som ska användas
        private bool DrawingRectangel = false;
        private bool DrawingEllipse = false; 
        Color new_Color;

        public Form1()
        {
            InitializeComponent();
            Penna = new Penna(Color.Black, 4.0f); // Skapa en Penna-instans med standardfärg och tjocklek  
            pic.Image = bm;
        }
        
        private void btn_Rensa_Click(object sender, EventArgs e)
        {
            // Rensa PictureBox genom att rita en tom bild
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
            }

            // Uppdatera PictureBox för att visa den tomma bilden
            pic.Invalidate();
            DrawingLine = false;
            DrawingEllipse = false;
            DrawingRectangel = false;
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            Drawing = true;
            previousPoint = e.Location;

            if (e.Button == MouseButtons.Left) //Om vänster mussknapp trycks ner startas händelsen 
            {
                // Händelse: Rita rektangel
                if (DrawingRectangel == true)
                {
                    startPoint = e.Location; // Spara startpunkten för linjen
                    endPoint = e.Location;  // Sätt slutpunkten till samma som startpunkten
                }
                else
                {
                    Drawing = false;  // Flagga för att indikera att vi ritar
                    previousPoint = e.Location; // Spara musens position som den tidigare punkten
                }
            }
           
            if (e.Button == MouseButtons.Left)
            {
                //Händelse: Rita linje
                if (DrawingLine == true)
                {
                    startPoint = e.Location; 
                    endPoint = e.Location;   
                }
                else
                {
                    
                    Drawing = false;             
                    previousPoint = e.Location;   
                }
            }

            if(e.Button == MouseButtons.Left)
            {
                //Händelse: Ritar Ellipse
                if(DrawingEllipse == true)
                {
                    startPoint = e.Location;
                    endPoint = e.Location;
                }
                else
                {
                Drawing = false;
                previousPoint = e.Location;
                }
            }
        }

        public void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drawing == true)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    // Använd Penna för att rita
                    Penna.Rita(previousPoint, e.Location, g);
                }
                previousPoint = e.Location;
                pic.Invalidate();
            }
            if (Sudda)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    // Kallar på Sudda funktionen
                    Penna.Sudda(previousPoint, e.Location, g);
                }
                previousPoint = e.Location;
                pic.Invalidate();
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            Drawing = false;
            previousPoint = e.Location;
            Sudda = false;

            if (e.Button == MouseButtons.Left)
            {
                if (DrawingRectangel== true)
                {
                    endPoint = e.Location;
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        //Tar alla koordinater och gör om de till variabler.
                        Pen pen = new Pen(Penna.FärgPenna, Penna.tjocklek);
                        int a = Math.Min(startPoint.X, endPoint.X);
                        int b = Math.Min(startPoint.Y, endPoint.Y);
                        int c = Math.Abs(endPoint.X - startPoint.X);
                        int d = Math.Abs(endPoint.Y - startPoint.Y);
                        g.DrawRectangle(pen, new Rectangle(a, b, c, d));
                    }
                    pic.Invalidate();
                }
                else
                {
                    Drawing = false;
                }
            }
         
            if (e.Button == MouseButtons.Left)
            {
                if (DrawingLine == true )
                {
                    // Användaren ritar en rak linje
                    endPoint = e.Location; // Spara slutpunkten för linjen
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        Pen pen = new Pen(Penna.FärgPenna, Penna.tjocklek);
                        g.DrawLine(pen, startPoint, endPoint); // Rita linjen från startpunkt till slutpunkt
                    }
                    pic.Invalidate(); // Uppdatera PictureBox för att visa ritningen
                }
                else
                {
                    // Användaren ritar en frihandlinje
                    Drawing = false; // Sluta rita
                }
            } 
            
            if (DrawingEllipse == true)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    Pen pen = new Pen(Penna.FärgPenna, Penna.tjocklek);
                    g.DrawEllipse(pen, new Rectangle(endPoint.X, endPoint.Y, (e.Location.X - endPoint.X), (e.Location.Y - endPoint.Y)));
                }
                pic.Invalidate();
            }
            else
            {
                Drawing = false;
            }
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, Point.Empty);
        } 
        private void Färg_Click(object sender, EventArgs e)
        {
            Penna.ÄndraFärg();//Kopplar Ändrafärg funktionen till eventet färgclick 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Penna.ÄndraTjocklek(trackBar1.Value); //Kopplar ÄndraTjocklek funktionen till eventet trackbar1
        }

        //Händelsehanterar för vad som händer när användaren klickar på de olika knapparna 
        private void btn_Linje_Click(object sender, EventArgs e)
        {
            DrawingLine = true;
            DrawingRectangel = false;
            DrawingEllipse = false;
            Drawing = false;
            Sudda = false;
        }

        private void btn_Cirkel_Click(object sender, EventArgs e)
        {
            DrawingEllipse = true;
            DrawingRectangel = false;
            DrawingLine = false;
            Drawing = false;
            Sudda = false;
        }

        private void btn_Rektangel_Click(object sender, EventArgs e)
        {
            DrawingRectangel = true;
            DrawingEllipse = false;
            DrawingLine = false;
            Drawing = false;
            Sudda = false;
        }

        private void btn_Penna_Click(object sender, EventArgs e)
        {
            Drawing = true;
            Sudda = false;
            DrawingRectangel = false;
            DrawingLine = false;
            DrawingEllipse = false;
        }

        private void btn_Sudd_Click(object sender, EventArgs e)
        {
            Sudda = true;
            Drawing = false;
            DrawingEllipse = false;
            DrawingLine = false;
            DrawingRectangel = false;
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            //Gör det möjligt att ändra pic_color 
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }
 
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            //Gör det möjligt att välja en färg utifrån färgerna i pictureboxen
            Point point = set_point(pictureBox2, e.Location);
            pictureBox2.BackColor = ((Bitmap)pictureBox2.Image).GetPixel(point.X, point.Y);
            new_Color = pictureBox2.BackColor;
            Penna.FärgPenna = pictureBox2.BackColor;
        }

        public void Save()
        {
            //Metod som sparar användarens målning 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG files (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Spara bilden på den valda sökvägen som PNG
                bm.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
        }

        private void btn_Spara_Click(object sender, EventArgs e)
        {
            Save(); //Kallar på save funktionen
        }

    }
    
}
