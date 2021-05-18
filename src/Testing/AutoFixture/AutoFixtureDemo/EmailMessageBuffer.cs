using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoFixtureDemo
{
    public class EmailMessageBuffer
    {
        public List<EmailMessage> Emails = new();

        public int UnsentMessagesCount => Emails.Count;

        public void Add(EmailMessage message)
        {
            Emails.Add(message);
        }

        public void SendAll()
        {
            for (int i = 0; i < Emails.Count; i++)
            {
                var email = Emails[i];

                Send(email);
                Emails.Remove(email);
            }
        }

        public void SendLimited(int maximumMessagesToSend)
        {
            var limitedBatchOfMessages = Emails.Take(maximumMessagesToSend).ToArray();

            for (int i = 0; i < limitedBatchOfMessages.Length; i++)
            {
                var email = limitedBatchOfMessages[i];
                Send(email);
                Emails.Remove(email);
            }
        }

        private void Send(EmailMessage email)
        {
            //simulate sending email
            Debug.WriteLine($"Sending email to: {email.ToAddress}");
        }
 
    }
}
