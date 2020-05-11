using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using reNX;
using reNX.NXProperties;

namespace FaplesEditor
{
    static class FPXFormat
    {
        private static readonly byte[] PKG4 = { 0x50, 0x4B, 0x47, 0x34 }; // PKG4
        private static readonly bool _is64bit = IntPtr.Size == 8;
        private static string mapPath = Path.Combine(Utility.EXPORT_FILES_PATH, "Map.fpx");
        private static string audioPath = Path.Combine(Utility.EXPORT_FILES_PATH, "Audio.fpx");
        private static string uiPath = Path.Combine(Utility.EXPORT_FILES_PATH, "UI.fpx");
        private static string charPath = Path.Combine(Utility.EXPORT_FILES_PATH, "Character.fpx");
        private static readonly int sz8 = sizeof(ulong);
        private static readonly int sz4 = sizeof(uint);
        private static readonly int sz2 = sizeof(ushort);
        private static readonly int sz1 = sizeof(byte);

        static List<Node> nodes = new List<Node>();
        static Dictionary<string, int> strings = new Dictionary<string, int>();
        static List<Bitmap> bitmaps = new List<Bitmap>();
        static List<byte[]> mp3s = new List<byte[]>();  

        static Dictionary<string, Bitmap> oSceneryBitmaps = new Dictionary<string, Bitmap>();
        static Dictionary<string, Bitmap> oObjBitmaps = new Dictionary<string, Bitmap>();
        static Dictionary<string, Bitmap> oTileBitmaps = new Dictionary<string, Bitmap>();
        static Dictionary<string, byte[]> oSceneryAudio = new Dictionary<string, byte[]>();
        static Dictionary<string, Bitmap> oControlBitmaps = new Dictionary<string, Bitmap>();
        static Dictionary<string, Bitmap> oCharacterBitmaps = new Dictionary<string, Bitmap>();

        static public void ExportMap()
        {
            XmlDocument xDocObj = new XmlDocument();
            xDocObj.LoadXml(EditorManager.GetObjectCollection().OuterXml);

            XmlDocument xDocTile = new XmlDocument();
            xDocTile.LoadXml(EditorManager.GetTileCollection().OuterXml);

            XmlDocument xDocMap = new XmlDocument();
            xDocMap.LoadXml(EditorManager.GetMapCollection().OuterXml);

            List<Node> ObjCollections = new List<Node>();
            List<Node> TileCollections = new List<Node>();
            List<Node> MapCollections = new List<Node>();
            List<Node> SpriteCollections = new List<Node>();
            List<Node> RegionCollections = new List<Node>();
            // Construct object collection

            XmlNode xObjects = xDocObj.SelectSingleNode("//fpxObjects");

            foreach (XmlNode oCollection in xObjects.ChildNodes)
            {
                Node colNode = new Node();
                colNode.Name = oCollection.Attributes["Name"].Value;
                colNode.Type = 0;

                foreach (XmlNode oCategory in oCollection.ChildNodes)
                {
                    Node catNode = new Node();
                    catNode.Name = oCategory.Attributes["Name"].Value;
                    catNode.Type = 0;

                    foreach (XmlNode oSheet in oCategory.ChildNodes)
                    {
                        Node sheetNode = new Node();
                        sheetNode.Name = oSheet.Attributes["Name"].Value;
                        sheetNode.Type = 0;

                        if (!oObjBitmaps.ContainsKey(oSheet.Attributes["Name"].Value))
                        {
                            string sprite = oSheet.Attributes["Name"].Value;
                            oObjBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.OBJECTS_PATH, sprite)));
                        }

                        foreach (XmlNode oObj in oSheet.ChildNodes)
                        {
                            sheetNode.ChildNodes.Add(ConvertObject(oObj));
                        }

                        ObjCollections.Add(sheetNode);
                        //catNode.ChildNodes.Add(sheetNode);
                    }

                    //colNode.ChildNodes.Add(catNode);
                }

                //ObjCollections.Add(colNode);
            }

            XmlNode xTiles = xDocTile.SelectSingleNode("//fpxTiles");

            foreach (XmlNode oSheet in xTiles.ChildNodes)
            {
                Node sheetNode = new Node();
                sheetNode.Name = oSheet.Attributes["Name"].Value;
                sheetNode.Type = 0;

                if (!oTileBitmaps.ContainsKey(oSheet.Attributes["Name"].Value))
                {
                    string sprite = oSheet.Attributes["Name"].Value;
                    oTileBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.TILES_PATH, sprite)));
                }

                foreach (XmlNode oTile in oSheet.ChildNodes)
                {
                    sheetNode.ChildNodes.Add(ConvertTile(oTile));
                }

                TileCollections.Add(sheetNode);
            }

            XmlNode xMaps = xDocMap.SelectSingleNode("//fpxMaps");

            foreach(XmlNode oRegion in xMaps)
            {
                Node oReg = new Node();
                oReg.Name = oRegion.Attributes["ID"].Value;
                oReg.Type = 3;
                oReg.Type3Data = oRegion.Attributes["ID"].Value;

                Node oRegName = new Node();
                oRegName.Name = "Name";
                oRegName.Type = 3;
                oRegName.Type3Data = oRegion.Attributes["Name"].Value;

                oReg.ChildNodes.Add(oRegName);

                RegionCollections.Add(oReg);


                Node oMapReg = new Node();
                oMapReg.Name = oRegion.Attributes["ID"].Value;
                oMapReg.Type = 0;

                foreach (XmlNode oMap in oRegion.ChildNodes)
                {
                    oMapReg.ChildNodes.Add(ConvertMap(oMap));
                }
                

                MapCollections.Add(oMapReg);
            }



            foreach (string file in oSceneryBitmaps.Keys)
            {
                Node oBmp = new Node();
                oBmp.Name = file;
                oBmp.Type = 5;
                oBmp.Type5Data = oSceneryBitmaps[file];

                SpriteCollections.Add(oBmp);
            }

            foreach (string file in oObjBitmaps.Keys)
            {
                Node oBmp = new Node();
                oBmp.Name = file;
                oBmp.Type = 5;
                oBmp.Type5Data = oObjBitmaps[file];
                
                SpriteCollections.Add(oBmp);
            }

            foreach (string file in oTileBitmaps.Keys)
            {
                Node oBmp = new Node();
                oBmp.Name = file;
                oBmp.Type = 5;
                oBmp.Type5Data = oTileBitmaps[file];

                SpriteCollections.Add(oBmp);
            }

            Node oRootCollection = new Node();
            oRootCollection.Name = "";
            oRootCollection.Type = 0;

            Node oSprites = new Node();
            oSprites.Name = "SpriteSheets";
            oSprites.Type = 0;

            foreach (Node sprite in SpriteCollections)
            {
                oSprites.ChildNodes.Add(sprite);
            }

            oRootCollection.ChildNodes.Add(oSprites);

            oRootCollection.ChildNodes.Add(CreateGlobals());

            Node oMaps = new Node();
            oMaps.Name = "Maps";
            oMaps.Type = 0;

            foreach (Node oMap in MapCollections)
            {
                oMaps.ChildNodes.Add(oMap);
            }

            oRootCollection.ChildNodes.Add(oMaps);

            Node oObjects = new Node();
            oObjects.Name = "Objects";
            oObjects.Type = 0;

            foreach (Node oObj in ObjCollections)
            {
                oObjects.ChildNodes.Add(oObj);
            }

            oRootCollection.ChildNodes.Add(oObjects);

            Node oTiles = new Node();
            oTiles.Name = "Tiles";
            oTiles.Type = 0;    

            foreach (Node oTile in TileCollections)
            {
                oTiles.ChildNodes.Add(oTile);
            }

            oRootCollection.ChildNodes.Add(oTiles);

            Node oRegions = new Node();
            oRegions.Name = "Regions";
            oRegions.Type = 0;

            foreach (Node oRegion in RegionCollections)
            {
                oRegions.ChildNodes.Add(oRegion);
            }

            oRootCollection.ChildNodes.Add(oRegions);

            ExportCollection(oRootCollection, mapPath);
        }
        static public void ExportAudio()
        {
            Node oBgm = new Node();
            oBgm.Name = "bgm";
            oBgm.Type = 0;

            // for each region, we will use 0 for tutorial area, 1 for main land

            Node oRegion0 = new Node();
            oRegion0.Name = "00";
            oRegion0.Type = 0;

            Node oRegion1 = new Node();
            oRegion1.Name = "01";
            oRegion1.Type = 0;

            foreach(string key in oSceneryAudio.Keys)
            {
                Node oMusic = new Node();
                oMusic.Name = key;
                oMusic.Type = 6;
                oMusic.Type6Data = oSceneryAudio[key];

                int region = 1; // get Region ID

                if (region == 0)
                    oRegion0.ChildNodes.Add(oMusic);
                else if (region == 1)
                    oRegion1.ChildNodes.Add(oMusic);
            }

            oBgm.ChildNodes.Add(oRegion0);
            oBgm.ChildNodes.Add(oRegion1);

            Node oRootCollection = new Node();
            oRootCollection.Name = "";
            oRootCollection.Type = 0;

            oRootCollection.ChildNodes.Add(oBgm);

            ExportCollection(oRootCollection, audioPath);
        }
        static public void ExportUI()
        {
            XmlDocument xDocUI = new XmlDocument();
            xDocUI.LoadXml(EditorManager.GetUICollection().OuterXml);
            List<Node> UICollection = new List<Node>();
            List<Node> SpriteCollections = new List<Node>();

            XmlNode xUI = xDocUI.SelectSingleNode("//fpxUI");

            foreach (XmlNode uiNode in xUI.ChildNodes)
            {
                UICollection.Add(ConvertUI(uiNode));
            }

            foreach (string file in oControlBitmaps.Keys)
            {
                Node oBmp = new Node();
                oBmp.Name = file;
                oBmp.Type = (short) NodeType.eNBitmap;
                oBmp.Type5Data = oControlBitmaps[file];

                SpriteCollections.Add(oBmp);
            }

            Node oRootCollection = new Node();
            oRootCollection.Name = "";
            oRootCollection.Type = (short)NodeType.eNProperty;

            Node oSprites = new Node();
            oSprites.Name = "SpriteSheets";
            oSprites.Type = (short)NodeType.eNProperty;

            foreach (Node sprite in SpriteCollections)
            {
                oSprites.ChildNodes.Add(sprite);
            }

            oRootCollection.ChildNodes.Add(oSprites);

            Node oUIs = new Node();
            oUIs.Name = "UI";
            oUIs.Type = 0;

            foreach (Node oUI in UICollection)
            {
                oUIs.ChildNodes.Add(oUI);
            }

            oRootCollection.ChildNodes.Add(oUIs);

            Node oCursor = new Node();
            oCursor.Name = "Cursor";
            oCursor.Type = 0;

            Node oCursorImg = new Node();
            oCursorImg.Name = Path.Combine("Cursor2" + Utility.PNG_EXT);
            oCursorImg.Type = 5;
            oCursorImg.Type5Data = new Bitmap(Path.Combine(Utility.UI_PATH, "Cursor2" + Utility.PNG_EXT));

            oCursor.ChildNodes.Add(oCursorImg);

            oRootCollection.ChildNodes.Add(oCursor);

            ExportCollection(oRootCollection, uiPath);
        }
        static public void ExportCharacter()
        {
            XmlDocument xDocCharacter = new XmlDocument();
            xDocCharacter.LoadXml(EditorManager.GetCharacterCollection().OuterXml);
            List<Node> CharacterCollection = new List<Node>();
            List<Node> SpriteCollection = new List<Node>();

            XmlNode xCharacter = xDocCharacter.SelectSingleNode("//fpxCharacters");

            foreach (XmlNode characterNode in xCharacter.ChildNodes)
            {
                CharacterCollection.Add(ConvertCharacter(characterNode));
            }

            foreach (string file in oCharacterBitmaps.Keys)
            {
                Node oBmp = new Node();
                oBmp.Name = file;
                oBmp.Type = (short)NodeType.eNBitmap;
                oBmp.Type5Data = Utility.ResizeImage(oCharacterBitmaps[file], Utility.GetGameResolutionX(oCharacterBitmaps[file].Width), Utility.GetGameResolutionY(oCharacterBitmaps[file].Height));

                SpriteCollection.Add(oBmp);
            }

            Node oRootCollection = new Node();
            oRootCollection.Name = "";
            oRootCollection.Type = (short)NodeType.eNProperty;

            Node oSprites = new Node();
            oSprites.Name = "Sprites";
            oSprites.Type = (short)NodeType.eNProperty;

            foreach (Node sprite in SpriteCollection)
            {
                oSprites.ChildNodes.Add(sprite);
            }

            oRootCollection.ChildNodes.Add(oSprites);

            Node oCharacters = new Node();
            oCharacters.Name = "Characters";
            oCharacters.Type = 0;

            foreach (Node oCharacter in CharacterCollection)
            {
                oCharacters.ChildNodes.Add(oCharacter);
            }

            oRootCollection.ChildNodes.Add(oCharacters);

            ExportCollection(oRootCollection, charPath);
        }
        static public void Export(string sDirectory)
        {
            ExportUI();
            ExportMap();
            ExportAudio();
            ExportCharacter();

            nodes = new List<Node>();
            strings = new Dictionary<string, int>();
            bitmaps = new List<Bitmap>();
            mp3s = new List<byte[]>();

            oSceneryBitmaps = new Dictionary<string, Bitmap>();
            oObjBitmaps = new Dictionary<string, Bitmap>();
            oTileBitmaps = new Dictionary<string, Bitmap>();
            oSceneryAudio = new Dictionary<string, byte[]>();
            oControlBitmaps = new Dictionary<string, Bitmap>();
            oCharacterBitmaps = new Dictionary<string, Bitmap>();
        }

        static public void ExportCollection(Node Collection, string sFileName)
        {
            nodes.Clear();
            strings.Clear();
            bitmaps.Clear();
            mp3s.Clear();

            using (FileStream outFs = new FileStream(sFileName, FileMode.Create, FileAccess.ReadWrite,
                 FileShare.None))
            using (BinaryWriter bw = new BinaryWriter(outFs))
            {
                PopulateNodeRow(Collection);

                // Write Magic Header
                bw.Write(PKG4);

                // Mark 52 bytes worth of memory for Header information
                bw.Write(new byte[(sz4 + sz8) * sz4]);

                // Check that stream is 4 bytes long
                outFs.EnsureMultiple(sz4);
                ulong nodeOffset = (ulong)bw.BaseStream.Position;

                foreach (Node oNode in nodes)
                {
                    WriteNodeData(oNode, bw);
                }

                ulong nodeMarker = (ulong)bw.BaseStream.Position;

                ulong stringOffset;
                int stringCount = strings.Count;
                ulong[] soffsets = new ulong[stringCount];

                Dictionary<int, string> strnormal = strings.ToDictionary(kpv => kpv.Value, kpv => kpv.Key);
                for (int i = 0; i < strings.Count; i++)
                {
                    outFs.EnsureMultiple(sz2);
                    soffsets[i] = (ulong)bw.BaseStream.Position;
                    byte[] toWrite = Encoding.UTF8.GetBytes(strnormal[i]);
                    bw.Write((ushort)toWrite.Length);
                    bw.Write(toWrite);
                }
                outFs.EnsureMultiple(sz8);
                stringOffset = (ulong)bw.BaseStream.Position;
                for (uint idx = 0; idx < stringCount; ++idx)
                    bw.Write(soffsets[idx]);

                ulong stringMarker = (ulong)bw.BaseStream.Position;

                ulong bitmapOffset = 0UL;
                uint bitmapCount = 0U;
                bitmapCount = (uint)bitmaps.Count;
                ulong[] boffsets = new ulong[bitmapCount];
                for (int i = 0; i < bitmaps.Count; i++)
                {
                    outFs.EnsureMultiple(sz8);
                    boffsets[i] = (ulong)bw.BaseStream.Position;

                    Bitmap b = bitmaps[i];

                    byte[] compressed = GetCompressedBitmap(b);

                    bw.Write((uint)compressed.Length);
                    bw.Write(compressed);
                }
                outFs.EnsureMultiple(sz8);
                bitmapOffset = (ulong)bw.BaseStream.Position;
                for (uint idx = 0; idx < bitmapCount; ++idx)
                    bw.Write(boffsets[idx]);

                ulong bitmapmarker = (ulong)bw.BaseStream.Position;

                ulong soundOffset = 0UL;
                uint soundCount = 0U;
                soundCount = (uint)mp3s.Count;
                ulong[] aoffsets = new ulong[soundCount];
                for (int i = 0; i < mp3s.Count; i++)
                {
                    outFs.EnsureMultiple(sz8);
                    aoffsets[i] = (ulong)bw.BaseStream.Position;

                    byte[] abytes = mp3s[i];
                    bw.Write(abytes);
                    abytes = null;
                }
                outFs.EnsureMultiple(sz8);
                soundOffset = (ulong)bw.BaseStream.Position;
                for (uint idx = 0; idx < soundCount; ++idx)
                    bw.Write(aoffsets[idx]);

                bw.Seek(sz4, SeekOrigin.Begin);
                bw.Write((uint)nodes.Count); // Number of nodes
                bw.Write(nodeOffset);
                bw.Write(stringCount);
                bw.Write(stringOffset);
                bw.Write(bitmapCount);
                bw.Write(bitmapOffset);
                bw.Write(soundCount);
                bw.Write(soundOffset);

                ulong totalMark = (ulong)bw.Seek(0, SeekOrigin.End);
            }
        }
        static private Node ConvertCharacter(XmlNode characterNode)
        {
            Node oCharacter = new Node();
            oCharacter.Name = characterNode.Attributes["Name"].Value;
            oCharacter.Type = 0;

            Node oCharacterUseSkeleton = new Node();
            oCharacterUseSkeleton.Name = "UseSkeleton";
            oCharacterUseSkeleton.Type = (short)NodeType.eNInt64;
            oCharacterUseSkeleton.Type1Data = bool.Parse(characterNode.Attributes["UseSkeleton"].Value) ? 1 : 0;

            Node oCharacterAnimated = new Node();
            oCharacterAnimated.Name = "Animated";
            oCharacterAnimated.Type = (short)NodeType.eNInt64;
            oCharacterAnimated.Type1Data = bool.Parse(characterNode.Attributes["Animated"].Value) ? 1 : 0;

            oCharacter.ChildNodes.Add(oCharacterUseSkeleton);
            oCharacter.ChildNodes.Add(oCharacterAnimated);

            Node oCharacterSkeleton = new Node();
            oCharacterSkeleton.Name = "Skeleton";
            oCharacterSkeleton.Type = (short)NodeType.eNProperty;

            Node oCharacterAnimations = new Node();
            oCharacterAnimations.Name = "Animations";
            oCharacterAnimations.Type = (short)NodeType.eNProperty;

            Node oCharacterFeatures = new Node();
            oCharacterFeatures.Name = "Features";
            oCharacterFeatures.Type = (short)NodeType.eNProperty;

            foreach (XmlNode oChild in characterNode.ChildNodes)
            {
                if(oChild.Name == "Sprites")
                {
                    foreach(XmlNode oSprite in oChild.ChildNodes)
                    {
                        string sprite = oSprite.Attributes["Name"].Value;

                        if (!oCharacterBitmaps.ContainsKey(sprite))
                            oCharacterBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.CHARACTER_PATH, sprite)));
                    }
                }
                else if(oChild.Name == "Skeleton")
                {
                    foreach (XmlNode oSkeletonPart in oChild.ChildNodes)
                    {
                        Node oPart = new Node();
                        oPart.Name = oSkeletonPart.Attributes["Name"].Value;
                        oPart.Type = (short)NodeType.eNProperty;

                        Node oPartType = new Node();
                        oPartType.Name = "Type";
                        oPartType.Type = (short)NodeType.eNString;
                        oPartType.Type3Data = oSkeletonPart.Attributes["Type"].Value;

                        Node oPartSprite = new Node();
                        oPartSprite.Name = "Sprite";
                        oPartSprite.Type = (short)NodeType.eNString;
                        oPartSprite.Type3Data = oSkeletonPart.Attributes["Sprite"].Value;

                        Node oPartZIndex = new Node();
                        oPartZIndex.Name = "ZIndex";
                        oPartZIndex.Type = (short)NodeType.eNInt64;
                        oPartZIndex.Type1Data = int.Parse(oSkeletonPart.Attributes["ZIndex"].Value);

                        Node oPartOffsetX = new Node();
                        oPartOffsetX.Name = "OffsetX";
                        oPartOffsetX.Type = (short)NodeType.eNInt64;
                        oPartOffsetX.Type1Data = int.Parse(oSkeletonPart.Attributes["OffsetX"].Value);

                        Node oPartOffsetY = new Node();
                        oPartOffsetY.Name = "OffsetY";
                        oPartOffsetY.Type = (short)NodeType.eNInt64;
                        oPartOffsetY.Type1Data = int.Parse(oSkeletonPart.Attributes["OffsetY"].Value);

                        oPart.ChildNodes.Add(oPartType);
                        oPart.ChildNodes.Add(oPartSprite);
                        oPart.ChildNodes.Add(oPartZIndex);
                        oPart.ChildNodes.Add(oPartOffsetX);
                        oPart.ChildNodes.Add(oPartOffsetY);

                        foreach (XmlNode oChildPoint in oSkeletonPart.ChildNodes)
                        {
                            if (oChildPoint.Name == "StartPoint")
                            {
                                Node oPartStartID = new Node();
                                oPartStartID.Name = "StartID";
                                oPartStartID.Type = (short)NodeType.eNString;
                                oPartStartID.Type3Data = oChildPoint.Attributes["ID"].Value;

                                Node oPartStartX = new Node();
                                oPartStartX.Name = "x1";
                                oPartStartX.Type = (short)NodeType.eNInt64;
                                oPartStartX.Type1Data = int.Parse(oChildPoint.Attributes["X"].Value);

                                Node oPartStartY = new Node();
                                oPartStartY.Name = "y1";
                                oPartStartY.Type = (short)NodeType.eNInt64;
                                oPartStartY.Type1Data = int.Parse(oChildPoint.Attributes["Y"].Value);

                                oPart.ChildNodes.Add(oPartStartID);
                                oPart.ChildNodes.Add(oPartStartX);
                                oPart.ChildNodes.Add(oPartStartY);
                            }
                            else if (oChildPoint.Name == "EndPoint")
                            {
                                Node oPartEndID = new Node();
                                oPartEndID.Name = "EndID";
                                oPartEndID.Type = (short)NodeType.eNString;
                                oPartEndID.Type3Data = oChildPoint.Attributes["ID"].Value;

                                Node oPartEndX = new Node();
                                oPartEndX.Name = "x2";
                                oPartEndX.Type = (short)NodeType.eNInt64;
                                oPartEndX.Type1Data = int.Parse(oChildPoint.Attributes["X"].Value);

                                Node oPartEndY = new Node();
                                oPartEndY.Name = "y2";
                                oPartEndY.Type = (short)NodeType.eNInt64;
                                oPartEndY.Type1Data = int.Parse(oChildPoint.Attributes["Y"].Value);

                                oPart.ChildNodes.Add(oPartEndID);
                                oPart.ChildNodes.Add(oPartEndX);
                                oPart.ChildNodes.Add(oPartEndY);
                            }
                        }

                        oCharacterSkeleton.ChildNodes.Add(oPart);
                    }
                }
                else if(oChild.Name == "Animations")
                {
                    foreach (XmlNode oCharAnimation in oChild.ChildNodes)
                    {
                        Node oAnimation = new Node();
                        oAnimation.Name = oCharAnimation.Attributes["Name"].Value;
                        oAnimation.Type = (short)NodeType.eNProperty;

                        Node oAnimationWidth = new Node();
                        oAnimationWidth.Name = "Width";
                        oAnimationWidth.Type = (short)NodeType.eNInt64;
                        oAnimationWidth.Type1Data = int.Parse(oCharAnimation.Attributes["Width"].Value);

                        Node oAnimationHeight = new Node();
                        oAnimationHeight.Name = "Height";
                        oAnimationHeight.Type = (short)NodeType.eNInt64;
                        oAnimationHeight.Type1Data = int.Parse(oCharAnimation.Attributes["Height"].Value);

                        Node oAnimationSpeed = new Node();
                        oAnimationSpeed.Name = "Speed";
                        oAnimationSpeed.Type = (short)NodeType.eNDouble;
                        oAnimationSpeed.Type2Data = double.Parse(oCharAnimation.Attributes["Speed"].Value);

                        Node oFrames = new Node();
                        oFrames.Name = "Frames";
                        oFrames.Type = (short)NodeType.eNProperty;

                        oAnimation.ChildNodes.Add(oAnimationWidth);
                        oAnimation.ChildNodes.Add(oAnimationHeight);
                        oAnimation.ChildNodes.Add(oAnimationSpeed);

                        foreach (XmlNode oAnimFrame in oCharAnimation.ChildNodes)
                        {
                            Node oFrame = new Node();
                            oFrame.Name = oAnimFrame.Attributes["ID"].Value;
                            oFrame.Type = (short)NodeType.eNProperty;

                            foreach (XmlNode oSkeletonPart in oAnimFrame.ChildNodes)
                            {
                                Node oPart = new Node();
                                oPart.Name = oSkeletonPart.Attributes["Name"].Value;
                                oPart.Type = (short)NodeType.eNProperty;

                                Node oPartType = new Node();
                                oPartType.Name = "Type";
                                oPartType.Type = (short)NodeType.eNString;
                                oPartType.Type3Data = oSkeletonPart.Attributes["Type"].Value;

                                Node oPartSprite = new Node();
                                oPartSprite.Name = "Sprite";
                                oPartSprite.Type = (short)NodeType.eNString;
                                oPartSprite.Type3Data = oSkeletonPart.Attributes["Sprite"].Value;

                                Node oPartZIndex = new Node();
                                oPartZIndex.Name = "ZIndex";
                                oPartZIndex.Type = (short)NodeType.eNInt64;
                                oPartZIndex.Type1Data = int.Parse(oSkeletonPart.Attributes["ZIndex"].Value);

                                Node oPartOffsetX = new Node();
                                oPartOffsetX.Name = "OffsetX";
                                oPartOffsetX.Type = (short)NodeType.eNInt64;
                                oPartOffsetX.Type1Data = int.Parse(oSkeletonPart.Attributes["OffsetX"].Value);

                                Node oPartOffsetY = new Node();
                                oPartOffsetY.Name = "OffsetY";
                                oPartOffsetY.Type = (short)NodeType.eNInt64;
                                oPartOffsetY.Type1Data = int.Parse(oSkeletonPart.Attributes["OffsetY"].Value);

                                oPart.ChildNodes.Add(oPartType);
                                oPart.ChildNodes.Add(oPartSprite);
                                oPart.ChildNodes.Add(oPartZIndex);
                                oPart.ChildNodes.Add(oPartOffsetX);
                                oPart.ChildNodes.Add(oPartOffsetY);

                                foreach (XmlNode oChildPoint in oSkeletonPart.ChildNodes)
                                {
                                    if (oChildPoint.Name == "StartPoint")
                                    {
                                        Node oPartStartID = new Node();
                                        oPartStartID.Name = "StartID";
                                        oPartStartID.Type = (short)NodeType.eNString;
                                        oPartStartID.Type3Data = oChildPoint.Attributes["ID"].Value;

                                        Node oPartStartX = new Node();
                                        oPartStartX.Name = "x1";
                                        oPartStartX.Type = (short)NodeType.eNInt64;
                                        oPartStartX.Type1Data = int.Parse(oChildPoint.Attributes["X"].Value);

                                        Node oPartStartY = new Node();
                                        oPartStartY.Name = "y1";
                                        oPartStartY.Type = (short)NodeType.eNInt64;
                                        oPartStartY.Type1Data = int.Parse(oChildPoint.Attributes["Y"].Value);

                                        oPart.ChildNodes.Add(oPartStartID);
                                        oPart.ChildNodes.Add(oPartStartX);
                                        oPart.ChildNodes.Add(oPartStartY);
                                    }
                                    else if (oChildPoint.Name == "EndPoint")
                                    {
                                        Node oPartEndID = new Node();
                                        oPartEndID.Name = "EndID";
                                        oPartEndID.Type = (short)NodeType.eNString;
                                        oPartEndID.Type3Data = oChildPoint.Attributes["ID"].Value;

                                        Node oPartEndX = new Node();
                                        oPartEndX.Name = "x2";
                                        oPartEndX.Type = (short)NodeType.eNInt64;
                                        oPartEndX.Type1Data = int.Parse(oChildPoint.Attributes["X"].Value);

                                        Node oPartEndY = new Node();
                                        oPartEndY.Name = "y2";
                                        oPartEndY.Type = (short)NodeType.eNInt64;
                                        oPartEndY.Type1Data = int.Parse(oChildPoint.Attributes["Y"].Value);

                                        oPart.ChildNodes.Add(oPartEndID);
                                        oPart.ChildNodes.Add(oPartEndX);
                                        oPart.ChildNodes.Add(oPartEndY);
                                    }
                                }

                                oFrame.ChildNodes.Add(oPart);
                            }

                            oFrames.ChildNodes.Add(oFrame);
                        }

                        oAnimation.ChildNodes.Add(oFrames);

                        oCharacterAnimations.ChildNodes.Add(oAnimation);
                    }
                }
                else if(oChild.Name == "Features")
                {
                    foreach(XmlNode oCharFeature in oChild.ChildNodes)
                    {
                        Node oFeature = new Node();
                        oFeature.Name = oCharFeature.Attributes["Name"].Value;
                        oFeature.Type = (short)NodeType.eNProperty;

                        Node oFeaturePart = new Node();
                        oFeaturePart.Name = "Part";
                        oFeaturePart.Type = (short)NodeType.eNString;
                        oFeaturePart.Type3Data = oCharFeature.Attributes["Part"].Value;

                        Node oFeatureType = new Node();
                        oFeatureType.Name = "Type";
                        oFeatureType.Type = (short)NodeType.eNString;
                        oFeatureType.Type3Data = oCharFeature.Attributes["Type"].Value;

                        Node oFeatureSprite = new Node();
                        oFeatureSprite.Name = "Sprite";
                        oFeatureSprite.Type = (short)NodeType.eNString;
                        oFeatureSprite.Type3Data = oCharFeature.Attributes["Sprite"].Value;

                        Node oFeatureOffsetX= new Node();
                        oFeatureOffsetX.Name = "OffsetX";
                        oFeatureOffsetX.Type = (short)NodeType.eNInt64;
                        oFeatureOffsetX.Type1Data = int.Parse(oCharFeature.Attributes["X"].Value);

                        Node oFeatureOffsetY = new Node();
                        oFeatureOffsetY.Name = "OffsetY";
                        oFeatureOffsetY.Type = (short)NodeType.eNInt64;
                        oFeatureOffsetY.Type1Data = int.Parse(oCharFeature.Attributes["Y"].Value);

                        Node oFeatureWidth = new Node();
                        oFeatureWidth.Name = "Width";
                        oFeatureWidth.Type = (short)NodeType.eNInt64;
                        oFeatureWidth.Type1Data = int.Parse(oCharFeature.Attributes["Width"].Value);

                        Node oFeatureHeight = new Node();
                        oFeatureHeight.Name = "Height";
                        oFeatureHeight.Type = (short)NodeType.eNInt64;
                        oFeatureHeight.Type1Data = int.Parse(oCharFeature.Attributes["Height"].Value);

                        oFeature.ChildNodes.Add(oFeaturePart);
                        oFeature.ChildNodes.Add(oFeatureType);
                        oFeature.ChildNodes.Add(oFeatureSprite);
                        oFeature.ChildNodes.Add(oFeatureOffsetX);
                        oFeature.ChildNodes.Add(oFeatureOffsetY);
                        oFeature.ChildNodes.Add(oFeatureWidth);
                        oFeature.ChildNodes.Add(oFeatureHeight);

                        oCharacterFeatures.ChildNodes.Add(oFeature);
                    }
                }
            }

            oCharacter.ChildNodes.Add(oCharacterSkeleton);
            oCharacter.ChildNodes.Add(oCharacterAnimations);
            oCharacter.ChildNodes.Add(oCharacterFeatures);

            return oCharacter;
        }
        static private Node ConvertUI(XmlNode uiNode)
        {
            Node oUI = new Node();
            oUI.Name = uiNode.Attributes["Name"].Value;
            oUI.Type = (short)NodeType.eNProperty;

            Node oUIWidth = new Node();
            oUIWidth.Name = "width";
            oUIWidth.Type = (short)NodeType.eNInt64;
            oUIWidth.Type1Data = int.Parse(uiNode.Attributes["width"].Value);

            Node oUIHeight = new Node();
            oUIHeight.Name = "height";
            oUIHeight.Type = (short)NodeType.eNInt64;
            oUIHeight.Type1Data = int.Parse(uiNode.Attributes["height"].Value);

            Node oUIX = new Node();
            oUIX.Name = "x";
            oUIX.Type = (short)NodeType.eNInt64;
            oUIX.Type1Data = int.Parse(uiNode.Attributes["x"].Value);

            Node oUIY = new Node();
            oUIY.Name = "y";
            oUIY.Type = (short)NodeType.eNInt64;
            oUIY.Type1Data = int.Parse(uiNode.Attributes["y"].Value);

            Node oUIStatic = new Node();
            oUIStatic.Name = "static";
            oUIStatic.Type = (short)NodeType.eNInt64;
            oUIStatic.Type1Data = bool.Parse(uiNode.Attributes["static"].Value) ? 1 : 0;

            Node oLayers = new Node();
            oLayers.Name = "Layers";
            oLayers.Type = (short)NodeType.eNProperty;

            foreach (XmlNode oxLayer in uiNode.FirstChild.ChildNodes)
            {
                Node oLayer = new Node();
                oLayer.Name = oxLayer.Attributes["ID"].Value;
                oLayer.Type = (short)NodeType.eNProperty;

                Node oParallaxX = new Node();
                oParallaxX.Name = "ParallaxX";
                oParallaxX.Type = 2;
                oParallaxX.Type2Data = 0;

                Node oParallaxY = new Node();
                oParallaxY.Name = "ParallaxY";
                oParallaxY.Type = 2;
                oParallaxY.Type2Data = 0;

                Node oScrollX = new Node();
                oScrollX.Name = "ScrollX";
                oScrollX.Type = 2;
                oScrollX.Type2Data = double.Parse(oxLayer.Attributes["ScrollX"].Value);

                Node oScrollY = new Node();
                oScrollY.Name = "ScrollY";
                oScrollY.Type = 2;
                oScrollY.Type2Data = double.Parse(oxLayer.Attributes["ScrollY"].Value);

                Node oTileX = new Node();
                oTileX.Name = "TileX";
                oTileX.Type = 1;
                oTileX.Type1Data = 0;

                Node oTileY = new Node();
                oTileY.Name = "TileY";
                oTileY.Type = 1;
                oTileY.Type1Data = 0;

                Node oScenery = new Node();
                oScenery.Name = "Scenery";
                oScenery.Type = (short)NodeType.eNProperty;

                Node oObjects = new Node();
                oObjects.Name = "Objects";
                oObjects.Type = (short)NodeType.eNProperty;

                Node oTiles = new Node();
                oTiles.Name = "Tiles";
                oTiles.Type = (short)NodeType.eNProperty;

                Node oControls = new Node();
                oControls.Name = "Controls";
                oControls.Type = (short)NodeType.eNProperty;

                foreach (XmlNode oChild in oxLayer.ChildNodes)
                {
                    if (oChild.Name == "Scenery")
                    {
                        Node oSprite = new Node();
                        oSprite.Name = "background";
                        oSprite.Type = 3;
                        oSprite.Type3Data = oChild.Attributes["Sprite"].Value;

                        if (!string.IsNullOrWhiteSpace(oChild.Attributes["Sprite"].Value))
                        {
                            string sprite = oChild.Attributes["Sprite"].Value;

                            if (!oSceneryBitmaps.ContainsKey(sprite))
                                oSceneryBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.SCENERY_PATH, sprite)));
                        }

                        Node oFlipped = new Node();
                        oFlipped.Name = "flipped";
                        oFlipped.Type = 1;
                        oFlipped.Type1Data = bool.Parse(oChild.Attributes["Flipped"].Value) ? 1 : 0;

                        oScenery.ChildNodes.Add(oParallaxX);
                        oScenery.ChildNodes.Add(oParallaxY);
                        oScenery.ChildNodes.Add(oScrollX);
                        oScenery.ChildNodes.Add(oScrollY);
                        oScenery.ChildNodes.Add(oTileX);
                        oScenery.ChildNodes.Add(oTileY);
                        oScenery.ChildNodes.Add(oFlipped);
                        oScenery.ChildNodes.Add(oSprite);
                    }
                    else if (oChild.Name == "Objects")
                    {
                        foreach (XmlNode objn in oChild.ChildNodes)
                        {
                            Node oObject = new Node();
                            oObject.Name = oObjects.ChildNodes.Count.ToString();
                            oObject.Type = 0;

                            Node oObjName = new Node();
                            oObjName.Name = "Name";
                            oObjName.Type = 3;
                            oObjName.Type3Data = objn.Attributes["Name"].Value;


                            Node oObjSprSheet = new Node();
                            oObjSprSheet.Name = "SpriteSheet";
                            oObjSprSheet.Type = 3;
                            oObjSprSheet.Type3Data = objn.Attributes["SpriteSheet"].Value;

                            Node oMapCoorX = new Node();
                            oMapCoorX.Name = "mapX";
                            oMapCoorX.Type = 1;
                            oMapCoorX.Type1Data = int.Parse(objn.Attributes["MapCoorX"].Value);

                            Node oMapCoorY = new Node();
                            oMapCoorY.Name = "mapY";
                            oMapCoorY.Type = 1;
                            oMapCoorY.Type1Data = int.Parse(objn.Attributes["MapCoorY"].Value);

                            Node oFlipped = new Node();
                            oFlipped.Name = "flipped";
                            oFlipped.Type = 1;
                            oFlipped.Type1Data = bool.Parse(objn.Attributes["Flipped"].Value) ? 1 : 0;

                            Node oCannotDrop = new Node();
                            oCannotDrop.Name = "cannotDrop";
                            oCannotDrop.Type = 1;
                            oCannotDrop.Type1Data = 0; // not necessary for UI

                            oObject.ChildNodes.Add(oObjName);
                            oObject.ChildNodes.Add(oObjSprSheet);
                            oObject.ChildNodes.Add(oMapCoorX);
                            oObject.ChildNodes.Add(oMapCoorY);
                            oObject.ChildNodes.Add(oFlipped);
                            oObject.ChildNodes.Add(oCannotDrop);

                            oObjects.ChildNodes.Add(oObject);
                        }
                    }
                    else if (oChild.Name == "Tiles")
                    {
                        foreach (XmlNode tilen in oChild.ChildNodes)
                        {
                            Node oTile = new Node();
                            oTile.Name = oObjects.ChildNodes.Count.ToString();
                            oTile.Type = 0;

                            Node oTileName = new Node();
                            oTileName.Name = "Name";
                            oTileName.Type = 3;
                            oTileName.Type3Data = tilen.Attributes["Name"].Value;

                            Node oTileSprSheet = new Node();
                            oTileSprSheet.Name = "SpriteSheet";
                            oTileSprSheet.Type = 3;
                            oTileSprSheet.Type3Data = tilen.Attributes["SpriteSheet"].Value;

                            Node oMapCoorX = new Node();
                            oMapCoorX.Name = "mapX";
                            oMapCoorX.Type = 1;
                            oMapCoorX.Type1Data = int.Parse(tilen.Attributes["MapCoorX"].Value);

                            Node oMapCoorY = new Node();
                            oMapCoorY.Name = "mapY";
                            oMapCoorY.Type = 1;
                            oMapCoorY.Type1Data = int.Parse(tilen.Attributes["MapCoorY"].Value);

                            Node oFlipped = new Node();
                            oFlipped.Name = "flipped";
                            oFlipped.Type = 1;
                            oFlipped.Type1Data = bool.Parse(tilen.Attributes["Flipped"].Value) ? 1 : 0;

                            Node oCannotDrop = new Node();
                            oCannotDrop.Name = "cannotDrop";
                            oCannotDrop.Type = 1;
                            oCannotDrop.Type1Data = 0; // not necessary for UI

                            oTile.ChildNodes.Add(oTileName);
                            oTile.ChildNodes.Add(oTileSprSheet);
                            oTile.ChildNodes.Add(oMapCoorX);
                            oTile.ChildNodes.Add(oMapCoorY);
                            oTile.ChildNodes.Add(oFlipped);
                            oTile.ChildNodes.Add(oCannotDrop);

                            oTiles.ChildNodes.Add(oTile);
                        }
                    }
                    else if (oChild.Name == "Controls")
                    {
                        foreach (XmlNode controln in oChild.ChildNodes)
                        {
                            Node oControl = new Node();
                            oControl.Name = controln.Attributes["Name"].Value;
                            oControl.Type = (short)NodeType.eNProperty;

                            Node oControlType = new Node();
                            oControlType.Name = "Type";
                            oControlType.Type = (short)NodeType.eNString;
                            oControlType.Type3Data = controln.Attributes["Type"].Value;

                            Node oControlWidth = new Node();
                            oControlWidth.Name = "Width";
                            oControlWidth.Type = (short)NodeType.eNInt64;
                            oControlWidth.Type1Data = int.Parse(controln.Attributes["Width"].Value);

                            Node oControlHeight = new Node();
                            oControlHeight.Name = "Height";
                            oControlHeight.Type = (short)NodeType.eNInt64;
                            oControlHeight.Type1Data = int.Parse(controln.Attributes["Height"].Value);

                            Node oControlLocX = new Node();
                            oControlLocX.Name = "LocX";
                            oControlLocX.Type = (short)NodeType.eNInt64;
                            oControlLocX.Type1Data = int.Parse(controln.Attributes["LocX"].Value);

                            Node oControlLocY = new Node();
                            oControlLocY.Name = "LocY";
                            oControlLocY.Type = (short)NodeType.eNInt64;
                            oControlLocY.Type1Data = int.Parse(controln.Attributes["LocY"].Value);

                            Node oControlMultiline = new Node();
                            oControlMultiline.Name = "Multiline";
                            oControlMultiline.Type = (short)NodeType.eNInt64;
                            oControlMultiline.Type1Data = bool.Parse(controln.Attributes["Multiline"].Value) ? 1 : 0;

                            Node oControlFont = new Node();
                            oControlFont.Name = "Font";
                            oControlFont.Type = (short)NodeType.eNString;
                            oControlFont.Type3Data = controln.Attributes["Font"].Value;

                            Node oControlSize = new Node();
                            oControlSize.Name = "Size";
                            oControlSize.Type = (short)NodeType.eNInt64;
                            oControlSize.Type1Data = int.Parse(controln.Attributes["Size"].Value);

                            Node oControlLabel = new Node();
                            oControlLabel.Name = "Label";
                            oControlLabel.Type = (short)NodeType.eNInt64;
                            oControlLabel.Type1Data = bool.Parse(controln.Attributes["Label"].Value) ? 1 : 0;

                            Node oControlLabelValue = new Node();
                            oControlLabelValue.Name = "LabelValue";
                            oControlLabelValue.Type = (short)NodeType.eNString;
                            oControlLabelValue.Type3Data = controln.Attributes["LabelValue"].Value;

                            Node oControlColorR = new Node();
                            oControlColorR.Name = "ColorR";
                            oControlColorR.Type = (short)NodeType.eNInt64;
                            oControlColorR.Type1Data = int.Parse(controln.Attributes["ColorR"].Value);

                            Node oControlColorG = new Node();
                            oControlColorG.Name = "ColorG";
                            oControlColorG.Type = (short)NodeType.eNInt64;
                            oControlColorG.Type1Data = int.Parse(controln.Attributes["ColorG"].Value);

                            Node oControlColorB = new Node();
                            oControlColorB.Name = "ColorB";
                            oControlColorB.Type = (short)NodeType.eNInt64;
                            oControlColorB.Type1Data = int.Parse(controln.Attributes["ColorB"].Value);

                            Node oControlButtonBase = new Node();
                            oControlButtonBase.Name = "ButtonBase";
                            oControlButtonBase.Type = (short)NodeType.eNString;
                            oControlButtonBase.Type3Data = controln.Attributes["ButtonBase"].Value;

                            if (!string.IsNullOrWhiteSpace(controln.Attributes["ButtonBase"].Value))
                            {
                                string sprite = controln.Attributes["ButtonBase"].Value;

                                if (!oControlBitmaps.ContainsKey(sprite))
                                    oControlBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.CONTROL_PATH, sprite)));
                            }

                            Node oControlButtonHover = new Node();
                            oControlButtonHover.Name = "ButtonHover";
                            oControlButtonHover.Type = (short)NodeType.eNString;
                            oControlButtonHover.Type3Data = controln.Attributes["ButtonHover"].Value;

                            if (!string.IsNullOrWhiteSpace(controln.Attributes["ButtonHover"].Value))
                            {
                                string sprite = controln.Attributes["ButtonHover"].Value;

                                if (!oControlBitmaps.ContainsKey(sprite))
                                    oControlBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.CONTROL_PATH, sprite)));
                            }

                            Node oControlButtonClick = new Node();
                            oControlButtonClick.Name = "ButtonClick";
                            oControlButtonClick.Type = (short)NodeType.eNString;
                            oControlButtonClick.Type3Data = controln.Attributes["ButtonClick"].Value;

                            if (!string.IsNullOrWhiteSpace(controln.Attributes["ButtonClick"].Value))
                            {
                                string sprite = controln.Attributes["ButtonClick"].Value;

                                if (!oControlBitmaps.ContainsKey(sprite))
                                    oControlBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.CONTROL_PATH, sprite)));
                            }

                            Node oControlButtonFocus = new Node();
                            oControlButtonFocus.Name = "ButtonFocus";
                            oControlButtonFocus.Type = (short)NodeType.eNString;
                            oControlButtonFocus.Type3Data = controln.Attributes["ButtonFocus"].Value;

                            if (!string.IsNullOrWhiteSpace(controln.Attributes["ButtonFocus"].Value))
                            {
                                string sprite = controln.Attributes["ButtonFocus"].Value;

                                if (!oControlBitmaps.ContainsKey(sprite))
                                    oControlBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.CONTROL_PATH, sprite)));
                            }

                            Node oControlPercentBar = new Node();
                            oControlPercentBar.Name = "PercentBar";
                            oControlPercentBar.Type = (short)NodeType.eNString;
                            oControlPercentBar.Type3Data = controln.Attributes["PercentBar"].Value;

                            if (!string.IsNullOrWhiteSpace(controln.Attributes["PercentBar"].Value))
                            {
                                string sprite = controln.Attributes["PercentBar"].Value;

                                if (!oControlBitmaps.ContainsKey(sprite))
                                    oControlBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.CONTROL_PATH, sprite)));
                            }

                            Node oControlFlowDrection = new Node();
                            oControlFlowDrection.Name = "FlowDirection";
                            oControlFlowDrection.Type = (short)NodeType.eNString;
                            oControlFlowDrection.Type3Data = controln.Attributes["FlowDirection"].Value;

                            Node oControlRadio = new Node();
                            oControlRadio.Name = "Radio";
                            oControlRadio.Type = (short)NodeType.eNInt64;
                            oControlRadio.Type1Data = bool.Parse(controln.Attributes["Radio"].Value) ? 1 : 0;

                            oControl.ChildNodes.Add(oControlType);
                            oControl.ChildNodes.Add(oControlWidth);
                            oControl.ChildNodes.Add(oControlHeight);
                            oControl.ChildNodes.Add(oControlLocX);
                            oControl.ChildNodes.Add(oControlLocY);
                            oControl.ChildNodes.Add(oControlMultiline);
                            oControl.ChildNodes.Add(oControlFont);
                            oControl.ChildNodes.Add(oControlSize);
                            oControl.ChildNodes.Add(oControlLabel);
                            oControl.ChildNodes.Add(oControlLabelValue);
                            oControl.ChildNodes.Add(oControlColorR);
                            oControl.ChildNodes.Add(oControlColorG);
                            oControl.ChildNodes.Add(oControlColorB);
                            oControl.ChildNodes.Add(oControlButtonBase);
                            oControl.ChildNodes.Add(oControlButtonHover);
                            oControl.ChildNodes.Add(oControlButtonClick);
                            oControl.ChildNodes.Add(oControlButtonFocus);
                            oControl.ChildNodes.Add(oControlPercentBar);
                            oControl.ChildNodes.Add(oControlFlowDrection);
                            oControl.ChildNodes.Add(oControlRadio);

                            oControls.ChildNodes.Add(oControl);
                        }
                    }
                }

               
                oLayer.ChildNodes.Add(oScenery);
                oLayer.ChildNodes.Add(oObjects);
                oLayer.ChildNodes.Add(oTiles);
                oLayer.ChildNodes.Add(oControls);

                oLayers.ChildNodes.Add(oLayer);
            }

            oUI.ChildNodes.Add(oUIWidth);
            oUI.ChildNodes.Add(oUIHeight);
            oUI.ChildNodes.Add(oUIX);
            oUI.ChildNodes.Add(oUIY);
            oUI.ChildNodes.Add(oUIStatic);
            oUI.ChildNodes.Add(oLayers);

            return oUI;
        }
        static private Node ConvertObject(XmlNode objNode)
        {
            Node obj = new Node();
            obj.Name = objNode.Attributes["Name"].Value;
            obj.Type = (short)NodeType.eNString;
            obj.Type3Data = objNode.Attributes["Name"].Value;

            //Node oName = new Node();
            //oName.Name = "Name";
            //oName.Type = (short) NodeType.eNString;
            //oName.Type3Data = objNode.Attributes["Name"].Value;

            Node oX = new Node();
            oX.Name = "x";
            oX.Type = (short)NodeType.eNInt64;
            oX.Type1Data = int.Parse(objNode.Attributes["x"].Value);

            Node oY = new Node();
            oY.Name = "y";
            oY.Type = (short)NodeType.eNInt64;
            oY.Type1Data = int.Parse(objNode.Attributes["y"].Value);

            Node oWidth = new Node();
            oWidth.Name = "width";
            oWidth.Type = (short)NodeType.eNInt64;
            oWidth.Type1Data = int.Parse(objNode.Attributes["width"].Value);

            Node oHeight = new Node();
            oHeight.Name = "height";
            oHeight.Type = (short)NodeType.eNInt64;
            oHeight.Type1Data = int.Parse(objNode.Attributes["height"].Value);

            Node oAnim = new Node();
            oAnim.Name = "Anim";
            oAnim.Type = (short)NodeType.eNProperty;

            Node oAnimated = new Node();
            oAnimated.Name = "Animated";
            oAnimated.Type = (short)NodeType.eNInt64;
            oAnimated.Type1Data = bool.Parse(objNode.Attributes["Animated"].Value) ? 1 : 0;

            Node oAnimIndex = new Node();
            oAnimIndex.Name = "Index";
            oAnimIndex.Type = (short)NodeType.eNInt64;
            oAnimIndex.Type1Data = int.Parse(objNode.FirstChild.Attributes["Index"].Value);

            oAnim.ChildNodes.Add(oAnimated);
            oAnim.ChildNodes.Add(oAnimIndex);

            Node oAnimations = new Node();
            oAnimations.Name = "Animation";
            oAnimations.Type = (short)NodeType.eNProperty;

            int anCount = 0;
            foreach (XmlNode animation in objNode.FirstChild.ChildNodes)
            {
                Node oAnimation = new Node();
                oAnimation.Name = anCount.ToString();
                oAnimation.Type = (short)NodeType.eNProperty;

                Node oAnimName = new Node();
                oAnimName.Name = "Name";
                oAnimName.Type = (short)NodeType.eNString;
                oAnimName.Type3Data = animation.Attributes["Name"].Value;

                Node oAnimReelHeight = new Node();
                oAnimReelHeight.Name = "ReelHeight";
                oAnimReelHeight.Type = (short)NodeType.eNInt64;
                oAnimReelHeight.Type1Data = int.Parse(animation.Attributes["ReelHeight"].Value);

                Node oAnimReelIndex = new Node();
                oAnimReelIndex.Name = "ReelIndex";
                oAnimReelIndex.Type = (short)NodeType.eNInt64;
                oAnimReelIndex.Type1Data = int.Parse(animation.Attributes["ReelIndex"].Value);

                Node oAnimFrameWidth = new Node();
                oAnimFrameWidth.Name = "FrameWidth";
                oAnimFrameWidth.Type = (short)NodeType.eNInt64;
                oAnimFrameWidth.Type1Data = int.Parse(animation.Attributes["FrameWidth"].Value);

                Node oAnimTotalFrames = new Node();
                oAnimTotalFrames.Name = "TotalFrames";
                oAnimTotalFrames.Type = (short)NodeType.eNInt64;
                oAnimTotalFrames.Type1Data = int.Parse(animation.Attributes["TotalFrames"].Value);

                Node oAnimFrameSpeed = new Node();
                oAnimFrameSpeed.Name = "FrameSpeed";
                oAnimFrameSpeed.Type = (short)NodeType.eNDouble;
                oAnimFrameSpeed.Type2Data = double.Parse(animation.Attributes["FrameSpeed"].Value);

                oAnimation.ChildNodes.Add(oAnimName);
                oAnimation.ChildNodes.Add(oAnimReelHeight);
                oAnimation.ChildNodes.Add(oAnimReelIndex);
                oAnimation.ChildNodes.Add(oAnimFrameWidth);
                oAnimation.ChildNodes.Add(oAnimTotalFrames);
                oAnimation.ChildNodes.Add(oAnimFrameSpeed);

                oAnimations.ChildNodes.Add(oAnimation);
            }

            oAnim.ChildNodes.Add(oAnimations);

            Node oHolds = new Node();
            oHolds.Name = "Holds";
            oHolds.Type = (short)NodeType.eNProperty;

            foreach(XmlNode nHoldGroup in objNode.LastChild.ChildNodes)
            {
                Node oHoldGroup = new Node();
                oHoldGroup.Name = "g" + nHoldGroup.Attributes["id"].Value;
                oHoldGroup.Type = (short)NodeType.eNInt64;
                oHoldGroup.Type1Data = int.Parse(nHoldGroup.Attributes["id"].Value);

                foreach(XmlNode nHold in nHoldGroup.ChildNodes)
                {
                    Node oHold = new Node();
                    oHold.Name = nHold.Attributes["id"].Value;
                    oHold.Type = (short)NodeType.eNProperty;

                    Node oNextId = new Node();
                    oNextId.Name = "nextid";
                    oNextId.Type = (short)NodeType.eNInt64;
                    oNextId.Type1Data = int.Parse(nHold.Attributes["nextid"].Value);

                    Node oPrevId = new Node();
                    oPrevId.Name = "previd";
                    oPrevId.Type = (short)NodeType.eNInt64;
                    oPrevId.Type1Data = int.Parse(nHold.Attributes["previd"].Value);

                    Node oX1 = new Node();
                    oX1.Name = "x1";
                    oX1.Type = (short)NodeType.eNInt64;
                    oX1.Type1Data = int.Parse(nHold.Attributes["x1"].Value);

                    Node oY1 = new Node();
                    oY1.Name = "y1";
                    oY1.Type = (short)NodeType.eNInt64;
                    oY1.Type1Data = int.Parse(nHold.Attributes["y1"].Value);

                    Node oX2 = new Node();
                    oX2.Name = "x2";
                    oX2.Type = (short)NodeType.eNInt64;
                    oX2.Type1Data = int.Parse(nHold.Attributes["x2"].Value);

                    Node oY2 = new Node();
                    oY2.Name = "y2";
                    oY2.Type = (short)NodeType.eNInt64;
                    oY2.Type1Data = int.Parse(nHold.Attributes["y2"].Value);

                    Node oType = new Node();
                    oType.Name = "type";
                    oType.Type = (short)NodeType.eNInt64;
                    oType.Type1Data = nHold.Attributes["type"].Value == "seat" ? 2 : nHold.Attributes["type"].Value == "climb" ? 1 : 0;

                    Node oForce = new Node();
                    oForce.Name = "force";
                    oForce.Type = (short)NodeType.eNInt64;
                    oForce.Type1Data = int.Parse(nHold.Attributes["force"].Value);

                    Node oCantPass = new Node();
                    oCantPass.Name = "cantPass";
                    oCantPass.Type = (short)NodeType.eNInt64;
                    oCantPass.Type1Data = bool.Parse(nHold.Attributes["cantPass"].Value) ? 1 : 0;

                    Node oCantDrop = new Node();
                    oCantDrop.Name = "cantDrop";
                    oCantDrop.Type = (short)NodeType.eNInt64;
                    oCantDrop.Type1Data = bool.Parse(nHold.Attributes["cantDrop"].Value) ? 1 : 0;

                    Node oCantMove = new Node();
                    oCantMove.Name = "cantMove";
                    oCantMove.Type = (short)NodeType.eNInt64;
                    oCantMove.Type1Data = bool.Parse(nHold.Attributes["cantMove"].Value) ? 1 : 0;

                    oHold.ChildNodes.Add(oNextId);
                    oHold.ChildNodes.Add(oPrevId);
                    oHold.ChildNodes.Add(oX1);
                    oHold.ChildNodes.Add(oY1);
                    oHold.ChildNodes.Add(oX2);
                    oHold.ChildNodes.Add(oY2);
                    oHold.ChildNodes.Add(oType);
                    oHold.ChildNodes.Add(oForce);
                    oHold.ChildNodes.Add(oCantPass);
                    oHold.ChildNodes.Add(oCantDrop);
                    oHold.ChildNodes.Add(oCantMove);

                    oHoldGroup.ChildNodes.Add(oHold);
                }

                oHolds.ChildNodes.Add(oHoldGroup);
            }

            //obj.ChildNodes.Add(oName);
            // Add sprite sheet
            obj.ChildNodes.Add(oX);
            obj.ChildNodes.Add(oY);
            obj.ChildNodes.Add(oWidth);
            obj.ChildNodes.Add(oHeight);
            obj.ChildNodes.Add(oAnim);
            obj.ChildNodes.Add(oHolds);

            return obj;
        }

        static private Node ConvertTile(XmlNode tileNode)
        {
            int iRemove = tileNode.Attributes["Name"].Value.LastIndexOf("/");

            string sTileName = tileNode.Attributes["Name"].Value;

            if (iRemove != -1)
            {
                sTileName = sTileName.Remove(iRemove, 1).Insert(iRemove, "-");
            }

            Node obj = new Node();
            obj.Name = sTileName;
            obj.Type = (short)NodeType.eNString;
            obj.Type3Data = sTileName;

            Node oX = new Node();
            oX.Name = "x";
            oX.Type = (short)NodeType.eNInt64;
            oX.Type1Data = int.Parse(tileNode.Attributes["x"].Value);

            Node oY = new Node();
            oY.Name = "y";
            oY.Type = (short)NodeType.eNInt64;
            oY.Type1Data = int.Parse(tileNode.Attributes["y"].Value);

            Node oWidth = new Node();
            oWidth.Name = "width";
            oWidth.Type = (short)NodeType.eNInt64;
            oWidth.Type1Data = int.Parse(tileNode.Attributes["width"].Value);

            Node oHeight = new Node();
            oHeight.Name = "height";
            oHeight.Type = (short)NodeType.eNInt64;
            oHeight.Type1Data = int.Parse(tileNode.Attributes["height"].Value); 

            Node oHolds = new Node();
            oHolds.Name = "Holds";
            oHolds.Type = (short)NodeType.eNProperty;

            foreach (XmlNode nHoldGroup in tileNode.LastChild.ChildNodes)
            {
                Node oHoldGroup = new Node();
                oHoldGroup.Name = "g" + nHoldGroup.Attributes["id"].Value;
                oHoldGroup.Type = (short)NodeType.eNInt64;
                oHoldGroup.Type1Data = int.Parse(nHoldGroup.Attributes["id"].Value);

                foreach (XmlNode nHold in nHoldGroup.ChildNodes)
                {
                    Node oHold = new Node();
                    oHold.Name = nHold.Attributes["id"].Value;
                    oHold.Type = (short)NodeType.eNProperty;

                    Node oNextId = new Node();
                    oNextId.Name = "nextid";
                    oNextId.Type = (short)NodeType.eNInt64;
                    oNextId.Type1Data = int.Parse(nHold.Attributes["nextid"].Value);

                    Node oPrevId = new Node();
                    oPrevId.Name = "previd";
                    oPrevId.Type = (short)NodeType.eNInt64;
                    oPrevId.Type1Data = int.Parse(nHold.Attributes["previd"].Value);

                    Node oX1 = new Node();
                    oX1.Name = "x1";
                    oX1.Type = (short)NodeType.eNInt64;
                    oX1.Type1Data = int.Parse(nHold.Attributes["x1"].Value);

                    Node oY1 = new Node();
                    oY1.Name = "y1";
                    oY1.Type = (short)NodeType.eNInt64;
                    oY1.Type1Data = int.Parse(nHold.Attributes["y1"].Value);

                    Node oX2 = new Node();
                    oX2.Name = "x2";
                    oX2.Type = (short)NodeType.eNInt64;
                    oX2.Type1Data = int.Parse(nHold.Attributes["x2"].Value);

                    Node oY2 = new Node();
                    oY2.Name = "y2";
                    oY2.Type = (short)NodeType.eNInt64;
                    oY2.Type1Data = int.Parse(nHold.Attributes["y2"].Value);

                    Node oType = new Node();
                    oType.Name = "type";
                    oType.Type = (short)NodeType.eNInt64;
                    oType.Type1Data = nHold.Attributes["type"].Value == "seat" ? 2 : nHold.Attributes["type"].Value == "climb" ? 1 : 0;

                    Node oForce = new Node();
                    oForce.Name = "force";
                    oForce.Type = (short)NodeType.eNInt64;
                    oForce.Type1Data = int.Parse(nHold.Attributes["force"].Value);

                    Node oCantPass = new Node();
                    oCantPass.Name = "cantPass";
                    oCantPass.Type = (short)NodeType.eNInt64;
                    oCantPass.Type1Data = bool.Parse(nHold.Attributes["cantPass"].Value) ? 1 : 0;

                    Node oCantDrop = new Node();
                    oCantDrop.Name = "cantDrop";
                    oCantDrop.Type = (short)NodeType.eNInt64;
                    oCantDrop.Type1Data = bool.Parse(nHold.Attributes["cantDrop"].Value) ? 1 : 0;

                    Node oCantMove = new Node();
                    oCantMove.Name = "cantMove";
                    oCantMove.Type = (short)NodeType.eNInt64;
                    oCantMove.Type1Data = bool.Parse(nHold.Attributes["cantMove"].Value) ? 1 : 0;

                    oHold.ChildNodes.Add(oNextId);
                    oHold.ChildNodes.Add(oPrevId);
                    oHold.ChildNodes.Add(oX1);
                    oHold.ChildNodes.Add(oY1);
                    oHold.ChildNodes.Add(oX2);
                    oHold.ChildNodes.Add(oY2);
                    oHold.ChildNodes.Add(oType);
                    oHold.ChildNodes.Add(oForce);
                    oHold.ChildNodes.Add(oCantPass);
                    oHold.ChildNodes.Add(oCantDrop);
                    oHold.ChildNodes.Add(oCantMove);

                    oHoldGroup.ChildNodes.Add(oHold);
                }

                oHolds.ChildNodes.Add(oHoldGroup);
            }

            //obj.ChildNodes.Add(oName);
            // Add sprite sheet
            obj.ChildNodes.Add(oX);
            obj.ChildNodes.Add(oY);
            obj.ChildNodes.Add(oWidth);
            obj.ChildNodes.Add(oHeight);
            obj.ChildNodes.Add(oHolds);

            return obj;
        }

        static private Node ConvertMap(XmlNode mapNode)
        {
            Node oMap = new Node();
            oMap.Name = mapNode.Attributes["ID"].Value;
            oMap.Type = 0;

            Node oMapRegion = new Node();
            oMapRegion.Name = "Region";
            oMapRegion.Type = 3;
            oMapRegion.Type3Data = mapNode.Attributes["RID"].Value;

            Node oMapName = new Node();
            oMapName.Name = "Name";
            oMapName.Type = 3;
            oMapName.Type3Data = mapNode.Attributes["Name"].Value;

            Node oMapWidth = new Node();
            oMapWidth.Name = "width";
            oMapWidth.Type = 1;
            oMapWidth.Type1Data = int.Parse(mapNode.Attributes["width"].Value);

            Node oMapHeight = new Node();
            oMapHeight.Name = "height";
            oMapHeight.Type = 1;
            oMapHeight.Type1Data = int.Parse(mapNode.Attributes["height"].Value);

            Node oMapBoundR = new Node();
            oMapBoundR.Name = "boundR";
            oMapBoundR.Type = 1;
            oMapBoundR.Type1Data = int.Parse(mapNode.Attributes["boundR"].Value);

            Node oMapBoundL = new Node();
            oMapBoundL.Name = "boundL";
            oMapBoundL.Type = 1;
            oMapBoundL.Type1Data = int.Parse(mapNode.Attributes["boundL"].Value);

            Node oMapBoundT = new Node();
            oMapBoundT.Name = "boundT";
            oMapBoundT.Type = 1;
            oMapBoundT.Type1Data = int.Parse(mapNode.Attributes["boundT"].Value);

            Node oMapBoundB = new Node();
            oMapBoundB.Name = "boundB";
            oMapBoundB.Type = 1;
            oMapBoundB.Type1Data = int.Parse(mapNode.Attributes["boundB"].Value);

            Node oMapBGMusic = new Node();
            oMapBGMusic.Name = "bgmusic";
            oMapBGMusic.Type = 3;
            oMapBGMusic.Type3Data = mapNode.Attributes["BackgroundMusic"].Value;

            if (!string.IsNullOrWhiteSpace(mapNode.Attributes["BackgroundMusic"].Value))
            {
                string audio = mapNode.Attributes["BackgroundMusic"].Value;

                if (!oSceneryAudio.ContainsKey(audio))
                    oSceneryAudio.Add(audio, File.ReadAllBytes(Path.Combine(Utility.MUSIC_PATH, audio)));
            }

            Node oLayers = new Node();
            oLayers.Name = "Layers";
            oLayers.Type = 0;

            foreach (XmlNode oxLayer in mapNode.FirstChild.ChildNodes)
            {
                Node oLayer = new Node();
                oLayer.Name = oxLayer.Attributes["ID"].Value;
                oLayer.Type = 0;

                Node oParallaxX = new Node();
                oParallaxX.Name = "ParallaxX";
                oParallaxX.Type = 2;
                oParallaxX.Type2Data = double.Parse(oxLayer.Attributes["ParallaxX"].Value);

                Node oParallaxY = new Node();
                oParallaxY.Name = "ParallaxY";
                oParallaxY.Type = 2;
                oParallaxY.Type2Data = double.Parse(oxLayer.Attributes["ParallaxY"].Value);

                Node oScrollX = new Node();
                oScrollX.Name = "ScrollX";
                oScrollX.Type = 2;
                oScrollX.Type2Data = double.Parse(oxLayer.Attributes["ScrollX"].Value);

                Node oScrollY = new Node();
                oScrollY.Name = "ScrollY";
                oScrollY.Type = 2;
                oScrollY.Type2Data = double.Parse(oxLayer.Attributes["ScrollY"].Value);

                Node oTileX = new Node();
                oTileX.Name = "TileX";
                oTileX.Type = 1;
                oTileX.Type1Data = bool.Parse(oxLayer.Attributes["TileX"].Value) ? 1 : 0;

                Node oTileY = new Node();
                oTileY.Name = "TileY";
                oTileY.Type = 1;
                oTileY.Type1Data = bool.Parse(oxLayer.Attributes["TileY"].Value) ? 1 : 0;

                Node oScenery = new Node();
                oScenery.Name = "Scenery";
                oScenery.Type = 0;

                Node oObjects = new Node();
                oObjects.Name = "Objects";
                oObjects.Type = 0;

                Node oTiles = new Node();
                oTiles.Name = "Tiles";
                oTiles.Type = 0;

                Node oHolds = new Node();
                oHolds.Name = "Holds";
                oHolds.Type = 0;

                foreach (XmlNode oChild in oxLayer.ChildNodes)
                {
                    if (oChild.Name == "Scenery")
                    {
                        Node oSprite = new Node();
                        oSprite.Name = "background";
                        oSprite.Type = 3;
                        oSprite.Type3Data = oChild.Attributes["Sprite"].Value;

                        if (!string.IsNullOrWhiteSpace(oChild.Attributes["Sprite"].Value))
                        {
                            string sprite = oChild.Attributes["Sprite"].Value;

                            if (!oSceneryBitmaps.ContainsKey(sprite))
                                oSceneryBitmaps.Add(sprite, new Bitmap(Path.Combine(Utility.SCENERY_PATH, sprite)));
                        }

                        Node oFlipped = new Node();
                        oFlipped.Name = "flipped";
                        oFlipped.Type = 1;
                        oFlipped.Type1Data = bool.Parse(oChild.Attributes["Flipped"].Value) ? 1 : 0;

                        oScenery.ChildNodes.Add(oSprite);
                        oScenery.ChildNodes.Add(oFlipped);
                        oScenery.ChildNodes.Add(oParallaxX);
                        oScenery.ChildNodes.Add(oParallaxY);
                        oScenery.ChildNodes.Add(oScrollX);
                        oScenery.ChildNodes.Add(oScrollY);
                        oScenery.ChildNodes.Add(oTileX);
                        oScenery.ChildNodes.Add(oTileY);
                    }
                    else if(oChild.Name == "Objects")
                    {
                        foreach (XmlNode objn in oChild.ChildNodes)
                        {
                            Node oObject = new Node();
                            oObject.Name = oObjects.ChildNodes.Count.ToString();
                            oObject.Type = 0;

                            Node oObjName = new Node();
                            oObjName.Name = "Name";
                            oObjName.Type = 3;
                            oObjName.Type3Data = objn.Attributes["Name"].Value;


                            Node oObjSprSheet = new Node();
                            oObjSprSheet.Name = "SpriteSheet";
                            oObjSprSheet.Type = 3;
                            oObjSprSheet.Type3Data = objn.Attributes["SpriteSheet"].Value;

                            Node oMapCoorX = new Node();
                            oMapCoorX.Name = "mapX";
                            oMapCoorX.Type = 1;
                            oMapCoorX.Type1Data = int.Parse(objn.Attributes["MapCoorX"].Value);

                            Node oMapCoorY = new Node();
                            oMapCoorY.Name = "mapY";
                            oMapCoorY.Type = 1;
                            oMapCoorY.Type1Data = int.Parse(objn.Attributes["MapCoorY"].Value);

                            Node oFlipped = new Node();
                            oFlipped.Name = "flipped";
                            oFlipped.Type = 1;
                            oFlipped.Type1Data = bool.Parse(objn.Attributes["Flipped"].Value) ? 1 : 0;

                            Node oCannotDrop = new Node();
                            oCannotDrop.Name = "cannotDrop";
                            oCannotDrop.Type = 1;
                            oCannotDrop.Type1Data = bool.Parse(objn.Attributes["CannotDrop"].Value) ? 1 : 0;

                            oObject.ChildNodes.Add(oObjName);
                            oObject.ChildNodes.Add(oObjSprSheet);                   
                            oObject.ChildNodes.Add(oMapCoorX);
                            oObject.ChildNodes.Add(oMapCoorY);
                            oObject.ChildNodes.Add(oFlipped);
                            oObject.ChildNodes.Add(oCannotDrop);

                            oObjects.ChildNodes.Add(oObject);
                        }    
                    }
                    else if(oChild.Name == "Tiles")
                    {
                        foreach (XmlNode tilen in oChild.ChildNodes)
                        {
                            Node oTile = new Node();
                            oTile.Name = oObjects.ChildNodes.Count.ToString();
                            oTile.Type = 0;

                            int iRemove = tilen.Attributes["Name"].Value.LastIndexOf("/");

                            string sTileName = tilen.Attributes["Name"].Value;

                            if (iRemove != -1)
                            {
                                sTileName = sTileName.Remove(iRemove, 1).Insert(iRemove, "-");
                            }

                            Node oTileName = new Node();
                            oTileName.Name = "Name";
                            oTileName.Type = 3;
                            oTileName.Type3Data = sTileName;

                            Node oTileSprSheet = new Node();
                            oTileSprSheet.Name = "SpriteSheet";
                            oTileSprSheet.Type = 3;
                            oTileSprSheet.Type3Data = tilen.Attributes["SpriteSheet"].Value;

                            Node oMapCoorX = new Node();
                            oMapCoorX.Name = "mapX";
                            oMapCoorX.Type = 1;
                            oMapCoorX.Type1Data = int.Parse(tilen.Attributes["MapCoorX"].Value);

                            Node oMapCoorY = new Node();
                            oMapCoorY.Name = "mapY";
                            oMapCoorY.Type = 1;
                            oMapCoorY.Type1Data = int.Parse(tilen.Attributes["MapCoorY"].Value);

                            Node oFlipped = new Node();
                            oFlipped.Name = "flipped";
                            oFlipped.Type = 1;
                            oFlipped.Type1Data = bool.Parse(tilen.Attributes["Flipped"].Value) ? 1 : 0;

                            Node oCannotDrop = new Node();
                            oCannotDrop.Name = "cannotDrop";
                            oCannotDrop.Type = 1;
                            oCannotDrop.Type1Data = bool.Parse(tilen.Attributes["CannotDrop"].Value) ? 1 : 0;

                            oTile.ChildNodes.Add(oTileName);
                            oTile.ChildNodes.Add(oTileSprSheet);
                            oTile.ChildNodes.Add(oMapCoorX);
                            oTile.ChildNodes.Add(oMapCoorY);
                            oTile.ChildNodes.Add(oFlipped);
                            oTile.ChildNodes.Add(oCannotDrop);

                            oTiles.ChildNodes.Add(oTile);
                        }                     
                    }
                    else if(oChild.Name == "Holds")
                    {
                        foreach (XmlNode nHoldGroup in oChild.ChildNodes)
                        {
                            Node oHoldGroup = new Node();
                            oHoldGroup.Name = "g" + nHoldGroup.Attributes["id"].Value;
                            oHoldGroup.Type = (short)NodeType.eNInt64;
                            oHoldGroup.Type1Data = int.Parse(nHoldGroup.Attributes["id"].Value);

                            foreach (XmlNode nHold in nHoldGroup.ChildNodes)
                            {
                                Node oHold = new Node();
                                oHold.Name = nHold.Attributes["id"].Value;
                                oHold.Type = (short)NodeType.eNProperty;

                                Node oNextId = new Node();
                                oNextId.Name = "nextid";
                                oNextId.Type = (short)NodeType.eNInt64;
                                oNextId.Type1Data = int.Parse(nHold.Attributes["nextid"].Value);

                                Node oPrevId = new Node();
                                oPrevId.Name = "previd";
                                oPrevId.Type = (short)NodeType.eNInt64;
                                oPrevId.Type1Data = int.Parse(nHold.Attributes["previd"].Value);

                                Node oX1 = new Node();
                                oX1.Name = "x1";
                                oX1.Type = (short)NodeType.eNInt64;
                                oX1.Type1Data = int.Parse(nHold.Attributes["x1"].Value);

                                Node oY1 = new Node();
                                oY1.Name = "y1";
                                oY1.Type = (short)NodeType.eNInt64;
                                oY1.Type1Data = int.Parse(nHold.Attributes["y1"].Value);

                                Node oX2 = new Node();
                                oX2.Name = "x2";
                                oX2.Type = (short)NodeType.eNInt64;
                                oX2.Type1Data = int.Parse(nHold.Attributes["x2"].Value);

                                Node oY2 = new Node();
                                oY2.Name = "y2";
                                oY2.Type = (short)NodeType.eNInt64;
                                oY2.Type1Data = int.Parse(nHold.Attributes["y2"].Value);

                                Node oType = new Node();
                                oType.Name = "type";
                                oType.Type = (short)NodeType.eNInt64;
                                oType.Type1Data = nHold.Attributes["type"].Value == "seat" ? 2 : nHold.Attributes["type"].Value == "climb" ? 1 : 0;

                                Node oForce = new Node();
                                oForce.Name = "force";
                                oForce.Type = (short)NodeType.eNInt64;
                                oForce.Type1Data = int.Parse(nHold.Attributes["force"].Value);

                                Node oCantPass = new Node();
                                oCantPass.Name = "cantPass";
                                oCantPass.Type = (short)NodeType.eNInt64;
                                oCantPass.Type1Data = bool.Parse(nHold.Attributes["cantPass"].Value) ? 1 : 0;

                                Node oCantDrop = new Node();
                                oCantDrop.Name = "cantDrop";
                                oCantDrop.Type = (short)NodeType.eNInt64;
                                oCantDrop.Type1Data = bool.Parse(nHold.Attributes["cantDrop"].Value) ? 1 : 0;

                                Node oCantMove = new Node();
                                oCantMove.Name = "cantMove";
                                oCantMove.Type = (short)NodeType.eNInt64;
                                oCantMove.Type1Data = bool.Parse(nHold.Attributes["cantMove"].Value) ? 1 : 0;

                                oHold.ChildNodes.Add(oNextId);
                                oHold.ChildNodes.Add(oPrevId);
                                oHold.ChildNodes.Add(oX1);
                                oHold.ChildNodes.Add(oY1);
                                oHold.ChildNodes.Add(oX2);
                                oHold.ChildNodes.Add(oY2);
                                oHold.ChildNodes.Add(oType);
                                oHold.ChildNodes.Add(oForce);
                                oHold.ChildNodes.Add(oCantPass);
                                oHold.ChildNodes.Add(oCantDrop);
                                oHold.ChildNodes.Add(oCantMove);

                                oHoldGroup.ChildNodes.Add(oHold);
                            }

                            oHolds.ChildNodes.Add(oHoldGroup);
                        }
                    }
                }

                oLayer.ChildNodes.Add(oScenery);
                oLayer.ChildNodes.Add(oObjects);
                oLayer.ChildNodes.Add(oTiles);
                oLayer.ChildNodes.Add(oHolds);

                oLayers.ChildNodes.Add(oLayer);
            }

            Node oPortalsNode = new Node();
            oPortalsNode.Name = "Portals";
            oPortalsNode.Type = 0;

            foreach(XmlNode oxPortal in mapNode.LastChild.ChildNodes)
            {
                Node oPortal = new Node();
                oPortal.Name = oxPortal.Attributes["ID"].Value;
                oPortal.Type = 0;

                Node oPortalType = new Node();
                oPortalType.Name = "type";
                oPortalType.Type = 1;

                switch(oxPortal.Attributes["type"].Value)
                {
                    case "portal":
                        oPortalType.Type1Data = 0;
                        break;
                    case "spawnEnter":
                        oPortalType.Type1Data = 1;
                        break;
                    case "spawnExit":
                        oPortalType.Type1Data = 2;
                        break;
                    case "secret":
                        oPortalType.Type1Data = 3;
                        break;
                    case "hidden":
                        oPortalType.Type1Data = 4;
                        break;
                    default:
                        oPortalType.Type1Data = 0;
                        break;
                }

                Node oPortalRegionID = new Node();
                oPortalRegionID.Name = "regionID";
                oPortalRegionID.Type = 1;
                oPortalRegionID.Type1Data = int.Parse(oxPortal.Attributes["regionID"].Value);

                Node oPortalMapID = new Node();
                oPortalMapID.Name = "mapID";
                oPortalMapID.Type = 1;
                oPortalMapID.Type1Data = int.Parse(oxPortal.Attributes["mapID"].Value);

                Node oPortalTargetID = new Node();
                oPortalTargetID.Name = "targetID";
                oPortalTargetID.Type = 1;
                oPortalTargetID.Type1Data = int.Parse(oxPortal.Attributes["targetID"].Value);

                Node oPortalMapX = new Node();
                oPortalMapX.Name = "mapX";
                oPortalMapX.Type = 1;
                oPortalMapX.Type1Data = int.Parse(oxPortal.Attributes["mapCoorX"].Value);

                Node oPortalMapY = new Node();
                oPortalMapY.Name = "mapY";
                oPortalMapY.Type = 1;
                oPortalMapY.Type1Data = int.Parse(oxPortal.Attributes["mapCoorY"].Value);

                oPortal.ChildNodes.Add(oPortalType);
                oPortal.ChildNodes.Add(oPortalRegionID);
                oPortal.ChildNodes.Add(oPortalMapID);
                oPortal.ChildNodes.Add(oPortalTargetID);
                oPortal.ChildNodes.Add(oPortalMapX);
                oPortal.ChildNodes.Add(oPortalMapY);

                oPortalsNode.ChildNodes.Add(oPortal);
            }

            oMap.ChildNodes.Add(oMapName);
            oMap.ChildNodes.Add(oMapRegion);
            oMap.ChildNodes.Add(oMapWidth);
            oMap.ChildNodes.Add(oMapHeight);
            oMap.ChildNodes.Add(oMapBoundR);
            oMap.ChildNodes.Add(oMapBoundL);
            oMap.ChildNodes.Add(oMapBoundT);
            oMap.ChildNodes.Add(oMapBoundB);
            oMap.ChildNodes.Add(oMapBGMusic);
            oMap.ChildNodes.Add(oLayers);
            oMap.ChildNodes.Add(oPortalsNode);

            return oMap;
        }

        static private Node CreateGlobals()
        {
            Node oGlobals = new Node();
            oGlobals.Name = "Global";
            oGlobals.Type = 0;

            Node oPortals = new Node();
            oPortals.Name = "Portal";
            oPortals.Type = 0;

            Node oPortSprites = new Node();
            oPortSprites.Name = "Sprites";
            oPortSprites.Type = 0;

            Node oPort1 = new Node();
            oPort1.Name = "portal.png";
            oPort1.Type = 5;
            oPort1.Type5Data = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "portal.png"));

            Node oPort2 = new Node();
            oPort2.Name = "spawnEnter.png";
            oPort2.Type = 5;
            oPort2.Type5Data = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "spawnEnter.png"));

            Node oPort3 = new Node();
            oPort3.Name = "spawnExit.png";
            oPort3.Type = 5;
            oPort3.Type5Data = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "spawnExit.png"));

            Node oPort4 = new Node();
            oPort4.Name = "secret.png";
            oPort4.Type = 5;
            oPort4.Type5Data = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "secret.png"));

            Node oPort5 = new Node();
            oPort5.Name = "hidden.png";
            oPort5.Type = 5;
            oPort5.Type5Data = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "hidden.png"));

            oPortSprites.ChildNodes.Add(oPort1);
            oPortSprites.ChildNodes.Add(oPort2);
            oPortSprites.ChildNodes.Add(oPort3);
            oPortSprites.ChildNodes.Add(oPort4);
            oPortSprites.ChildNodes.Add(oPort5);

            oPortals.ChildNodes.Add(oPortSprites);

            oGlobals.ChildNodes.Add(oPortals);

            return oGlobals;
        }

        #region Node Class and Methods
        enum NodeType
        {
            eNProperty = 0,
            eNInt64 = 1,
            eNDouble = 2,
            eNString = 3,
            eNPoint = 4,
            eNBitmap = 5,
            eNAudio = 6
        }
        public class Node
        {
            public string Name { get; set; } = "";
            public int FirstChildID { get; set; } = 0;
            public List<Node> ChildNodes { get; } = new List<Node>();
            public short ChildCount
            {
                get { return (short)ChildNodes.Count; }
            }
            public short Type { get; set; } = 0;
            public long Type1Data { get; set; } = 0;
            public double Type2Data { get; set; } = 0;
            public string Type3Data { get; set; } = "";
            public int Type4DataX { get; set; } = 0;
            public int Type4DataY { get; set; } = 0;
            public Bitmap Type5Data { get; set; } = null;
            public byte[] Type6Data { get; set; } = null;
        }
        public static void PopulateNodeRow(Node oNode)
        {
            nodes = new List<Node>();
            oNode.FirstChildID = nodeBatch.Count + 1;
            noffset = oNode.ChildCount;
            nodeBatch.Add(oNode);
            IterateNodeRow(oNode.ChildNodes);
            nodes = nodeBatch;
        }

        static int noffset = 0;
        static int nchildoffset = 0;
        static List<Node> nodeBatch = new List<Node>();
        public static void IterateNodeRow(List<Node> currRow)
        {
            List<Node> children = new List<Node>();

            noffset = nodeBatch.Count + currRow.Count;

            for (int i = 0; i < currRow.Count; i++)
            {
                if (currRow[i].ChildCount > 0)
                {
                    if (nchildoffset != 0)
                        noffset = noffset + nchildoffset;
                    // noffset = noffset;
                   // else
                     

                    nchildoffset = currRow[i].ChildCount;
                    currRow[i].FirstChildID = noffset;
                }

                foreach (Node child in currRow[i].ChildNodes)
                {
                    children.Add(child);
                }

                nodeBatch.Add(currRow[i]);
            }
            nchildoffset = 0;

            if (children.Count > 0)
                IterateNodeRow(children);
        }
        public static void WriteNodeData(Node oNode, BinaryWriter bw)
        {
            bw.Write(CheckString(oNode.Name));  // 4
            bw.Write(oNode.FirstChildID);   // 8
            bw.Write(oNode.ChildCount);  // 10
            bw.Write(oNode.Type);  // 12

            if (oNode.Type == 0)
                bw.Write(0L); // 20
            else if (oNode.Type == 1)
                bw.Write(oNode.Type1Data); // 20
            else if (oNode.Type == 2)
                bw.Write(oNode.Type2Data); // 20
            else if (oNode.Type == 3)
            {
                bw.Write(CheckString(oNode.Type3Data)); // 16
                bw.Write(0U); // 20
            }
            else if (oNode.Type == 4)
            {
                bw.Write(oNode.Type4DataX); // 16
                bw.Write(oNode.Type4DataY); // 20
            }
            else if (oNode.Type == 5)
            {
                bw.Write(bitmaps.Count); // 16
                oNode.Type5Data.Tag = oNode.Name;
                bitmaps.Add(oNode.Type5Data);
                bw.Write((ushort)oNode.Type5Data.Width); // 18
                bw.Write((ushort)oNode.Type5Data.Height); // 20
            }
            else if (oNode.Type == 6)
            {
                bw.Write(mp3s.Count); // 16
                mp3s.Add(oNode.Type6Data);
                bw.Write((uint)oNode.Type6Data.Length); // 20
            }
        }
        #endregion

        #region Utility Methods
        private static void EnsureMultiple(this Stream s, int multiple)
        {
            int skip = (int)(multiple - (s.Position % multiple));
            if (skip == multiple)
                return;
            s.Write(new byte[skip], 0, skip);
        }
        private static byte[] GetCompressedBitmap(Bitmap b)
        {
            BitmapData bd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            int inLen = bd.Stride * bd.Height;
            int outLen = _is64bit ? EMaxOutputLen64(inLen) : EMaxOutputLen32(inLen);
            byte[] outBuf = new byte[outLen];
            outLen = _is64bit ? ECompressLZ464(bd.Scan0, outBuf, inLen, outLen, 0) : ECompressLZ432(bd.Scan0, outBuf, inLen, outLen, 0);
            b.UnlockBits(bd);
            Array.Resize(ref outBuf, outLen);
            return outBuf;
        }
        static public int CheckString(string value)
        {
            int ret = strings.Count;

            if (strings.ContainsKey(value))
                return strings[value];

            strings.Add(value, strings.Count);

            return ret;
        }
        #endregion

        #region DLL Calls
        [DllImport("lz4hc_32.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LZ4_compress_HC")]
        private static extern int ECompressLZ432(IntPtr source, byte[] dest, int inputLen, int maxSize, int level);

        [DllImport("lz4hc_64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LZ4_compress_HC")]
        private static extern int ECompressLZ464(IntPtr source, byte[] dest, int inputLen, int maxSize, int level);

        [DllImport("lz4hc_32.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LZ4_compressBound")]
        private static extern int EMaxOutputLen32(int inputLen);

        [DllImport("lz4hc_64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LZ4_compressBound")]
        private static extern int EMaxOutputLen64(int inputLen);
        #endregion
    }
}