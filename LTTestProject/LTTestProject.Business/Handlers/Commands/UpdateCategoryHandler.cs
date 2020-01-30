using AutoMapper;
using LTTestProject.DataAccess;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using LTTestProject.Models.HandlerCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LTTestProject.Business.Handlers.Commands
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = await _context.Categories.FindAsync(request.CategroryId);
            category.Name = request.Category.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
