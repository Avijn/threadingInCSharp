using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Wurklist.DataBase;
using Wurklist.General;
using Wurklist.Models;

namespace Wurklist.Kanban
{
    public class KanbanProject
    {
        private readonly DBCalls _dBCalls;

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public List<User> Contributors { get; set; }
        public string Created { get; set; }
        public string Deadline { get; set; }
        public List<CustomTask> Items;

        public KanbanProject(int Id, string projectName, string projectDescription, int projectCreatedByUserId, string projectCreated, string projectDeadline)
        {

            _dBCalls = new DBCalls();

            ID = Id;
            Name = projectName;
            Description = projectDescription;
            CreatedByUserId = projectCreatedByUserId;
            Created = projectCreated;
            Deadline = projectDeadline;
            Items = _dBCalls.GetKanbanItemsByProjectId(ID);
            Contributors = _dBCalls.GetUsersByProjectId(ID);
        }

        public KanbanProject(string projectName, string projectDescription, int projectCreatedByUserId, string projectCreated, string projectDeadline)
        {
            _dBCalls = new DBCalls();

            Name = projectName;
            Description = projectDescription;
            CreatedByUserId = projectCreatedByUserId;
            Created = projectCreated;
            Deadline = projectDeadline;
            Items = new List<CustomTask>();
            Contributors = new List<User>();
        }

        public List<CustomTask> getProjectItems()
        {
            return Items;
        }

        public void SetProjectItems()
        {
            Items = _dBCalls.GetKanbanItemsByProjectId(ID);
        }

        public void addProjectItem(CustomTask newKanbanItem)
        {
            Items.Add(newKanbanItem);
        }
        
        public bool AddContributor(User user)
        {
            Contributors.Add(user);
            return true;
        }

        public void deleteProjectItem(CustomTask kanbanItem)
        {
            Items.Remove(kanbanItem);
        }

        public void setProjectItems(List<CustomTask> newKanbanItems)
        {
            Items = newKanbanItems;
        }

        //public void OnTaskItemChange(TaskItem item)
        //{
        //    _dBCalls.UpdateTaskItem(item.Id);
        //}
    }
}
