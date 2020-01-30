using LTTestProject.Models.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LTTestProject.Models.ViewModels
{
    public class ProductVM
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
