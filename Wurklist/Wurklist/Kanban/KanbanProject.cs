﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wurklist.DataBase;
using Wurklist.Models;

namespace Wurklist.Kanban
{
    class KanbanProject
    {
        private readonly DBCalls _dBCalls;
        private String projectName { get; set; }
        private String projectDescription { get; set; }
        private List<TaskItem> projectItems = new List<TaskItem>();
        private String projectCreatedByUser { get; set; }
        private String projectContributors { get; set; }
        private DateTime projectCreated { get; set; }
        private DateTime projectDeadline { get; set; }

        public KanbanProject(string projectName, string projectDescription, List<TaskItem> projectItems, string projectCreatedByUser, string projectContributors, DateTime projectCreated, DateTime projectDeadline)
        {
            this.projectName = projectName;
            this.projectDescription = projectDescription;
            this.projectItems = projectItems;
            this.projectCreatedByUser = projectCreatedByUser;
            this.projectContributors = projectContributors;
            this.projectCreated = projectCreated;
            this.projectDeadline = projectDeadline;

            this._dBCalls = new DBCalls();
        }

        public List<TaskItem> getProjectItems()
        {
            return projectItems;
        }

        public void addProjectItem(TaskItem newKanbanItem)
        {
            projectItems.Add(newKanbanItem);
        }

        public void deleteProjectItem(TaskItem kanbanItem)
        {
            projectItems.Remove(kanbanItem);
        }

        public void setProjectItems(List<TaskItem> newKanbanItems)
        {
            projectItems = newKanbanItems;
        }

        //public void OnTaskItemChange(TaskItem item)
        //{
        //    _dBCalls.UpdateTaskItem(item.Id);
        //}
    }
}
