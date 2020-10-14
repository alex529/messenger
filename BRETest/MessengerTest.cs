using BRE;
using System;
using System.Threading;
using Xunit;

namespace BRETest
{
    public class MessengerTest
    {
        [Fact]
        public void Subscribe_SingleAdd()
        {
            var isCalled = false;
            Messenger.Instance.Subscribe<PhysicalProductPayment>(_ => isCalled = true);

            Messenger.Instance.Publish(new PhysicalProductPayment());

            Thread.Sleep(50);
            Assert.True(isCalled);
        }

        [Fact]
        public void Subscribe_Duplicate()
        {
            var noCalls = 0;
            Action<Message<PhysicalProductPayment>> tmp = _ => noCalls++;
            Messenger.Instance.Subscribe(tmp);
            Messenger.Instance.Subscribe(tmp);

            Messenger.Instance.Publish(new PhysicalProductPayment());

            Thread.Sleep(50);
            Assert.Equal(1, noCalls);
        }

        [Fact]
        public void Unsubscribe_NotRegisterd()
        {
            Action<Message<PhysicalProductPayment>> tmp = _ => { };

            try
            {
                Messenger.Instance.Unsubscribe(tmp);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Unsubscribe_Single()
        {
            var noCalls = 0;
            Action<Message<PhysicalProductPayment>> tmp = _ => noCalls++;
            Messenger.Instance.Subscribe(tmp);
            Messenger.Instance.Publish(new PhysicalProductPayment());

            Messenger.Instance.Unsubscribe(tmp);
            Messenger.Instance.Publish(new PhysicalProductPayment());

            Thread.Sleep(50);
            Assert.Equal(1, noCalls);

        }

        [Fact]
        public void Publish_10Events()
        {
            var noCalls = 0;
            Action<Message<PhysicalProductPayment>> tmp = _ => noCalls++;
            Messenger.Instance.Subscribe(tmp);

            var no = 10;
            for (int i = 0; i < no; i++)
            {
                Messenger.Instance.Publish(new PhysicalProductPayment());
            }

            Thread.Sleep(50);
            Assert.Equal(no, noCalls);
        }
    }
}
