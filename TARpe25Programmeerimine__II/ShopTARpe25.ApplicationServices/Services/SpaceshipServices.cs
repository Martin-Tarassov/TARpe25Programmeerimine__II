using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly ShopTARpe25Context _context;

        public SpaceshipServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship domain = new();

            domain.Id = dto.Id;
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = dto.BuiltDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = dto.ModifiedAt;

            await _context.Spaceships.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Spaceship?> DetailsAsync(Guid id)
        {
             var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            var domain = await _context.Spaceships.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null)
                throw new Exception("Spaceship not found");

            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = dto.BuiltDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task Delete(Guid id)
        {
            var domain = await _context.Spaceships.FirstOrDefaultAsync(x => x.Id == id);

            if (domain != null)
            {
                _context.Spaceships.Remove(domain);
                await _context.SaveChangesAsync();
            }
        }
    }
}