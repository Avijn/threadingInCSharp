using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Wurklist.Models;

namespace Wurklist.UI
{
    public class CustomButton : Button
    {
        public TaskItem taskItem { get; set; }

        public CustomButton()
        {
            
        }

        public TaskItem GetTaskItem()
        {
            return taskItem;
        }
    }
}
