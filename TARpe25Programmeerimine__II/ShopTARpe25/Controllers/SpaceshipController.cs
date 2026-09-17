using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;


namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipService;
        private readonly ShopTARpe25Context _context;

        public SpaceshipController
            (
                ISpaceshipServices spaceshipService,
                ShopTARpe25Context context
            )
        {
            _spaceshipService = spaceshipService;
            _context = context;
        }


        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew,
                    EnginePower = x.EnginePower
                })
                .ToList();

            return View(result);
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

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var domain = await _spaceshipService.DetailsAsync(id);

            var dto = new SpaceshipDetailsViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Classification = domain.Classification,
                BuiltDate = domain.BuiltDate,
                Crew = domain.Crew,
                EnginePower = domain.EnginePower,
                CreatedAt = domain.CreatedAt,
                ModifiedAt = domain.ModifiedAt
            };

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var domain = await _spaceshipService.DetailsAsync(id);

            var vm = new SpaceshipUpdateViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Classification = domain.Classification,
                BuiltDate = domain.BuiltDate,
                Crew = domain.Crew,
                EnginePower = domain.EnginePower
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            await _spaceshipService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var domain = await _spaceshipService.DetailsAsync(id);

            var vm = new SpaceshipDeleteViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Classification = domain.Classification
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _spaceshipService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}