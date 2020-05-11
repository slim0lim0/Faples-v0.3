using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaplesEditor
{
    public class fpxObject
    {
        public string Name { get; set; } = "";
        public string SpriteSheet { get; set; } = "";
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;
        public int width { get; set; } = 1;
        public int height { get; set; } = 1;
        public bool Animated { get; set; } = false;
        public int AnimationIndex { get; set; } = 0;
        public List<fpxAnimation> Animations { get; set; } = new List<fpxAnimation>();
        public Dictionary<int, List<fpxHold>> HoldGroups { get; set; } = new Dictionary<int, List<fpxHold>>();
    }

    public class fpxTile
    {
        public string Name { get; set; } = "";
        public string SpriteSheet { get; set; } = "";
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;
        public int width { get; set; } = 1;
        public int height { get; set; } = 1;
        public Dictionary<int, List<fpxHold>> HoldGroups { get; set; } = new Dictionary<int, List<fpxHold>>();
    }

    public class fpxAnimation
    {
        public string Name { get; set; } = "";
        public int ReelIndex { get; set; } = 1;
        public int TotalFrames { get; set; } = 1;
        public int ReelHeight { get; set; } = 1;
        public int FrameWidth { get; set; } = 1;

        public decimal FrameSpeed { get; set; } = 1m;

        public fpxAnimation()
        {

        }
    }

    public class fpxHold
    {
        public int id { get; set; } = 0;
        public int gid { get; set; } = 0;
        public int lid { get; set; } = 0;
        public int x1 { get; set; } = 0;
        public int y1 { get; set; } = 0;
        public int x2 { get; set; } = 0;
        public int y2 { get; set; } = 0;
        public int nextid { get; set; } = -1;
        public int previd { get; set; } = -1;
        public fpxHold next { get; set; } = null;
        public fpxHold prev { get; set; } = null;
        public int force { get; set; } = 0;
        public bool cantPass { get; set; } = true;
        public bool cantDrop { get; set; } = false;
        public bool cantMove { get; set; } = false;
        public string type = "foothold";
    }

    public class fpxRegion
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public List<fpxMap> Maps { get; set; } = new List<fpxMap>();
    }

    public class fpxMap
    {
        public int MapID { get; set; } = 0;
        public int RegionID { get; set; } = 0;
        public string MapName { get; set; } = "";
        public int MapWidth { get; set; } = 1;
        public int MapHeight { get; set; } = 1;
        public int MapBoundR { get; set; } = 0;
        public int MapBoundL { get; set; } = 0;
        public int MapBoundT { get; set; } = 0;
        public int MapBoundB { get; set; } = 0;
        public string BackgroundMusic { get; set; } = "";
        public List<fpxMapPortal> MapPortals { get; set; } = new List<fpxMapPortal>();
        public List<fpxMapLayer> MapLayers { get; set; } = new List<fpxMapLayer>();
    }
    public class fpxSprite
    {
        public string SpriteSheet { get; set; } = "";
        public string SpriteName { get; set; } = "";
        public Bitmap Sprite { get; set; } = null;
        public int UsageCount { get; set; } = 0;
    }

    public class fpxMapScenery
    {
        public string SpriteName { get; set; } = "";
        public Bitmap Sprite { get; set; } = null;
        public bool Flipped { get; set; } = false;
    }

    public class fpxMapObject
    {
        public Point MapCoor { get; set; } = new Point(0, 0);
        public bool Flipped { get; set; } = false;
        public fpxObject Object { get; set; } = null;
        public Bitmap Sprite { get; set; } = null;
        public bool CannotDrop { get; set; } = false;
    }

    public class fpxMapTile
    {
        public Point MapCoor { get; set; } = new Point(0, 0);
        public bool Flipped { get; set; } = false;
        public fpxTile Tile { get; set; } = null;
        public Bitmap Sprite { get; set; } = null;
        public bool CannotDrop { get; set; } = false;
    }

    public class fpxMapPortal
    {
        public int ID { get; set; } = 0;
        public Point MapCoor { get; set; } = new Point(0, 0);
        public string Type { get; set; } = "";
        public int RegionID { get; set; } = 0;
        public int MapID { get; set; } = 0;

        public int TargetID { get; set; } = 0;
    }

    public class fpxMapLayer
    {
        public int LayerID { get; set; } = 0;
        public decimal ParallaxX { get; set; } = 1m;
        public decimal ParallaxY { get; set; } = 1m;
        public decimal ScrollX { get; set; } = 0m;
        public decimal ScrollY { get; set; } = 0m;
        public bool TileX;
        public bool TileY;
        public fpxMapScenery MapScenery { get; set; } = new fpxMapScenery();
        public List<fpxSprite> MapSprites { get; set; } = new List<fpxSprite>();
        public List<fpxMapObject> MapObjects { get; set; } = new List<fpxMapObject>();
        public List<fpxMapTile> MapTiles { get; set; } = new List<fpxMapTile>();
        public Dictionary<int, List<fpxHold>> MapHolds { get; set; } = new Dictionary<int, List<fpxHold>>();
    }

    public class fpxUI
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public int Width { get; set; } = 1;
        public int Height { get; set; } = 1;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public bool Static { get; set; } = true;
        public List<fpxUILayer> UILayers { get; set; } = new List<fpxUILayer>();
    }


    public class fpxUILayer
    {
        public int LayerID { get; set; } = 0;
        public decimal ScrollX { get; set; } = 0m;
        public decimal ScrollY { get; set; } = 0m;
        public fpxMapScenery UIScenery { get; set; } = new fpxMapScenery();
        public List<fpxSprite> UISprites { get; set; } = new List<fpxSprite>();
        public List<fpxMapObject> UIObjects { get; set; } = new List<fpxMapObject>();
        public List<fpxMapTile> UITiles { get; set; } = new List<fpxMapTile>();
        public List<fpxControl> UIControls { get; set; } = new List<fpxControl>();
    }

    public class fpxControl
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public int LocX { get; set; } = 0;
        public int LocY { get; set; } = 0;

        // Textbox Values
        public bool Multiline { get; set; } = false;
        public FontFamily Font { get; set; } = Utility.GetFontFamilyByName("Lato Black");
        public int Size { get; set; } = 8;
        public Color Color { get; set; } = Color.Black;
        public bool Label { get; set; } = false;
        public string LabelValue { get; set; } = "";

        // Button Values
        public Bitmap SpriteBase { get; set; } = null;
        public string SpriteBaseName { get; set; } = "";
        public Bitmap SpriteHover { get; set; } = null;
        public string SpriteHoverName { get; set; } = "";
        public Bitmap SpriteClick { get; set; } = null;
        public string SpriteClickName { get; set; } = "";
        public Bitmap SpriteFocus { get; set; } = null;
        public string SpriteFocusName { get; set; } = "";

        // Percent Bar Values
        public Bitmap PercentBar { get; set; } = null;
        public string PercentBarName { get; set; } = "";
        public decimal PercentValue { get; set; } = 100;

        // Checkbox/Radio Buttons
        public string FlowDirection { get; set; } = "Horizontal";
        public bool Radio { get; set; } = false;
        public List<fpxOption> Options { get; set; } = new List<fpxOption>();

        // Combobox Values
        public List<string> ComboDisplays { get; set; } = new List<string>();
        public List<string> ComboValues { get; set; } = new List<string>();
    }

    public class fpxOption
    {
        public string SpriteCheckOnName { get; set; } = "";
        public string SpriteCheckOffName { get; set; } = "";
        public Bitmap SpriteCheckOn { get; set; } = null;
        public Bitmap SpriteCheckOff { get; set; } = null;
    }
    public class fpxCharacter
    {
        public string Name { get; set; } = "";

        public bool UseSkeleton { get; set; } = false;
        public bool Animated { get; set; } = false;
        public int FrameWidth { get; set; } = 800;
        public int FrameHeight { get; set; } = 640;
        public List<string> Sprites = new List<string>();
        public List<fpxSkeletonPart> Skeleton { get; set; } = new List<fpxSkeletonPart>();
        public List<fpxCharacterAnimation> Animations { get; set; } = new List<fpxCharacterAnimation>();
        public List<fpxCharacterFeature> Features { get; set; } = new List<fpxCharacterFeature>();
    }
    public class fpxCharacterAnimation
    {
        public string Name { get; set; } = "";
        public decimal Speed { get; set; } = 1m;
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 640;
        public List<fpxAnimationFrame> Frames { get; set; } = new List<fpxAnimationFrame>();
    }
    public class fpxAnimationFrame
    {
        public List<fpxSkeletonPart> Skeleton { get; set; } = new List<fpxSkeletonPart>();
    }
    public class fpxSkeletonPart
    {
        public string Name { get; set; } = "";
        public fpxSkeletonPoint StartPoint { get; set; } = null;
        public fpxSkeletonPoint EndPoint { get; set; } = null;
        public string Type { get; set; } = "Limb";
        public string Sprite { get; set; } = "";
        public Bitmap SpriteBitmap { get; set; } = null;
        public int ZIndex { get; set; } = 0;
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
        public int RelWidth { get; set; } = 0;
        public int RelHeight { get; set; } = 0;
    }
    public class fpxSkeletonPoint
    {
        public Guid ID { get; set; } = new Guid();
        public Point Location { get; set; } = new Point(0, 0);
    }
    public class fpxCharacterFeature
    {
        public string Name { get; set; } = "";
        public string Sprite { get; set; } = "";
        public Bitmap SpriteBitmap { get; set; } = null;
        public string Part { get; set; } = "";
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
        public int Width { get; set; } = 1;
        public int Height { get; set; } = 1;

        public int RelWidth { get; set; } = 0;
        public int RelHeight { get; set; } = 0;
    }
}
