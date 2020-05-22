using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Lab9
{
  
    public partial class Form1 : Form
    {

       static Random rnd = new Random();
        Size resolution = Screen.PrimaryScreen.Bounds.Size;

        //double value = random.NextDouble(1.23, 5.34);
        Thread th;
        //static int recx = 0;
        //static int recY = 0;
        //static int recH = 400;
        //static int recW = 400;

       

        //int centerX = (int)(recx + recW / 2);
        //int centerY = (int)(recY + recH / 2);
        //int radius = (int)recH / 2;
        public Form1()
        {
            InitializeComponent();
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
           

        }
        public void Draw(int radius, int recx, int recY, int recW, int recH, int centerX, int centerY)
        {
            Pen blackPen = new Pen(Color.Black, 1);

            // Create rectangle.
            Rectangle rect = new Rectangle(recx, recY, recW, recH);
            var g = CreateGraphics();
            // Draw rectangle to screen.
            g.DrawRectangle(blackPen, rect);
            g.DrawEllipse(blackPen, centerX - radius, centerY - radius,
                      radius + radius, radius + radius);
        }
        /*private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 1);
         
            // Create rectangle.
            Rectangle rect = new Rectangle(recx, recY, recW, recH);
           
            // Draw rectangle to screen.
            e.Graphics.DrawRectangle(blackPen, rect);
            e.Graphics.DrawEllipse(blackPen, centerX - radius, centerY - radius,
                      radius + radius, radius + radius);
            
        }*/



        static List<double> piList = new List<double>();
        static List<double> dotsin = new List<double>();
        //static List<EventWaitHandle> handles = new List<EventWaitHandle>();
        static List<Task> handles = new List<Task>();
        void MonteCarloMethod(int pointNumber, int radius, int recx, int recY, int recW, int sleeptime)
        {
            
            //Random r = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

            //AutoResetEvent wh = (AutoResetEvent)handle;
            double circle = 0;

            for (int i = 0; i < pointNumber; i++)
            {
                double x = (rnd.Next(recx, recx+recW));
                double y = rnd.Next(recY, recY+recW);
                CreateGraphics().DrawRectangle(new Pen(randomColor, 1),
                 new Rectangle((int)x, (int)y, 1, 1));
                
                if (IsCircle(radius,x, recx+recW/2, y, recY +recW/2))
                {
                   // dotsin.Add(circle);
                    circle++;
                    Thread.Sleep(sleeptime);  
                    //Task.Delay(sleeptime);
                }
            }
            piList.Add((recW * recW * circle) / (double)pointNumber);
            
            //label1.Text = ((160000 * circle) / (double)pointNumber).ToString();
            //wh.Set();
        }

        bool IsCircle(double radius, double x,  double x0, double y, double y0)
        {
            return (((x-x0) * (x-x0) + (y-y0) * (y-y0)) <= radius * radius);
        }


        async void StartCalculating(int thrAm,  int radius, int recx, int recY, int recW, int sleeptime)
        {
            //Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            int pointNumber = 1000;
            //List<Color> col = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Cyan,Color.Magenta };
            Task[] tasks = new Task[thrAm];
            
            for (int i = 0; i < tasks.Length; i++)
            {
                int j = i;
                tasks[i] = new Task(() => MonteCarloMethod(pointNumber, radius, recx, recY,
                recW, sleeptime));
                //Task.Factory.StartNew(() => MonteCarloMethod(pointNumber,radius,recx,recY,
                //recW, sleeptime)); 
                tasks[i].Start();
            }

          
            await Task.WhenAll(tasks);
          
            label1.Text = Math.Round(piList.Average()).ToString();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            //label6.Text = resolution.Width.ToString() + resolution.Height.ToString();
            int radius = 1;
            int thrAm = 1;
            int sleeptime=1;
            try
            {
                radius = Convert.ToInt32(textBox1.Text);
                thrAm = Convert.ToInt32(textBox2.Text);
                sleeptime = Convert.ToInt32(textBox3.Text);
                if (Check(radius,thrAm,sleeptime))
                    flag = true;
            }
            catch { MessageBox.Show("Wrong parameters"); }
            int recx = 250;
            int recY = 10;
            int recH = radius*2;
            int recW = radius*2;

            double exparea = 3.14 * radius * radius;
            int centerX = (int)(recx + recW / 2);
            int centerY = (int)(recY + recH / 2);
            int newH;
            if (recY + recH > 364)
                newH = recY + recH;
            else newH = 364;
            //int newW = recx + recW;
            
            if (flag)
            {
                label6.Text= (Math.Round(exparea, 2)).ToString();
                this.ClientSize = new Size(recx + recW, newH);
                Draw(radius, recx, recY, recW, recH, centerX, centerY);
                StartCalculating(thrAm, radius, recx, recY, recW, sleeptime);

            }
            else MessageBox.Show("Wrong radius");
        }
        public bool Check(int rad, int thr, int sleep)
        {

            if (rad >= 50 && resolution.Height / 2 > rad && resolution.Width / 2 - 300 > rad && thr >= 1 && sleep >= 1)
                return true;
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var g = CreateGraphics();
            g.Clear(Color.White);
            this.ClientSize = new Size(270, 364);
            label1.Text = "";
            label6.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }











        /* int threadCount = 5;
         int pointNumber = 100;

         for (int i = 0; i < threadCount; i++)
         {
             //EventWaitHandle handle = new AutoResetEvent(false);

             // new Thread(delegate () { MonteCarloMethod(pointNumber, handle); }).Start();
             //handles.Add(handle);

             new Thread(delegate () { MonteCarloMethod(pointNumber, handle); }).Start();
         }*/
        // WaitHandle.WaitAny(handles.ToArray());
        // WaitHandle.WaitAny(handles.ToArray());
        // label1.Text = piList.Average().ToString();
        //Console.WriteLine("Avarage of Pi is: {0}", piList.Average());
        //Console.ReadLine();






        /*static List<double> piList = new List<double>();
        static double sum = 0;
         void Method_Monte_Karlo(object num_p)
         {
             int n_point = (int)num_p;
             double pi;
             double n_circle = 0;
             Random r = new Random();
             for (int i = 0; i < n_point; i++)
             {
                 if (IsPointInCircle(1.0, r.NextDouble(), r.NextDouble()))
                 {
                     n_circle++;
                 }
             }
             pi = (4 * n_circle) / (double)n_point;
             piList.Add((4 * n_circle) / (double)n_point);
            //label1.Text = pi.ToString();
            //Console.WriteLine(pi);
        }



         bool IsPointInCircle(double R, double x, double y)
         {
             return ((x * x + y * y) <= R * R);
         }

         void Threads(int numThreads, int num)
         {
             ThreadPool.SetMaxThreads(numThreads, 0);
             for (int i = 0; i < numThreads; i++)
             {
                 ThreadPool.QueueUserWorkItem(new WaitCallback(Method_Monte_Karlo), num);
             }
         }

         void StartCalculating()
         {
             int numThr;
             int _num;
             //Console.WriteLine("Введите количество потоков: ");
             numThr = 5;//Convert.ToInt32(Console.ReadLine());
             //Console.WriteLine("Введите максимальное количество точек: ");
             _num = 5000; Convert.ToInt32(Console.ReadLine());
             Threads(numThr, _num);
            label1.Text = piList.Average().ToString();
            // Console.ReadKey();
         }*/
    }
}
