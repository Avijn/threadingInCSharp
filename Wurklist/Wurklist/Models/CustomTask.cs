namespace Wurklist.Models
{
    public class CustomTask : TaskItem
    {
        public CustomTask(
            int id,
            string name,
            string description,
            string activity,
            string deadline,
            int? projectId,
            int? userId,
            int? priority,
            int? lastEditedByUserId,
            string itemCreated
            ) : base (id, name, description, deadline, projectId, userId, lastEditedByUserId, itemCreated)
        {
            ID = id;
            Name = name;
            Description = description;
            switch (activity)
            {
                case "ToDo":
                    Activity = KanbanItemPositions.ToDo;
                    break;
                case "Doing":
                    Activity = KanbanItemPositions.Doing;
                    break;
                case "Done":
                    Activity = KanbanItemPositions.Done;
                    break;
                default:
                    break;
            }
            Deadline = deadline;
            ProjectId = projectId;
            UserId = userId;
            switch (priority)
            {
                case 0:
                    itemPriority = KanbanItemPriority.Low;
                    break;
                case 1:
                    itemPriority = KanbanItemPriority.Medium;
                    break;
                case 2:
                    itemPriority = KanbanItemPriority.High;
                    break;
                default:
                    break;
            }
            LastEditedByUserId = lastEditedByUserId;
            Created = itemCreated;
        }
    }
}
