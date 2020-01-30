using AutoMapper;
using LTTestProject.DataAccess;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using LTTestProject.Models.HandlerCommands;
using LTTestProject.Models.HandlerResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LTTestProject.Business.Handlers.Commands
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CategoryDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DeleteCategoryHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = await _context.Categories.FindAsync(request.CategoryId);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
