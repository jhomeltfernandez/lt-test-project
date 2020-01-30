using LTTestProject.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerQueries
{
    public class GetProductsQuery : IRequest<List<ProductDTO>>
    {
        public string SearchPhrase { get; set; }
        public int CategoryId { get; set; }
    }
}
