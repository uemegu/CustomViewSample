using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JudgeJanken
{
    public class CameraPreview : View
    {
        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraPreview),
            defaultValue: CameraOptions.Rear);

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public static readonly BindableProperty IsPreviewingProperty = BindableProperty.Create(
            propertyName: "IsPreviewing",
            returnType: typeof(bool),
            declaringType: typeof(CameraPreview),
            defaultValue: true);

        public bool IsPreviewing
        {
            get { return (bool)GetValue(IsPreviewingProperty); }
            set { SetValue(IsPreviewingProperty, value); }
        }

        public async Task NotifyImageSaved(Stream path)
        {
            if (this.BindingContext != null && this.BindingContext is ICameraPreviewReceiver)
                await ((ICameraPreviewReceiver)this.BindingContext).PreviewCreated(path);
        }

    }

    public enum CameraOptions
    {
        Rear,
        Front
    }

    public interface ICameraPreviewReceiver
    {
        Task PreviewCreated(Stream image);
    }

}