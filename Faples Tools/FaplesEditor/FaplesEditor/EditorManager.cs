using System.Xml;

namespace FaplesEditor
{

    class EditorManager
    {
        static private XmlDocument gObjectCollection = new XmlDocument();
        static private XmlDocument gTileCollection = new XmlDocument();
        static private XmlDocument gMapCollection = new XmlDocument();
        static private XmlDocument gUICollection = new XmlDocument();
        static private XmlDocument gCharacterCollection = new XmlDocument();

        static public void LoadObjects(XmlDocument xDoc)
        {
            gObjectCollection = xDoc;
        }
        static public void LoadTiles(XmlDocument xDoc)
        {
            gTileCollection = xDoc;
        }
        static public void LoadMaps(XmlDocument xDoc)
        {
            gMapCollection = xDoc;
        }

        static public void LoadUI(XmlDocument xDoc)
        {
            gUICollection = xDoc;
        }
        static public void LoadCharacters(XmlDocument xDoc)
        {
            gCharacterCollection = xDoc;
        }

        static public XmlDocument GetObjectCollection()
        {
            if (string.IsNullOrWhiteSpace(gObjectCollection.OuterXml))
            {
                XmlNode nCollections = gObjectCollection.CreateElement("fpxObjects");
                gObjectCollection.AppendChild(nCollections);
            }

            return gObjectCollection;
        }

        static public void SetObjectCollection(XmlDocument xDoc)
        {
            XmlDocument xdDoc = xDoc;
            gObjectCollection = xdDoc;
        }

        static public XmlDocument GetTileCollection()
        {
            if (string.IsNullOrWhiteSpace(gTileCollection.OuterXml))
            {
                XmlNode nTiles = gTileCollection.CreateElement("fpxTiles");
                gTileCollection.AppendChild(nTiles);
            }

            return gTileCollection;
        }

        static public void SetTileCollection(XmlDocument xDoc)
        {
            XmlDocument xdDoc = xDoc;
            gTileCollection = xdDoc;
        }
        static public XmlDocument GetMapCollection()
        {
            if (string.IsNullOrWhiteSpace(gMapCollection.OuterXml))
            {
                XmlNode nMaps = gMapCollection.CreateElement("fpxMaps");
                gMapCollection.AppendChild(nMaps);
            }

            return gMapCollection;
        }

        static public void SetMapCollection(XmlDocument xDoc)
        {
            XmlDocument xdDoc = xDoc;
            gMapCollection = xdDoc;
        }

        static public XmlDocument GetUICollection()
        {
            if (string.IsNullOrWhiteSpace(gUICollection.OuterXml))
            {
                XmlNode nUI = gUICollection.CreateElement("fpxUI");
                gUICollection.AppendChild(nUI);
            }

            return gUICollection;
        }

        static public void SetUICollection(XmlDocument xDoc)
        {
            XmlDocument xdDoc = xDoc;
            gUICollection = xdDoc;
        }
        static public XmlDocument GetCharacterCollection()
        {
            if (string.IsNullOrWhiteSpace(gCharacterCollection.OuterXml))
            {
                XmlNode nPlayer = gCharacterCollection.CreateElement("fpxCharacters");
                gCharacterCollection.AppendChild(nPlayer);
            }

            return gCharacterCollection;
        }

        static public void SetCharacterCollection(XmlDocument xDoc)
        {
            XmlDocument xdDoc = xDoc;
            gCharacterCollection = xdDoc;
        }
    }
}
