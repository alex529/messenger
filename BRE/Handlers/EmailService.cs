using System;

namespace BRE.Handlers
{
    public class EmailService : IEmailService
    {
        public void SendActivationEmail(string receiver)
        {
            Console.WriteLine($"Account for {receiver} is active");
        }
        public void SendUpgradeEmail(string receiver)
        {
            Console.WriteLine($"Account for {receiver} is upgraded");
        }
    }

    public interface IEmailService
    {
        void SendActivationEmail(string receiver);
        void SendUpgradeEmail(string receiver);
    }
}
