using System;
using Windows.UI.Xaml.Controls;
using Wurklist.Models;

namespace Wurklist.UI
{
    public class CustomButton : Button
    {
        public TaskItem taskItem { get; set; }
        public ContentDialog contentDialog { get; set; }

        public TaskItem GetTaskItem()
        {
            return taskItem;
        }
    }
}
