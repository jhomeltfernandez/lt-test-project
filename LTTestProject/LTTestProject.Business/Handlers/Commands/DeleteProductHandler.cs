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
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ProductDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DeleteProductHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _context.Products.FindAsync(request.ProductId);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
