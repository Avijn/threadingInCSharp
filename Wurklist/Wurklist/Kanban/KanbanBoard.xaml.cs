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
using Wurklist.Models;

namespace Wurklist.Kanban
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KanbanBoard : Page
    {
        public KanbanBoard()
        {
            this.InitializeComponent();
            FillBtn(firsttodo);
            DateTime datum1 = new DateTime(2008, 3, 1, 7, 0, 0);
            DateTime datum2 = new DateTime(2009, 3, 1, 7, 0, 0);
            TaskItem kanban = new TaskItem("Taak1", "Beschrijving1", "SomeUser1", datum1, datum2);
            AddBtn(kanban);
        }

        public void FillBtn(Button button)
        {
            button.Content = "Taak1";
            button.FontSize = 20;            
        }

        public void AddBtn(TaskItem kanbanItem)
        {
            Button button = new Button();
            button.Content = kanbanItem.itemName;
            button.FontSize = 20;
            button.Click += ShowKanbanItem_Click;
            button.Width = 400;
            button.Height = 150;
            ToDoBlock.Children.Add(button);
        }

        private async void ShowKanbanItem_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog showKanbanItem = new ContentDialog
            {
                Title = "Subscribe to App Service?",
                Content = "Listen, watch, and play in high definition for only $9.99/month. Free to try, cancel anytime.",
                CloseButtonText = "Close"
            };

            ContentDialogResult result = await showKanbanItem.ShowAsync();
        }
    }
}
