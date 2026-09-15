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

        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud ISpaceshipServices liideses
        //lisage Context
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
            //loome vaheinstantsi domaini ja viewModeli vahel.
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

        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //kui oled teinud vormi, siis see meetod käivitatakse
        //saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks

            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        //kui kasutaja klikib "Details" nuppu, siis see meetod käivitatakse
        //otsib ühe spaceshipi id järgi ja näitab selle andmeid
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var domain = await _spaceshipService.GetById(id);
            if (domain == null) return NotFound();

            //luua vaheinstants domaini ja viewModeli vahel
            var vm = new SpaceshipDetailsViewModel
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

            return View(vm);
        }

        //kui kasutaja klikib "Update" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu on juba täidetud olemasolevad andmed
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var domain = await _spaceshipService.GetById(id);
            if (domain == null) return NotFound();

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

        //kui oled vormi muutnud ja saadad selle ära, siis see meetod käivitatakse
        //saadab uuendatud andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks
            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            //kutsuda teenuse meetodit, mis uuendab andmed andmebaasis
            await _spaceshipService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        //kui kasutaja klikib "Delete" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale kinnituslehe enne kustutamist
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var domain = await _spaceshipService.GetById(id);
            if (domain == null) return NotFound();

            var vm = new SpaceshipDeleteViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Classification = domain.Classification
            };

            return View(vm);
        }

        //kui kasutaja kinnitab kustutamise, siis see meetod käivitatakse
        //kustutab andmed andmebaasist
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _spaceshipService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}