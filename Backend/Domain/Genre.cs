﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<MovieGenre> Movies { get; set; }
    }
}
