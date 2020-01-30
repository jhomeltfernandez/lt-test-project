using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LTTestProject.Models.ViewModels
{
    public class CategoryVM
    {
        [Required]
        public string Name { get; set; }
    }
}
