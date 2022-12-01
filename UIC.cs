using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/* 
UIC.cs © 2020 Peter Grimshaw, BTM Innovation Pty Ltd
Licensed under a Creative Commons Attribution 3.0 Unported License.
*/  

public class UICItem
{
    public ulong UICVal0 { get; set; } = 0;
    public ulong UICVal1 { get; set; } = 0;
    public ulong UICVal2 { get; set; } = 0;
    public ulong UICVal3 { get; set; } = 0;
    public ulong UICVal4 { get; set; } = 0;
    public ulong UICVal5 { get; set; } = 0;
    public ulong UICVal6 { get; set; } = 0;
    public ulong UICVal7 { get; set; } = 0;
    public ulong UICVal8 { get; set; } = 0;
    public ulong UICVal9 { get; set; } = 0;
    public string UICName { get; set; } = "";
    public int UICQty { get; set; } = 0;
}
public class UIC
{
    const ulong MASK_NOPART = 0x8000000000000000;
    const ulong MASK_LEFT = 0x4000000000000;
    const ulong MASK_DOUBLE = 0x80000000;
    const int MASK_INST = 0x7FFFFFFF;
    const int MASK_FAMILY = 0x7;
    const int MASK_DOMAIN = 0xFFF;
    const int MASK_GENUS = 0x3FFF;
    const int MASK_DESCRIPT = 0x1FFF;
    const int MASK_NUMBER = 0x1F;

    public enum UICTypes { utDomain, utGenre, utInstrument, utDescription, utNamed };

    public ulong[] UICVal = new ulong[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public string sLastError = "";

    private XDocument UICGenre;
    private XDocument UICInst;
    private XDocument UICDomain;
    private XDocument UICDescript;
    private XDocument UICNamed;
    
    private string sCurrentLang = "";

    public UIC()
    {
        LoadUICfromZinfonia();
    }

    private string ElementOrEmpty(XElement ThisElement)
    {
        if (ThisElement == null) return "";
        else return ThisElement.Value;
    }
    public void AddUICItemsToDictionary(UICTypes ut, Dictionary<ulong, string> dItems)
    {
        dItems.Add(0, "");
        switch (ut)
        {
            case UICTypes.utDomain:
                if (UICDomain != null)
                {
                    var results = UICDomain.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("DomainName"),
                        id = (ulong)x.Element("DomainID")
                    }).ToList();
                    foreach(var sitem in results)
                    {
                        dItems.Add(sitem.id,sitem.name);
                    }
                }
                break;
            case UICTypes.utGenre:
                if (UICGenre != null)
                {
                    var results = UICGenre.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("GenusName"),
                        id = (ulong)x.Element("GenusID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        dItems.Add(sitem.id, sitem.name);
                    }
                }
                break;
            case UICTypes.utDescription:
                if (UICDescript != null)
                {
                    var results = UICDescript.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("DescriptorName"),
                        id = (ulong)x.Element("DescriptorID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        dItems.Add(sitem.id, sitem.name);
                    }
                }
                break;
            case UICTypes.utInstrument:
                if (UICInst != null)
                {
                    var results = UICInst.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("InstName"),
                        id = (ulong)x.Element("InstUIC")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        dItems.Add(sitem.id, sitem.name);
                    }
                }
                break;
            case UICTypes.utNamed:
                if (UICNamed != null)
                {
                    var results = UICNamed.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("NamedGroup"),
                        id = (ulong)x.Element("NamedGroupID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        dItems.Add(sitem.id, sitem.name);
                    }
                }
                break;
        }
    }

    public void AddUICItemsToDictionary(UICTypes ut, List<UICItem> dItems)
    {
        dItems.Add(new UICItem());
        switch (ut)
        {
            case UICTypes.utDomain:
                if (UICDomain != null)
                {
                    var results = UICDomain.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("DomainName"),
                        id = (ulong)x.Element("DomainID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        UICItem nc = new UICItem();
                        nc.UICName = sitem.name;
                        nc.UICVal0 = sitem.id;
                        dItems.Add(nc);
                    }
                }
                break;
            case UICTypes.utGenre:
                if (UICGenre != null)
                {
                    var results = UICGenre.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("GenusName"),
                        id = (ulong)x.Element("GenusID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        UICItem nc = new UICItem();
                        nc.UICName = sitem.name;
                        nc.UICVal0 = sitem.id;
                        dItems.Add(nc);
                    }
                }
                break;
            case UICTypes.utDescription:
                if (UICDescript != null)
                {
                    var results = UICDescript.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("DescriptorName"),
                        id = (ulong)x.Element("DescriptorID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        UICItem nc = new UICItem();
                        nc.UICName = sitem.name;
                        nc.UICVal0 = sitem.id;
                        dItems.Add(nc);
                    }
                }
                break;
            case UICTypes.utInstrument:
                if (UICInst != null)
                {
                    var results = UICInst.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("InstName"),
                        id = (ulong)x.Element("InstUIC")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        UICItem nc = new UICItem();
                        nc.UICName = sitem.name;
                        nc.UICVal0 = sitem.id;
                        dItems.Add(nc);
                    }
                }
                break;
            case UICTypes.utNamed:
                if (UICNamed != null)
                {
                    var results = UICNamed.Descendants("UIC").Select(x => new {
                        name = (string)x.Element("NamedGroup"),
                        id = (ulong)x.Element("NamedGroupID")
                    }).ToList();
                    foreach (var sitem in results)
                    {
                        UICItem nc = new UICItem();
                        nc.UICName = sitem.name;
                        nc.UICVal0 = sitem.id;
                        dItems.Add(nc);
                    }
                }
                break;
        }
    }

    public ulong GetUICSortValue(ulong UIC)
    {
        ulong result = 0;
        UICToValues(UIC, out int InstrumentID, out int DomainID, out int DescripionID, out int Number, out bool lIsDouble, out bool lIsLeft, out bool lNoPart);
        var inst = UICInst.Descendants("UIC").FirstOrDefault(x => (int)x.Element("InstUIC") == InstrumentID);
        if (inst != null)
        {
            result = ((ulong)inst.Element("InstOrder") << 18);
            int nDomain = 4096;
            if (DomainID != 0)
            {
                var dom = UICDomain.Descendants("UIC").FirstOrDefault(x => (int)x.Element("DomainID") == DomainID);
                if (dom != null)
                {
                    nDomain = ((int)dom.Element("DomainOrder")) & 4095;
                }
            }
            result += ((ulong)nDomain << 49);

            if (DescripionID != 0)
            {
                var desc = UICDescript.Descendants("UIC").FirstOrDefault(x => (int)x.Element("DescriptorID") == DescripionID);
                if (desc != null)
                {
                    int nTemp = ((int)desc.Element("DescriptorOrder")) & 8191;
                    result += (ulong)nTemp;
                }
            }
            if (Number != 0)
            {
                result += ((ulong)Number << 13);
            }
        }
        return result;
    }
    public ulong GetUICSortValue(int nInstUIC)
    {
        var inst = UICInst.Descendants("UIC").FirstOrDefault(x => (int)x.Element("InstUIC") == nInstUIC);
        if (inst != null) return (ulong)inst.Element("InstOrder");
        else return 0;
    }


    public string GetUICAsIcon(ulong nUIC)
    {
        if (nUIC == 0) return "";
        int nInstUIC = (int)(nUIC & MASK_INST);
        int nFamily = (int)(nUIC & MASK_FAMILY);
        int nGenre = (int)((nUIC >> 3) & MASK_GENUS);

        string sInstName = "";
        //First check for specific Instrument
        var inst = UICInst.Descendants("UIC").FirstOrDefault(x => (int)x.Element("InstUIC") == nInstUIC);
        if (inst != null) sInstName = ElementOrEmpty(inst.Element("InstIcon"));
        if (sInstName == "")
        {
            string sGenusFamily;
            if (nFamily == 1) sGenusFamily = "1.General";
            else if (nFamily == 2) sGenusFamily = "2.Vocal";
            else if (nFamily == 3) sGenusFamily = "3.Wind";
            else if (nFamily == 4) sGenusFamily = "4.Brass";
            else if (nFamily == 5) sGenusFamily = "5.Other";
            else sGenusFamily = "6.Strings";
            var genre = UICGenre.Descendants("UIC").FirstOrDefault(x => (int)x.Element("GenusID") == nGenre && (string)x.Element("GenusType") == sGenusFamily);
            if (genre != null) sInstName = ElementOrEmpty(genre.Element("GenusIcon"));
        }
        if (sInstName == "")
        {
            if (nFamily == 1) sInstName = "FullScore";
            else if (nFamily == 2) sInstName = "Voices";
            else if (nFamily == 3) sInstName = "Wind";
            else if (nFamily == 4) sInstName = "Brass";
            else if (nFamily == 6) sInstName = "Strings";
            else sInstName = "Generic";
        }
        return sInstName;
    }

    private string GetDictionaryValue(int nPos, Dictionary<int, int> dVals, string format, out string sIcon)
    {
        int nKey = dVals.ElementAt(nPos).Key;
        int nValue = dVals.ElementAt(nPos).Value;
        sIcon = GetUICAsIcon((ulong)nKey);
        return GetUICCntAsText(nKey, nValue, format);
    }

    private void AddUICToList(List<ulong> lUIC, string sUIC)
    {
        if (sUIC.Trim() != "")
        {
            ulong nUIC;
            if (ulong.TryParse(sUIC, out nUIC))
            {
                if (nUIC != 0) lUIC.Add(nUIC);
            }
        }
    }

    private string GetAmpersand(string format)
    {
        if ((format == "zh") || (format == "ko") || (format == "ja")) return ",";
        else if (format == "ru") return "и";
        return "&";
    }

    public ulong ValueToUIC(int InstrumentID = 0, int DomainID = 0, int DescripionID = 0, int Number = 0, bool lIsDouble = false, bool lIsLeft = false, bool lNoPart = false)
    {
        ulong uRes = (ulong)(InstrumentID & MASK_INST);
        if ((DomainID & MASK_DOMAIN) > 0) uRes += (ulong)(DomainID & MASK_DOMAIN) << 51;
        if ((DescripionID & MASK_DESCRIPT) > 0) uRes += (ulong)(DescripionID & MASK_DESCRIPT) << 37;
        if ((Number & MASK_NUMBER) > 0) uRes += (ulong)(Number & MASK_NUMBER) << 32;
        if (lIsDouble) uRes += MASK_DOUBLE;
        if (lIsLeft) uRes += MASK_LEFT;
        if (lNoPart) uRes += MASK_NOPART;
        return uRes;
    }

    public void UICToValues(ulong nUIC, out int InstrumentID, out int DomainID, out int DescripionID, out int Number, out bool lIsDouble, out bool lIsLeft, out bool lNoPart)
    {
        InstrumentID = (int)(nUIC & MASK_INST);
        DomainID = (int)((nUIC >> 51) & 4095);
        DescripionID = (int)((nUIC >> 37) & 8191);
        Number = (int)((nUIC >> 32) & 31);
        lIsDouble = (nUIC & MASK_DOUBLE) > 0;
        lIsLeft = (nUIC & MASK_LEFT) > 0;
        lNoPart = (nUIC & MASK_NOPART) > 0;
    }


    public void ClearValue(sbyte nPos)
    {
        if ((nPos < 0)  || (nPos > 9))
        {
            for (sbyte i = 0; i < 10; i++) UICVal[i] = 0;
        }
        else UICVal[nPos] = 0;
    }

    
    public void SetValue (UICItem ThisUIC)
    {
        UICVal[0] = ThisUIC.UICVal0;
        UICVal[1] = ThisUIC.UICVal1;
        UICVal[2] = ThisUIC.UICVal2;
        UICVal[3] = ThisUIC.UICVal3;
        UICVal[4] = ThisUIC.UICVal4;
        UICVal[5] = ThisUIC.UICVal5;
        UICVal[6] = ThisUIC.UICVal6;
        UICVal[7] = ThisUIC.UICVal7;
        UICVal[8] = ThisUIC.UICVal8;
        UICVal[9] = ThisUIC.UICVal9;
    }

    public void SetValue(ulong nUIC, sbyte nPos = -1)
    {
        if (nPos < 10)
        {
            if (nPos < 0) //Find First Free Slot
            {
                for (sbyte i = 0; i < 10; i++)
                {
                    if (UICVal[i] == 0)
                    {
                        nPos = i;
                        break;
                    }
                }
            }
            UICVal[nPos] = nUIC;
        }
    }

    public void SetValue(int InstrumentID, int DomainID = 0, int DescripionID = 0, int Number = 0, bool lIsDouble = false, bool lIsLeft = false, bool lNoPart = false, sbyte nPos = -1)
    {
        if (nPos < 10)
        {
            if (nPos < 0) //Find First Free Slot
            {
                for (sbyte i = 0; i < 10; i++)
                {
                    if (UICVal[i] == 0)
                    {
                        nPos = i;
                        break;
                    }
                }
            }
            UICVal[nPos] = ValueToUIC(InstrumentID, DomainID, DescripionID, Number, lIsDouble, lIsLeft, lNoPart);
        }
    }

    public void SetUICLanguage(string sLang)
    {
        if (sLang.Equals("de", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "de";
        else if (sLang.Equals("fr", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "fr";
        else if (sLang.Equals("it", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "it";
        else if (sLang.Equals("pl", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "pl";
        else if (sLang.Equals("es", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "es";
        else if (sLang.Equals("pt", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "pt";
        else if (sLang.Equals("ko", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "ko";
        else if (sLang.Equals("ja", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "ja";
        else if (sLang.Equals("ru", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "ru";
        else if (sLang.Equals("zh", StringComparison.CurrentCultureIgnoreCase)) sCurrentLang = "zh";
        else sCurrentLang = "en";

    }

    public void LoadUICfromZinfonia(string sLang = "en")
    {
        try
        {
            if (sCurrentLang == "")
            {
                if (UICGenre != null) UICGenre.RemoveNodes();
                UICGenre = XDocument.Load("https://uic.btmi.au/genus.xml");
                if (UICInst != null) UICInst.RemoveNodes();
                UICInst = XDocument.Load("https://uic.btmi.au/instrument.xml");
                if (UICDomain != null) UICDomain.RemoveNodes();
                UICDomain = XDocument.Load("https://uic.btmi.au/domain.xml");
                if (UICDescript != null) UICDescript.RemoveNodes();
                UICDescript = XDocument.Load("https://uic.btmi.au/descriptor.xml");
                if (UICNamed != null) UICNamed.RemoveNodes();
                UICNamed = XDocument.Load("https://uic.btmi.au/named.xml");
            }
            SetUICLanguage(sLang);
        }
        catch (Exception ex)
        {
            sLastError = ex.Message;
            sCurrentLang = "";
        }
    }
    private string GetInstNameFromUICValues(int nDomain, int nInst, int nNumber, int nDescriptor, bool lLeft, bool lPlural = false)
    {
        StringBuilder sbInst = new StringBuilder();
        if (nInst > 0)
        {
            string sFieldExt = "";
            string sDomainName = "";
            string sInstName = "";
            string sDescName = "";
            if (sCurrentLang != "en") sFieldExt = sCurrentLang.ToUpper();

            var inst = UICInst.Descendants("UIC").FirstOrDefault(x => (int)x.Element("InstUIC") == nInst);
            if (inst != null)
            {
                if (inst.Element("InstName" + ((lPlural) ? "Plural" : "") + sFieldExt) != null)
                    sInstName = inst.Element("InstName" + ((lPlural) ? "Plural" : "") + sFieldExt).Value;
                else sInstName = inst.Element("InstName" + ((lPlural) ? "Plural" : "")).Value;
            }
            if ((sInstName != null) && (sInstName != ""))
            {
                //Locate Domain ID
                if (nDomain > 0)
                {
                    var domain = UICDomain.Descendants("UIC").FirstOrDefault(x => (int)x.Element("DomainID") == nDomain);
                    if (domain != null)
                    {
                        if (domain.Element("DomainName" + sFieldExt) != null)
                            sDomainName = domain.Element("DomainName" + sFieldExt).Value;
                        else sDomainName = domain.Element("DomainName").Value;
                    }
                }
                //Locate Desc
                if (nDescriptor > 0)
                {
                    var descript = UICDescript.Descendants("UIC").FirstOrDefault(x => (int)x.Element("DescriptorID") == nDescriptor);
                    if (descript != null)
                    {
                        if (descript.Element("DescriptorName" + sFieldExt) != null)
                          sDescName = descript.Element("DescriptorName" + sFieldExt).Value;
                        else sDescName = descript.Element("DescriptorName").Value;
                    }
                }
                if ((sDomainName != null) && (sDomainName != "")) sbInst.Append("[" + sDomainName + "] ");
                if ((lLeft) && (sDescName != null) && (sDescName != "")) sbInst.Append(sDescName + " ");
                sbInst.Append(sInstName);
                if (nNumber > 0) sbInst.Append(" " + nNumber.ToString());
                if ((!lLeft) && (sDescName != null) && (sDescName != "")) sbInst.Append(" "+sDescName);
            }
        }
        return sbInst.ToString();
    }

    private void GetUICAsText(ulong nUIC, string sLang, StringBuilder sbRes)
    {
        ClearValue(-1);
        SetValue(nUIC);
        GetUICAsText(sLang, sbRes);
    }

    private string GetUICCntAsText(int nInst, int nCnt, string sLang)
    {
        if (nInst == 0) return "";
        LoadUICfromZinfonia(sLang);
        if (!sCurrentLang.Equals(sLang, StringComparison.CurrentCultureIgnoreCase)) return "";
        else
        {
           return ((nCnt>1) ? nCnt.ToString()+" ":"") + GetInstNameFromUICValues(0, nInst, 0, 0, false, (nCnt > 1));
        }
    }

    private void GetUICAsText(string sLang, StringBuilder sbRes, bool lUseDomain = true)
    {
        if (UICVal[0] == 0) return;
        LoadUICfromZinfonia(sLang);
        if (!sCurrentLang.Equals(sLang, StringComparison.CurrentCultureIgnoreCase)) sbRes.Append(sLastError);
        else
        {
            for (sbyte i = 0; i < 10; i++)
            {
                if (UICVal[i] == 0) break;

                int nDomain = 0;
                int nInst = 0;
                int nNumber = 0;
                int nDescriptor = 0;
                if ((i == 0) && (lUseDomain)) nDomain = (int)((UICVal[i] >> 51) & MASK_DOMAIN);
                bool lLeft = (UICVal[i] & MASK_LEFT) > 0;
                bool lIsDouble = (UICVal[i] & MASK_DOUBLE) > 0;
                nDescriptor = (int)((UICVal[i] >> 37) & MASK_DESCRIPT);
                nNumber = (int)((UICVal[i] >> 32) & MASK_NUMBER);
                nInst = (int)(UICVal[i] & MASK_INST);
                string sInstName = GetInstNameFromUICValues(nDomain, nInst, nNumber, nDescriptor, lLeft);
                if (sInstName != "")
                {
                    if ((UICVal[i] & 7) < 7)
                    {
                        if (sbRes.Length > 0)
                        {
                            if (lIsDouble) sbRes.Append("/");
                            else sbRes.Append(GetAmpersand(sLang));
                        }
                        sbRes.Append(sInstName);
                    }
                }
            }
        }
    }

    public override string ToString()
    {
        return ToString("");

    }

    private string getFirstField(XElement inst, string sField, string sFieldExt)
    {
        if (inst == null) return "";
        if (inst.Element(sField + sFieldExt) != null) return inst.Element(sField + sFieldExt).Value;
        if ((sFieldExt != "") && (inst.Element(sField) != null)) return inst.Element(sField).Value;
        if (sField != "InstName") {
            if (inst.Element("InstName" + sFieldExt) != null) return inst.Element("InstName" + sFieldExt).Value;
            if ((sFieldExt != "") && (inst.Element("InstName") != null)) return inst.Element("InstName").Value;
        }
        return "";
    }

    public (string, string, string) GetAllNames(int nInst, string format = "")
    {
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        LoadUICfromZinfonia(format);
        var inst = UICInst.Descendants("UIC").FirstOrDefault(x => (int)x.Element("InstUIC") == nInst);
        string sFieldExt = "";
        if (format != "en") sFieldExt = sCurrentLang.ToUpper();
        string fullname = getFirstField(inst, "InstName", sFieldExt);
        string pluralname = getFirstField(inst, "InstNamePlural", sFieldExt); 
        string shortname = getFirstField(inst, "InstNameShort", sFieldExt);
        return (fullname, pluralname, shortname);
    }
    public string forText(string format = "")
    {
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        if (format == "en") return "for ";
        else if (format == "de") return "for ";
        else if (format == "fr") return "pour ";
        else if (format == "it") return "per ";
        else if (format == "pl") return "na ";
        else if ((format == "es") || (format == "pt")) return "para ";
        else if (format == "ru") return "для ";
        else return "";
        //|| (format == "ko") || (format == "ja") || (format == "zh")
    }
    public string andText(string format = "")
    {
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        if (format == "en") return " and ";
        else if (format == "de") return " und ";
        else if (format == "fr") return " et ";
        else if ((format == "it") || (format == "pt")) return " e ";
        else if (format == "pl") return " & ";
        else if (format == "es") return " y ";
        else if (format == "ru") return "для ";
        else if (format == "ko") return " 그리고 ";
        else if (format == "ja") return " と ";
        else if (format == "zh") return "和";
        else return " & ";
    }

    public string ampText(string format = "")
    {
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        if ((format == "zh") || (format == "ko") || (format == "ja")) return ", ";
        else if (format == "ru") return " и ";
        return " & ";
    }

    public string searchNamedList(string name, string format = "")
    {
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        LoadUICfromZinfonia(format);
        string sFieldExt = "";
        if (sCurrentLang != "en") sFieldExt = sCurrentLang.ToUpper();
        var nl = UICNamed.Descendants("UIC").FirstOrDefault(x => (string)x.Element("NamedList") == name);
        if (nl != null)
        {
            if (nl.Element("NamedGroup" + sFieldExt) != null)
                return nl.Element("NamedGroup" + sFieldExt).Value;
        }
        return "";
    }

    public string searchDomainList(int DomainID, string format = "")
    {
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        LoadUICfromZinfonia(format);
        string sFieldExt = "";
        if (sCurrentLang != "en") sFieldExt = sCurrentLang.ToUpper();
        var domain = UICDomain.Descendants("UIC").FirstOrDefault(x => (int)x.Element("DomainID") == DomainID);
        if (domain != null)
        {
            if (domain.Element("DomainName" + sFieldExt) != null)
                return domain.Element("DomainName" + sFieldExt).Value;
            else return domain.Element("DomainName").Value;
        }
        return "";
    }

    public string ToString(string format = "")
    {
        StringBuilder sbRes = new StringBuilder();
        if (format == "") format = sCurrentLang;
        else format = format.ToLower();
        if ((format=="en") || (format == "de") || (format == "fr") || (format == "it") || (format == "pl")
             || (format == "es") || (format == "pt") || (format == "ko") || (format == "ja") || (format == "ru") || (format == "zh"))
        {
            SetUICLanguage(format);
            GetUICAsText(format,sbRes);
        }
        else
        {
            for (sbyte i = 0; i < 10; i++)
            {
                if (UICVal[i] == 0) break;
                if (sbRes.Length > 0) sbRes.Append(" ");
                sbRes.Append(UICVal[i].ToString());
            }
        }
        return sbRes.ToString();
    }
}