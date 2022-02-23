using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNET.Business;
using RestWithASPNET.Model;

namespace RestWithASPNET.Controllers
{
    [ApiVersion("1", Deprecated = true)]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonController : ControllerBase
    {
       
        private readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personBusiness;

        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindByID(id);

            if(person == null)
                return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonModel person)
        {
            if (!IsPersonValid(person))
                return BadRequest();
            return Ok(_personBusiness.Create(person));
        }

        [HttpPut]
        public IActionResult Put([FromBody] PersonModel person)
        {
            if (person == null)
                return BadRequest();
            return Ok(_personBusiness.Update(person));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }

        public bool IsPersonValid(PersonModel person)
        {
            if (person == null) return false;

            if (string.IsNullOrEmpty(person?.FirstName) || string.IsNullOrEmpty(person?.LastName) ||  string.IsNullOrEmpty(person?.Address) || string.IsNullOrEmpty(person?.Gender))
                return false;     
            
            return true;
        }
    }

    [ApiVersion("2", Deprecated = true)]
    [ApiVersion("3.1")]
    [ApiController]
    [Route("api/Person/v{version:apiVersion}")]
    public class PersonControllerV2 : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personBusiness;

        public PersonControllerV2(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet]
        [MapToApiVersion("3.1")]
        public IActionResult GetV3(ApiVersion apiVersion)
        {
            var minorVersion = apiVersion.MinorVersion == null ? 0 : apiVersion.MinorVersion;

            return Ok($"This is a post method, major version is {apiVersion.MajorVersion}," +
                $"minor Version is { minorVersion} .Hello from version 3.1");
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindByID(id);

            if (person == null)
                return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonModel person)
        {
            if (!IsPersonValid(person))
                return BadRequest();
            return Ok(_personBusiness.Create(person));
        }

        [HttpPut]
        public IActionResult Put([FromBody] PersonModel person)
        {
            if (person == null)
                return BadRequest();
            return Ok(_personBusiness.Update(person));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }

        public bool IsPersonValid(PersonModel person)
        {
            if (person == null) return false;

            if(string.IsNullOrEmpty(person?.Address) || person?.Age == 0 || string.IsNullOrEmpty(person?.FirstName) 
                || string.IsNullOrEmpty(person?.Gender) || string.IsNullOrEmpty(person?.LastName)) return false;

            return true;
        }
    }
}

