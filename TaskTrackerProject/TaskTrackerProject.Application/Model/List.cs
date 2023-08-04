﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackerProject.Application.Model
{
    public class List
    {
        public List(string name, User user)
        {
            Name = name;
            User = user;
            UserId = user.Id;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected List() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public List<ListTask> Tasks { get; } = new();
    }
}
