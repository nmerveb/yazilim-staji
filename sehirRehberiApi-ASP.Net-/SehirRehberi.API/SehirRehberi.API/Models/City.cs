﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Models
{
    public class City
    {
        public City()
        {
            Photos = new List<Photo>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public List<Photo> Photos { get; set; }
        public User users { get; set; }
    }



}
