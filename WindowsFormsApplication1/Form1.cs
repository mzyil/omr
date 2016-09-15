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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            gray_image.ThresholdBinaryInv(new Gray(150), new Gray(255));
            //gray_image = gray_image.ThresholdAdaptive(new Gray(255), Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 75, new Gray(10));
            //CvInvoke.BitwiseNot(gray_image, gray_image);
            gray_image = gray_image.Canny(0, 170);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            IInputOutputArray hierarchy = null;
            CvInvoke.FindContours(gray_image, contours, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            int contourCount = contours.Size;
            int screenCnt = 0;
            double maxContourArea = 0;
            Console.WriteLine("Contours: " + contourCount);
            //VectorOfVectorOfPoint foundSquares = new VectorOfVectorOfPoint();
            int biggestRect = 0;
            for (int i = 0; i < contourCount; i++)
            {
                VectorOfPoint contour = contours[i];
                Rectangle rect = CvInvoke.BoundingRectangle(contour);
                //double k = (rect.Height + 0.0) / rect.Width;
                double area = rect.Height * rect.Width;
                if (area > 5000 && area > maxContourArea)
                {
                    //CvInvoke.DrawContours(My_Image, contours, i, new MCvScalar(0, 255, 0), 1, LineType.FourConnected, hierarchy, 0, new Point());
                    maxContourArea = area;
                    biggestRect = i;
                    screenCnt++;
                    //foundSquares.Push(contour);
                }
            }
            Console.WriteLine("Found: " + screenCnt);
            if (contourCount > 0)
            {
                //CvInvoke.DrawContours(My_Image, contours, biggestRect, new MCvScalar(0, 255, 0), 1, LineType.FourConnected, hierarchy, 0, new Point());
                //My_Image.Draw(CvInvoke.BoundingRectangle(contours[biggestRect]), new Bgr(0, 255, 0), 1);
                /*Rectangle roi = CvInvoke.BoundingRectangle(contours[biggestRect]);
                VectorOfPoint src = new VectorOfPoint(new Point[] { 
                    new Point(roi.Left, roi.Top),
                    new Point(roi.Right, roi.Top),
                    new Point(roi.Left, roi.Bottom),
                    new Point(roi.Right, roi.Bottom)
                });
                VectorOfPoint dst = new VectorOfPoint(new Point[] { 
                    new Point(0, 0),
                    new Point(0, roi.Width),
                    new Point(0, roi.Height),
                    new Point(roi.Height, roi.Width)
                });
                Mat perspectiveMat = CvInvoke.GetPerspectiveTransform(src, dst);
                Matrix<Bgr> perspectiveMatrix = null;
                perspectiveMat.ConvertTo(perspectiveMatrix, DepthType.Default);
                return My_Image.WarpPerspective<Bgr>(perspectiveMatrix, Inter.Nearest, Warp.FillOutliers, BorderType.Constant, new Bgr(0,255,0)).ToBitmap();
                using (Mat matrix = CvInvoke.GetPerspectiveTransform(contours[biggestRect], dst))
                {
                    using (Mat cutImagePortion = new Mat())
                    {
                        CvInvoke.WarpPerspective(My_Image, cutImagePortion, matrix, roi.Size, Inter.Cubic);
                        return cutImagePortion.ToImage<Bgr, byte>().ToBitmap();
                    }
                }*/
                
                VectorOfPointF approx = new VectorOfPointF();
                CvInvoke.ApproxPolyDP(contours[biggestRect], approx, CvInvoke.ArcLength(contours[biggestRect], true) * 0.02, true);
                if (approx.Size == 4)
                {
                    Rectangle roi = CvInvoke.BoundingRectangle(approx);
                    My_Image.Draw(roi, new Bgr(0, 0, 255), 2);
                    VectorOfPointF dst = new VectorOfPointF(new PointF[] { 
                        new PointF(roi.Left, roi.Top),
                        new PointF(roi.Right, roi.Top),
                        new PointF(roi.Left, roi.Bottom),
                        new PointF(roi.Right, roi.Bottom)
                    });
                    Mat perspectiveMat = CvInvoke.GetPerspectiveTransform(sort(approx.ToArray()), sort(dst.ToArray()));
                    Mat transformed = new Emgu.CV.Mat(My_Image.Rows, My_Image.Cols, DepthType.Cv8U, 3);
                    CvInvoke.WarpPerspective(My_Image, transformed, perspectiveMat, My_Image.Size);
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
            double[] angles = new double[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                angles[i] = Math.Atan2(points[i].Y, points[i].X);
            }
            Array.Sort(angles, points);
            return points;
        }
    }
}
