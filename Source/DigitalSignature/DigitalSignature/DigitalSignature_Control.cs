using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SourceCode.Forms.Controls.Web.SDK;
using SourceCode.Forms.Controls.Web.SDK.Attributes;
using System.Web.UI.HtmlControls;

[assembly: WebResource("DigitalSignature.DigitalSignature.DigitalSignature_Script.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.DigitalSignature_Stylesheet.css", "text/css", PerformSubstitution = true)]
//control icon
[assembly: WebResource("DigitalSignature.DigitalSignature.DS_Icon.png", "image/png")]
//For Digital signature
[assembly: WebResource("DigitalSignature.DigitalSignature.jquery.signaturepad.min.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.json2.min.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.flashcanvas.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.jquery.signaturepad.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.pen.cur", "image/bmp")]

namespace DigitalSignature.DigitalSignature
{
    [ControlTypeDefinition("DigitalSignature.DigitalSignature.DigitalSignature_Definition.xml")]
    [ClientScript("DigitalSignature.DigitalSignature.DigitalSignature_Script.js")]
    [ClientCss("DigitalSignature.DigitalSignature.DigitalSignature_Stylesheet.css")]

    //For Digital signature
    [ClientScript("DigitalSignature.DigitalSignature.jquery.signaturepad.min.js")]
    [ClientScript("DigitalSignature.DigitalSignature.json2.min.js")]
    [ClientScript("DigitalSignature.DigitalSignature.flashcanvas.js")]
    [ClientCss("DigitalSignature.DigitalSignature.jquery.signaturepad.css")]

    //specifies the location of the client-side resource file for the control. You will need to add a resource file to the project properties
    //[ClientResources("DigitalSignature.Resources.[ResrouceFileName]")]
    public class Control : BaseControl
    {
        private ClientScriptManager cm;
        private Type rs;

        #region Control Properties
        public string Title
        {
            get
            {
                return this.GetOption<string>("title", "Signature");
            }
            set
            {
                this.SetOption<string>("title", value, "Signature");
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.GetOption<bool>("isvisible", true);
            }
            set
            {
                this.SetOption<bool>("isvisible", value, true);
            }
        }

        public string Width
        {
            get
            {
                return this.GetOption<string>("width", "200");
            }
            set
            {
                this.SetOption<string>("width", value, "200");
            }
        }

        public string Height
        {
            get
            {
                return this.GetOption<string>("height", "60");
            }
            set
            {
                this.SetOption<string>("height", value, "60");
            }
        }

        public string File
        {
            get
            {
                return this.GetOption<string>("file", string.Empty);
            }
            set
            {
                this.SetOption<string>("file", value, string.Empty);
            }
        }

        #region IDs
        public string ControlID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public override string ClientID
        {
            get
            {
                return base.ID;
            }
        }

        public override string UniqueID
        {
            get
            {
                return base.ID;
            }
        }
        #endregion

        #endregion

        #region Contructor
        public Control() 
            : base("div")
        {
            this.cm = this.Page.ClientScript;
            this.rs = base.GetType();
            ((SourceCode.Forms.Controls.Web.Shared.IControl)this).DesignFormattingPaths
                .Add("stylecss", "DigitalSignature.DigitalSignature.DigitalSignature_Stylesheet.css");
        }
        #endregion

        #region Control Methods
        protected override void CreateChildControls()
        {
            base.EnsureChildControls();

            if (base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime)
            {
                //assign a temp unique Id for the control
                this.ID = Guid.NewGuid().ToString();
            }

            base.CreateChildControls();
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            base.RenderContents(writer);

            if (base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime)
            {
                #region sample
                //                string html = string.Format(@"
                //                                <div class='sigPad'>
                //                                    <p class='sigTitle'>{0}</p>
                //                                    <div class='sig sigWrapper'>
                //                                        <canvas class='pad' width='{1}px' height='{2}px'></canvas>
                //                                        <input type='hidden' name='output' class='output'>
                //                                    </div>
                //                                </div>
                //                                <div class='sigImg' style='display:none'>
                //                                </div>
                //                            ", this.Title, this.Width, this.Height);
                #endregion
                //sig pad
                //<div class='sigPad'>
                HtmlGenericControl divTagPad = new HtmlGenericControl();
                divTagPad.Attributes.Add("class", "sigPad");
                // <p class='sigTitle'>
                HtmlGenericControl pTagTitle = new HtmlGenericControl("p");
                pTagTitle.Attributes.Add("class", "sigTitle");
                pTagTitle.InnerText = this.Title;
                divTagPad.Controls.Add(pTagTitle);
                // <div class='sig sigWrapper'>
                HtmlGenericControl divTagWrapper = new HtmlGenericControl("div");
                divTagWrapper.Attributes.Add("class", "sig sigWrapper");
                // <canvas class='pad'
                HtmlGenericControl canvasTag = new HtmlGenericControl("canvas");
                canvasTag.Attributes.Add("class", "pad");
                canvasTag.Attributes.Add("width", this.Width);
                canvasTag.Attributes.Add("height", this.Height);
                divTagWrapper.Controls.Add(canvasTag);
                // <input type='hidden'
                HtmlInputHidden hiddenOutput = new HtmlInputHidden();
                hiddenOutput.Name = "output";
                hiddenOutput.Attributes.Add("class", "output");
                divTagWrapper.Controls.Add(hiddenOutput);
                divTagPad.Controls.Add(divTagWrapper);
                divTagPad.RenderControl(writer);

                //sig img
                // <div class='sigImg' 
                HtmlGenericControl divTagImg = new HtmlGenericControl("div");
                divTagImg.Attributes.Add("class", "sigImg");
                divTagImg.Style.Add(HtmlTextWriterStyle.Display, "none");
                divTagImg.RenderControl(writer);
            }
            else
            {
                //design or preview
                HtmlGenericControl divTagBase = new HtmlGenericControl("div");
                HtmlImage icon = new HtmlImage();
                icon.Src = this.cm.GetWebResourceUrl(this.rs, "DigitalSignature.DigitalSignature.DS_Icon.png");
                icon.Border = 0;
                divTagBase.Controls.Add(icon);
                HtmlGenericControl lblTag = new HtmlGenericControl("span");
                lblTag.InnerText = "Digital Signature Control";
                divTagBase.Controls.Add(lblTag);
                divTagBase.RenderControl(writer);
            }
        }
        #endregion
    }
}
