using CurseDimaBilash.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurseDimaBilash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Color backgroundColor;
        struct Element
        {
            public int Type;
            public int X;
            public int Y;
            public int SizeX;
            public int SizeY;
            public int Rotation;

            public Element(int type, int x,int y, int sizeX, int sizeY, int rotation)
            {
                Type = type;
                X = x;
                Y = y;
                SizeX = sizeX;
                SizeY = sizeY;
                Rotation = rotation;
            }
            public Element(string line)
            {
                line = line.Substring(6);
                Type = Convert.ToInt32(line.Substring(0,line.IndexOf("<x>")));
                line = line.Substring(line.IndexOf("<x>")+3);
                X = Convert.ToInt32(line.Substring(0, line.IndexOf("<y>")));
                line = line.Substring(line.IndexOf("<y>")+3);
                Y = Convert.ToInt32(line.Substring(0, line.IndexOf("<sizeX>")));
                line = line.Substring(line.IndexOf("<sizeX>") +7);
                SizeX = Convert.ToInt32(line.Substring(0, line.IndexOf("<sizeY>")));
                line = line.Substring(line.IndexOf("<sizeY>") +7);
                SizeY = Convert.ToInt32(line.Substring(0, line.IndexOf("<rotation>")));
                line = line.Substring(line.IndexOf("<rotation>") +10);
                Rotation = Convert.ToInt32(line.Substring(0)); ;
            }

            public override string ToString()
            {
                //Console.WriteLine($"Имя: {name}  Возраст: {age}");
                return "<type>"+Type.ToString()+
                    "<x>"+X.ToString()+"<y>"+Y.ToString()+
                    "<sizeX>"+SizeX.ToString()+"<sizeY>"+SizeY.ToString()+
                    "<rotation>"+Rotation.ToString();
            }
        }
        List<Element> elements = new List<Element>();
        private Bitmap newImage;
        private Bitmap preColorImage;
        private List<Bitmap> images;
        const int sizeXMap = 700;
        const int sizeYMap = 700;
        bool drag = false;
        int dragId = 0;
        int dragDeltaX = 0;
        int dragDeltaY = 0;
        int selectedId = -1;
        int resizeType = -1;
        public void loadPreColor()
        {
            preColorImage = new Bitmap(100, 100);
            preColorImage.SetResolution(100, 100);
            Graphics g = Graphics.FromImage(preColorImage);
            SolidBrush blueBrush = new SolidBrush(backgroundColor);

            // Create rectangle.
            Rectangle rect = new Rectangle(0, 0, 100, 100);
            g.FillRectangle(blueBrush, rect);
            pictureBox3.Image = preColorImage;
        }
        public void reDraw()
        {

            newImage = new Bitmap(sizeXMap,sizeYMap);
            newImage.SetResolution(100, 100);
            Graphics g = Graphics.FromImage(newImage);
            SolidBrush blueBrush = new SolidBrush(backgroundColor);

            // Create rectangle.
            Rectangle rect = new Rectangle(0, 0, sizeXMap, sizeYMap);

            // Fill rectangle to screen.
            g.FillRectangle(blueBrush, rect);
            int idElement = 0;
            foreach (var element in elements)
            {
                if (element.Rotation == 1)
                {
                    images[element.Type].RotateFlip(RotateFlipType.Rotate90FlipNone);
                }else if (element.Rotation == 2)
                {
                    images[element.Type].RotateFlip(RotateFlipType.Rotate180FlipNone);
                }else if (element.Rotation == 3)
                {
                    images[element.Type].RotateFlip(RotateFlipType.Rotate270FlipNone);
                }
                g.DrawImage(images[element.Type], element.X, element.Y,element.SizeX,element.SizeY);
                if (element.Rotation == 1)
                {
                    images[element.Type].RotateFlip(RotateFlipType.Rotate270FlipNone);
                }
                else if (element.Rotation == 2)
                {
                    images[element.Type].RotateFlip(RotateFlipType.Rotate180FlipNone);
                }
                else if (element.Rotation == 3)
                {
                    images[element.Type].RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                if (idElement==selectedId)
                {
                    Pen selPen = new Pen(Color.LightBlue);
                    g.DrawRectangle(selPen, element.X-1, element.Y-1, element.SizeX+2, element.SizeY+2);
                    SolidBrush lightBrush = new SolidBrush(Color.Blue);
                    //g.DrawEllipse
                    // Create rectangle.
                    //Ellipse elips = new Rectangle(0, 0, sizeXMap, sizeYMap);

                    // Fill rectangle to screen.
                    g.FillEllipse(lightBrush, element.X - 5, element.Y - 5,10,10);
                    g.FillEllipse(lightBrush, element.X - 5+element.SizeX, element.Y - 5,10,10);
                    g.FillEllipse(lightBrush, element.X - 5, element.Y - 5 + element.SizeY, 10,10);
                    g.FillEllipse(lightBrush, element.X - 5+element.SizeX, element.Y - 5 + element.SizeY, 10,10);
                }
                idElement++;
            }
            
            pictureBox1.Image = newImage;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundColor = Color.Pink;
            loadPreColor();
            listBox1.BackColor = System.Drawing.Color.FromArgb(174, 93, 28);
            listBox1.ForeColor = System.Drawing.Color.FromArgb(254, 200, 50);
            this.BackColor = System.Drawing.Color.FromArgb(73, 51, 111);
            images = new List<Bitmap>()
            {
                Resources.wall,
                Resources.window,
                Resources.door,
                Resources.bath,
                Resources.stove,
                Resources.toilet,
                Resources.waterDispanser
                
            };
            listBox1.Items.Clear();
            listBox1.Items.Add("Nothing");
            listBox1.Items.Add("Wall");
            listBox1.Items.Add("Window");
            listBox1.Items.Add("Door");
            listBox1.Items.Add("Bath");
            listBox1.Items.Add("Stove");
            listBox1.Items.Add("Toilet");
            listBox1.Items.Add("Water Dispanser");
            //elements.Add(new Element(0, 10, 10, 32, 16,0));
            reDraw();   
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
     
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            //MessageBox.Show(coordinates.X.ToString());
            //Console.WriteLine(coordinates.X.ToString()+" "+ coordinates.Y.ToString());
            for (int i = elements.Count-1; i >=0; i=i-1)
            {
                //MessageBox.Show("q");
                if ((elements[i].X< coordinates.X)&& (elements[i].X+elements[i].SizeX > coordinates.X)&& (elements[i].Y < coordinates.Y) && (elements[i].Y + elements[i].SizeY > coordinates.Y))
                {
                    //MessageBox.Show(i.ToString());
                    Element tempElement = elements[i];
                    tempElement.Rotation++;
                    int tempInt = tempElement.SizeY;
                    tempElement.SizeY = tempElement.SizeX;
                    tempElement.SizeX = tempInt;
                    if (tempElement.Rotation == 4)
                    {
                        tempElement.Rotation = 0;
                    }
                    elements[i] = tempElement;
                    break;
                }
            }
            reDraw();
        }
        public void changeSelection(MouseEventArgs e)
        {
            Point coordinates = e.Location;
            bool findElement = false;
            //Console.WriteLine(coordinates.X.ToString() + " " + coordinates.Y.ToString());
            for (int i = elements.Count - 1; i >= 0; i = i - 1)
            {
                if ((elements[i].X < coordinates.X) && (elements[i].X + elements[i].SizeX > coordinates.X) && (elements[i].Y < coordinates.Y) && (elements[i].Y + elements[i].SizeY > coordinates.Y))
                {
                    findElement = true;
                    if (selectedId == i)
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            elements.RemoveAt(i);
                            selectedId = -1;
                            break;
                        }
                        
                    }
                    
                    //selectedId = -1;
                    drag = true;
                    dragDeltaX = coordinates.X - elements[i].X;
                    dragDeltaY = coordinates.Y - elements[i].Y;
                    dragId = i;
                    selectedId = i;
                    break;
                }
            }
            if (!findElement)
            {
                selectedId = -1;
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point coordinates = e.Location;
            if (listBox1.SelectedIndex > 0)
            {
                elements.Add(new Element(listBox1.SelectedIndex - 1, coordinates.X - Convert.ToInt32(images[listBox1.SelectedIndex - 1].Width / 2), coordinates.Y - Convert.ToInt32(images[listBox1.SelectedIndex - 1].Height / 2), Convert.ToInt32(images[listBox1.SelectedIndex - 1].Width*1.5), Convert.ToInt32(images[listBox1.SelectedIndex - 1].Height*1.5),0));
                listBox1.SelectedIndex = 0;
            }
            else if ((selectedId > -1) && (e.Button == MouseButtons.Left))
                {
                //Console.WriteLine((elements[selectedId].X + elements[selectedId].SizeX - 5).ToString()+"<"+ coordinates.X.ToString()+());
                if ((elements[selectedId].X + elements[selectedId].SizeX-5 < coordinates.X) && (elements[selectedId].X + elements[selectedId].SizeX+10 > coordinates.X) && (elements[selectedId].Y+ elements[selectedId].SizeY-5 < coordinates.Y) && (elements[selectedId].Y + elements[selectedId].SizeY +10 > coordinates.Y))
                {
                    //Console.WriteLine("qq");
                    resizeType = 3;
                }else if ((elements[selectedId].X  - 5 < coordinates.X) && (elements[selectedId].X  + 10 > coordinates.X) && (elements[selectedId].Y + elements[selectedId].SizeY - 5 < coordinates.Y) && (elements[selectedId].Y + elements[selectedId].SizeY + 10 > coordinates.Y))
                {
                    //Console.WriteLine("qq");
                    resizeType = 2;
                }else if ((elements[selectedId].X + elements[selectedId].SizeX - 5 < coordinates.X) && (elements[selectedId].X + elements[selectedId].SizeX + 10 > coordinates.X) && (elements[selectedId].Y  - 5 < coordinates.Y) && (elements[selectedId].Y  + 10 > coordinates.Y))
                {
                    //Console.WriteLine("qq");
                    resizeType = 1;
                }else if ((elements[selectedId].X - 5 < coordinates.X) && (elements[selectedId].X + 10 > coordinates.X) && (elements[selectedId].Y  - 5 < coordinates.Y) && (elements[selectedId].Y  + 10 > coordinates.Y))
                {
                    //Console.WriteLine("qq");
                    resizeType = 1;
                }
                else
                {
                    changeSelection(e);
                }
            }
            else
            {
                changeSelection(e);
            }

            reDraw();
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (resizeType ==3)
            {
                Point coordinates = e.Location;
                Element tempElement = elements[selectedId];
                tempElement.SizeX = coordinates.X - tempElement.X;
                tempElement.SizeY = coordinates.Y - tempElement.Y;
                if (tempElement.SizeX < 10)
                {
                    tempElement.SizeX = 10;
                }
                if (tempElement.SizeY < 10)
                {
                    tempElement.SizeY = 10;
                }
                elements[dragId] = tempElement;
                reDraw();
            }
            else if (resizeType == 2)
            {
                Point coordinates = e.Location;
                Element tempElement = elements[selectedId];
                tempElement.SizeX += tempElement.X - coordinates.X;
                if (tempElement.SizeX < 10)
                {
                    tempElement.SizeX = 10;
                }
                else
                {
                    tempElement.X = coordinates.X;
                }
                
                
                //tempElement.SizeX = coordinates.X - tempElement.X;
                tempElement.SizeY = coordinates.Y - tempElement.Y;

                if (tempElement.SizeY < 10)
                {
                    tempElement.SizeY = 10;
                }
                elements[dragId] = tempElement;
                reDraw();
            }else if (resizeType == 1)
            {
                Point coordinates = e.Location;
                Element tempElement = elements[selectedId];
                tempElement.SizeY += tempElement.Y - coordinates.Y;
                if (tempElement.SizeY < 10)
                {
                    tempElement.SizeY = 10;
                }
                else
                {
                    tempElement.Y = coordinates.Y;
                }
                
                
                //tempElement.SizeX = coordinates.X - tempElement.X;
                tempElement.SizeX = coordinates.X - tempElement.X;

                if (tempElement.SizeX < 10)
                {
                    tempElement.SizeX = 10;
                }
                elements[dragId] = tempElement;
                reDraw();
            }else if (resizeType == 0)
            {
                Point coordinates = e.Location;
                Element tempElement = elements[selectedId];
                tempElement.SizeY += tempElement.Y - coordinates.Y;
                if (tempElement.SizeY < 10)
                {
                    tempElement.SizeY = 10;
                }
                else
                {
                    tempElement.Y = coordinates.Y;
                }

                tempElement.SizeX += tempElement.X - coordinates.X;
                if (tempElement.SizeX < 10)
                {
                    tempElement.SizeX = 10;
                }
                else
                {
                    tempElement.X = coordinates.X;
                }
                elements[dragId] = tempElement;
                reDraw();
            }
            else if (drag == true)
            {
                Point coordinates = e.Location;
                Element tempElement = elements[dragId];
                tempElement.X = coordinates.X - dragDeltaX;
                int deltaMagnetick = 10;
                if ((tempElement.Type == 0) || (tempElement.Type == 1) || (tempElement.Type == 2))
                {
                    foreach (var element in elements)
                    {
                        if ((element.Type == 0) || (element.Type == 1) || (element.Type == 2))
                        {
                            if ((element.X + element.SizeX - deltaMagnetick < tempElement.X) && (element.X + element.SizeX + deltaMagnetick > tempElement.X))
                            {
                                tempElement.X = element.X + element.SizeX;
                                break;
                            }
                            if ((element.X - deltaMagnetick < tempElement.X) && (element.X + deltaMagnetick > tempElement.X))
                            {
                                tempElement.X = element.X;
                                break;
                            }
                        }

                    }
                }

                tempElement.Y = coordinates.Y - dragDeltaY;
                if ((tempElement.Type == 0) || (tempElement.Type == 1) || (tempElement.Type == 2))
                {
                    foreach (var element in elements)
                    {
                        if ((element.Type == 0) || (element.Type == 1) || (element.Type == 2))
                        {
                            if ((element.Y + element.SizeY - deltaMagnetick < tempElement.Y) && (element.Y + element.SizeY + deltaMagnetick > tempElement.Y))
                            {
                                tempElement.Y = element.Y + element.SizeY;
                                break;
                            }
                            if ((element.Y - deltaMagnetick < tempElement.Y) && (element.Y + deltaMagnetick > tempElement.Y))
                            {
                                tempElement.Y = element.Y;
                                break;
                            }
                        }
                    }
                }
                elements[dragId] = tempElement;
                reDraw();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            resizeType = -1;
        }
        public void saveData()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Floor Plane(*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, System.Text.Encoding.Default))
                {
                    foreach (var element in elements)
                    {
                        sw.WriteLine(element.ToString());
                    }
                    
                }
            }

        }
        public void saveBMP()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap(*.bmp)|*.bmp";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                newImage.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
            }

        }
        public void loadData()
        {
            elements.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Floor Plane(*.png)|*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openFileDialog1.FileName, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        elements.Add(new Element(line));
                    }
                }
            }
            reDraw();
            

        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void exportToBmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveBMP();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                backgroundColor = colorDialog1.Color;
            }
            loadPreColor();
            reDraw();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elements.Clear();
            reDraw();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://dec.nure.ua/ru/ru-about-department/");
        }

        
    }
}
