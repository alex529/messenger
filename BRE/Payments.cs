using System;

namespace BRE
{
    public class Payment
    {
        public int Total { get; set; }
    }

    public class PhysicalProductPayment : Payment
    {
        public Guid ProductID { get; set; } = Guid.NewGuid();
        public Guid AgentID { get; set; } = Guid.NewGuid();
    }
    public class BookPayment : Payment
    {
        public Guid ProductID { get; set; } = Guid.NewGuid();
        public Guid AgentID { get; set; } = Guid.NewGuid();
    }
    public class MembershipPayment : Payment
    {
        public string Email { get; set; }
        public Guid MebershipID { get; set; } = Guid.NewGuid();
    }
    public class UpgradeMembershipPayment : Payment
    {
        public Guid MebershipID { get; set; } = Guid.NewGuid();
    }
    public class VideoPayment : Payment
    {
        public Guid ProductID { get; set; } = Guid.NewGuid();
        public string VideoName { get; set; }

    }
}
