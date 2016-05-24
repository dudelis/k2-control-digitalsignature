using System;
using System.Web;
using System.Configuration;
using SCHC = SourceCode.Hosting.Client.BaseAPI;
using SCSMOC = SourceCode.SmartObjects.Client;
using SourceCode.Forms.Controls.Web.SDK.Attributes;

namespace DigitalSignature.DigitalSignature
{
    [ClientAjaxHandler("DigitalSignatureImage.handler")]
    public class DigitalSignature_ShowImage : IHttpHandler
    {
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string imgID = context.Request.QueryString["id"];

            SCSMOC.SmartObjectClientServer smoSvr = new SCSMOC.SmartObjectClientServer();
            try
            {
                smoSvr.CreateConnection();
                smoSvr.Connection.Open(GetSMOConnStr());

                SCSMOC.SmartObject smoObj = smoSvr.GetSmartObject("DigitalSignature");
                smoObj.Properties["ID"].Value = imgID;

                smoObj.MethodToExecute = "Load";
                smoObj = smoSvr.ExecuteScalar(smoObj);

                context.Response.Write(smoObj.Properties["Signature"].Value);
            }
            finally
            {
                if (smoSvr.Connection != null && smoSvr.Connection.IsConnected)
                    smoSvr.Connection.Dispose();
            }
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
