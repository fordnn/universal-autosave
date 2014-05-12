var uaAnonymousGUID = "uaAnonymousGUID";

function uaGetUniqID(objVal)
{
    var uniqID = "#" + objVal.attr("id");
    if ((uniqID == "") || (uniqID == null) || (uniqID.indexOf("undefined") != -1))
    {
        uniqID = "[name=\"" + objVal.attr("name") + "\"]";
    }
    if ((uniqID == null) || (uniqID.indexOf("undefined") != -1))
    {
        uniqID = "";
    }
    return uniqID;
}

function uaProcessConfig(uaParams)
{
    var lstControls = $("input[type=\"text\"],input[type=\"radio\"],input[type=\"checkbox\"],textarea,select,a,input[type=\"submit\"]");
    if (lstControls.length == 0)
    {
        return;
    }

    $("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + uaParams.path + "ua.css\">").appendTo("head");

    $.each(lstControls,
		function (ind, val)
		{
		    var objVal = $(val);
		    if (objVal.hasClass("uaWizardButton"))
		    {
		        return;
		    }
		    var className = (uaParams.config.AutosaveIcon) ? "uaConfig" : "uaConfig uaHidden";
		    var objType = objVal[0].nodeName + ((objVal.attr("type") == null) ? "" : ("[" + objVal.attr("type") + "]"));
		    var isEvent = (objType.toLowerCase() == "a") || (objType.toLowerCase() == "input[type=\"submit\"]");

		    var uniqID = uaGetUniqID(objVal);
		    if ((uniqID == "") || (uniqID == null))
		    {
		        //nothing to do with element we can't identify (for now - in this version)
		        return;
		    }

		    if (isEvent)
		    {
		        $.each(uaParams.events, function (ind, val)
		        {
		            if ((val.Selector == uniqID) && (val.Enabled))
		            {
		                className += "Selected";
		            }
		        });
		    }
		    else
		    {
		        $.each(uaParams.controls, function (ind, val)
		        {
		            if ((val.Selector == uniqID) && (val.Enabled))
		            {
		                className += "Selected";
		            }
		        }
                );
		    }

		    objVal.after("<div class=\"uaButton\"><a href=\"#\" class=\"" + className + "\" ua=\"" + uniqID + "\"></a></div>");
		}
	);

    $("a[ua]").click(function () { return uaConfigClick(uaParams, this); });
}

function uaProcess(uaParams)
{
    $.each(uaParams.events, function (ind, val)
    {
        val.isEvent = true;
    });

    var lstControls = uaParams.controls.concat(uaParams.events);
    if (lstControls.length == 0)
    {
        return;
    }

    var anon = dnn.dom.getCookie(uaAnonymousGUID);
    if ((uaParams.anonymousGUID != "") && (anon == null))
    {
        dnn.dom.setCookie(uaAnonymousGUID, uaParams.anonymousGUID, 365, "/");
    }

    $("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + uaParams.path + "ua.css\">").appendTo("head");

    $.each(lstControls,
		function (ind, val)
		{
		    var objVal = $(val.Selector);
		    var className = (uaParams.config.AutosaveIcon) ? "uaWork" : "uaWork uaHidden";

		    if ((val.RestoreOnLoad) && (val.Enabled) && (!val.isEvent))
		    {
		        //restore values
		        if (val.RestoreIfEmpty)
		        {
		            if ($(val.Selector).val() == "")
		            {
		                $(val.Selector).val(val.Value);
		            }
		        }
		        else
		        {
		            $(val.Selector).val(val.Value);
		        }
		        objVal.after("<div class=\"uaButton\"><a href=\"#\" class=\"" + className + "\" ua=\"" + val.Selector + "\"></a></div>");
		    }

		    if ((val.isEvent) && (val.Enabled))
		    {
		        $(val.Selector).click(function () { uaCloseSession(uaParams); });
		    }
		}
	);

    //handler for dialog with values
    $("a[ua]").click(function () { return uaWorkClick(uaParams, this); });

    //Handler for controls && autosave onBlur handler
    var lstTrackControls = new Array();
    $.each($("a[ua]"),
            function (ind, val)
            {
                var objCtl = $($(val).attr("ua"));
                objCtl.attr("uaOldValue", objCtl.val());
                lstTrackControls.push(objCtl);
                if (uaParams.config.AutosaveOnBlur)
                {
                    objCtl.blur(function () { uaTrackChanges(uaParams, this); });
                }
            }
     );

    //Autosave interval handler
    if ((uaParams.config.AutosavePeriod != 0) && (lstTrackControls.length > 0))
    {
        uaParams.intervalID =
                setInterval(
                    function ()
                    {
                        $.each(lstTrackControls,
                            function (ind, val)
                            {
                                uaTrackChanges(uaParams, this);
                            }
                        );
                    },
                    uaParams.config.AutosavePeriod * 1000);
    }
}

function uaInit(uaParams)
{
    $(document).ready(
		function ()
		{
		    if (uaParams.config.ConfigurationMode)
		    {
		        uaProcessConfig(uaParams);
		    }
		    else
		    {
		        uaProcess(uaParams);
		    }
		}
	);
}

function uaTrackChanges(uaParams, src)
{
    var objThis = $(src);
    var isModified = false;

    if (objThis.attr("uaOldValue") == null)
    {
        if (objThis.val() != "")
        {
            isModified = true;
            objThis.attr("uaOldValue", "");
        }
    }
    else
    {
        if ((objThis.attr("uaOldValue") != objThis.val()) && (objThis.val() != ""))
        {
            isModified = true;
        }
    }

    if (isModified)
    {
        var uniqID = uaGetUniqID(objThis);
        var objLink = $('a[ua="' + uniqID + '"]');
        objLink.addClass("uaProgress");

        var objSend = new Object();
        objSend.configurationID = uaParams.config.ConfigurationID;
        objSend.action = "trackChanges";
        objSend.value = objThis.val();
        objSend.selector = uniqID;
        objSend.anonymousGUID = dnn.dom.getCookie(uaAnonymousGUID);
        objSend.urlID = uaParams.urlID;

        $.ajax({
            type: "POST",
            async: true,
            url: uaParams.path + "UAHandler.aspx",
            data: objSend,
            contentType: "application/json",
            dataType: "json",
            success: function (msg)
            {
                if (msg != "1")
                {
                    alert(msg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown)
            {
                alert(textStatus);
            }
        });

        objThis.attr("uaOldValue", objThis.val());

        objLink.removeClass("uaProgress");
    }
}

function uaWorkClick(uaParams, src)
{
    var objThis = $(src);
    $("#uaPopup").dialog(
	{
	    modal: true,
	    closeOnEscape: true,
	    width: "75%",
	    height: 300,
	    title: $("#uaPopup span.uaHidden").html(),
	});

    uaGetHistory(uaParams, src);

    return false;
}

function uaGetHistory(uaParams, src)
{
    $("#uaPopup div").html("");
    $("#uaPopup span.NormalRed").show();

    var objSend = new Object();
    objSend.configurationID = uaParams.config.ConfigurationID;
    objSend.action = "getHistory";
    objSend.selector = uaGetUniqID($($(src).attr("ua")));
    objSend.anonymousGUID = dnn.dom.getCookie(uaAnonymousGUID);
    objSend.urlID = uaParams.urlID;

    $.ajax({
        type: "POST",
        async: true,
        url: uaParams.path + "UAHandler.aspx",
        data: objSend,
        contentType: "application/json",
        dataType: "text",
        success: function (msg)
        {
            $("#uaPopup span.NormalRed").hide();
            $("#uaPopup div").html(msg);
            $("#uaPopup div table tr td.value").parent().click(function () { uaRestoreValue(this); });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown)
        {
            alert(textStatus);
        }
    });
}

function uaRestoreValue(src)
{
    var objTd = $(src);
    if (objTd.length != 0)
    {
        $(objTd.attr("ua")).val(objTd.find("td.value").html()).focus();
        $("#uaPopup").dialog("close");
    }
}

function uaConfigClick(uaParams, src)
{
    var objThis = $(src);
    var action = (objThis.hasClass("uaConfigSelected")) ? "removeControl" : "addControl";
    var objSelector = $(objThis.attr("ua"));
    var objType = objSelector[0].nodeName + ((objSelector.attr("type") == null) ? "" : ("[" + objSelector.attr("type") + "]"));
    objThis.addClass("uaProgress");

    $.get(
		uaParams.path + "UAHandler.aspx",
		{ configurationID: uaParams.config.ConfigurationID, action: action, selector: objThis.attr("ua"), type: objType },
		function (data)
		{
		    objThis.removeClass("uaProgress");
		    if (data == "1")
		    {
		        if (action == "removeControl")
		        {
		            objThis.removeClass("uaConfigSelected").addClass("uaConfig");
		        }
		        else
		        {
		            objThis.removeClass("uaConfig").addClass("uaConfigSelected");
		        }
		    }
		    else
		    {
		        alert(data);
		    }
		}
	);
    return false;
}

function uaCloseSession(uaParams)
{
    var objSend = new Object();
    objSend.configurationID = uaParams.config.ConfigurationID;
    objSend.action = "closeSession";
    objSend.anonymousGUID = dnn.dom.getCookie(uaAnonymousGUID);
    objSend.urlID = uaParams.urlID;

    $.ajax({
        type: "POST",
        async: true,
        url: uaParams.path + "UAHandler.aspx",
        data: objSend,
        contentType: "application/json",
        dataType: "text",
        success: function (msg)
        {
            if (msg != "1")
            {
                alert(msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown)
        {
        }
    });
}