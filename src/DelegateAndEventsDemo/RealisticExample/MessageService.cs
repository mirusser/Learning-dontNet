using System;

namespace DelegateAndEventsDemo.RealisticExample
{
    public class MessageService
    {
        public void OnVideoEncoded(object source, VideoEventArgs e)
        {
            Console.WriteLine($"MessageService: Sending a text message. Video title: {e.Video.Title}");
        }
    }
}