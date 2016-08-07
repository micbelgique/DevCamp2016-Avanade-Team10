using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPreview.Kinect;
using Zeugma.Models;
using Zeugma.Helpers;

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
        Sentence sentence = new Sentence("Le petit cheval brun mange de l'avoine bio. S'il fait chaud, il faut lui donner de l'eau.");
        int numberPeoples = 0;
        int numberLeftHandClosedPeople = 0;

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
                    // number of peoples
                    var bodiesTracked = (from b in bodies where b.IsTracked select b).ToList();
                    // number of participating peoples
                    var bodiesLeftHandClosed = (from b in bodiesTracked where b.HandLeftState == HandState.Closed || b.HandRightState == HandState.Closed select b).ToList();

                    textBox.Text = bodiesTracked.Count + "";
                    textBox2.Text = bodiesLeftHandClosed.Count + "";

                    if (bodiesTracked.Count != numberPeoples || bodiesLeftHandClosed.Count != numberLeftHandClosedPeople)
                    {
                        numberPeoples = bodiesTracked.Count;
                        numberLeftHandClosedPeople = bodiesLeftHandClosed.Count;
                        sentence = Algorythm.MakeSentence(sentence, numberPeoples, numberLeftHandClosedPeople);

                        /** TODO => send sentence data to view **/

                        // for tests only
                        drawPhrase(sentence);
                    }
                }
            }
        }

        // test function
        private void drawPhrase(Sentence sentence)
        {
            string phraseVisible = "";
            string phraseFormated = "";

            var wordsVisible = (from retrieve in sentence.Words
                         where retrieve.Formation == true
                         where retrieve.Visible == true
                         select retrieve).ToList();

            var wordsFormated = (from retrieve in sentence.Words
                               where retrieve.Formation == true
                               select retrieve).ToList();

            foreach (var word in wordsVisible)
            {
                phraseVisible += word.Value + " ";
            }

            foreach (var word in wordsFormated)
            {
                phraseFormated += word.Value + " ";
            }

            textBoxResult.Text = phraseFormated;
            textBoxResult2.Text = phraseVisible;
        }
    }
}
