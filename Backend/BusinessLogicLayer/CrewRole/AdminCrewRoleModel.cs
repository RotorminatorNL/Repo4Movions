﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class AdminCrewRoleModel
    {
        public int ID { get; set; }
        public Roles Role { get; set; }
        public string CharacterName { get; set; }
        public int MovieID { get; set; }
        public AdminMovieModel Movie { get; set; }
        public int PersonID { get; set; }
        public AdminPersonModel Person { get; set; }

        public enum Roles
        {
            Actor,
            Director,
            Editor,
            Producer,
            Writer
        }
    }
}
