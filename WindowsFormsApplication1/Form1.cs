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
            CvInvoke.FindContours(gray_image, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
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
                VectorOfPointF approx = new VectorOfPointF();
                CvInvoke.ApproxPolyDP(contours[biggestRect], approx, CvInvoke.ArcLength(contours[biggestRect], true) * 0.02, true);
                if (approx.Size == 4)
                {
                    PointF[] sortedApproxArr = sort(approx.ToArray());
                    VectorOfPointF sortedApprox = new VectorOfPointF(sortedApproxArr);
                    double rotAngle = 0;
                    if (sortedApproxArr[0].X > sortedApproxArr[3].X)
                    {
                        //should rotate CCW
                        rotAngle = Math.Atan2((sortedApproxArr[2].Y - sortedApproxArr[1].Y), (sortedApproxArr[2].X - sortedApproxArr[1].X));
                        //RotationMatrix2D rotMatrix = new RotationMatrix2D(sortedApproxArr[0], rotAngle, 1);
                        //rotMatrix.RotatePoints(sortedApproxArr);
                    }
                    else if (sortedApproxArr[0].X < sortedApproxArr[3].X)
                    {
                        rotAngle = Math.Atan2((sortedApproxArr[3].Y - sortedApproxArr[0].Y), (sortedApproxArr[3].X - sortedApproxArr[0].X));
                    }
                    Rectangle orgBoundary = CvInvoke.BoundingRectangle(sortedApprox);
                    PointF orgBoundaryCenter = new PointF(orgBoundary.Width/2, orgBoundary.Height/2);
                    RotatedRect rotatedRect = new RotatedRect(orgBoundaryCenter, orgBoundary.Size, Convert.ToSingle(rotAngle));
                    Rectangle rotatedBoundary = rotatedRect.MinAreaRect();
                    My_Image.Draw(orgBoundary, new Bgr(0, 0, 255), 1);
                    VectorOfPointF dst = new VectorOfPointF(new PointF[] { 
                        new PointF(rotatedBoundary.Left, rotatedBoundary.Top),
                        new PointF(rotatedBoundary.Right, rotatedBoundary.Top),
                        new PointF(rotatedBoundary.Left, rotatedBoundary.Bottom),
                        new PointF(rotatedBoundary.Right, rotatedBoundary.Bottom)
                    });
                    Mat perspectiveMat = CvInvoke.GetPerspectiveTransform(sort(rotatedRect.GetVertices()), sort(dst.ToArray()));
                    Mat transformed = new Emgu.CV.Mat(My_Image.Rows, My_Image.Cols, DepthType.Cv8U, 2);
                    CvInvoke.WarpPerspective(My_Image, transformed, perspectiveMat, My_Image.Size);
                    //My_Image.DrawPolyline(approx.ToArray(), true, new Bgr(0, 255, 155));
                    Image<Bgr, byte> warped = transformed.ToImage<Bgr, byte>();
                    warped.Draw(rotatedBoundary, new Bgr(0, 0, 255), 2);
                    pictureBox2.Image = warped.ToBitmap();
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
