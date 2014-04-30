using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SourceCode.Forms.Controls.Web.SDK;
using SourceCode.Forms.Controls.Web.SDK.Attributes;

[assembly: WebResource("DigitalSignature.DigitalSignature.DigitalSignature_Script.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.DigitalSignature_Stylesheet.css", "text/css", PerformSubstitution = true)]

//For Digital signature
[assembly: WebResource("DigitalSignature.DigitalSignature.jquery.signaturepad.min.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.json2.min.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.flashcanvas.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("DigitalSignature.DigitalSignature.jquery.signaturepad.css", "text/css", PerformSubstitution = true)]

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
        public Control() { }
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
            LiteralControl ctrl = new LiteralControl();
            if (base.State == SourceCode.Forms.Controls.Web.Shared.ControlState.Runtime)
            {
                string html = string.Format(@"
                                <div class='sigPad'>
                                    <p class='sigTitle'>{0}</p>
                                    <div class='sig sigWrapper'>
                                        <canvas class='pad' width='{1}px' height='{2}px'></canvas>
                                        <input type='hidden' name='output' class='output'>
                                    </div>
                                </div>
                                <div class='sigImg' style='display:none'>
                                </div>
                            ", this.Title, this.Width, this.Height);
                #region sample html
                //                @"
//                                <div class='sigPad'>
//                                    <label for='name'>Print your name</label>
//                                    <input type='text' name='name' id='name' class='name'>
//                                    <p class='typeItDesc'>Review your signature</p>
//                                    <p class='drawItDesc'>Draw your signature</p>
//                                    <ul class='sigNav'>
//                                        <li class='typeIt'><a href='#type-it' class='current'>Type It</a></li>
//                                        <li class='drawIt'><a href='#draw-it'>Draw It</a></li>
//                                        <li class='clearButton'><a href='#clear'>Clear</a></li>
//                                    </ul>
//                                    <div class='sig sigWrapper'>
//                                    <div class='typed'></div>
//                                        <canvas class='pad' width='198' height='55'></canvas>
//                                        <input type='hidden' name='output' class='output'>
//                                    </div>
//                                </div>
                //                            ";
                #endregion

                ctrl.Text = html;
            }
            else
            {
                //design or preview
                ctrl.Text = "[Digital Signature Control]";
            }
            ctrl.RenderControl(writer);
        }
        #endregion
    }
}
