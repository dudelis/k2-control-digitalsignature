(function ($) {
    if (typeof K2NE === 'undefined' || K2NE === null) K2NE = {};
    if (typeof K2NE.Controls === 'undefined' || K2NE.Controls === null) K2NE.Controls = {};
    if (typeof K2NE.Controls.SignaturePad === 'undefined' || K2NE.Controls.SignaturePad === null) K2NE.Controls.SignaturePad = {};

    var K2NESignaturePad;
    K2NE.Controls.SignaturePad.Control = K2NESignaturePad = $.extend({}, {
        
        getProperty: function (objInfo) {
            if (objInfo.property.toLowerCase() == "value") {
                return K2NESignaturePad.getValue(objInfo);
            } else {
                return $('#' + objInfo.CurrentControlId).data(objInfo.property);
            }
        },
        getValue: function (objInfo) {
            //alert("getValue() for control " + objInfo.CurrentControlId);
            var instance = K2NE.Controls.PivotControl._getInstance(objInfo.CurrentControlId);
            return instance.value;
        },

        getDefaultValue: function (objInfo) {
            //alert("getDefaultValue() for control " + objInfo.CurrentControlId);
            getValue(objInfo);
        },

        setValue: function (objInfo) {
            //alert("setValue() for control " + objInfo.CurrentControlId);
            var instance = K2NE.Controls.PivotControl._getInstance(objInfo.CurrentControlId);
            var oldValue = instance.value;
            //only change the value if it has actually changed, and then raise the OnChange event
            if (oldValue != objInfo.Value) {
                instance.value = objInfo.Value;
                raiseEvent(objInfo.CurrentControlId, 'Control', 'OnChange');
            }
        },

        //retrieve a property for the control
        getProperty: function (objInfo) {
            //alert("getProperty(" + objInfo.property + ") for control " + objInfo.CurrentControlId);
            if (objInfo.property.toLowerCase() == "value") {
                return K2NE.Controls.PivotControl.getValue(objInfo);
            } else {
                return $('#' + objInfo.CurrentControlId).data(objInfo.property);
            }
        },

        //set a property for the control. note case statement to call helper methods
        setProperty: function (objInfo) {

            switch (objInfo.property.toLowerCase()) {
                case "style":
                    K2NE.Controls.PivotControl.setStyles(null, objInfo.Value, $('#' + objInfo.CurrentControlId));
                    break;
                case "value":
                    K2NE.Controls.PivotControl.setValue(objInfo);
                    break;
                case "isvisible":
                    K2NE.Controls.PivotControl.setIsVisible(objInfo);
                    break;
                case "isenabled":
                    K2NE.Controls.PivotControl.setIsEnabled(objInfo);
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
            //alert("set_isVisible: " + objInfo.Value);
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isVisible = value;
            var displayValue = (value === false) ? "none" : "block";
            var instance = K2NE.Controls.PivotControl._getInstance(objInfo.CurrentControlId);
            instance.style.display = displayValue;
        },

        //helper method to set control "enabled" state
        setIsEnabled: function (objInfo) {
            //alert("set_isEnabled: " + objInfo.Value);
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isEnabled = value;
            var instance = K2NE.Controls.PivotControl._getInstance(objInfo.CurrentControlId);
            instance.readOnly = !value;
        },
        execute: function (objInfo) {
            var method = objInfo.methodName;
            switch (method.toLowerCase()) {
                case "initialize":
                    var instance = K2NEPivotControl._getInstance(objInfo.CurrentControlId);
                    $(instance).K2NEPivotControl('initialize');
                    break;
                default:
                    break;
            }
        }

    });

    $.widget('sfc.K2NESignaturePad', {
        options: {
            id: '',
            isvisible: true,
            isenabled: true,
            labels: [],
            previous: {}

        },
        _create: function (options) {
            var elementOptions = $(this.element[0]).data('options');
            var keyNames = Object.keys(elementOptions);
            for (var i = 0; i < keyNames.length; i++) {
                this.options[keyNames[i]] = elementOptions[keyNames[i]];
            }
            this.options.id = this.element[0].id;
            this.options.sigpad = $(this.element[0]).find('.sigPad')[0];
            this.options.previous = this.options;
            this._setWidth(this.options.width, this);
            this._setHeight(this.options.height, this);
            this._setIsVisible(this.options.isvisible, this);
            this._setIsEnabled(this.options.isenabled, this);
            this._initialize(this);
        },
        _setOption: function (key, value) {
            switch (key) {
                case 'width':
                    this._setWidth(value, this);
                    break;
                case 'height':
                    this._setHeight(value, this);
                    break;
                case 'isvisible':
                    this._setIsVisible(value, this);
                    break;
                case 'isenabled':
                    this._setIsEnabled(value, this);
                    break;
                default:
                    this.options.previous[key] = this.options[key];
                    this.options[key] = value;
                    break;
            }
        },
        _setWidth: function (value, self) {
            value = ("" + value).trim();
            var isPercentage = value.length > 0 && value.charAt(value.length - 1) === '%';
            var control = self.element[0];
            var wrapper = $(control).find('.sigpadwrapper');
            var pad = $(control).find('.sigPad');
            var img = $(control).find('.sigImg');
            var canvas = $(control).find('canvas');
            if (value === "" || value === "null") {
                $(control).css('width', '100%');

                if (wrapper.length > 0) {
                    wrapper[0].style.width = "100%";
                }
                $(pad).css('width', '100%');
                $(img).css('width', '100%');
            } else {

                if (isPercentage) {
                    $(control).css('width', value);
                    if (wrapper.length > 0) {
                        wrapper[0].style.width = '100%';
                    }
                    $(pad).css('width', '100%');
                    $(img).css('width', '100%');
                } else {
                    $(control).css('width', value);

                    if (wrapper.length > 0) {
                        wrapper[0].style.width = '100%';
                    }
                    $(pad).css('width', '100%');
                    $(img).css('width', '100%');
                }
            }
            $(canvas).attr('width', $(pad).width());
            this.options.previous['width'] = this.options['width'];
            this.options['width'] = value;
        },
        _setHeight: function (value, self) {
            value = ("" + value).trim();
            var isPercentage = value.length > 0 && value.charAt(value.length - 1) === '%';
            var control = self.element[0];
            var wrapper = $(control).find('.sigpadwrapper');
            var pad = $(control).find('.sigPad');
            var img = $(control).find('.sigImg');
            var canvas = $(control).find('canvas');
            if (value === "") {
                $(control).css('height', '100%');

                if (wrapper.length > 0) {
                    wrapper[0].style.height = "100%";
                }
                $(pad).css('height', '100%');
                $(img).css('height', '100%');
            } else {
                if (isPercentage) {
                    $(control).css('height', value);
                    if (wrapper.length > 0) {
                        $(wrapper[0]).css('height', '100%');
                    }
                    $(pad).css('height', '100%');
                    $(img).css('height', '100%');
                } else {
                    $(control).css('height', value);

                    if (wrapper.length > 0) {
                        $(wrapper[0]).css('height', value);
                    }
                    $(pad).css('height', value);
                    $(img).css('height', value);
                }
            }

            $(canvas).attr('height', $(pad).height());
            this.options.previous['height'] = this.options['height'];
            this.options['height'] = value;
        },
        _setIsVisible: function (value, self) {
            var control = self.element[0];
            var displayValue = (value === false || value == 'false') ? "none" : "block";
            control.style.display = displayValue;
            this.options.previous['isvisible'] = this.options['isvisible'];
            this.options['isvisible'] = value;
        },
        _setIsEnabled: function (value, self) {
            this._isEnabled = value;
            var control = self.element[0];
            $(control).readOnly = !value;
            //adding a class to make the control blurred
            if (value === false || value == 'false') {
                $(control).addClass('disabled');
            } else {
                $(control).removeClass('disabled');
            }
            this.options.previous['isenabled'] = this.options['isenabled'];
            this.options['isenabled'] = value;
        },
        _getOptions: function (self) {
            return {
                defaultAction: 'drawIt',
                displayOnly: false,
                drawOnly: self.options.drawOnly,
                bgColour: self.options.bgColour,
                penColour: self.options.penColour,
                penWidth: self.options.penWidth,
                penCap: self.options.penCap,
                lineColour: self.options.lineColour,
                lineWidth: self.options.lineWidth,
                lineMargin: self.options.lineMargin,
                lineTop: self.options.lineTop,
                drawOnly: self.options.drawOnly
            };
        },
        _initialize: function (self) {
            var options = self._getOptions(self);
            $(self.options.sigpad).signaturePad(options);
        }
    });



})(jQuery);

$(document).ready(function () {
    $('.SFC.K2NE-Controls-SignaturePad-SignaturePadControl-Control').each(function (e, element) {
        $(element).K2NESignaturePad();
    });
});