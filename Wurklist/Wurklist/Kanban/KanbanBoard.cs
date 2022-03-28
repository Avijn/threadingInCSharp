using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Wurklist.Kanban
{
    public class KanbanBoard : ContentPage
    {
        public KanbanBoard()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Kanbanboard" }
                }
            };
        }
    }
}