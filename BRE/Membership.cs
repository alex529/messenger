using System;

namespace BRE
{
    internal class Membership
    {
        public Guid ID { get; set; }
        public string Email { get; set; }

        public Membership(Guid iD)
        {
            ID = iD;
        }

        public void Activate() => Console.WriteLine("membership activated");
        public void Upgrade() => Console.WriteLine("membership upgraded");
    }
}
