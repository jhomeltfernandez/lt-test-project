using LTTestProject.Models.DTOs;
using LTTestProject.Models.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerCommands
{
    public class AddCategoryCommand : IRequest<CategoryDTO>
    {
        public CategoryVM Category { get; set; }
    }
}
