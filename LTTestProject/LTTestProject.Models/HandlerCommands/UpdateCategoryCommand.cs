using LTTestProject.Models.DTOs;
using LTTestProject.Models.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerCommands
{
    public class UpdateCategoryCommand : IRequest<CategoryDTO>
    {
        public int CategroryId { get; set; }
        public CategoryVM Category { get; set; }
    }
}
