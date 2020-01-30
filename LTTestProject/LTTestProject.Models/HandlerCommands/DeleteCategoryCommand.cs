using LTTestProject.Models.DTOs;
using LTTestProject.Models.HandlerResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerCommands
{
    public class DeleteCategoryCommand : IRequest<CategoryDTO>
    {
        public int CategoryId { get; set; }
    }
}
