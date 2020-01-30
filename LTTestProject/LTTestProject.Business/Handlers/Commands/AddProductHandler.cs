using AutoMapper;
using LTTestProject.Business.Services;
using LTTestProject.DataAccess;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.Entities;
using LTTestProject.Models.HandlerCommands;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LTTestProject.Business.Handlers.Commands
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, ProductDTO>
    {
        private readonly LTTestProjectDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public AddProductHandler(LTTestProjectDbContext medtrackContext, IMediator mediator, IMapper mapper, IFileService fileService, IConfiguration configuration)
        {
            _context = medtrackContext;
            _mediator = mediator;
            _mapper = mapper;
            _fileService = fileService;
            _configuration = configuration;
        }

        public async Task<ProductDTO> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Category category = await _context.Categories.FindAsync(request.Product.CategoryId);
            if (category == null) throw new Exception("Selected category not found.");

            Product product = new Product()
            {
                Category = category,
                Name = request.Product.Name,
                Description = request.Product.Description
            };

            string imaegUri = await _fileService.UploadFileToFolder(request.Product.Image, @"wwwroot\images");
            product.ImageUri = $"{_configuration.GetSection("Host").GetValue<string>("Url")}/{imaegUri}";

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
