using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Vision;
using CoreML;
using CoreImage;
using CoreFoundation;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(JudgeJanken.iOS.JankenJudgeService))]
namespace JudgeJanken.iOS
{
    public class JankenJudgeService : IJankenJudgeService
    {
        private static VNCoreMLModel _vnmodel;
        private Action<IList<JudgeResult>> _callback;

        static JankenJudgeService()
        {
            // Load the ML model
            var assetPath = NSBundle.MainBundle.GetUrlForResource("jankenmodel", "mlmodelc");
            var friedOrNotFriedModel = MLModel.Create(assetPath, out _);
            _vnmodel = VNCoreMLModel.FromMLModel(friedOrNotFriedModel, out _);
        }

        public Task<IList<JudgeResult>> DetectAsync(CIImage ciImage)
        {
            var taskSource = new TaskCompletionSource<IList<JudgeResult>>();
            void handleClassification(VNRequest request, NSError error)
            {
                var observations = request.GetResults<VNClassificationObservation>();
                if (observations == null)
                {
                    taskSource.SetException(new Exception("Unexpected result type from VNCoreMLRequest"));
                    return;
                }

                if (observations.Length == 0)
                {
                    taskSource.SetResult(null);
                    return;
                }

                var result = new List<JudgeResult>();
                foreach (var o in observations)
                {
                    result.Add(new JudgeResult()
                    {
                        Label = o.Identifier,
                        Confidence = o.Confidence
                    });
                }
                taskSource.SetResult(result);
                _callback?.Invoke(result);
            }

            var handler = new VNImageRequestHandler(ciImage, new VNImageOptions());
            DispatchQueue.DefaultGlobalQueue.DispatchAsync(() =>
            {
                handler.Perform(new VNRequest[] { new VNCoreMLRequest(_vnmodel, handleClassification) }, out _);
            });

            return taskSource.Task;
        }

        public void NotifyDetect(Action<IList<JudgeResult>> callback)
        {
            _callback = callback;
        }
    }
}
