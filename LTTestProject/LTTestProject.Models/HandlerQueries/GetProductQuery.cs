using LTTestProject.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerQueries
{
    public class GetProductQuery : IRequest<ProductDTO>
    {
        public int ProductId { get; set; }
    }
}
