using System;
using System.Web;
using SCHC = SourceCode.Hosting.Client.BaseAPI;
using SCSMOC = SourceCode.SmartObjects.Client;
using System.Configuration;
using SourceCode.Forms.Controls.Web.SDK.Attributes;
using System.Drawing;
using ConsumedByCode.SignatureToImage;
using System.Text;
using System.IO;

namespace DigitalSignature.DigitalSignature
{
    [ClientAjaxHandler("DigitalSignatureSave.handler")]
    public class DigitalSignatureHandler : IHttpHandler
    {
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            #region Get image in PNG format
            //take JSON string and convert to png
            string jsonStr = context.Request.Form["json"];
            
            SignatureToImage sit = new SignatureToImage();
            
            #region set the width and height (IMPORTANT!!)
            try
            {
                sit.CanvasWidth = Convert.ToInt32(context.Request.Form["width"]);
            }
            catch
            {
                sit.CanvasWidth = 200;
            }
            try
            {
                sit.CanvasHeight = Convert.ToInt32(context.Request.Form["height"]);
            }
            catch
            {
                sit.CanvasHeight = 60;
            }
            #endregion
            
            Bitmap bitmapImg = sit.SigJsonToImage(jsonStr);

            string pngStr = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                bitmapImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] pngBin = new byte[ms.Length];
                pngBin = ms.ToArray();
                pngStr = "data:image/png;base64," + Convert.ToBase64String(pngBin);
            }
            #endregion

            #region save to smartobject and return the new obj ID
            SCSMOC.SmartObjectClientServer smoSvr = new SCSMOC.SmartObjectClientServer();
            try
            {
                smoSvr.CreateConnection();
                smoSvr.Connection.Open(GetSMOConnStr());

                SCSMOC.SmartObject smoObj = smoSvr.GetSmartObject("DigitalSignature");
                smoObj.Properties["Signature"].Value = pngStr;
                smoObj.Properties["FormURL"].Value = context.Request.Form["url"];
                smoObj.Properties["UserFQN"].Value = context.Request.Form["fqn"];
                smoObj.Properties["Date"].Value = DateTime.Now.ToString();

                smoObj.MethodToExecute = "Create";
                smoObj = smoSvr.ExecuteScalar(smoObj);

                context.Response.Write(smoObj.Properties["ID"].Value.ToString());
            }
            finally
            {
                if (smoSvr.Connection != null && smoSvr.Connection.IsConnected)
                    smoSvr.Connection.Dispose();
            }
            #endregion
        }


        private string GetSMOConnStr()
        {
            SCHC.SCConnectionStringBuilder connStr = new SCHC.SCConnectionStringBuilder();
            connStr.Host = ConfigurationManager.AppSettings["HostName"];
            connStr.Port = Convert.ToUInt32(ConfigurationManager.AppSettings["HostPort"]);
            connStr.Integrated = true;
            connStr.IsPrimaryLogin = true;
            return connStr.ConnectionString;            
        }
    }
}
