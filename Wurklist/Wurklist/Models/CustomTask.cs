using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wurklist.Models
{
    public class CustomTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Activity { get; set; }
        public string Deadline { get; set; }
        public int ProjectId { get;set; }
        public int UserId { get; set; }
        public int Priority { get; set; }

    }
}
