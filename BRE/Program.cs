using BRE.Handlers;
using System;
using System.Collections.Generic;

namespace BRE
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Setup(Messenger.Instance, new EmailService());

            //msg.Publish(new PhysicalProductPayment());
            //msg.Publish(new BookPayment());
            //var mem = new MembershipPayment();
            //msg.Publish(mem);
            //Thread.Sleep(500); // as the events are run concurently a sall delay needs to be added to allow membershipRepo to be populated 

            //msg.Publish(new UpgradeMembershipPayment() { MebershipID = mem.MebershipID });
            //msg.Publish(new VideoPayment());
            //msg.Publish(new VideoPayment() { VideoName = "Learning to Ski" });
            //Thread.Sleep(1000); //wait untill the task have processed
        }

        /// <summary>
        /// This is a "mock" function, as both the payments and handlers are very light, and i don't get it from the req where to register the event processors they are wrapped in this function here.
        /// The main point of the function is to show that IMessenger needs to be injected in the place where the "processors" are going to be registered.
        /// </summary>
        /// <param name="msg"></param>
        public static void Setup(IMessenger msg, IEmailService mail)
        {
            var membershipRepo = new Dictionary<Guid, Membership>();//this should be injected as well

            msg.Subscribe<PhysicalProductPayment>(p =>
            {
                var slip = new PackingSlip();
                slip.Products.Add(p.Payload.ProductID);
                slip.Generate();

                CommissionPaymentGenerator(p.Payload.Total, p.Payload.AgentID);
            });

            msg.Subscribe<BookPayment>(b =>
            {
                var slip = new PackingSlip();
                slip.Products.Add(b.Payload.ProductID);
                slip.Departments.Add("royality");
                slip.Generate();

                CommissionPaymentGenerator(b.Payload.Total, b.Payload.AgentID);
            });

            msg.Subscribe<MembershipPayment>(m =>
            {
                var ms = new Membership(m.Payload.MebershipID) { Email = m.Payload.Email };
                ms.Activate();
                membershipRepo[ms.ID] = ms;

                mail.SendActivationEmail(m.Payload.Email);
            });

            msg.Subscribe<UpgradeMembershipPayment>(m =>
            {
                membershipRepo[m.Payload.MebershipID].Upgrade();

                mail.SendUpgradeEmail(membershipRepo[m.Payload.MebershipID].Email);
            });

            msg.Subscribe<VideoPayment>(v =>
            {
                var slip = new PackingSlip();
                slip.Products.Add(v.Payload.ProductID);
                if (v.Payload.VideoName == "Learning to Ski")
                    slip.Products.Add(ProductNameToProductID("First Aid"));

                slip.Generate();
            });
        }

        private static Guid ProductNameToProductID(string name) => Guid.NewGuid();
        private static void CommissionPaymentGenerator(int total, Guid agentID) => Console.WriteLine($"Agent: {agentID} receives: {0.01 * total}");
    }
}
