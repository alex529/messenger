using BRE;
using BRE.Handlers;
using Moq;
using System.Threading;
using Xunit;

namespace BRETest
{
    public class ProgramTest
    {
        //[Fact]
        //public void Setup_AllPaymentsHaveHandlers()
        //{
        //    var types = new List<Type>();
        //    var mockMessenger = new Mock<IMessenger>();
        //    mockMessenger.Setup(m => m.Subscribe(It.IsAny<Action<Message<It.IsAnyType>>>())).Callback((x)=>  types.Add(x.GetType()));

        //    Program.Setup(mockMessenger.Object, new EmailService());

        //    mockMessenger.Verify(m => m.Subscribe(It.IsAny<Action<Message<It.IsAnyType>>>()), Times.Exactly(5));
        //    //CHECK IF ALL THE TYPES ARE PROPERLY INSERTED in types
        //} this test is not properly done, it should be possible to achieve a bit more research is needed in how generic methods need to be setup 


        /// <summary>
        /// Using this technique of injecting dependencies we can test mot of the code, 
        /// the same kind of design can be implemented for CommissionPaymentGenerator, and for PackingSlip, the generate function ca be injected and afterward it can be mocked in tests.
        /// </summary>
        [Fact]
        public void Setup_SendEmails()
        {
            var emailMock = new Mock<IEmailService>();
            Program.Setup(Messenger.Instance, emailMock.Object);

            var mem = new MembershipPayment();
            Messenger.Instance.Publish(mem);
            Thread.Sleep(500); //this should no be added to tests, a better way of finding out when the task are done should be implemented inside of the Messenger class
            Messenger.Instance.Publish(new UpgradeMembershipPayment() { MebershipID = mem.MebershipID });
            Thread.Sleep(500);

            emailMock.Verify(e => e.SendActivationEmail(It.IsAny<string>()), Times.Once);
            emailMock.Verify(e => e.SendUpgradeEmail(It.IsAny<string>()), Times.Once);
        }
    }
}
