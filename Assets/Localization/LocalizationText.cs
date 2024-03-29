using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class LocalizationText
{
    private static IDictionary<string, string> _content = new Dictionary<string, string>();
    private static string _language = "EN";
    private static string Language
    {
        get
        {
            return LocalizationText._language;
        }
        set
        {
            if (LocalizationText._language != value)
            {
                LocalizationText._language = value;
                CreateContent();
            }
        }
    }
	public static string GetText(string key)
	{       
		string result="";		
		LocalizationText.Content.TryGetValue(key,out result);
		
		if(string.IsNullOrEmpty(result))
			return key+"["+LocalizationText.Language+"]"+" NO DEFINIDO";
		
		
		return result;
	}
	public static string GetLanguage()
	{
		return LocalizationText.Language;
	}
	
	public static void SetLanguage(string language)
	{
		LocalizationText.Language=language;
		updateAllButtonReferences ();
	}
	private static IDictionary<string, string> Content
	{
		get
		{
			if(_content==null || _content.Count == 0)
				CreateContent();
			return _content;			
		}
	}
    private static IDictionary<string, string> GetContent()
    {
        if (LocalizationText._content == null || LocalizationText._content.Count == 0)
        {
            LocalizationText.CreateContent();
        }
        return LocalizationText._content;
    }

    private static void AddContent(XmlNode xNode)
    {
        foreach (XmlNode node in xNode.ChildNodes)
        {
            if (node.LocalName == "TextKey")
            {
                string value = node.Attributes.GetNamedItem("name").Value;
                string text = string.Empty;
                foreach (XmlNode langNode in node)
                {
                    if (langNode.LocalName == LocalizationText._language)
                    {
                        text = langNode.InnerText;
                        if (LocalizationText._content.ContainsKey(value))
                        {
                            LocalizationText._content.Remove(value);
                            LocalizationText._content.Add(value, value + " has been found multiple times in the XML allowed only once!");
                        }
                        else
                        {
                            LocalizationText._content.Add(value, (!string.IsNullOrEmpty(text)) ? text : ("No Text for " + value + " found"));
                        }
                        break;
                    }
                }
            }
        }
    }
    private static void CreateContent()
    {
		XmlDocument xmlDocument = new XmlDocument ();
		xmlDocument.LoadXml (Resources.Load ("LocalizationText").ToString ());
        if (xmlDocument == null)
        {
            System.Console.WriteLine("Couldnt Load Xml");
            return;
        }
        if (LocalizationText._content != null)
        {
            LocalizationText._content.Clear();
        }
        XmlNode xNode = xmlDocument.ChildNodes.Item(1).ChildNodes.Item(0);
        LocalizationText.AddContent(xNode);
    }

	private static void updateAllButtonReferences() {
		LocalizeButton[] buttons = GameObject.FindObjectsOfType<LocalizeButton> ();
		foreach (LocalizeButton button in buttons) {
			button.updateText ();
		}
	}
}
