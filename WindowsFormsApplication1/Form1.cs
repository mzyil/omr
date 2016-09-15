using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        ConcurrentQueue<Bitmap> processed = new ConcurrentQueue<Bitmap>();
        private double maxMarkArea;
        private double minOuterRectArea;
        private double cannyThreshold;
        private double cannyThresholdLinking;
        private double binThresholdMax;
        private double binThresholdMin;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            binThresholdMin = 253;
            binThresholdMax = 255;
            cannyThreshold = 200;
            cannyThresholdLinking = 250;
            minOuterRectArea = 5000;
            maxMarkArea = 200;

            tbThresholdMin.Text = binThresholdMin.ToString();
            tbThresholdMax.Text = binThresholdMax.ToString();
            tbCannyThreshold.Text = cannyThreshold.ToString();
            tbCannyThresholdLinking.Text = cannyThresholdLinking.ToString();
            tbMinOuterRectArea.Text = minOuterRectArea.ToString();
            tbMaxMarkArea.Text = maxMarkArea.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            binThresholdMin = Convert.ToDouble(tbThresholdMin.Text);
            binThresholdMax = Convert.ToDouble(tbThresholdMax.Text);
            cannyThreshold = Convert.ToDouble(tbCannyThreshold.Text);
            cannyThresholdLinking = Convert.ToDouble(tbCannyThresholdLinking.Text);
            minOuterRectArea = Convert.ToDouble(tbMinOuterRectArea.Text);
            maxMarkArea = Convert.ToDouble(tbMaxMarkArea.Text);
            Thread processor = new Thread(new ThreadStart(work1));
            processor.Start();
        }

        private void work1()
        {
            //Image<Bgr, Byte> My_Image = new Image<Bgr, byte>("C:\\Users\\YILDIZ\\Downloads\\test6.jpg");
            Emgu.CV.Capture videocap = new Emgu.CV.Capture("E:\\Temp\\Clip0000.avi");
            int totalFrames = 0;
            Mat currFrame = null;
            DateTime startTime, endTime;
            if (videocap != null)
            {
                startTime = DateTime.Now;
                while ((currFrame = videocap.QueryFrame()) != null)
                {
                    Image<Bgr, Byte> My_Image = currFrame.ToImage<Bgr, Byte>();
                    var currImage = calc(My_Image.Clone());
                    pictureBox1.BackgroundImage = currImage;
                    totalFrames++;
                }
                endTime = DateTime.Now;
                Console.WriteLine("Total: " + totalFrames);
                Console.WriteLine("In: " + ((TimeSpan)(endTime - startTime)).TotalMilliseconds);
            }
            videocap.Dispose();
        }

        private Bitmap calc(Image<Bgr,Byte> My_Image){
            Image<Gray, byte> gray_image = My_Image.Convert<Gray, byte>();
            gray_image._SmoothGaussian(9);
            gray_image.ThresholdBinaryInv(new Gray(binThresholdMin), new Gray(binThresholdMax));
            //gray_image = gray_image.ThresholdAdaptive(new Gray(255), Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 75, new Gray(10));
            //CvInvoke.BitwiseNot(gray_image, gray_image);
            gray_image = gray_image.Canny(cannyThreshold, cannyThresholdLinking);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            IInputOutputArray hierarchy = null;
            CvInvoke.FindContours(gray_image, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            int contourCount = contours.Size;
            int screenCnt = 0;
            double maxContourArea = 0;
            //Console.WriteLine("Contours: " + contourCount);
            //VectorOfVectorOfPoint foundSquares = new VectorOfVectorOfPoint();
            int biggestRect = 0;
            for (int i = 0; i < contourCount; i++)
            {
                VectorOfPoint contour = contours[i];
                Rectangle rect = CvInvoke.BoundingRectangle(contour);
                //double k = (rect.Height + 0.0) / rect.Width;
                double area = rect.Height * rect.Width;
                if (area > minOuterRectArea && area > maxContourArea)
                {
                    //CvInvoke.DrawContours(My_Image, contours, i, new MCvScalar(0, 255, 0), 1, LineType.FourConnected, hierarchy, 0, new Point());
                    maxContourArea = area;
                    biggestRect = i;
                    screenCnt++;
                    //foundSquares.Push(contour);
                }else if(area < maxMarkArea){
                    gray_image.Draw(rect, new Gray(255), 1);
                    //My_Image.Draw(rect, new Bgr(255, 0, 0), 2);
                }
            }
            //Console.WriteLine("Found: " + screenCnt);
            if (contourCount > 0)
            {
                VectorOfPointF approx = new VectorOfPointF();
                CvInvoke.ApproxPolyDP(contours[biggestRect], approx, CvInvoke.ArcLength(contours[biggestRect], true) * 0.02, true);
                if (approx.Size == 4)
                {
                    Rectangle roi = CvInvoke.BoundingRectangle(approx);
                    My_Image.Draw(roi, new Bgr(0, 0, 255), 2);
                    int shift = 0;
                    VectorOfPointF dst = new VectorOfPointF(new PointF[] { 
                        new PointF(roi.Left-shift, roi.Top-shift),
                        new PointF(roi.Right+shift, roi.Top-shift),
                        new PointF(roi.Left-shift, roi.Bottom+shift),
                        new PointF(roi.Right+shift, roi.Bottom+shift)
                    });
                    var sortedApproxArr = sort(approx.ToArray());
                    var sortedDstArr = sort(dst.ToArray());
                    Mat perspectiveMat = CvInvoke.GetPerspectiveTransform(sortedApproxArr, sortedDstArr);
                    Mat transformed = new Emgu.CV.Mat(gray_image.Rows, gray_image.Cols, DepthType.Cv8U, 1);
                    CvInvoke.WarpPerspective(gray_image, transformed, perspectiveMat, gray_image.Size, Inter.Linear, Warp.Default, BorderType.Wrap);
                    //My_Image.DrawPolyline(approx.ToArray(), true, new Bgr(0, 255, 155));
                    pictureBox2.BackgroundImage = transformed.ToImage<Bgr, byte>().GetSubRect(roi).ToBitmap();
                }
            }

            return My_Image.ToBitmap();
            //CvInvoke.Imshow("result", My_Image.Resize(0.7, Inter.Linear));
            //CvInvoke.Imshow("processed", gray_image);
            //pictureBox1.Image = My_Image.ToBitmap();
        }
        private PointF[] sort(PointF[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < points.Length-1; j++)
                {
                    if (points[i].Y < points[j].Y)
                    {
                        swap(points, i, j);
                    }
                }
            }
            if (points[0].X > points[1].X)
            {
                swap(points, 0, 1);
            }

            if (points[2].X < points[3].X)
            {
                swap(points, 2, 3);
            }
            return points;
        }

        private static void swap<T>(T[] points, int i, int j)
        {
            T temp = points[i];
            points[i] = points[j];
            points[j] = temp;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
