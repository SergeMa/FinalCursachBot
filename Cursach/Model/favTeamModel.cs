using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Cursach.Model
{
    public class favTeamModel
    {
        public BsonObjectId _id { get; set; }
        public double team_id { get; set; }
        public double rating { get; set; }
        public double wins { get; set; }
        public double losses { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
        public string logo_url { get; set; }
    }
}
