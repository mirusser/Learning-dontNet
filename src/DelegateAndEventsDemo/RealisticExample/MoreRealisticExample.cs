namespace DelegateAndEventsDemo.RealisticExample
{
    public static class MoreRealisticExample
    {
        public static void Run()
        {
            var videoEncoder = new VideoEncoder(); //publisher
            var mailService = new MailService(); //subscrber
            var messageService = new MessageService(); //another subscriber

            videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
            videoEncoder.VideoEncoded += messageService.OnVideoEncoded;

            var video = new Video { Title = "Test video title" };
            videoEncoder.Encode(video);
        }
    }
}