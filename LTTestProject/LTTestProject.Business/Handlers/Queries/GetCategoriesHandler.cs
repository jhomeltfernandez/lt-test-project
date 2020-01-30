using AutoMapper;
using LTTestProject.DataAccess;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LTTestProject.Models.HandlerQueries;
using Microsoft.EntityFrameworkCore;
using LTTestProject.Common.Extensions;

namespace LTTestProject.Business.Handlers.Queries
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetCategoriesHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            List<Category> categories = await _context.Categories.WhereIf(!string.IsNullOrEmpty(request.SearchPhrase), _ => _.Name.Contains(request.SearchPhrase, StringComparison.OrdinalIgnoreCase))
                                        .ToListAsync(cancellationToken);

            return _mapper.Map<List<CategoryDTO>>(categories);
        }
    }
}
