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
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryDTO>> Get(int categoryId)
        {
            try
            {
                var response = await _mediator.Send(new GetCategoryQuery()
                {
                    CateogoryId = categoryId
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
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories(string searchPhrase)
        {
            try
            {
                var response = await _mediator.Send(new GetCategoriesQuery()
                {
                    SearchPhrase = searchPhrase
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
        public async Task<ActionResult<CategoryDTO>> Add(CategoryVM category)
        {
            try
            {
                var response = await _mediator.Send(new AddCategoryCommand()
                {
                    Category = category
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("categoryId")]
        public async Task<ActionResult<CategoryDTO>> Update(int categoryId, [FromBody]CategoryVM category)
        {
            try
            {
                var response = await _mediator.Send(new UpdateCategoryCommand()
                {
                    CategroryId = categoryId,
                    Category = category
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
        public async Task<ActionResult<CategoryDTO>> Delete(int categoryId)
        {
            try
            {
                var response = await _mediator.Send(new DeleteCategoryCommand()
                {
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
    }
}