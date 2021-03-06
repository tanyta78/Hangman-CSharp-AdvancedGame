﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    public class Words
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(Int32.MaxValue)]
        public string Name { get; set; }

        [Required]
        public int Level { get; set; }
    }
}
