using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;

namespace BoxOffice.Core.Data.Entities
{
    public static partial class SpectacleMapper
    {
        public static SpectacleDto AdaptToDto(this Spectacle p1)
        {
            return p1 == null ? null : new SpectacleDto()
            {
                Id = p1.Id,
                Name = p1.Name,
                Description = p1.Description,
                TotalTicket = p1.TotalTicket,
                StartTime = p1.StartTime,
                EndTime = p1.EndTime
            };
        }
        public static SpectacleDto AdaptTo(this Spectacle p2, SpectacleDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            SpectacleDto result = p3 ?? new SpectacleDto();
            
            result.Id = p2.Id;
            result.Name = p2.Name;
            result.Description = p2.Description;
            result.TotalTicket = p2.TotalTicket;
            result.StartTime = p2.StartTime;
            result.EndTime = p2.EndTime;
            return result;
            
        }
    }
}