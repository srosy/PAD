using System.Collections.Generic;

namespace PAD.Shared.ViewModels
{
    public class ProfileViewModel : ViewModel
    {
        public List<Data.Models.Account> Accounts { get; set; }
        public string BaseImageUrl { get; set; }
    }
}

