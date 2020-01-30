using LTTestProject.Models.DTOs;
using LTTestProject.Models.HandlerResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerCommands
{
    public class DeleteProductCommand : IRequest<ProductDTO>
    {
        public int ProductId { get; set; }
    }
}
