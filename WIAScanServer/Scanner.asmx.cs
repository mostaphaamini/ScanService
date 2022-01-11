using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WIAScanServer.WiaWrapper;
using WIA;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace WIAScanServer
{
    /// <summary>
    /// Summary description for Scaner
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Scanner : System.Web.Services.WebService
    {

        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatGIF = "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatTIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";

        public struct Page
        {
            public long size { get; set; }
            public int id { get; set; }
            public string data { get; set; }
        }

        public struct PType
        {
            public int id { get; set; }
            public string name { get; set; }

           
        }

        public struct Source
        {
            public string id { get; set; }
            public string name { get; set; }
            public Dpi[] dpis { get; set; }
            public PType[] pixelTypes { get; set; }
            public bool duplex { get; set; }
            public bool feeder { get; set; }
            public bool autoFeed { get; set; }
            public string error { get; set; }
        }

        public struct Dpi
        {
            public int val { get; set; }

           
        }

        public class Options
        {
            public String url { get; set; }
            public String mypass { get; set; }
            public string source { get; set; }

            public bool feeder { get; set; }
            public bool feederDuplex { get; set; }
            public int dpi { get; set; }
            public int pixelType { get; set; }
            public bool autoFeed { get; set; }
            public float brithness { get; set; }
            public float contrast { get; set; }

        }



        [WebMethod]
        public string About()
        {
            return "Developed By mostapha aminipour";
        }

        [WebMethod]
        public Source[] getSourceList(String mypass)
        {
            List<Source> res = new List<Source>();
            WIA.DeviceManager manager = new WIA.DeviceManager();

            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {

                if(info.Type == WIA.WiaDeviceType.ScannerDeviceType)
                {
                    Source device = new Source();
                    device.id = info.DeviceID;
                    Device wiadevice = info.Connect();
                  

                    foreach(WIA.Property p in info.Properties)
                    {
                        Trace.WriteLine(p.PropertyID + " : " + p.Name);
                        if(p.Name == "Name")
                        {
                            device.name = p.get_Value();
                        }
                    }
                    device.pixelTypes = new PType[2];
                    device.pixelTypes[0].id = 1;
                    device.pixelTypes[0].name = "RGB";
                    device.pixelTypes[1].id = 2;
                    device.pixelTypes[1].name = "GRAY";
                    device.pixelTypes[1].id = 4;
                    device.pixelTypes[1].name = "BW";

                    device.dpis = new Dpi[3];
                    device.dpis[0].val = 100;
                    device.dpis[1].val = 150;
                    device.dpis[2].val = 300;

                    res.Add(device);
                }
            }

            return res.ToArray();
        }

        [WebMethod]
        public Page[] scan(String optsStr)
        {
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                Options opts = (Options)json_serializer.Deserialize<Options>(optsStr);
                List<Page> pages = Scan.StartScan(opts);
                return pages.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string getEVal(WiaProperty property)
        {
            return ((int)property).ToString();
        }
    }
}
