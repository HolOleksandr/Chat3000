namespace Chat.Blazor.WebAssembly.Models.DTO
{
    public class GroupInfoViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string AdminId { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public int TotalMessages { get; set; }
        public int Members { get; set; }
    }
}