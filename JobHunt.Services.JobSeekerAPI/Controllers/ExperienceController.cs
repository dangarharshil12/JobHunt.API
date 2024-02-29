using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/experience")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IMapper _mapper;
        public ExperienceController(IMapper mapper)
        {
            _mapper = mapper;
        }


    }
}
