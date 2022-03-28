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

        private KanbanItemPositions itemPosition = KanbanItemPositions.ToDo; //a new kanban item is standard in the todo list
        private String itemName { get; set; }
        private String itemDescription { get; set; }
        private String itemlastEditByUser { get; set; }
        private DateTime itemCreated { get; set; }
        private DateTime itemDeadline { get; set; }
        private KanbanItemPriority itemPriority = KanbanItemPriority.Low; //a new kanban item is standard in a low priority

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
