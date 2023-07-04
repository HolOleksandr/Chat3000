using Chat.Blazor.WebAssembly.Models.DTO;
using IndexedDB.Blazor;
using Microsoft.JSInterop;

namespace Chat.Blazor.WebAssembly.IndexDb.Db
{
    public class IndexDbContext : IndexedDb
    {
        public IndexDbContext(IJSRuntime JSRuntime, string name, int version) : base(JSRuntime, name, version) { }

        public IndexedSet<UserDTO>? Users { get; set; }
    }
}
