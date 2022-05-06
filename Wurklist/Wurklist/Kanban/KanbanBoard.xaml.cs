using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Wurklist.DataBase;
using Wurklist.Models;
using Wurklist.UI;
using Wurklist.General;
using static Wurklist.Models.TaskItem;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System.Threading.Tasks;

namespace Wurklist.Kanban
{
    /// <summary>
    /// A kanbanboard page that has multiple threading methods and connectivity with a database
    /// </summary>
    public sealed partial class KanbanBoard : Page
    {
        private readonly DBCalls _dbcalls; //Database calls
        private int UserId { get; set; } //User id from database
        private List<KanbanProject> allProjectsFromUser = new List<KanbanProject>(); //List of kanban project from user
        private string languageDirectory = @""; //Directory of the language file of the page

        public KanbanBoard()
        {
            //Initializing kanbanboard
            this.InitializeComponent();
            _dbcalls = new DBCalls();
            SetAllProjectsFromUser();
            FillInComboBoxWithAllProjects(allProjectsFromUser);
            ReadFileAsyncIO(); 
            FillInLanguagesComboBox(GetAllLanguagesFiles());
        }

        public void SetUserId(int userid)
        {
            this.UserId = userid;
        }

        public int GetUserId()
        {
            return UserId;
        }

        /// <summary>
        /// Sets all project from user that is logged in
        /// </summary>
        /// <param></param>
        /// <returns> Task </returns>
        public async Task SetAllProjectsFromUser()
        {
            allProjectsFromUser = await GetAllProjectTasksFromUser(User.GetUserId());
        }

        /// <summary>
        /// Adds a button to the todo, doing or done list
        /// </summary>
        /// <param TaskItem="item"></param>
        /// <returns> void </returns>
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

        /// <summary>
        /// Moves the button to a different todo, doing, done block
        /// </summary>
        /// <param CustomButton="btn" KanbanItemPositions="positions"></param>
        /// <returns> void </returns>
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

        /// <summary>
        /// Shows the kanban item with additional values when clicked on
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
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
                switch (button.taskItem.getItemPosition())
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

        /// <summary>
        /// Deletes the kanban item when the delete button in the kanban item is clicked on
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
        private void DeleteTaskItem_Click(object sender, RoutedEventArgs e)
        {
            CustomButton button = (CustomButton)sender;
            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync((workItem) =>
            {
                if (workItem.Status == AsyncStatus.Canceled)
                {
                    return;
                }

                _dbcalls.DeleteTask(button.GetTaskItem());
            });

            asyncAction.Completed = new AsyncActionCompletedHandler((IAsyncAction asyncInfo, AsyncStatus asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Canceled)
                {
                    return;
                }

            });

            Frame.Navigate(typeof(KanbanBoard));
            button.contentDialog.Hide();
        }

        /// <summary>
        /// Gets all project tasks from user
        /// </summary>
        /// <param int="userId"></param>
        /// <returns> Task<List<KanbanProject>> </returns>
        public async Task<List<KanbanProject>> GetAllProjectTasksFromUser(int userId)
        {
            List<int> ids = _dbcalls.GetProjectIdsByUserId(userId);
            List <KanbanProject> projects = new List<KanbanProject>();

            foreach (int id in ids)
            {
                projects.Add(await _dbcalls.GetProjectsByProjectId(id));
            }

            return projects;
        }

        /// <summary>
        /// Fill in combobox with all the projects from user
        /// </summary>
        /// <param List<KanbanProject>="kanbanProjects"></param>
        /// <returns> void </returns>
        public void FillInComboBoxWithAllProjects(List<KanbanProject> kanbanProjects)
        {
            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync((workItem) =>
            {
                if (workItem.Status == AsyncStatus.Canceled)
                {
                    return;
                }

                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
                { 
                    // UI update code
                    foreach (KanbanProject project in kanbanProjects)
                    {
                        ComboBoxItem comboBoxItem = new ComboBoxItem();
                        comboBoxItem.Content = project.Name;
                        comboBoxItem.Tag = project.ID;
                        ShowAllProjects.Items.Add(comboBoxItem);
                    }
                });
            });

            asyncAction.Completed = new AsyncActionCompletedHandler((IAsyncAction asyncInfo, AsyncStatus asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Canceled)
                {
                    return;
                }

            });
        }

        /// <summary>
        /// Adds all kanban items in the kanbanboard with an project id
        /// </summary>
        /// <param int="projectID"></param>
        /// <returns> void </returns>
        public void LoadProject(int projectID)
        {
            List<CustomTask> items = _dbcalls.GetKanbanItemsByProjectId(projectID);

            foreach (CustomTask item in items)
            {
                AddBtn(item);
            }
        }

        /// <summary>
        /// Content dialog popup for adding a task to the project
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
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
                if (taskItemName.Text.Equals("") || taskItemDescription.Text.Equals("") || taskItemDeadline.Date.ToString().Equals("") || Int32.Parse(taskItemProjectID.Text).Equals(""))
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
                    Frame.Navigate(typeof(KanbanBoard));
                }
            }
        }

        /// <summary>
        /// Content dialog popup for adding a project to the user
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
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
                if (taskItemName.Text.Equals("") || taskItemDescription.Text.Equals("") || taskItemDeadline.Date.ToString().Equals(""))
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

        /// <summary>
        /// Returns the user to the Mainpage when clicked on returntohome button
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
        private void ReturnToHome_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Loads in kanban items from project when changing the project in the combobox ShowAllProjects
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
        private void ShowAllProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = ShowAllProjects.SelectedItem as ComboBoxItem; //*get value of combobox
            LoadProject((int)selectedComboBoxItem.Tag);
        }

        /// <summary>
        /// Reads the language files in async IO and adds the translated texts to the UIElements
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
        private async void ReadFileAsyncIO()
        {
            string contents = "";

            if(languageDirectory.Equals(""))
            {
                languageDirectory = @"Languages\English.txt";
            }

            using (StreamReader SourceReader = File.OpenText(languageDirectory))
            {
                while ((contents = await SourceReader.ReadLineAsync()) != null)
                {
                    string[] contentsSplitted = contents.Split(',');

                    object UIElement = FindName(contentsSplitted[0]);

                    if(UIElement is Button)
                    {
                        Button buttonUIElement = (Button)UIElement;
                        buttonUIElement.Content = contentsSplitted[1];
                    }
                    else if(UIElement is TextBlock)
                    {
                        TextBlock buttonUIElement = (TextBlock)UIElement;
                        buttonUIElement.Text = contentsSplitted[1];
                    }
                    else
                    {
                        
                    }
                }
            }
        }

        /// <summary>
        /// Gets all language files (.txt) that are in the directory 'Wurklist\Languages\..'
        /// </summary>
        /// <param></param>
        /// <returns> List<string> </returns>
        private List<string> GetAllLanguagesFiles()
        {
            List<string> allLanguages = new List<string>();
            try
            {
                string[] dirs = Directory.GetFiles(@"Languages\", "*.txt");
                foreach (string dir in dirs)
                {
                    //string[] dirSplitted = dir.Split("\\");
                    //string dirLanguage = dirSplitted[1].Substring(0, dirSplitted[1].Length - 4);
                    allLanguages.Add(dir);
                }
            }
            catch (Exception e)
            {
                
            }
            return allLanguages;
        }

        /// <summary>
        /// Fills in the combobox SelectLanguage with all the possible languages
        /// </summary>
        /// <param List<string>="languagesFiles"></param>
        /// <returns> void </returns>
        private void FillInLanguagesComboBox(List<string> languagesFiles)
        {
            foreach(string languageFile in languagesFiles)
            {
                ComboBoxItem languageFileItem = new ComboBoxItem();
                languageFileItem.Content = languageFile;
                SelectLanguage.Items.Add(languageFileItem);
            }
            
        }

        /// <summary>
        /// Loads in the language file that is selected in the combobox SelectedLanguage
        /// </summary>
        /// <param></param>
        /// <returns> void </returns>
        private void SelectLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = SelectLanguage.SelectedItem as ComboBoxItem; //*get value of combobox
            languageDirectory = selectedComboBoxItem.Content.ToString();
            ReadFileAsyncIO();
        }
    }
}
