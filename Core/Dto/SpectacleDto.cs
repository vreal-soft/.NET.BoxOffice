namespace BoxOffice.Core.Dto
{
    public class SpectacleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint TotalTicket { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
    }

    public class CreateSpectacle
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint TotalTicket { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
    }
}
