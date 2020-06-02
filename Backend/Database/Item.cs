using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class Item
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNote { get; set; }
        public int    ItemFrameType { get; set; }
        public string ItemIconAddress { get; set; }
        public string TabId { get; set; }
        public string ItemAffixes { get; set; }
    }
}
