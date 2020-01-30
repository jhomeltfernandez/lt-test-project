using LTTestProject.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.HandlerQueries
{
    public class GetCategoriesQuery : IRequest<List<CategoryDTO>>
    {
        public string SearchPhrase { get; set; }
    }
}
