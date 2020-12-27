﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toDoList.Models
{
    public class Lists
    {
        public Lists()
        {
            inProgress = new List<inProgress>();
            Pendings = new List<Pending>();
            Done = new List<Done>();
        }
        public List<inProgress> inProgress;
        public List<Pending> Pendings;
        public List<Done> Done;
        public User user { get; set; }
    }
}
