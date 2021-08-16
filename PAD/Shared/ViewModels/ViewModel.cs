using PAD.Data;
using PAD.Data.Models;

namespace PAD.Shared.ViewModels
{
    public abstract class ViewModel
    {
        public bool Loading { get; set; } = true;
        public bool UserAuthenticated { get; set; } = false;
        public Account Account { get; set; }
        public Session Session { get; set; }
    }
}
