using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Wurklist.DataBase;
using Wurklist.Models;
using Wurklist.UI;

namespace Wurklist.Kanban
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KanbanBoard : Page
    {
        private readonly DBCalls _dbcalls;
        private int UserId;

        public KanbanBoard()
        {
            this.InitializeComponent();
            FillBtn(firsttodo);
            DateTime datum1 = new DateTime(2008, 3, 1, 7, 0, 0);
            DateTime datum2 = new DateTime(2009, 3, 1, 7, 0, 0);
            TaskItem kanban = new TaskItem("Taak1", "Beschrijving1", "02-02-2222", 1, 1, 1, "02-02-2222");
            AddBtn(kanban);
            _dbcalls = new DBCalls();
        }

        public void SetUserId(int userid)
        {
            this.UserId = userid;
        }

        public int GetUserId()
        {
            return UserId;
        }

        public void FillBtn(Button button)
        {
            button.Content = "Taak1";
            button.FontSize = 20;
        }

        public void AddBtn(TaskItem item)
        {
            CustomButton button = new CustomButton();
            button.Content = item.Name;
            button.taskItem = item;
            button.FontSize = 20;
            button.Click += ShowKanbanItem_Click;
            button.Width = 400;
            button.Height = 100;
            ToDoBlock.Children.Add(button);
        }

        public async void AddProjects()
        {
            List<int> ids = _dbcalls.GetProjectIdsByUserId(UserId);
            List<KanbanProject> projects = new List<KanbanProject>();
            foreach (int id in ids) {
                projects.Add(await _dbcalls.GetProjectsByProjectId(id));
            }

            foreach (KanbanProject project in projects)
            {
                CustomButton button = new CustomButton();
                button.Content = project.Name;
                button.Width = 400;
                button.Height = 100;
                ToDoBlock.Children.Add(button);
            }

        }

        private async void ShowKanbanItem_Click(object sender, RoutedEventArgs e)
        {
            CustomButton button = (CustomButton)sender;
            TaskItem taskItem = button.GetTaskItem();
            ContentDialog showKanbanItem = new ContentDialog
            {
                Title = taskItem.Name,
                Content = taskItem.Description,
                CloseButtonText = "Close"
            };

            ContentDialogResult result = await showKanbanItem.ShowAsync();
        }

        public async void GetAllProjectTasksFromUser()
        {
            List<int> ids = _dbcalls.GetProjectIdsByUserId(UserId);
            List<KanbanProject> allProjectsFromUser = new List<KanbanProject>();

            foreach (int id in ids)
            {
                allProjectsFromUser.Add(await _dbcalls.GetProjectsByProjectId(id));
            }
        }

        public void LoadProject(object sender, RoutedEventArgs e)
        {
            List<TaskItem> items = _dbcalls.GetKanbanItemsByProjectId(1);

            foreach(TaskItem item in items)
            {
                AddBtn(item);
            }
        }


        public void ShowPopupAddTask(object sender, RoutedEventArgs e)
        {
            if (!AddTaskPopup.IsOpen) { AddTaskPopup.IsOpen = true; }
        }

        private void ClosePopupAddTask(object sender, RoutedEventArgs e)
        {
            if (AddTaskPopup.IsOpen) { AddTaskPopup.IsOpen = false; }
        }
    }
}
