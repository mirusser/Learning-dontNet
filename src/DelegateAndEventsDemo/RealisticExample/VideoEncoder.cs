using System;
using System.Threading;

namespace DelegateAndEventsDemo.RealisticExample
{
    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; } = null!;
    }

    public class VideoEncoder
    {
        //1 - define a delegate (you can skip this step if are using Action or Func (or old fashioned EventHandler)
        //public delegate void VideoEncodeEventHandler(object source, EventArgs args);

        //2 - define the event (based on delegate)
        //public event VideoEncodeEventHandler VideoEncoded;
        //public EventHandler<VideoEventArgs> VideoEncoded;
        public event Action<object, VideoEventArgs>? VideoEncoded;

        public void Encode(Video video)
        {
            Console.WriteLine("Encoding video...");
            Thread.Sleep(3000);
            Console.WriteLine("Finished Encoding video.");

            //3 - publish/raise the event
            OnVideoEncoded(video);
        }

        protected virtual void OnVideoEncoded(Video video)
        {
            //VideoEncoded?.Invoke(this, EventArgs.Empty);

            VideoEncoded?.Invoke(this, new VideoEventArgs { Video = video });
        }
    }
}