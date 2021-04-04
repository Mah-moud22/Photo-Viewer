using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photo_Viewer
{
    public partial class Form1 : Form
    {
        string[] PhotosName = new string[1000];
        Bitmap[] Photos=new Bitmap[1000];
        Bitmap[] multis = new Bitmap[1000];
        MenuItem mnSingle = new MenuItem("Single Picture Mode");
        MenuItem mnMulti = new MenuItem("Multi-Picture Mode");
        MenuItem mnSlide = new MenuItem("Slide Show Mode");
        MenuItem mnLeave = new MenuItem("Leave Mode");
        MenuItem mnExit = new MenuItem("Exit");
        int counter = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void mn_Single_Click(object sender, EventArgs e)
        {
            mnMulti.Enabled = false;
            mnSlide.Enabled = false;
            listBox1.SelectionMode = SelectionMode.One;
            if(listBox1.SelectedIndices.Count > 0)
            {
                panel1.Controls.Clear();
                PictureBox pictures = new PictureBox();
                pictures.Image = new Bitmap(listBox1.SelectedItems[0].ToString());
                int x = panel1.Location.X, y = panel1.Location.Y;
                pictures.Location = new Point(x, y);
                pictures.SizeMode = PictureBoxSizeMode.StretchImage;
                pictures.Width = panel1.Width;
                pictures.Height = panel1.Height;
                panel1.Controls.Add(pictures);
            }
        }

        private void mn_multi_CLick(object sender, EventArgs e)
        {
            mnSingle.Enabled = false;
            mnSlide.Enabled = false;
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            int x = panel1.Location.X, y = panel1.Location.Y;
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
            {
                PictureBox pictures = new PictureBox();
                pictures.Image = multis[i];
                pictures.SizeMode = PictureBoxSizeMode.StretchImage;
                panel1.AutoScroll = true;
                int width = (int)((panel1.Width) / 5);
                pictures.Location = new Point(x, y);
                pictures.Width = width;
                pictures.Height = width;
                if ((x + width) > (panel1.Width - panel1.Location.Y))
                {
                    x = panel1.Location.X;
                    y += width;
                }
                else
                {
                    x += width;
                }
               
                panel1.Controls.Add(pictures);
            }
        }

        private void mn_Slide_Click(object sender, EventArgs e)
        {
            mnMulti.Enabled = false;
            mnSingle.Enabled = false;
            listBox1.SelectionMode = SelectionMode.None;
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        private void mn_Leave_Mode(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            timer1.Enabled = false;
            mnMulti.Enabled = true;
            mnSingle.Enabled = true;
            mnSlide.Enabled = true;
        }

        private void mn_Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Select";
            ContextMenu mn = new ContextMenu();
            mn.MenuItems.Add(mnSingle);
            mn.MenuItems.Add(mnMulti);
            mn.MenuItems.Add(mnSlide);
            mn.MenuItems.Add(mnLeave);
            mn.MenuItems.Add(mnExit);
            panel2.ContextMenu = mn;
            mnSingle.Click += new EventHandler(mn_Single_Click);
            mnMulti.Click += new EventHandler(mn_multi_CLick);
            mnSlide.Click += new EventHandler(mn_Slide_Click);
            mnLeave.Click += new EventHandler(mn_Leave_Mode);
            mnExit.Click += new EventHandler(mn_Exit);
            panel2.BackColor = Color.CornflowerBlue;
            //listBox1.BackColor = Color.Azure;
            Form1_SizeChanged(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "select images";
            dialog.Multiselect = true;
            dialog.RestoreDirectory = true;
            dialog.Filter = "Image Files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png;...";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PhotosName = dialog.FileNames;
                listBox1.Items.AddRange(dialog.FileNames);
                for (int i = 0; i < dialog.FileNames.Length; i++)
                {
                    Photos[i] = new Bitmap(dialog.FileNames[i], true);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!(mnSingle.Enabled && mnMulti.Enabled))
            {
                if (mnSingle.Enabled)
                {
                    this.mn_Single_Click(sender, e);
                }
                else if (mnMulti.Enabled)
                {
                    panel1.Controls.Clear();
                    for(int i = 0; i < listBox1.SelectedItems.Count; i++)
                    {
                        multis[i] = new Bitmap(listBox1.SelectedItems[i].ToString());
                    }
                    this.mn_multi_CLick(sender, e);
                }
            }
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (counter == listBox1.Items.Count - 1)
            {
                counter = 0;
            }
            else
            {
                counter++;
            }
            panel1.Controls.Clear();
            PictureBox pictures = new PictureBox();
            pictures.Image = Photos[counter];
            int x = panel1.Location.X, y = panel1.Location.Y;
            pictures.Location = new Point(x, y);
            pictures.SizeMode = PictureBoxSizeMode.StretchImage;
            pictures.Width = panel1.Width;
            pictures.Height = panel1.Height;
            panel1.Controls.Add(pictures);
            string Photo_Location = PhotosName[counter];
            StringBuilder Photo_Name = new StringBuilder();
            for (int i = Photo_Location.Length - 1; i >= 0; i--)
            {
                if (Photo_Location[i] == '\\')
                {
                    break;
                }
                Photo_Name.Append(Photo_Location[i]);
            }
            for (int i = 0; i <= Photo_Name.Length / 2; i++)
            {
                char q = Photo_Name[i];
                Photo_Name[i] = Photo_Name[Photo_Name.Length - i - 1];
                Photo_Name[Photo_Name.Length - i - 1] = q;
            }
            statusBar1.Text = Photo_Name.ToString();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

            this.panel2.Width = Width;
            this.panel2.Height = Height;

            int button1_x = Convert.ToInt32(this.Width * 0.59158415);
            int button1_y = Convert.ToInt32(this.Height * 0.81048387);

            int button1_width = Convert.ToInt32(this.Width * 0.37871287);
            int button1_height = Convert.ToInt32(this.Height * 0.072580645);

            this.button1.Width = button1_width;
            this.button1.Height = button1_height;

            this.button1.Location = new Point(button1_x, button1_y);


            int listBox1_x = Convert.ToInt32(this.Width * 0.591584158);
            int listBox1_y = Convert.ToInt32(this.Height * 0.03830645161);

            int listBox1_width = Convert.ToInt32(this.Width * 0.3787128713);
            int listBox1_height = Convert.ToInt32(this.Height * 0.7822580645);

            this.listBox1.Width = listBox1_width;
            this.listBox1.Height = listBox1_height;

            this.listBox1.Location = new Point(listBox1_x, listBox1_y);


            int panel1_x = Convert.ToInt32(this.Width * 0);
            int panel1_y = Convert.ToInt32(this.Height * 0.02419354839);

            int panel1_width = Convert.ToInt32(this.Width * 0.5904455446);
            int panel1_height = Convert.ToInt32(this.Height * 0.8185483871);

            this.panel1.Width = panel1_width;
            this.panel1.Height = panel1_height;

            this.panel1.Location = new Point(panel1_x, panel1_y);

            int statusBar1_x = Convert.ToInt32(this.Width * 0);
            int statusBar1_y = Convert.ToInt32(this.Height * 0.8366935484);

            int statusBar1_width = Convert.ToInt32(this.Width * 0.5841584158);
            int statusBar1_height = Convert.ToInt32(this.Height * 0.04);

            this.statusBar1.Width = statusBar1_width;
            this.statusBar1.Height = statusBar1_height;

            this.statusBar1.Location = new Point(statusBar1_x, statusBar1_y);
            if (!(mnSingle.Enabled && mnMulti.Enabled))
            {
                if (mnSingle.Enabled)
                {
                    this.mn_Single_Click(sender, e);
                }
                else if (mnMulti.Enabled)
                {
                    panel1.Controls.Clear();
                    for (int i = 0; i < listBox1.SelectedItems.Count; i++)
                    {
                        multis[i] = new Bitmap(listBox1.SelectedItems[i].ToString());
                    }
                    this.mn_multi_CLick(sender, e);
                }
            }
        }
    }
}
