using System;
using System.Collections.Generic;
using System.Text;

namespace Necessity
{
    public class PMRequest
    {
        public Type type { get; set; }
        public object Request { get; set; }
        public bool isActive { get; set; }
        public ulong UserID { get; set; }
        public string Username { get; set; }
        public DateTime StartTime { get; set; }
    }
    /// //////////////////////////////////////////////////////////
    public class LateRequest
    {
        public bool Complete { get; set; }
        public RequestRaidDate RaidDate { get; set; }
        public RequestReasonText Reason { get; set; }
        public RequestLateTime HowMuchLate { get; set; }
    }
    public class OMessageRequest
    {
        public bool Complete { get; set; }
        public string Text { get; set; }

    }
    public class RequestLateTime
    {
        public bool Complete { get; set; }
        public string Text { get; set; }

    }
    /// //////////////////////////////////////////////////////////
    public class RequestRaidDate
    {
        public bool Complete { get; set; }
        public DateTime Time { get; set; }

    }
    /// //////////////////////////////////////////////////////////
    public class RequestReasonText
    {
       public bool Complete { get; set; }
       public string Text { get; set; }

    }
    public class NotAttendingRequest
    {
        public bool Complete { get; set; }
        public RequestRaidDate RaidDate { get; set; }
        public RequestReasonText Reason { get; set; }
    }

}
