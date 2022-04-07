using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wurklist.Kanban
{
    class KanbanItem
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

        public KanbanItemPositions itemPosition = KanbanItemPositions.ToDo; //a new kanban item is standard in the todo list
        public string itemName { get; set; }
        public string itemDescription { get; set; }
        public string itemlastEditByUser { get; set; }
        public DateTime itemCreated { get; set; }
        public DateTime itemDeadline { get; set; }
        public KanbanItemPriority itemPriority = KanbanItemPriority.Low; //a new kanban item is standard in a low priority

        public KanbanItem(string itemName, string itemDescription, string itemlastEditByUser, DateTime itemCreated, DateTime itemDeadline)
        {
            this.itemName = itemName;
            this.itemDescription = itemDescription;
            this.itemlastEditByUser = itemlastEditByUser;
            this.itemCreated = itemCreated;
            this.itemDeadline = itemDeadline;
        }

        public KanbanItemPositions getItemPosition()
        {
            return itemPosition;
        }

        public void setItemPosition(KanbanItemPositions newItemPosition)
        {
            itemPosition = newItemPosition;
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
