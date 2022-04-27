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
using static Wurklist.Models.TaskItem;

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

        public void AddBtn(TaskItem item)
        {
            CustomButton button = new CustomButton();
            button.Content = item.Name;
            button.taskItem = item;
            button.taskItem.setItemPosition(KanbanItemPositions.ToDo);
            button.taskItem.setItemPriority(KanbanItemPriority.Low);
            button.FontSize = 20;
            button.Click += ShowKanbanItem_Click;
            button.Width = 400;
            button.Height = 100;
            button.Margin = new Thickness(10);
            ToDoBlock.Children.Add(button);
        }

        public void MoveBtn(CustomButton btn, KanbanItemPositions positions)
        {
            CustomButton tempCustomButton = new CustomButton();
            tempCustomButton = btn;
            switch(positions)
            {
                case KanbanItemPositions.ToDo:
                    DoingBlock.Children.Remove(btn);
                    DoneBlock.Children.Remove(btn);
                    ToDoBlock.Children.Add(btn);
                    break;
                case KanbanItemPositions.Doing:
                    ToDoBlock.Children.Remove(btn);
                    DoneBlock.Children.Remove(btn);
                    DoingBlock.Children.Add(btn);
                    break;
                case KanbanItemPositions.Done:
                    ToDoBlock.Children.Remove(btn);
                    DoingBlock.Children.Remove(btn);
                    DoneBlock.Children.Add(btn);
                    break;
                default:
                    break;
            }
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
                PrimaryButtonText = "<- Move",
                SecondaryButtonText = "Move ->",
            };
            
            ContentDialogResult result = await showKanbanItem.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                switch(taskItem.getItemPosition())
                {
                    case KanbanItemPositions.ToDo:
                        break;
                    case KanbanItemPositions.Doing:
                        button.taskItem.setItemPosition(KanbanItemPositions.ToDo);
                        MoveBtn(button, KanbanItemPositions.ToDo);
                        break;
                    case KanbanItemPositions.Done:
                        button.taskItem.setItemPosition(KanbanItemPositions.Doing);
                        MoveBtn(button, KanbanItemPositions.Doing);
                        break;
                }
            }
            else if (result == ContentDialogResult.Secondary)
            {
                switch (taskItem.getItemPosition())
                {
                    case KanbanItemPositions.ToDo:
                        button.taskItem.setItemPosition(KanbanItemPositions.Doing);
                        MoveBtn(button, KanbanItemPositions.Doing);
                        break;
                    case KanbanItemPositions.Doing:
                        button.taskItem.setItemPosition(KanbanItemPositions.Done);
                        MoveBtn(button, KanbanItemPositions.Done);
                        break;
                    case KanbanItemPositions.Done:
                        break;
                }
            }
            else
            {

            }
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

        public async void ShowPopupAddTask(object sender, RoutedEventArgs e)
        {
            StackPanel stackpanel = new StackPanel();
            TextBox taskItemName = new TextBox();
            TextBox taskItemDescription = new TextBox();
            DatePicker taskItemDeadline = new DatePicker();
            TextBox taskItemProjectID = new TextBox();
            taskItemName.PlaceholderText = "Name";
            taskItemDescription.PlaceholderText = "Description";
            taskItemProjectID.PlaceholderText = "Project ID";
            stackpanel.Children.Add(taskItemName);
            stackpanel.Children.Add(taskItemDescription);
            stackpanel.Children.Add(taskItemDeadline);
            stackpanel.Children.Add(taskItemProjectID);


            ContentDialog showKanbanItem = new ContentDialog
            {
                Title = "Add Task",
                Content = stackpanel,
                PrimaryButtonText = "Add",
                CloseButtonText = "Close"
            };

            ContentDialogResult result = await showKanbanItem.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                TaskItem newTaskItem = new TaskItem(taskItemName.Text, taskItemDescription.Text, taskItemDeadline.Date.ToString(), Int32.Parse(taskItemProjectID.Text), 1, 1, DateTime.Now.ToString());
                AddBtn(newTaskItem);
            }
        }
    }
}
