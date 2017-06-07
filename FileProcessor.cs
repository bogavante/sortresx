using System;
using System.Resources;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace ResourceFileSorterTrigger
{
    
    public class FileProcessor
    {
        private ArrayList mResourceNameList = null;
	private Hashtable mResourceNodes = null;
	private XmlDocument mDoc = null;
        
	string mPath = null;

        public FileProcessor(string path)
        {               
            mPath = path;
            mResourceNameList = new ArrayList();
            mResourceNodes = new Hashtable();
            mDoc = new XmlDocument();
            mDoc.Load(mPath);
        }

        public void Process()
        {
            string[] sortedNames;

            ExtractResources();
            sortedNames = SortResourceList();
            WriteOrderedResources(sortedNames);
        }


        public void TearDown()
        {
            List<string> things = new List<string>();

        }



	private void ExtractResources()
	{            
	    XmlNodeList rootList = mDoc.GetElementsByTagName("root");
    	    foreach (XmlNode rootNode in rootList)
	    {                        
	        foreach (XmlNode node in rootNode.ChildNodes)
		{
		    if (node.Name.EndsWith("data"))
		    {                        
		        foreach (XmlAttribute attr in node.Attributes)
			{
			    if (attr.Name.EndsWith("name"))
			    {
                                AddXmlNode(node, attr);			        
			    }
			}
		    }
	        }

		XmlNodeList deleteList = rootNode.SelectNodes("/root/data");
		foreach(XmlNode delNode in deleteList)
		{
		    rootNode.RemoveChild(delNode);
		}
                
	    }          
        }

        private void AddXmlNode(XmlNode node, XmlAttribute attribute)
        {
            //If node doesn't exist, add to the final document.
            //To identify the data node, it is neccesary its "name" attribute.
            if ( !mResourceNodes.ContainsKey(attribute.Value.ToString()))
            {            
                mResourceNodes.Add(attribute.Value.ToString(), node);
                mResourceNameList.Add(attribute.Value.ToString());
            }
        }

	private string[] SortResourceList()
	{
	    string[] names = new string[mResourceNameList.Count];
            for (int i = 0; i < mResourceNameList.Count; i++)
	        names[i] = (string)mResourceNameList[i];

	    Array.Sort(names);           
	    return names;
	}

	private void WriteOrderedResources(string[] names)
	{            
	    XmlNodeList rootList = mDoc.GetElementsByTagName("root");
	    foreach (XmlNode rootNode in rootList)
	    {   
	        foreach (string key in names)
		{
		    rootNode.AppendChild((XmlNode)mResourceNodes[key]);
		}                                
	    }
            mDoc.Save(mPath);         
	}
    }
}