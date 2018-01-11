﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommonLibrary
{
    public class Prediction : BaseModel
    {
        public string ProjectId { get; set; }
        public string TrainingKey { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public DateTime? TimeStamp { get; set; }
        public Dictionary<string, decimal> Results { get; set; }

        [JsonIgnore]
        public string Description
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var tag in Results)
                    sb.Append($"{tag.Key} : {tag.Value.ToString("P1")}, ");

                if (sb.Length == 0)
                    sb.Append("no matching tags");

                return sb.ToString().Trim().TrimEnd(',');
            }
        }
    }
}
