/*Ajax Methods */
function BlockPost() {
    document.getElementById('BlockPostBack').value = true;
    document.getElementById('Action').value = 'No Action';
}
function PostBack() {
    document.getElementById('BlockPostBack').value = false;
}



function UnBlockPostNonAjaxWithParam(action, id) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById('BlockPostBack').value = false;
    document.getElementById("AjaxUpdate").value = ''
    document.getElementById("Action").value = action
    document.getElementById("Param").value = id
}

function Ajax(action) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action;
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWithAction2(action, action2) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action;
    document.getElementById("Action2").value = action2;
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWithParam(action, id) {
    debugger;
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action
    document.getElementById("Param").value = id
    document.getElementById('AjaxUpdate').value = true;
}

function AjaxWith2Params(action, param1, param2) {
    
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
   
    document.getElementById("Action").value = action
    document.getElementById("Param").value = param1
    document.getElementById("Param2").value = param2
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWith2ParamAndDate(action, param1, param2, dt) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action;
    document.getElementById("Param").value = param1;
    document.getElementById("Param2").value = param2;
    document.getElementById("ParamDate").value = dt;
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWithThreeParam(action, param1, param2, param3) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }

    document.getElementById("Action").value = action
    document.getElementById("Param").value = param1
    document.getElementById("Param2").value = param2
    document.getElementById("Param3").value = param3
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWithStringParam(action, id) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }

    document.getElementById("Action").value = action
    document.getElementById("StringParam").value = id
    document.getElementById("AjaxUpdate").value = true

}
function AjaxPartialUpdateWithParamAndStringParam(action, id, param, divtoupdate) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }

    document.getElementById("Action").value = action
    document.getElementById("StringParam").value = id
    document.getElementById("Param").value = param
    document.getElementById("AjaxUpdate").value = true
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}
function AjaxWithConfirmation(action, message) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    if (confirm(message)) {
        document.getElementById("Action").value = action;
    }
    else {
        document.getElementById("Action").value = 'Cancel';
    }
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWithParamAndConfirmation(action, param, message) {
    
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    if (confirm(message)) {
        document.getElementById("Action").value = action;
        document.getElementById("Param").value = param;
    }
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxPartialUpdate(action, divtoupdate) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action;
    document.getElementById('AjaxUpdate').value = true;
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}
function AjaxPartialUpdateWith2Param(action, param1, param2, divtoupdate) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action
    document.getElementById("Param").value = param1
    document.getElementById("Param2").value = param2
    document.getElementById('AjaxUpdate').value = true;
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}
function AjaxPartialUpdateWithConfirmation(action1, divtoupdate, confirmation) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    if (confirm(confirmation)) {
        document.getElementById("Action").value = action1;
    }
    document.getElementById('AjaxUpdate').value = true;
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}
function AjaxPartialUpdateWithParam(action, param, divtoupdate) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action;
    document.getElementById('AjaxUpdate').value = true;
    document.getElementById("Param").value = param;
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}
function AjaxWithParamAndDate(action, param, dt) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }

    document.getElementById("Action").value = action;
    document.getElementById("Param").value = param;
    document.getElementById("ParamDate").value = dt;
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxPartialUpdateWithParamAndDate(action, param, dt, divtoupdate) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }

    document.getElementById("Action").value = action;
    document.getElementById("Param").value = param;
    document.getElementById("ParamDate").value = dt;
    document.getElementById('AjaxUpdate').value = true;
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}
function AjaxPartialUpdateWithParamAndConfirmation(action, param, divtoupdate,confirmation) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
     if (confirm(confirmation)) {
        document.getElementById("Action").value = action;
    }

    document.getElementById('AjaxUpdate').value = true;
    document.getElementById("Param").value = param;
    if (document.getElementById('DivToUpdate') != null) {
        document.getElementById('DivToUpdate').value = "#" + divtoupdate;
    }
}

function AjaxWith3ParamAndDate(action, param1, param2, param3, dt) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action;
    document.getElementById("Param").value = param1;
    document.getElementById("Param2").value = param2;
    document.getElementById("Param3").value = param3;
    document.getElementById("ParamDate").value = dt;
    document.getElementById('AjaxUpdate').value = true;
}

function NonAjax(action) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("AjaxUpdate").value = ''
    document.getElementById("Action").value = action;
}
function NonAjaxWithParam(action, id) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("AjaxUpdate").value = ''
    document.getElementById("Action").value = action
    document.getElementById("Param").value = id
}


function NonAjaxWithStringParam(action, id) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("AjaxUpdate").value = ''
    document.getElementById("Action").value = action
    document.getElementById("StringParam").value = id
}
function NonAjaxWith2Params(action, param1, param2) {
    
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }

    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action
    document.getElementById("Param").value = param1
    document.getElementById("Param2").value = param2
    document.getElementById('AjaxUpdate').value = '';
}

function NonAjaxWith3Param(action, id, id2, id3) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("AjaxUpdate").value = ''
    document.getElementById("Action").value = action
    document.getElementById("Param").value = id
    document.getElementById("Param2").value = id2
    document.getElementById("Param3").value = id3
}
function NonAjaxWithStringParam(action, id) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("AjaxUpdate").value = ''
    document.getElementById("Action").value = action
    document.getElementById("StringParam").value = id
}
function NonAjaxWithConfirmation(action, message) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("AjaxUpdate").value = ''
    if (confirm(message)) {
        document.getElementById("Action").value = action;
    }
}
function NonAjaxWithParamAndConfirmation(action, param, message) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    if (confirm(message)) {
        document.getElementById("AjaxUpdate").value = ''
        document.getElementById("Action").value = action;
        document.getElementById("Param").value = param;
    }
    else {
        document.getElementById("Action").value = '';
        document.getElementById("AjaxUpdate").value = '';
    }
}
function NonAjaxWithDate(action, dt) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    document.getElementById("Action").value = action
    document.getElementById("ParamDate").value = dt
    document.getElementById('AjaxUpdate').value = ''
}
/*Helper Functions*/
function ShowPopup() {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    if (document.getElementById('popup') != null) {
        document.getElementById('popup').style.zIndex = '200';
    }
}
function ClosePopup() {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'none';
    }
    if (document.getElementById('popup') != null) {
        document.getElementById('popup').style.zIndex = '-10';
    }
}
function CloseModalPopup() {
    $('.modal-backdrop').remove();
    document.getElementById('body').style.cssText = "";
    document.getElementById('body').classList = "body";
}
function Hide(itemname) {
    document.getElementById(itemname).style.display = "none";
}
function Show(itemname) {
    document.getElementById(itemname).style.display = "";
}
function ShowHide(showitem, hideitems) {
    document.getElementById(showitem).style.display = "";
    var hideitemsarray = hideitems.split(",");
    for (i = 0; i < hideitemsarray.length; i++)
        document.getElementById(hideitemsarray[i]).style.display = "none";
}
function ShowIfChecked(checkboxid, showifcheckedid, showifuncheckedid) {
    var checkbox = document.getElementById(checkboxid);
    if (checkbox.checked) {
        document.getElementById(showifcheckedid).style.display = '';
        if (showifuncheckedid != '')
            document.getElementById(showifuncheckedid).style.display = 'none';
    }
    else {
        document.getElementById(showifcheckedid).style.display = 'none';
        if (showifuncheckedid != '')
            document.getElementById(showifuncheckedid).style.display = '';
    }
}
function ClickButton(buttonid) {
    document.getElementById(buttonid).click();
}
function showloading() {
    document.getElementById("loading").style.removeProperty("display");
}
function highlightcurrentpage(navigrationid) {
    if (document.getElementById(navigrationid) != null) {
        document.getElementById(navigrationid).className = "active";
        if (document.getElementById(navigrationid).parentElement.parentElement.parentElement.parentElement.className == "sidebar-dropdown") {
            document.getElementById(navigrationid).parentElement.parentElement.parentElement.style.display = "block";
            document.getElementById(navigrationid).parentElement.parentElement.parentElement.parentElement.className = "sidebar-dropdown active";
            document.getElementById(navigrationid).parentElement.parentElement.parentElement.parentElement.getElementsByTagName('a')[0].className = "space-between-sec active";
           
        }
    }
}
/* Validation */
function validaterequiredfield(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    if (document.getElementById(fieldname).value != "") {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validateradio(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    var radioselections = document.getElementsByClassName("requiredradio");
    for (x = 0; x < radioselections.length; x++) {
        if (radioselections[x].id == fieldname)
            if (radioselections[x].checked) {
                document.getElementById(fieldname + "_Invalid").style.display = "none";
                return true;
            }
    }
    document.getElementById(fieldname + "_Invalid").style.display = "inline";
    return false;
}
function validatenumeric(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    document.getElementById(fieldname).value = document.getElementById(fieldname).value.replace(/[^\d]/, '');
    if (document.getElementById(fieldname).value != "" && document.getElementById(fieldname).value != "0") {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validatedecimal(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    document.getElementById(fieldname).value = document.getElementById(fieldname).value.replace(/[^0-9.]/g, '');
    if (document.getElementById(fieldname).value != "" && document.getElementById(fieldname).value != "0.00" && document.getElementById(fieldname).value != "0") {    
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validatedropdown(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    if (document.getElementById(fieldname).value != "0" && document.getElementById(fieldname).value != "") {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validatezipcode(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    var zip = document.getElementById(fieldname).value;
    document.getElementById(fieldname).value = zip.replace(/[^\d]/, '');
    if (zip.length > 5) {
        document.getElementById(fieldname).value = zip.substring(0, 5);
    }
    if (zip.length == 5) {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validateyear(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    var year = document.getElementById(fieldname).value;
    document.getElementById(fieldname).value = year.replace(/[^\d]/, '');
    if (year.length > 4) {
        document.getElementById(fieldname).value = year.substring(0, 4);
    }
    if (year.length == 4) {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validatephone(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    formatphone(fieldname);
    if (document.getElementById(fieldname).value.length == 12) {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function formatphone(fieldname) {
    if (document.getElementById(fieldname) != null) {
        var phonenumber = document.getElementById(fieldname).value;
        var lastindexofphonenumber = phonenumber.length;
        phonenumber = phonenumber.replace(/[^\d]/, '');
        phonenumber = phonenumber.replace(/[^0-9]/g, '');
        if (phonenumber.length > 0)
            block1 = phonenumber.substring(0, 3);
        else
            block1 = '';
        if (block1.length == 3) {
            block1 = block1 + '-';
        }
        block2 = phonenumber.substring(3, 6);
        if (block2.length == 3) {
            block2 = block2 + '-';
        }
        block3 = phonenumber.substring(6, 10);
        phonenumber = block1 + block2 + block3;
        if (event != null)
            if (event.key == 'Backspace') {
                if (phonenumber.substring(phonenumber.length - 1, phonenumber.length) == '-') {
                    phonenumber = phonenumber.substring(0, phonenumber.length - 1);
                }
            }
        document.getElementById(fieldname).value = phonenumber;
    }
}
function validatetime(fieldname) {
    if (document.getElementById(fieldname + "_Hour") == null) {
        alert('Unable to locate ' + fieldname + "_Hour");
        return false;
    }
    if (document.getElementById(fieldname + "_Minute") == null) {
        alert('Unable to locate ' + fieldname + "_Minute");
        return false;
    }
    if (document.getElementById(fieldname + "_AMPM") == null) {
        alert('Unable to locate ' + fieldname + "_AMPM");
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    if (document.getElementById(fieldname + "_Hour").value != "" &&
        document.getElementById(fieldname + "_Minute").value != "" &&
        document.getElementById(fieldname + "_AMPM").value != "") {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validatepassword(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    var confirmpassword = fieldname.replace("Password", "ConfirmPassword");
    var password = fieldname;
    if (document.getElementById(confirmpassword) == null) {
        alert('Unable to locate ' + confirmpassword);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    if (document.getElementById(fieldname).value != "") {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
    }
    document.getElementById("PasswordsDoNotMatch").style.display = "none";
    if (document.getElementById(fieldname).value != "" && document.getElementById(confirmpassword).value != "" && document.getElementById(password).value != document.getElementById(confirmpassword).value) {
        document.getElementById("PasswordsDoNotMatch").style.display = "";
        return false;
    }
    if (document.getElementById(fieldname).value != "") {
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function validateconfirmpassword(fieldname) {
    if (document.getElementById(fieldname) == null) {
        alert('Unable to locate ' + fieldname);
        return false;
    }
    var password = fieldname.replace("ConfirmPassword", "Password");
    var confirmpassword = fieldname;
    if (document.getElementById(confirmpassword) == null) {
        alert('Unable to locate ' + confirmpassword);
        return false;
    }
    if (document.getElementById(fieldname + "_Invalid") == null) {
        alert('Unable to locate ' + fieldname + "_Invalid");
        return false;
    }
    if (document.getElementById(fieldname).value != "") {
        document.getElementById(fieldname + "_Invalid").style.display = "none";
    }
    document.getElementById("PasswordsDoNotMatch").style.display = "none";
    if (document.getElementById(fieldname).value != "" && document.getElementById(confirmpassword).value != "" && document.getElementById(password).value != document.getElementById(confirmpassword).value) {
        document.getElementById("PasswordsDoNotMatch").style.display = "";
        return false;
    }
    if (document.getElementById(fieldname).value != "") {
        return true;
    }
    else {
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}
function ValidateForm() {
    var valid = true;
    var validations = document.getElementsByClassName("required");
    for (i = 0; i < validations.length; i++) {
        var validatefieldname = validations[i].id.replace("_Invalid", "");
        var validatefield = document.getElementById(validatefieldname);
        if (validatefield == null)
            alert('Unable to locate ' + validatefieldname);
        else {
            for (j = 0; j < validatefield.classList.length; j++) {
                if (validatefield.classList[j] == "requiredfield")
                    if (validaterequiredfield(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requirednumeric")
                    if (validatenumeric(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requireddecimal")
                    if (validatedecimal(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requireddropdown") {
                    if (validatedropdown(validatefieldname) == false)
                        valid = false;
                }
                if (validatefield.classList[j] == "requiredphone")
                    if (validatephone(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requiredtime")
                    if (validatetime(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requiredradio")
                    if (validateradio(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requiredpassword")
                    if (validatepassword(validatefieldname) == false)
                        valid = false;
                if (validatefield.classList[j] == "requiredconfirmpassword")
                    if (validateconfirmpassword(validatefieldname) == false)
                        valid = false;
            }
        }
    }
    if (valid) {
        var buttons = document.getElementsByClassName("button");
        for (i = 0; i < buttons.length; i++) {
            if (buttons[i].id == "Valid")
                buttons[i].style.display = "inline";
        }
        if (document.getElementById("Invalid") != null)
            document.getElementById("Invalid").style.display = "none";
        ValidItems = document.getElementsByClassName("valid");
        for (i = 0; i < ValidItems.length; i++) {
            ValidItems[i].style.display = "";
        }
        InvalidItems = document.getElementsByClassName("invalid");
        for (i = 0; i < InvalidItems.length; i++) {
            InvalidItems[i].style.display = "none";
        }
        InvalidItems = document.getElementsByClassName("validation-message");
        for (i = 0; i < InvalidItems.length; i++) {
            InvalidItems[i].style.display = "none";
        }
    }
    else {
        var buttons = document.getElementsByClassName("button");
        for (i = 0; i < buttons.length; i++) {
            if (buttons[i].id == "Valid") {
                buttons[i].style.display = "none";
            }
        }
        if (document.getElementById("Invalid") != null)
            document.getElementById("Invalid").style.display = "";
        ValidItems = document.getElementsByClassName("valid");
        for (i = 0; i < ValidItems.length; i++) {
            ValidItems[i].style.display = "none";
        }
        InvalidItems = document.getElementsByClassName("invalid");
        for (i = 0; i < InvalidItems.length; i++) {
            InvalidItems[i].style.display = "";
        }
        InvalidItems = document.getElementsByClassName("validation-message");
        for (i = 0; i < InvalidItems.length; i++) {
            InvalidItems[i].style.display = "";
        }
    }
}
function validatecreditcard() {
    document.getElementById('CreditCard_NameOnCard').value = document.getElementById('NameOnCard').value;
    document.getElementById('CreditCard_CCNumber').value = document.getElementById('CCNumber').value;
    document.getElementById('CreditCard_CVV').value = document.getElementById('CVV').value;
    document.getElementById('CreditCard_ExpirationYear').value = document.getElementById('ExpirationYear').value;
    document.getElementById('CreditCard_ExpirationMonth').value = document.getElementById('ExpirationMonth').value;
    document.getElementById('CreditCard_ZipCode').value = document.getElementById('ZipCode').value;
    var validamount = true;
    if (document.getElementById('Amount') != null) {
        document.getElementById('CreditCard_Amount').value = document.getElementById('Amount').value;
        validamount = false;
        document.getElementById('Amount').value = document.getElementById('Amount').value.replace(/[^0-9.]/g, '');
        if (document.getElementById('Amount').value != "" && document.getElementById('Amount').value != "0.00") {
            document.getElementById('Amount_Invalid').style.display = "none";
            validamount = true;
        }
        else {
            document.getElementById(fieldname + "Amount_Invalid").style.display = "";
        }
    }
    var validnameoncard = false;
    if (document.getElementById('NameOnCard').value != "") {
        document.getElementById('NameOnCard_Invalid').style.display = "none";
        validnameoncard = true;
    }
    else {
        document.getElementById('NameOnCard_Invalid').style.display = "";
    }
    var validzipcode = true;
    var zip = document.getElementById('ZipCode').value;
    document.getElementById('ZipCode').value = zip.replace(/[^\d]/, '');
    if (zip.length > 5) {
        document.getElementById('ZipCode').value = zip.substring(0, 5);
    }
    if (zip.length == 5) {
        document.getElementById("ZipCode_Invalid").style.display = "none";
    }
    else {
        document.getElementById("ZipCode_Invalid").style.display = "";
        validzipcode = false;
    }
    var validcard = false;
    var cardnumber = document.getElementById('CCNumberDisplay').value;
    cardnumber = cardnumber.replace(/[^\d]/, '');
    cardnumber = cardnumber.replace(/[^0-9]/g, '');
    document.getElementById('CCNumber').value = cardnumber;
    var cvv = document.getElementById('CVV').value;
    cvv = cvv.replace(/[^\d]/, '');
    var creditcardtype = '';
    if (cardnumber.length > 4) {
        if (cardnumber.substring(0, 2) == '34') creditcardtype = 'American Express';
        else if (cardnumber.substring(0, 2) == '37') creditcardtype = 'American Express';
        else if (cardnumber.substring(0, 1) == '4') creditcardtype = 'Visa';
        else if (cardnumber.substring(0, 2) == '51') creditcardtype = 'Master Card';
        else if (cardnumber.substring(0, 2) == '52') creditcardtype = 'Master Card';
        else if (cardnumber.substring(0, 2) == '53') creditcardtype = 'Master Card';
        else if (cardnumber.substring(0, 2) == '54') creditcardtype = 'Master Card';
        else if (cardnumber.substring(0, 2) == '55') creditcardtype = 'Master Card';
        else if (cardnumber.substring(0, 4) == '6011') creditcardtype = 'Discover';
        if (document.getElementById('CreditCard_CardType') != null)
            document.getElementById('CreditCard_CardType').value = creditcardtype;
    }
    var block1 = '';
    var block2 = '';
    var block3 = '';
    var block4 = '';
    if (cardnumber.length > 0)
        block1 = cardnumber.substring(0, 4);
    else
        block1 = '';
    if (block1.length == 4) {
        block1 = block1 + ' ';
    }
    if (creditcardtype == 'Visa' || creditcardtype == 'Master Card' || creditcardtype == 'Discover') {
        // for 4X4 cards
        block2 = cardnumber.substring(4, 8);
        if (block2.length == 4) {
            block2 = block2 + ' ';
        }
        block3 = cardnumber.substring(8, 12);
        if (block3.length == 4) {
            block3 = block3 + ' ';
        }
        block4 = cardnumber.substring(12, 16);
    }
    else if (creditcardtype == 'American Express') {
        // for Amex cards
        block2 = cardnumber.substring(4, 10);
        if (block2.length == 6) {
            block2 = block2 + ' ';
        }
        block3 = cardnumber.substring(10, 15);
        block4 = '';
    }
    cardnumber = block1 + block2 + block3 + block4;
    if (event != null)
        if (field = 'cc')
            if (event.key == 'Backspace')
                if (cardnumber.substring(cardnumber.length - 1, cardnumber.length) == ' ')
                    cardnumber = cardnumber.substring(0, cardnumber.length - 1);
    document.getElementById('CCNumberDisplay').value = cardnumber;
    cardnumber = cardnumber.replace(/[^\d]/, '');
    cardnumber = cardnumber.replace(/[^0-9]/g, '');
    document.getElementById('CCNumber').value = cardnumber;
    if (creditcardtype == 'Visa') {
        var cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        if (cardnumber.match(cardno)) {
            document.getElementById('CCNumber_Invalid').style.display = "none";
            validcard = true;
        }
        else {
            document.getElementById('CCNumber_Invalid').style.display = "";
        }
    }
    else if (creditcardtype == 'Master Card') {
        var cardno = /^(?:5[1-5][0-9]{14})$/;
        if (cardnumber.match(cardno)) {
            document.getElementById('CCNumber_Invalid').style.display = "none";
            validcard = true;
        }
        else {
            document.getElementById('CCNumber_Invalid').style.display = "";
        }
    }
    else if (creditcardtype == 'American Express') {
        var cardno = /^(?:3[47][0-9]{13})$/;
        if (cardnumber.match(cardno)) {
            validcard = true;
            document.getElementById('CCNumber_Invalid').style.display = "none";
        }
        else {
            document.getElementById('CCNumber_Invalid').style.display = "";
        }
    }
    else if (creditcardtype == 'Discover') {
        var cardno = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
        if (cardnumber.match(cardno)) {
            validcard = true;
            document.getElementById('CCNumber_Invalid').style.display = "none";
        }
        else {
            document.getElementById('CCNumber_Invalid').style.display = "";
        }
    }
    else {
        document.getElementById('CCNumber_Invalid').style.display = "";
    }
    //cvv Validation
    var validcvv = true;
    if (creditcardtype == 'American Express') {
        if (cvv.length > 4) {
            document.getElementById('CVV').value = cvv.substring(0, 3);
        }
        if (document.getElementById('CVV').value.length == 4) {
            document.getElementById('CVV_Invalid').style.display = "none";
        }
        else {
            validcvv = false;
            document.getElementById('CVV_Invalid').style.display = "";
        }
    }
    else {
        if (cvv.length > 3) {
            document.getElementById('CVV').value = cvv.substring(0, 3);
        }
        if (document.getElementById('CVV').value.length == 3) {
            document.getElementById('CVV_Invalid').style.display = "none";
        }
        else {
            validcvv = false;
            document.getElementById('CVV_Invalid').style.display = "";
        }
    }
    var validexpiration = true;
    if (document.getElementById('ExpirationMonth').value == "")
        validexpiration = false;
    if (document.getElementById('ExpirationYear').value == "")
        validexpiration = false;
    if (validexpiration)
        document.getElementById('ExpirationYear_Invalid').style.display = "none";
    else
        document.getElementById('ExpirationYear_Invalid').style.display = "";
    if (validnameoncard && validzipcode && validcard && validcvv && validexpiration && validamount) {
        document.getElementById('creditcard_Invalid').style.display = "none";
        document.getElementById('creditcard_Valid').style.display = "";
    }
    else {
        document.getElementById('creditcard_Invalid').style.display = "";
        document.getElementById('creditcard_Valid').style.display = "none";
    }
}


function validateACH() {

    document.getElementById('ACHPayment_NameOnAccount').value = document.getElementById('NameOnAccount').value;
    document.getElementById('ACHPayment_RoutingNumber').value = document.getElementById('RoutingNumber').value;
    document.getElementById('ACHPayment_AccountNumber').value = document.getElementById('AccountNumber').value;
    var validamount = true;
    if (document.getElementById('Amount') != null) {
        document.getElementById('ACHPayment_Amount').value = document.getElementById('Amount').value;
        validamount = false;
        document.getElementById('Amount').value = document.getElementById('Amount').value.replace(/[^0-9.]/g, '');
        if (document.getElementById('Amount').value != "" && document.getElementById('Amount').value != "0.00") {
            document.getElementById('Amount_Invalid').style.display = "none";
            validamount = true;
        }
        else {
            document.getElementById(fieldname + "Amount_Invalid").style.display = "";
        }
    }
    var validnameonaccount = false;
    if (document.getElementById('NameOnAccount').value != "") {
        document.getElementById('NameOnAccount_Invalid').style.display = "none";
        validnameonaccount = true;
    }
    else {
        document.getElementById('NameOnAccount_Invalid').style.display = "";
    }

    var validroutingnumber = false;
    if (document.getElementById('RoutingNumber').value != "") {
        document.getElementById('RoutingNumber_Invalid').style.display = "none";
        validroutingnumber = true;
    }
    else {
        document.getElementById('RoutingNumber_Invalid').style.display = "";
    }

    var validaccountnumber = false;
    if (document.getElementById('AccountNumber').value != "") {
        document.getElementById('AccountNumber_Invalid').style.display = "none";
        validaccountnumber = true;
    }
    else {
        document.getElementById('AccountNumber_Invalid').style.display = "";
    }

    if (validnameonaccount && validroutingnumber && validaccountnumber) {
        document.getElementById('ach_Invalid').style.display = "none";
        document.getElementById('ach_Valid').style.display = "";
    }
    else {
        document.getElementById('ach_Invalid').style.display = "";
        document.getElementById('ach_Valid').style.display = "none";
    }
}

function showprocessingcard() {
    document.getElementById('processingcard').style.display = "";
    document.getElementById('submitpaymentbuttons').style.display = "none";
}
function AjaxWithFileUploadCheckConfirmations(action, message, BrowsebtnId) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    // Check if a file has been selected
    var browsebtn = document.getElementById(BrowsebtnId);
    if (browsebtn && browsebtn.files.length === 0) {
        alert("Please select a file first."); // You can replace this with a more sophisticated message or a popup.
    }
    else if (confirm(message)) {
        document.getElementById("Action").value = action;
    }
    document.getElementById('AjaxUpdate').value = true;
}
function AjaxWithMultipleFileUploadCheckConfirmations(action, message, firstBrowsebtnId, secondbrowsebtnId) {
    if (document.getElementById('popupbackground') != null) {
        document.getElementById('popupbackground').style.display = 'inline';
    }
    // Check if a file has been selected
    var firstbrowsebtn = document.getElementById(firstBrowsebtnId);
    var secondbrowsebtn = document.getElementById(secondbrowsebtnId);
    if ((firstbrowsebtn && firstbrowsebtn.files.length !== 0) || (secondbrowsebtn && secondbrowsebtn.files.length !== 0)) {
        if (confirm(message)) {
            document.getElementById("Action").value = action;
        }
        // You can replace this with a more sophisticated message or a popup.
    }
    else {
        alert("Please select a file first.");
    }
    document.getElementById('AjaxUpdate').value = true;
}
function ShowBrowseButtonByBtnId(BrowsebtnId) {
    var browsebtn = document.getElementById(BrowsebtnId);
    browsebtn.style.visibility = "visible";
}
function HideBrowseButtonByBtnId(BrowsebtnId) {
    var browsebtn = document.getElementById(BrowsebtnId);
    browsebtn.style.visibility = "hidden";
}
function AutoCompleteSuggestions(inp, arr) {
    
    var currentFocus;
    if (inp != null) {
        if (inp[0] != null) {
            inp[0].addEventListener("input", function (e) {
                var val = this.value;
                closeAllLists();
                if (!val) { return false; }
                currentFocus = -1;
                var a = document.createElement("DIV");
                a.setAttribute("id", this.id + "autocomplete-list");
                a.setAttribute("class", "autocomplete-items");
                this.parentNode.appendChild(a);
                for (var i = 0; i < arr.length; i++) {
                    var studentname = arr[i].toUpperCase();
                    if (studentname.includes(val.toUpperCase())) {
                        var b = document.createElement("DIV");
                        b.innerHTML = arr[i].replace(new RegExp(val, "gi"), "<strong>$&</strong>");
                        b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                        b.addEventListener("click", function (e) {
                            inp[0].value = this.getElementsByTagName("input")[0].value;
                            document.getElementById("btnfilter").click();
                            closeAllLists();
                        });
                        a.appendChild(b);
                    }
                }
            });
            inp[0].addEventListener("keydown", function (e) {
                var x = document.getElementById(this.id + "autocomplete-list");
                if (x) x = x.getElementsByTagName("div");
                if (e.keyCode == 40) {
                    currentFocus++;
                    addActive(x);
                } else if (e.keyCode == 38) {
                    currentFocus--;
                    addActive(x);
                } else if (e.keyCode == 13) {
                    e.preventDefault();
                    if (currentFocus > -1) {
                        if (x) x[currentFocus].click();
                    }
                }
            });
            function addActive(x) {
                if (!x) return false;
                removeActive(x);
                if (currentFocus >= x.length) currentFocus = 0;
                if (currentFocus < 0) currentFocus = (x.length - 1);
                x[currentFocus].classList.add("autocomplete-active");
            }
            function removeActive(x) {
                for (var i = 0; i < x.length; i++) {
                    x[i].classList.remove("autocomplete-active");
                }
            }
            function closeAllLists(elmnt) {
                var x = document.getElementsByClassName("autocomplete-items");
                for (var i = 0; i < x.length; i++) {
                    if (elmnt != x[i] && elmnt != inp) {
                        x[i].parentNode.removeChild(x[i]);
                    }
                }
            }
            document.addEventListener("click", function (e) {
                closeAllLists(e.target);
            });
        }
    }
}
function SelectMultiSelectItem(DivId) {
    var selecteditems = "";
    for (var i = 0; i < document.getElementById(DivId).children[1].children.length; i++) {
        var listitem = document.getElementById(DivId).children[1].children[i];
        if (listitem.children[0].children[0].checked) {
            if (selecteditems != "")
                selecteditems += ", ";
            selecteditems += listitem.children[1].innerHTML.trim();
        }
        if (selecteditems != "")
            document.getElementById(DivId).children[0].innerHTML = selecteditems;
        else
            document.getElementById(DivId).children[0].innerHTML = "Select Items";
    }
}
function OpenMultiSelectDropdown(Id, templateGroupId) {
    var $templateGroup = $('#' + templateGroupId);
    var $id = $('#' + Id);
    if ($templateGroup.length && $id.length) {
        if ($templateGroup.is(":hidden")) {
            $id.off('click').on('click', function () {
                $(this).addClass('visible').focus();
            });
        } else {
            setTimeout(function () {
                $id.removeClass('visible');
            }, 100);
        }
    }
}

function CloseMultiSelectDropdown(Id) {
     var $templateGroup = $('#' + templateGroupId);
    var $id = $('#' + Id);
    if ($templateGroup.length && $id.length) {
        if ($templateGroup.is(":hidden")) {
            $id.off('click').on('click', function () {
                $(this).addClass('visible').focus();
            });
        } else {
            setTimeout(function () {
                $id.removeClass('visible');
            }, 100);
        }
    }
}

//function AutoCompleteSuggestions(inp, arr) {
//    var currentFocus;
//    inp[0].addEventListener("input", function (e) {
//        var val = this.value;
//        closeAllLists();
//        if (!val) { return false; }
//        currentFocus = -1;
//        var a = document.createElement("DIV");
//        a.setAttribute("id", this.id + "autocomplete-list");
//        a.setAttribute("class", "autocomplete-items");
//        this.parentNode.appendChild(a);
//        for (var i = 0; i < arr.length; i++) {
//            var studentname = arr[i].toUpperCase();
//            if (studentname.includes(val.toUpperCase())) {
//                var b = document.createElement("DIV");
//                b.innerHTML = arr[i].replace(new RegExp(val, "gi"), "<strong>$&</strong>");
//                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
//                b.addEventListener("click", function (e) {
//                    inp[0].value = this.getElementsByTagName("input")[0].value;
//                    document.getElementById("btnfilter").click();
//                    closeAllLists();
//                });
//                a.appendChild(b);
//            }
//        }
//    });
//    inp[0].addEventListener("keydown", function (e) {
//        var x = document.getElementById(this.id + "autocomplete-list");
//        if (x) x = x.getElementsByTagName("div");
//        if (e.keyCode == 40) {
//            currentFocus++;
//            addActive(x);
//        } else if (e.keyCode == 38) {
//            currentFocus--;
//            addActive(x);
//        } else if (e.keyCode == 13) {
//            e.preventDefault();
//            if (currentFocus > -1) {
//                if (x) x[currentFocus].click();
//            }
//        }
//    });
//    function addActive(x) {
//        if (!x) return false;
//        removeActive(x);
//        if (currentFocus >= x.length) currentFocus = 0;
//        if (currentFocus < 0) currentFocus = (x.length - 1);
//        x[currentFocus].classList.add("autocomplete-active");
//    }
//    function removeActive(x) {
//        for (var i = 0; i < x.length; i++) {
//            x[i].classList.remove("autocomplete-active");
//        }
//    }
//    function closeAllLists(elmnt) {
//        var x = document.getElementsByClassName("autocomplete-items");
//        for (var i = 0; i < x.length; i++) {
//            if (elmnt != x[i] && elmnt != inp) {
//                x[i].parentNode.removeChild(x[i]);
//            }
//        }
//    }
//    document.addEventListener("click", function (e) {
//        closeAllLists(e.target);
//    });
//}