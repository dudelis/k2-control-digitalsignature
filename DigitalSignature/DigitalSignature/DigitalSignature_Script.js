(function ($) {
    if (typeof DigitalSignature === "undefined" || DigitalSignature == null) DigitalSignature = {};
    if (typeof DigitalSignature.DigitalSignature === "undefined" || DigitalSignature.DigitalSignature == null) DigitalSignature.DigitalSignature = {};

    DigitalSignature.DigitalSignature = {
        
        _getInstance: function (id) {
            var control = jQuery('#' + id);
            if (control.length == 0) {
                throw 'DigitalSignature \'' + id + '\' not found';
            } else {
                return control[0];
            }
        },

        _getSigPadInstance: function (id) {
            var control = jQuery('#' + id + ' .sigPad');
            if (control.length == 0) {
                throw 'DigitalSignature \'' + id + '\' SignaturePad not found';
            }
            return control.signaturePad();
        },

        _getSigPadInstance: function (id, options) {
            var control = jQuery('#' + id + ' .sigPad');
            if (control.length == 0) {
                throw 'DigitalSignature \'' + id + '\' SignaturePad not found';
            }
            return control.signaturePad(options);
        },

        getValue: function (objInfo) {
            return DigitalSignature.DigitalSignature._getSigPadInstance(objInfo.CurrentControlId).getSignatureString(); //JSON
        },

        getDefaultValue: function (objInfo) {
            getValue(objInfo);
        },

        setValue: function (objInfo) {
            return DigitalSignature.DigitalSignature._getSigPadInstance(objInfo.CurrentControlId).regenerate(objInfo.Value);
        },

        //retrieve a property for the control
        getProperty: function (objInfo) {
            if (objInfo.property.toLowerCase() == "value") {
                return DigitalSignature.DigitalSignature.getValue(objInfo);
            }
            else {
                return $('#' + objInfo.CurrentControlId).data(objInfo.property);
            }
        },

        //set a property for the control. note case statement to call helper methods
        setProperty: function (objInfo) {
            switch (objInfo.property.toLowerCase()) {
                case "value":
                    DigitalSignature.DigitalSignature.setValue(objInfo);
                    break;
                case "isvisible":
                    DigitalSignature.DigitalSignature.setIsVisible(objInfo);
                    break;
                case "title":
                    DigitalSignature.DigitalSignature.setTitle(objInfo);
                    break;
                case "width":
                    DigitalSignature.DigitalSignature.setSize(objInfo, 'width');
                    break;
                case "height":
                    DigitalSignature.DigitalSignature.setSize(objInfo, 'height');
                    break;
                default:
                    $('#' + objInfo.CurrentControlId).data(objInfo.property).value = objInfo.Value;
            }
        },

        validate: function (objInfo) {
            //alert("validate for control " + objInfo.CurrentControlId);
        },

        //helper method to set visibility
        setIsVisible: function (objInfo) {
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isVisible = value;
            var displayValue = (value === false) ? "none" : "inherit";
            var instance = DigitalSignature.DigitalSignature._getInstance(objInfo.CurrentControlId);
            instance.style.display = displayValue;
        },

        setTitle: function (objInfo) {
            $('#' + objInfo.CurrentControlId + ' p.sigTitle').text(objInfo.Value);
        },

        setSize: function (objInfo, attr) {
            $('#' + objInfo.CurrentControlId + ' canvas.pad').attr(attr, objInfo.Value);
        },

        execute: function (objInfo) {
            //var controlId = objInfo.CurrentControlID;
            var parameters = objInfo.methodParameters;
            var method = objInfo.methodName;
            var result = "";
            var currentControlID = objInfo.CurrentControlID; //need to use CurrentControlID instead of CurrentControlId in this method.
            switch (method) {

                //for signing               
                case "initsign":
                    var options = {
                        defaultAction: 'typeit'
                        , drawOnly: true
                        , lineTop: $('#' + currentControlID + ' canvas.pad').attr('height').replace('px') * 0.9 //10% from bottom
                    };
                    DigitalSignature.DigitalSignature._getSigPadInstance(currentControlID, options);
                    break;

                //display JSON signature               
                case "displayjson":
                    //hide image tag
                    $('#' + currentControlID + ' .sigImg').hide();

                    var options = {
                        displayOnly: true
                    };
                    if (parameters["sigpadjson"] != '') {
                        //use JSON input
                        var test = DigitalSignature.DigitalSignature._getSigPadInstance(currentControlID, options).regenerate(parameters["sigpadjson"]);
                    }
                    else {
                        //empty display
                        DigitalSignature.DigitalSignature._getSigPadInstance(currentControlID, options);
                    }
                    break;

                //display signature image       
                case "displayimg":
                    //hide signature pad
                    $('#' + currentControlID + ' .sigPad').hide();
                    //show image
                    $.ajax(
                	{
                	    type: 'GET',
                	    url: 'DigitalSignatureImage.handler',
                	    cache: false,
                	    data: { id: parameters['sigimgid'] },
                	    dataType: 'text',
                	    async: false
                	}).success(function (data) {
                	    $('#' + currentControlID + ' .sigImg').html('<img src="' + data + '" />').show();
                	});
                    break;

                //ajax call to save the bitmap to the smartobject              
                case "saveasimg":
                    //JSON string
                    var jsonStr = DigitalSignature.DigitalSignature._getSigPadInstance(currentControlID).getSignatureString();
                    //Width and height of canvas. Important. The C# conversion class needs the width and height
                    // 2013-08-30: Ensure the replaced value is an empty string, else WebKit browsers will take it as 'undefined'
                    var canvasWidth = $('#' + currentControlID + ' canvas.pad').attr('width').replace('px', '');
                    var canvasHeight = $('#' + currentControlID + ' canvas.pad').attr('height').replace('px', '');

                    $.ajax(
                	{
                	    type: 'POST',
                	    url: 'DigitalSignature/DigitalSignatureSave.handler',
                	    cache: false,
                	    data: { json: jsonStr, width:canvasWidth, height:canvasHeight, url: parameters['url'], fqn: parameters['fqn'] },
                	    dataType: 'text',
                	    async: false
                	}).done(function (number) {
                	    objInfo.Value = number;
                	    result = number;
                	});
                    break;

                //convert on server side and get the image xml
                case "getimg":
                    //JSON string
                    var jsonStr = DigitalSignature.DigitalSignature._getSigPadInstance(currentControlID).getSignatureString();
                    //Width and height of canvas. Important. The C# conversion class needs the width and height
                    // 2013-08-30: Ensure the replaced value is an empty string, else WebKit browsers will take it as 'undefined'
                    var canvasWidth = $('#' + currentControlID + ' canvas.pad').attr('width').replace('px', '');
                    var canvasHeight = $('#' + currentControlID + ' canvas.pad').attr('height').replace('px', '');
                    $.ajax(
                	{
                	    type: 'POST',
                	    url: 'DigitalSignature/DigitalSignatureGetImage.handler',
                	    cache: false,
                	    data: { json: jsonStr, width: canvasWidth, height: canvasHeight },
                	    dataType: 'text',
                	    async: false
                	}).done(function (xmlResult) {
                	    if (jQuery.isXMLDoc(parseXML(xmlResult))) {
                	        $('#' + objInfo.CurrentControlId).data('file').value = xmlResult;
                	    }
                        else
                	    {
                	        alert(xmlResult);
                	    }
                	});
                    break;

                //clear the canvas               
                case "clear":
                    DigitalSignature.DigitalSignature._getSigPadInstance(currentControlID).clearCanvas();
                    break;
            }

            return result;
        }
    };

})(jQuery);




