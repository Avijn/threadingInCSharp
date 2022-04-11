using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wurklist.DataBase;
using Wurklist.General;
using Wurklist.Models;

namespace Wurklist.Kanban
{
    public class KanbanProject
    {
        private readonly DBCalls _dBCalls;

        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public string Contributors { get; set; }
        public string Created { get; set; }
        public string Deadline { get; set; }
        public List<TaskItem> Items;

        public KanbanProject(int? Id, string projectName, string projectDescription, List<TaskItem> projectItems, int projectCreatedByUserId, string projectContributors, string projectCreated, string projectDeadline)
        {

            _dBCalls = new DBCalls();

            ID = Id;
            Name = projectName;
            Description = projectDescription;
            Items = projectItems;
            CreatedByUserId = projectCreatedByUserId;
            Contributors = projectContributors;
            Created = projectCreated;
            Deadline = projectDeadline;
            Items = new List<TaskItem>();
        }

        public List<TaskItem> getProjectItems()
        {
            return _dBCalls.GetKanbanItemsByProjectId(User.GetUserId());
        }

        public void addProjectItem(TaskItem newKanbanItem)
        {
            Items.Add(newKanbanItem);
        }

        public void deleteProjectItem(TaskItem kanbanItem)
        {
            Items.Remove(kanbanItem);
        }

        public void setProjectItems(List<TaskItem> newKanbanItems)
        {
            Items = newKanbanItems;
        }

        //public void OnTaskItemChange(TaskItem item)
        //{
        //    _dBCalls.UpdateTaskItem(item.Id);
        //}
    }
}
