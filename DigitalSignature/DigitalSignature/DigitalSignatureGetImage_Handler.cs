using ConsumedByCode.SignatureToImage;
using SourceCode.Forms.AppFramework;
using SourceCode.Forms.Controls.Web.SDK.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace DigitalSignature.DigitalSignature
{
    [ClientAjaxHandler("DigitalSignatureGetImage.handler")]
    public class DigitalSignatureGetImage_Handler : IHttpHandler
    {
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
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

            try
            {
                string result = string.Empty;
                Bitmap bitmapImg = sit.SigJsonToImage(jsonStr);
                long length = 0;
                string directoryName = string.Empty;
                string fileName = string.Empty;
                string filePath = FileUtility.EnsurePath(ref directoryName, ref fileName);
                //string pngStr = string.Empty;
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmapImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    length = ms.Length;
                    //byte[] pngBin = new byte[ms.Length];
                    //pngBin = ms.ToArray();
                    //pngStr = "data:image/png;base64," + Convert.ToBase64String(pngBin);

                    using (Stream fileStream = FileUtility.Open(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        this.CopyStream(ms, fileStream);
                    }
                }

                result = "<collection><object><fields><field name='FileName'><value>" + XmlConvert.EncodeName(fileName) +"</value></field><field name='FilePath'><value>" + XmlConvert.EncodeName(filePath) + "</value></field></fields></object></collection>";

                context.Response.ContentType = "text/xml";
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.ClearContent();
                Logging.LogException(ex, string.Format("Error getting image: {0}", ex.ToString()), false, true);
                context.Response.Write("Error getting signature");
            }
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}
