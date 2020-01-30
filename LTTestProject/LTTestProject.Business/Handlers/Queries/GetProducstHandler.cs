using AutoMapper;
using LTTestProject.DataAccess;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using LTTestProject.Models.HandlerQueries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LTTestProject.Common.Extensions;

namespace LTTestProject.Business.Handlers.Queries
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetProductsHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = await _context.Products
                                    .WhereIf(!string.IsNullOrWhiteSpace(request.SearchPhrase), _=> _.Name.Contains(request.SearchPhrase, StringComparison.OrdinalIgnoreCase)
                                             || _.Description.Contains(request.SearchPhrase, StringComparison.OrdinalIgnoreCase))
                                    .WhereIf(request.CategoryId > 0, _=>_.CategoryId==request.CategoryId)
                                    .ToListAsync(cancellationToken);

            return _mapper.Map<List<ProductDTO>>(products);
        }
    }
}
