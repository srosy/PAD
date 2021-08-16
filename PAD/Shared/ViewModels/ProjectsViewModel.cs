namespace PAD.Shared.ViewModels
{
    public class ProjectsViewModel : ViewModel
    {
        public string BaseImageUrl { get; set; }
        public string NoImageUrl => "https://pixelartdesigner.blob.core.windows.net/images/placeholder.webp";
    }
}
