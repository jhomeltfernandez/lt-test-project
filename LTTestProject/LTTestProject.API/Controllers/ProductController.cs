using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using LTTestProject.Models.DTOs;
using LTTestProject.Models.HandlerCommands;
using LTTestProject.Models.HandlerQueries;
using LTTestProject.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LTTestProject.API.Controllers
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDTO>> Get(int productId)
        {
            try
            {
                var response = await _mediator.Send(new GetProductQuery()
                {
                    ProductId = productId
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("list")]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts(string searchPhrase, int categoryId)
        {
            try
            {
                var response = await _mediator.Send(new GetProductsQuery()
                {
                    SearchPhrase = searchPhrase,
                    CategoryId = categoryId
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Add([FromForm]ProductVM product)
        {
            try
            {
                var response = await _mediator.Send(new AddProductCommand()
                {
                    Product = product
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("productId")]
        public async Task<ActionResult<ProductDTO>> Update(int productId, [FromForm]ProductVM product)
        {
            try
            {
                var response = await _mediator.Send(new UpdateProductCommand()
                {
                    ProductId = productId,
                    Product = product
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ProductDTO>> Delete(int productId)
        {
            try
            {
                var response = await _mediator.Send(new DeleteProductCommand()
                {
                    ProductId = productId
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}