using AutoMapper;
using LTTestProject.Business.Services;
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
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UpdateProductHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper, IFileService fileService)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ProductDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _context.Products.FindAsync(request.ProductId);
            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            product.CategoryId = request.Product.CategoryId;

            if (request.Product.Image != null && request.Product.Image.Length > 0)
            {
                string imaegUri = await _fileService.UploadFileToFolder(request.Product.Image, @"wwwroot\images");
                product.ImageUri = imaegUri;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
