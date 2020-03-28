using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Backend
{
    public class Stash
    {
        public string id { get; set; }
        public bool @public { get; set; }
        public string accountName { get; set; }
        public string lastCharacterName { get; set; }
        public string stash { get; set; }
        public string stashType { get; set; }
        public string league { get; set; }
        public List<JObject> items { get; set; }
    }

    public class RootObject
    {
        public string next_change_id { get; set; }
        public List<Stash> stashes { get; set; }
    }
}
