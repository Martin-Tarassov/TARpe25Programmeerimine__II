using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipService;

        public SpaceshipController(ISpaceshipServices spaceshipService)
        {
            _spaceshipService = spaceshipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}