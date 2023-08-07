using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackerProject.Application.Model
{
    public class ListTask
    {
        public ListTask(string name, string status, string priority, UserList list, DateTime date)
        {
            Name = name;
            Status = status;
            Priority = priority;
            List = list;
            ListId = list.Id;
            Date = date;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected ListTask() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int ListId { get; private set; }
        public UserList List { get; private set; }
        public DateTime Date { get; set; }
    }
}
