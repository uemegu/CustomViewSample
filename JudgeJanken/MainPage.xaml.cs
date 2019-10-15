using System;
using System.ComponentModel;
using System.IO;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace JudgeJanken
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            DependencyService.Get<IJankenJudgeService>().NotifyDetect((result) =>
            {
                if (result.Count != 4) return;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label1.Text = result[0].Label;
                    Value1.Text = result[0].Confidence.ToString();
                    Label2.Text = result[1].Label;
                    Value2.Text = result[1].Confidence.ToString();
                    Label3.Text = result[2].Label;
                    Value3.Text = result[2].Confidence.ToString();
                    Label4.Text = result[3].Label;
                    Value4.Text = result[3].Confidence.ToString();
                });
            });
        }

    }
}
