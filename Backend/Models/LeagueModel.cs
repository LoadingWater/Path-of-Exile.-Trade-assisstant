using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class LeagueModel
    {
        public string Id { get; set; }
        public string Realm { get; set; }
        public string Description { get; set; }
        public DateTimeOffset RegisterAt { get; set; }
        public Uri Url { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public bool DelveEvent { get; set; }
        public Rule[] Rules { get; set; }

        public override string ToString()
        {
            return this.Id;
        }
    }

    public class Rule
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
}
