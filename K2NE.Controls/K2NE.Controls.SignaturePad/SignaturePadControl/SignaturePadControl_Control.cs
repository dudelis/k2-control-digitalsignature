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

//Control files
[assembly: WebResource("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_Script.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_BaseScript.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_Stylesheet.css", "text/css", PerformSubstitution = true)]

//Resources scripts
[assembly: WebResource("K2NE.Controls.SignaturePad.Resources.signature_pad.min.js", "text/javascript", PerformSubstitution = true)]


//Images
[assembly: WebResource("K2NE.Controls.SignaturePad.Resources.icon.png", "image/png")]
[assembly: WebResource("K2NE.Controls.SignaturePad.Resources.placeholder.png", "image/png")]
[assembly: WebResource("K2NE.Controls.SignaturePad.Resources.pen.cur", "image/bmp")]

namespace K2NE.Controls.SignaturePad.SignaturePadControl
{

    [ClientScript("K2NE.Controls.SignaturePad.Resources.signature_pad.min.js", ClientScriptAttribute.Scopes.Both, 0)]
    [ClientScript("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_BaseScript.js", ClientScriptAttribute.Scopes.Both, 1)]
    [ClientScript("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_Script.js", ClientScriptAttribute.Scopes.Both, 2)]
    [ControlTypeDefinition("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_Definition.xml")]
    [ClientCss("K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_Stylesheet.css")]
    public class Control : BaseControl
    {
        #region Control Properties
        #region General
        public string ControlName { get; set; }
        public bool IsVisible
        {
            get { return this.GetOption<bool>("isvisible", true); }
            set { this.SetOption<bool>("isvisible", value, true); }
        }
        public bool IsEnabled
        {
            get { return this.GetOption<bool>("isenabled", true); }
            set { this.SetOption<bool>("isenabled", value, true); }
        }
        public string Value { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string FileName { get; set; }

        public bool DrawOnly { get; set; }
        public string BackgroundColor { get; set; }
        //Pen properties
        public string PenColor { get; set; }
        public int PenWidth { get; set; }
        public string PenCap { get; set; }
        //Line properties
        public string LineColor { get; set; }
        public int LineWidth { get; set; }
        public int LineMargin { get; set; }
        public string LineTop { get; set; }
        #endregion

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
            : base("div")  //TODO: if needed, inherit from a HTML type like div or input
        {

        }
        #endregion

        #region Control Methods
        protected override void OnPreRender(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptResource(GetType(), "K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_BaseScript.js");
            this.Page.ClientScript.RegisterClientScriptResource(GetType(), "K2NE.Controls.SignaturePad.SignaturePadControl.SignaturePadControl_Script.js");
            base.OnPreRender(e);
        }
        protected override void CreateChildControls()
        {
            base.EnsureChildControls();
            //Perform state-specific operations
            switch (base.State)
            {
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Designtime:
                    //assign a temp unique Id for the control
                    this.ID = Guid.NewGuid().ToString();
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Preview:
                    //do any Preview-time manipulation here
                    break;
                case SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime:
                    //do any runtime manipulation here
                    this.Options.Add("filename", this.FileName);
                    this.Options.Add("width", this.Width);
                    this.Options.Add("height", this.Height);
                    this.Options.Add("drawOnly", this.DrawOnly);
                    this.Options.Add("bgColour", this.BackgroundColor);
                    this.Options.Add("penColour", this.PenColor);
                    this.Options.Add("penWidth", this.PenWidth);
                    this.Options.Add("penCap", this.PenCap);
                    this.Options.Add("lineColour", this.LineColor);
                    this.Options.Add("lineWidth", this.LineWidth);
                    this.Options.Add("lineMargin", this.LineMargin);
                    this.Options.Add("lineTop", this.LineTop);
                    break;
            }
            // Call base implementation last
            base.CreateChildControls();
        }
        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            base.RenderContents(writer);
            if (base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime)
            {
                var divWrapper = new HtmlGenericControl("div");
                divWrapper.Attributes.Add("class", "signaturepad-wrapper");
                divWrapper.ID = this.ID + "_wrapper";

                var canvas = new HtmlGenericControl("canvas");
                canvas.Attributes.Add("class", "signaturepad-canvas");
                canvas.ID = this.ID + "_canvas";

                divWrapper.Controls.Add(canvas);

                divWrapper.RenderControl(writer);

                // //sig pad
                // //<div class='sigPad'>
                // var divSigPad = new HtmlGenericControl("div");
                // divSigPad.Attributes.Add("class", "sigPad");

                //// <div class='sig sigWrapper'>
                // var divTagWrapper = new HtmlGenericControl("div");
                // divTagWrapper.Attributes.Add("class", "sig sigWrapper");
                // // <canvas class='pad'
                // HtmlGenericControl canvasTag = new HtmlGenericControl("canvas");
                // canvasTag.Attributes.Add("class", "pad");
                // divTagWrapper.Controls.Add(canvasTag);

                // divSigPad.Controls.Add(divTagWrapper);



                // //sig img
                // // <div class='sigImg' 
                // HtmlGenericControl divTagImg = new HtmlGenericControl("div");
                // divTagImg.Attributes.Add("class", "sigImg");
                // divTagImg.Style.Add(HtmlTextWriterStyle.Display, "none");


                // divSigPadWrapper.Controls.Add(divSigPad);
                // divSigPadWrapper.Controls.Add(divTagImg);


            }
            else
            {
                var icon = new Image();
                var page = this.Page;
                if (page == null)
                {
                    page = new System.Web.UI.Page();
                }
                icon.ImageUrl = page.ClientScript.GetWebResourceUrl(this.GetType(), string.Format("K2NE.Controls.SignaturePad.Resources.placeholder.png"));
                icon.Width = new Unit("100%");
                icon.Height = new Unit("100%");
                icon.RenderControl(writer);
            }
        }
        #endregion
    }
}
