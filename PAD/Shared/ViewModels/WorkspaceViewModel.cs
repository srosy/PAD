using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using PAD.Data.Models;
using System.Collections.Generic;

namespace PAD.Shared.ViewModels
{
    public class WorkspaceViewModel : ViewModel
    {
        public List<Palette> Palletes { get; set; }
        public Project Project { get; set; }

        public bool HasProjects { get; set; } = false;
        public bool NotUsersProject { get; set; } = true;

        public int NumGridRows { get; set; }
        public int DefaultNumGridRows => 700;
        public int NumGridCols { get; set; }
        public int DefaultNumGridCols => 1000;
        public int Scale => 10;

        public double mouseX { get; set; }
        public double mouseY { get; set; }
        public double lastX { get; set; }
        public double lastY { get; set; }
        public bool mouseDown { get; set; }
        public bool leftClick { get; set; }
        public bool isDrawing { get; set; }

        public int gridSize1 = 50;
        public int gridSize2 = 50;
        public bool isShow = true;
        public Canvas2DContext Context { get; set; }
        public BECanvasComponent Canvas { get; set; } = new BECanvasComponent();

        public int BrushSize { get; set; } = 5;
        public string SelectedColor { get; set; }
        public string AltSelectedColor { get; set; }
        public string SelectedBrushShape { get; set; }

        public PointerType CurrentPointer { get; set; } = PointerType.BRUSH;
        public StampType CurrentStamp { get; set; } = StampType.LIKE;
        public string PointerClass { get; set; }

        public Dictionary<PointerType, string> PointerClasses = new Dictionary<PointerType, string>()
        {
            { PointerType.BRUSH, "paintbrush" }, // PointerType, CssClass
            { PointerType.ARROW, "paintarrow"},
            { PointerType.BUCKET, "paintbucket"},
            { PointerType.TRANSFORMATIONS, "transformations"},
            { PointerType.EYEDROPPER, "painteyedropper"},
            { PointerType.STAMP, "paintstamp" },
        };
    }

    public enum PointerType
    {
        BRUSH,
        ARROW,
        BUCKET,
        TRANSFORMATIONS,
        EYEDROPPER,
        STAMP
    }

    public enum StampType
    {
        LIKE,
        LOVE,
        WOW,
        COOL,
        SAD,
        HATE
    }
}
