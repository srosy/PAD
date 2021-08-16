using System;

namespace PAD.Data.Models
{
    public class SessionObj
    {
        public Guid SessionId { get; set; }
        public DateTime ExpireDate { get; set; }
        // put whatever objects are useful to keep
    }
}
