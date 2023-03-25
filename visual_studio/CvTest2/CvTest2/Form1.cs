using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XImgproc;
using Emgu.Util;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.LinkLabel;

namespace CvTest2
{
    public partial class Form1 : Form
    {

        private VideoCapture _capture = null;
        private bool _captureInProgress;
        private Mat _frame;
        private Mat _grayFrame;
        private Mat _smallGrayFrame;
        private Mat _smoothedGrayFrame;
        private Mat _cannyFrame;

        private int valOne;
        private int valTwo;

        private bool edge = false;
        private bool type = false;

        public Form1()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            try
            {
                _capture = new VideoCapture(1, VideoCapture.API.DShow);
         



                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            _frame = new Mat();
            _grayFrame = new Mat();
            _smallGrayFrame = new Mat();
            _smoothedGrayFrame = new Mat();
            _cannyFrame = new Mat();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);

                CvInvoke.CvtColor(_frame, _grayFrame, ColorConversion.Bgr2Gray);

                //CvInvoke.PyrDown(_grayFrame, _smallGrayFrame);

                //CvInvoke.PyrUp(_smallGrayFrame, _smoothedGrayFrame);

                CvInvoke.Canny(_grayFrame, _cannyFrame, valOne, valTwo);


                Mat input = new Mat();

                if (edge == true)
                {
                    _cannyFrame.CopyTo(input);
                } else
                {
                    _grayFrame.CopyTo(input);
                }

                Image<Bgr, Byte> image = new Image<Bgr, Byte>(1920, 1200, new Bgr(255, 0, 0));

                if (type == false)
                {
                    Mat draw_frame = new Mat();
                    FastLineDetector fld = new FastLineDetector();

                    

                    //try
                    //{
                    LineSegment2DF[] lineSegments = fld.Detect(input);
                    CvInvoke.CvtColor(_grayFrame, draw_frame, ColorConversion.Gray2Bgr);


                    // CvInvoke.CvtColor(draw_frame, ColorConversion.Gray2Bgr);
                    fld.DrawSegments(image, lineSegments);

                    
                } else
                {
                    Mat imgGray = new Mat();
                    VectorOfPointF vector = new VectorOfPointF();
                    LineSegment2D[] lines;




                    CvInvoke.HoughLines(input, vector, 10, Math.PI / 30, 10);
                    //PointF[] pts = vp.ToArray();

                    var linesList = new List<LineSegment2D>();
                    for (var i = 0; i < vector.Size; i++)
                    {
                        var rho = vector[i].X;
                        var theta = vector[i].Y;
                        var pt1 = new Point();
                        var pt2 = new Point();
                        var a = Math.Cos(theta);
                        var b = Math.Sin(theta);
                        var x0 = a * rho;
                        var y0 = b * rho;
                        pt1.X = (int)Math.Round(x0 + input.Width * (-b));
                        pt1.Y = (int)Math.Round(y0 + input.Height * (a));
                        pt2.X = (int)Math.Round(x0 - input.Width * (-b));
                        pt2.Y = (int)Math.Round(y0 - input.Height * (a));

                        linesList.Add(new LineSegment2D(pt1, pt2));
                    }

                    lines = linesList.ToArray();

                    foreach (var line in lines)
                    {

                        
                        



                        MCvScalar color = new MCvScalar(0, 255, 0);
                        CvInvoke.Line(image, line.P1, line.P2, color, 1, LineType.AntiAlias);
                    }

                }

                this.lineImageBox.Image = image;













                //}
                // catch (Emgu.CV.Util.CvException)
                //{

                //}


                this.CamImageBox.Image = _frame;
                this.EdgeImageBox.Image = _cannyFrame;
                
                //grayscaleImageBox.Image = _grayFrame;
               // smoothedGrayscaleImageBox.Image = _smoothedGrayFrame;
               // cannyImageBox.Image = _cannyFrame;
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    captureButton.Text = "Start Capture";
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    captureButton.Text = "Stop";

                    Debug.WriteLine(_capture.Set(CapProp.FrameWidth, 1920));
                    Debug.WriteLine(_capture.Set(CapProp.FrameHeight, 1200));
                    Debug.WriteLine(_capture.Get(CapProp.FrameWidth));
                    Debug.WriteLine(_capture.Get(CapProp.FrameHeight));
                    


                    _capture.Start();



                    // _capture.Set(CapProp.Zoom, 1);
                    //_capture.Read(_frame);
                    //this.CamImageBox.Image = _frame;

                }

                _captureInProgress = !_captureInProgress;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (edge == true)
            {
                edge = false;
                button1.Text = "Input";
            }
            else
            {
                edge = true;
                button1.Text = "Edge";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (type == true)
            {
                type = false;
                button2.Text = "Fast";
            }
            else
            {
                type = true;
                button2.Text = "Hough";
            }
        }

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            valOne = trackBar1.Value;
            valOne = (int)Remap(valOne, 0, 9, 0, 255);
            Debug.WriteLine(valOne);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            valTwo= trackBar2.Value;

            Debug.WriteLine(valTwo);
        }
    }
}