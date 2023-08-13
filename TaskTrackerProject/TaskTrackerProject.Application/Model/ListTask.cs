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
        public ListTask(string name, Status status, Priority priority, bool isfavorite, UserList list, DateTime date)
        {
            Name = name;
            Status = status;
            Priority = priority;
            IsFavorite = isfavorite;
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
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public bool IsFavorite { get; set; }
        public int ListId { get; set; }
        public UserList List { get; set; }
        public DateTime Date { get; set; }
    }
}
