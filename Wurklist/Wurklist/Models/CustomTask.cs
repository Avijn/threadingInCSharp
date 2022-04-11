namespace Wurklist.Models
{
    public class CustomTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Activity { get; set; }
        public string Deadline { get; set; }
        public int? ProjectId { get;set; }
        public int? UserId { get; set; }
        public int? LastEditedByUserId { get; set; }
        public int? Priority { get; set; }
        public string ItemCreated { get; set; }

        public CustomTask(string name, string description, string activity, string deadline, int? projectId, int? userId, int? priority, int? lastEditedByUserId, string itemCreated)
        {
            Name = name;
            Description = description;
            Activity = activity;
            Deadline = deadline;
            ProjectId = projectId;
            UserId = userId;
            Priority = priority;
            LastEditedByUserId = lastEditedByUserId;
            ItemCreated = itemCreated;
        }
    }
}
