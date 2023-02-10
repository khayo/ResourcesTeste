using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ResourcesTeste.Resources;

namespace ResourcesTeste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResoucesTesteController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public ResoucesTesteController(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("ResourceStringList")]
        public List<string> ResourceStringList()
        {
            return _localizer.GetAllStrings().ToList().Select(s => s.Value).ToList();
        }

        [HttpGet("ResourceList")]
        public List<LocalizedString> ResourceList()
        {
            return _localizer.GetAllStrings().ToList();
        }

        [HttpGet("ResourceCadeira")]
        public string ResourceCadeira()
        {
            return _localizer["stringCadeira"];
        }
    }
}
