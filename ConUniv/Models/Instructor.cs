﻿using DataAccess.Scaffold.Attributes;
using Needletail.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConUniv.Models
{
    public class Instructor
    {

        [Required]
        [TableKey(CanInsertKey = true)]
        public Guid Id { get; set; }

        [Required]
        [MaxLen(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLen(50)]
        public string FirstName { get; set; }


        public DateTime HireDate { get; set; }


        [Required]
        [MaxLen(128)]
        public string Discriminator { get { return "Instructor"; } set { } }

    }
}