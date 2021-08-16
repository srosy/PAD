using PAD.Data.Models;
using System.Collections.Generic;

namespace PAD.Shared.ViewModels
{
    public class PalletteEditorViewModel : ViewModel
    {
        public bool HasPallettes { get; set; }
        public string _baseImagePath;
        public List<PAD.Data.Models.Palette> _palettes;
        public string pickedHexCode = string.Empty;
    }
}
