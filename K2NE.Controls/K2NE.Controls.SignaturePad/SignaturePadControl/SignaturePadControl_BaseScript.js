(function ($) {
    if (typeof K2NE === 'undefined' || K2NE === null) K2NE = {};
    if (typeof K2NE.Controls === 'undefined' || K2NE.Controls === null) K2NE.Controls = {};
    if (typeof K2NE.Controls.SignaturePad === 'undefined' || K2NE.Controls.SignaturePad === null) K2NE.Controls.SignaturePad = {};
    /**
    * The base class which is inherited by all js objects of all chart controls.
    * It contains reusable methods, used by all Chart controls.
    */
    K2NE.Controls.SignaturePad.BaseType =
	{
	    _getInstance: function (id) {
	        //alert("_getInstance(" + id + ")");
	        var control = $('#' + id);
	        if (control.length == 0) {
	            throw 'PivotControl \'' + id + '\' not found';
	        } else {
	            return control[0];
	        }
	    },
	    //Designer Functions
	    /**
        * Set the width in the designer pane
        * @param {Object} wrapper - The object, which contains the control in the designer
        * @param {Object} width - The value of the width.
        * @param {Object} controlXmlText - The xml properties of the control.
        */
	    _setWidth: function (wrapper, width, controlXmlText) {
	        width = ("" + width).trim();
	        var isPercentage = width.length > 0 && width.charAt(width.length - 1) === '%';
	        var resizeWrappers = wrapper.find('.resizewrapper');
	        var ctr = wrapper.find("[class^='SFC K2NE-Controls-SignaturePad-SignaturePadControl-Control']").first();
	        if (width === "") {
	            //set the display value to allow the wrapper to wrap the content
	            //reset the wrappers to adjust to the content
	            if (wrapper.length > 0) {
	                wrapper[0].style.width = "";
	            };
	            resizeWrappers.each(function () {
	                $(this).css('width', "100%");
	            });

	            $(ctr).css('width', '100%');
	        }
	        else {

	            if (isPercentage) {
	                if (wrapper.length > 0) {
	                    $(wrapper[0]).css('width', width);
	                };
	                resizeWrappers.each(function () {
	                    $(this).css('width', "100%");
	                });
	                $(ctr).css('width', "100%");
	            } else {
	                if (wrapper.length > 0) {
	                    $(wrapper[0]).css('width', width);
	                };
	                resizeWrappers.each(function () {
	                    $(this).css('width', "100%");
	                });
	                $(ctr).css('width', "100%");
	            }
	        }
	        return wrapper.outerWidth(true);
	    },
	    /**
        * Returns the actual width of the control.
        * @param {Object} wrapper - The object, which contains the control in the designer
        */
	    _getWidth: function (wrapper) {
	        var el = wrapper.find("[class^='SFC K2NE-Controls-SignaturePad-SignaturePadControl-Control']").first();
	        var width = el.outerWidth(true);
	        if (width > 0) {
	            width += "px";
	        }
	        else {
	            width = "";
	        }
	        return width;
	    },
	    /**
        * Set the height in the designer pane
        * @param {Object} wrapper - The object, which contains the control in the designer
        * @param {Object} height - The value of the height.
        * @param {Object} controlXmlText - The xml properties of the control.
        */
	    _setHeight: function (wrapper, height, controlSetProperties) {
	        height = ("" + height).trim();
	        var resizeWrappers = wrapper.find('.resizewrapper');
	        var ctr = wrapper.find("[class^='SFC K2NE-Controls-SignaturePad-SignaturePadControl-Control']").first();
	        if (height === "") {
	            //set the display value to allow the wrapper to wrap the content
	            //reset the wrappers to adjust to the content
	            if (wrapper.length > 0) {
	                wrapper[0].style.height = "100%";
	            }
	            resizeWrappers.each(function () {
	                this.style.height = "100%";
	            });

	            $(ctr).css('height', 'auto');
	        }
	        else {
	            if (wrapper.length > 0) {
	                wrapper[0].style.height = "";
	            }
	            resizeWrappers.each(function () {
	                this.style.height = "";
	            });


	            $(ctr).css('height', height);
	        }
	        return ctr.outerHeight(true);
	    },
	    /**
        * Returns the actual height of the control.
        * @param {Object} wrapper - The object, which contains the control in the designer
        */
	    _getHeight: function (wrapper) {
	        var el = wrapper.find("[class^='SFC K2NE-Controls-SignaturePad-SignaturePadControl-Control']").first();
	        var height = el.outerHeight(true);
	        if (height > 0) {
	            height += "px";
	        }
	        else {
	            height = "";
	        }
	        return height;
	    },
	    /**
        * Changes visibility of the control
        * @param {Object} wrapper - The object, which contains the control in the designer
        * @param {bool} isVisible - If the control must be visible or not.
        */
	    _setVisible: function (wrapper, isVisible) {
	        typeof (isVisible) === 'string' && (isVisible = isVisible.toLowerCase() === 'true');
	        wrapper.toggleClass('invisible', !isVisible);
	    },
	    /**
        * Enables or Disables the control
        * @param {Object} wrapper - The object, which contains the control in the designer
        * @param {bool} isEnabled - Value of the enabled attribute.
        */
	    _setEnabled: function (wrapper, isEnabled) {
	        typeof (isEnabled) === 'string' && (isEnabled = isEnabled.toLowerCase() === 'true');
	        var _isFormControl = wrapper.hasClass('form-control');
	        var target = _isFormControl ? wrapper.children() : wrapper.find(".resizewrapper :not(.controloverlay):not(.ui-resizable-handle)");
	        target.first().toggleClass('disabled', !isEnabled);
	    },
	    //End Designer Functions

	    /**
        * Set styles for the control
        * @param {Object} wrapper - The object, which contains the control in the designer
        * @param {Object} styles - The object with all styles values.
        * @param {Object} target - The object, which contains the control in the runtime.
        */
	    _setStyles: function (wrapper, styles, target) {
	        var isRuntime = (wrapper == null);
	        var options = {};
	        var element = isRuntime ? jQuery(target) : wrapper.find("[class^='SFC K2NE-Controls-SignaturePad-SignaturePadControl-Control']");

	        jQuery.extend(options, {
	            "border": element,
	            "background": element,
	            "margin": element,
	            "padding": element,
	            "font": element,
	            "horizontalAlign": element
	        });

	        StyleHelper.setStyles(options, styles);
	    },
	    /**
        * Returns an array with random colors in HEX format
        * @param {int} num - Length of the array
        */
	    _getRandomPalette: function (num) {
	        var letters = '0123456789ABCDEF';
	        var palette = [];
	        for (var n = 0; n < num; n++) {
	            var color = '#';
	            for (var i = 0; i < 6; i++) {
	                color += letters[Math.floor(Math.random() * 16)];
	            }
	            palette.push(color);
	        }
	        return palette;
	    },

	};


}(jQuery));