using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wurklist.Kanban
{
    class KanbanProject
    {
        private String projectName { get; set; }
        private String projectDescription { get; set; }
        private List<KanbanItem> projectItems = new List<KanbanItem>();
        private String projectCreatedByUser { get; set; }
        private String projectContributors { get; set; }
        private DateTime projectCreated { get; set; }
        private DateTime projectDeadline { get; set; }

        public KanbanProject(string projectName, string projectDescription, List<KanbanItem> projectItems, string projectCreatedByUser, string projectContributors, DateTime projectCreated, DateTime projectDeadline)
        {
            this.projectName = projectName;
            this.projectDescription = projectDescription;
            this.projectItems = projectItems;
            this.projectCreatedByUser = projectCreatedByUser;
            this.projectContributors = projectContributors;
            this.projectCreated = projectCreated;
            this.projectDeadline = projectDeadline;
        }

        public List<KanbanItem> getProjectItems()
        {
            return projectItems;
        }

        public void addProjectItem(KanbanItem newKanbanItem)
        {
            projectItems.Add(newKanbanItem);
        }

        public void deleteProjectItem(KanbanItem kanbanItem)
        {
            projectItems.Remove(kanbanItem);
        }

        public void setProjectItems(List<KanbanItem> newKanbanItems)
        {
            projectItems = newKanbanItems;
        }
    }
}
