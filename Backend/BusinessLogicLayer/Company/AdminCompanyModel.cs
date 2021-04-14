﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class AdminCompanyModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Types Type { get; set; }
        public IEnumerable<MovieModel> Movies { get; set; }

        public enum Types
        {
            Distributor,
            Producer
        }
    }
}
