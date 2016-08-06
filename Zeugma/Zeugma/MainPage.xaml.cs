using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsPreview.Kinect;
using Zeugma.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Zeugma
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader frameReader;
        IList<Body> bodies;
        Sentence sentence = new Sentence("Le petit cheval brun mange de l'avoine bio.");
        

        public MainPage()
        {
            this.InitializeComponent();
            foreach(var str in sentence.Words)
            {
                Debug.WriteLine(str.Value);
            }
            
            Loaded += MainPage_Loaded;
            Unloaded += MainPage_Unloaded;
        }
        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_sensor != null && _sensor.IsOpen)
            {
                _sensor.Close();
            }
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();
            if (_sensor != null)
            {
                _sensor.Open();
                frameReader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
                frameReader.MultiSourceFrameArrived += FrameReader_MultiSourceFrameArrived;
            }
        }

        private void FrameReader_MultiSourceFrameArrived(MultiSourceFrameReader sender, MultiSourceFrameArrivedEventArgs args)
        {
            var reference = args.FrameReference.AcquireFrame();
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    bodies = new Body[frame.BodyFrameSource.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);
                    var bodiesTracked = (from b in bodies where b.IsTracked select b).ToList();
                    var bodiesLeftHandClosed = (from b in bodiesTracked where b.HandLeftState == HandState.Closed || b.HandRightState == HandState.Closed select b).ToList();
                    
                    //TODO => envoyé les données reçues

                    textBox.Text = bodiesTracked.Count + "";
                    textBox2.Text = bodiesLeftHandClosed.Count + "";
                }
            }
        }
    }
}
