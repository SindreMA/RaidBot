using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityBot.DTO
{
    public class AhitemLIstDTO
    {
        public List<AHItemDTO> List { get; set; }
        public ulong MessageID { get; set; }
        public string Prefix { get; set; }
        public string best { get; set; }
        public string url { get; set; }
        public string UpdatedString { get; set; }
        public string msg { get; set; }
    }
}
