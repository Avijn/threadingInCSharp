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
using Wurklist.General;
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
        List<KanbanProject> allProjectsFromUser = new List<KanbanProject>();

        public KanbanBoard()
        {
            this.InitializeComponent();
            _dbcalls = new DBCalls();
            GetAllProjectTasksFromUser();
            

            foreach (KanbanProject project in allProjectsFromUser)
            {
                ShowAllProjects.Items.Add(project.Name);
                LoadProject(project.ID);
            }

            
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
            button.GetTaskItem().setItemPosition(item.getItemPosition());
            button.GetTaskItem().setItemPriority(item.getItemPriority());
            button.FontSize = 20;
            button.Click += ShowKanbanItem_Click;
            button.Width = 400;
            button.Height = 100;
            button.Margin = new Thickness(10);
            
            switch (button.GetTaskItem().getItemPosition())
            {
                case KanbanItemPositions.ToDo:
                    ToDoBlock.Children.Add(button);
                    break;
                case KanbanItemPositions.Doing:
                    DoingBlock.Children.Add(button);
                    break;
                case KanbanItemPositions.Done:
                    DoneBlock.Children.Add(button);
                    break;
                default:
                    break;
            }
        }

        public void MoveBtn(CustomButton btn, KanbanItemPositions positions)
        {
            btn.taskItem.setItemPosition(positions);
            _dbcalls.UpdateTask(btn.taskItem);
            switch (positions)
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
            ContentDialog showKanbanItem = new ContentDialog();
            StackPanel stackPanel = new StackPanel();
            TextBlock taskItemName = new TextBlock();
            CustomButton deleteTaskItem = new CustomButton();

            taskItemName.Text = button.taskItem.Name;
            deleteTaskItem.Content = "Delete";
            deleteTaskItem.taskItem = button.taskItem;
            deleteTaskItem.contentDialog = showKanbanItem;
            deleteTaskItem.Click += DeleteTaskItem_Click;

            stackPanel.Children.Add(taskItemName);
            stackPanel.Children.Add(deleteTaskItem);

            showKanbanItem.Title = stackPanel;
            showKanbanItem.Content = button.taskItem.Description;
            showKanbanItem.PrimaryButtonText = "<- Move";
            showKanbanItem.SecondaryButtonText = "Move ->";
            
            ContentDialogResult result = await showKanbanItem.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                switch(button.taskItem.getItemPosition())
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
                switch (button.taskItem.getItemPosition())
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
        }

        private void DeleteTaskItem_Click(object sender, RoutedEventArgs e)
        {
            CustomButton button = (CustomButton)sender;
            _dbcalls.DeleteTask(button.GetTaskItem());
            Frame.Navigate(typeof(KanbanBoard));
            button.contentDialog.Hide();
        }

        public async void GetAllProjectTasksFromUser()
        {
            List<int> ids = _dbcalls.GetProjectIdsByUserId(User.GetUserId());

            foreach (int id in ids)
            {
                allProjectsFromUser.Add(await _dbcalls.GetProjectsByProjectId(id));
            }
        }

        public void LoadProject(int projectID)
        {
            List<CustomTask> items = _dbcalls.GetKanbanItemsByProjectId(projectID);

            foreach(CustomTask item in items)
            {
                AddBtn(item);
            }
        }

        public async void ShowPopupAddTask(object sender, RoutedEventArgs e)
        {
            StackPanel stackpanel = new StackPanel();
            TextBlock taskItemNameLabel = new TextBlock();
            TextBox taskItemName = new TextBox();
            TextBlock taskItemDescriptionLabel = new TextBlock();
            TextBox taskItemDescription = new TextBox();
            TextBlock taskItemDeadlineLabel = new TextBlock();
            DatePicker taskItemDeadline = new DatePicker();
            TextBlock taskItemProjectIDLabel = new TextBlock();
            TextBox taskItemProjectID = new TextBox();

            taskItemNameLabel.Text = taskItemName.PlaceholderText = "Name";
            taskItemDescriptionLabel.Text = taskItemDescription.PlaceholderText = "Description";
            taskItemDeadlineLabel.Text = "Deadline";
            taskItemProjectIDLabel.Text = taskItemProjectID.PlaceholderText = "Project ID";

            stackpanel.Children.Add(taskItemNameLabel);
            stackpanel.Children.Add(taskItemName);
            stackpanel.Children.Add(taskItemDescriptionLabel);
            stackpanel.Children.Add(taskItemDescription);
            stackpanel.Children.Add(taskItemDeadlineLabel);
            stackpanel.Children.Add(taskItemDeadline);
            stackpanel.Children.Add(taskItemProjectIDLabel);
            stackpanel.Children.Add(taskItemProjectID);

            ContentDialog showKanbanItem = new ContentDialog
            {
                Title = "Add Task",
                Content = stackpanel,
                PrimaryButtonText = "Add",
                CloseButtonText = "Close"
            };

            ContentDialogResult showKanbanItemResult = await showKanbanItem.ShowAsync();

            if (showKanbanItemResult == ContentDialogResult.Primary)
            {
                if(taskItemName.Text.Equals("") || taskItemDescription.Text.Equals("") || taskItemDeadline.Date.ToString().Equals("") || Int32.Parse(taskItemProjectID.Text).Equals(""))
                {
                    ContentDialog warningMessage = new ContentDialog
                    {
                        Title = "Not all fields are filled in, closing..",
                        CloseButtonText = "Ok"
                    };

                    ContentDialogResult warningMessageResult = await warningMessage.ShowAsync();
                }
                else
                {
                    TaskItem newTaskItem = new TaskItem(taskItemName.Text, taskItemDescription.Text, taskItemDeadline.Date.DateTime.ToString(), Int32.Parse(taskItemProjectID.Text), User.GetUserId(), User.GetUserId(), DateTime.Now.ToString());
                    _dbcalls.InsertKanbanTask(newTaskItem);
                    AddBtn(newTaskItem);
                }
            }
        }

        private async void AddProject_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackpanel = new StackPanel();
            TextBlock taskItemNameLabel = new TextBlock();
            TextBox taskItemName = new TextBox();
            TextBlock taskItemDescriptionLabel = new TextBlock();
            TextBox taskItemDescription = new TextBox();
            TextBlock taskItemDeadlineLabel = new TextBlock();
            DatePicker taskItemDeadline = new DatePicker();

            taskItemNameLabel.Text = taskItemName.PlaceholderText = "Name";
            taskItemDescriptionLabel.Text = taskItemDescription.PlaceholderText = "Description";
            taskItemDeadlineLabel.Text = "Deadline";

            stackpanel.Children.Add(taskItemNameLabel);
            stackpanel.Children.Add(taskItemName);
            stackpanel.Children.Add(taskItemDescriptionLabel);
            stackpanel.Children.Add(taskItemDescription);
            stackpanel.Children.Add(taskItemDeadlineLabel);
            stackpanel.Children.Add(taskItemDeadline);

            ContentDialog showKanbanItem = new ContentDialog
            {
                Title = "Add Project",
                Content = stackpanel,
                PrimaryButtonText = "Add",
                CloseButtonText = "Close"
            };

            ContentDialogResult showKanbanItemResult = await showKanbanItem.ShowAsync();

            if (showKanbanItemResult == ContentDialogResult.Primary)
            {
                if (taskItemName.Text.Equals("") || taskItemDescription.Text.Equals("") || taskItemDeadline.Date.ToString().Equals("") )
                {
                    ContentDialog warningMessage = new ContentDialog
                    {
                        Title = "Not all fields are filled in, closing..",
                        CloseButtonText = "Ok"
                    };

                    ContentDialogResult warningMessageResult = await warningMessage.ShowAsync();
                }
                else
                {
                    KanbanProject newKanbanProject = new KanbanProject(taskItemName.Text, taskItemDescription.Text, User.GetUserId(), DateTime.Now.ToString(), taskItemDeadline.Date.DateTime.ToString());
                    _dbcalls.InsertProject(newKanbanProject);
                }
            }
        }

        private void ReturnToHome_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
