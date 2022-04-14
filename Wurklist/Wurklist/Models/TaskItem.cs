using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wurklist.Models;

namespace Wurklist.Models
{
    public class TaskItem
    {
        public enum KanbanItemPositions
        {
            ToDo,
            Doing,
            Done
        }

        public enum KanbanItemPriority
        {
            High,
            Medium,
            Low
        }

        public KanbanItemPositions Activity = KanbanItemPositions.ToDo; //a new kanban item is standard in the todo list
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string Deadline { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public int? LastEditedByUserId { get; set; }
        public KanbanItemPriority itemPriority = KanbanItemPriority.Low; //a new kanban item is standard in a low priority

        public TaskItem(int id, string name, string description, string deadline, int? projectId, int? userId, int? lastEditedByUserId, string itemCreated)
        {
            ID = id;
            Name = name;
            Description = description;
            Deadline = deadline;
            ProjectId = projectId;
            UserId = userId;
            LastEditedByUserId = lastEditedByUserId;
            Created = itemCreated;
        }

        public TaskItem(string name, string description, string deadline, int? projectId, int? userId, int? lastEditedByUserId, string itemCreated)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
            ProjectId = projectId;
            UserId = userId;
            LastEditedByUserId = lastEditedByUserId;
            Created = itemCreated;
        }

        public KanbanItemPositions getItemPosition()
        {
            return Activity;
        }

        public void setItemPosition(KanbanItemPositions newItemPosition)
        {
            Activity = newItemPosition;
        }

        public KanbanItemPriority getItemPriority()
        {
            return itemPriority;
        }

        public void setItemPriority(KanbanItemPriority newItemPriority)
        {
            itemPriority = newItemPriority;
        }
    }
}
