using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wurklist.Models
{
    public class CustomTask
    {
        // TODO Decide if the public int Id should be here, do we NEED the Id attriubute
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Activity { get; set; }
        public string Deadline { get; set; }
        public int? ProjectId { get;set; }
        public int? UserId { get; set; }
        public int? Priority { get; set; }

        public CustomTask(string name, string description, string activity, string deadline, int? projectId, int? userId, int? priority)
        {
            Name = name;
            Description = description;
            Activity = activity;
            Deadline = deadline;
            ProjectId = projectId;
            UserId = userId;
            Priority = priority;
        }
    }
}
