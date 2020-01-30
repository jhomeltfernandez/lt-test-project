using LTTestProject.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerQueries
{
    public class GetCategoryQuery : IRequest<CategoryDTO>
    {
        public int CateogoryId { get; set; }
    }
}
