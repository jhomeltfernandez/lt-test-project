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
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetCategoryHandler(LTTestProjectDbContext medtrackContext,IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            Category category = await _context.Categories.FindAsync(request.CateogoryId);

            return _mapper.Map<CategoryDTO>(category);
        }

    }
}
