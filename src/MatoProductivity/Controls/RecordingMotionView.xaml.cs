using System;
using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;


namespace MatoProductivity.Controls
{
    public partial class RecordingMotionView : ContentView
    {
        private bool isOpen = false;
        private Animation[] animations = null;
        public RecordingMotionView()
        {
            InitializeComponent();
            Loaded+=PlayingMotionView_Loaded;
            animations=new Animation[1];
        }
        public static readonly BindableProperty IsOnProperty =
   BindableProperty.Create("IsOn", typeof(bool), typeof(RecordingMotionView), true, BindingMode.TwoWay, propertyChanged: IsOnChanged);

        private static void IsOnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if ((bool)newValue)
            {
                (bindable as RecordingMotionView).Start();
            }
            else
            {
                (bindable as RecordingMotionView).Stop();

            }
        }

        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }

        private void PlayingMotionView_Loaded(object sender, EventArgs e)
        {
            Init();
        }

        public void Start()
        {
            foreach (var item in animations)
            {
                item.Resume();
            }
        }

        public void Stop()
        {
            foreach (var item in animations)
            {
                item.Pause();
            }
        }

        public void Init()
        {
            viewbox1down();
        }

        private bool viewbox1down()
        {

            double origin = 1;
            uint duration = 1000;
            Action<double> currentAction = v => this.BoxView01.Opacity = v;

            Animation flashingAnimation = new Animation();
            Animation flashingAnimation1;
            Animation flashingAnimation2;

            flashingAnimation1 = new Animation(currentAction, origin, 0, Easing.CubicInOut);
            flashingAnimation2 = new Animation(currentAction, 0, origin, Easing.CubicInOut);
            flashingAnimation.Add(0, 0.5, flashingAnimation1);
            flashingAnimation.Add(0.5, 1, flashingAnimation2);
            flashingAnimation.Commit(this, "RestoreAnimation1", 16, duration, repeat: () => true);
            animations[0]= flashingAnimation;
            return isOpen;
        }


    }
}
