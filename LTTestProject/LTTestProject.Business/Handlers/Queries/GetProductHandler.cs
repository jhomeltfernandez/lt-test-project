using AutoMapper;
using LTTestProject.DataAccess;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using LTTestProject.Models.HandlerQueries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LTTestProject.Business.Handlers.Queries
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetProductHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            Product product = await _context.Products.FindAsync(request.ProductId);

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
