using LTTestProject.Models.DTOs;
using LTTestProject.Models.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerCommands
{
    public class UpdateProductCommand : IRequest<ProductDTO>
    {
        public int ProductId { get; set; }
        public ProductVM Product { get; set; }
    }
}
