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
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, CategoryDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AddCategoryHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new Category()
            {
                Name = request.Category.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
