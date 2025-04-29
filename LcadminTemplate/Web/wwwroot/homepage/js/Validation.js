function validaterequiredfield(fieldname) {

    if (document.getElementById(fieldname).value != "") {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }

}

function validatephone(fieldname) {
    var validphone = true;
    if (document.getElementById(fieldname + '1').value.length != 3) {
        validphone = false;
    }

    if (document.getElementById(fieldname + '2').value.length != 3) {
        validphone = false;
    }

    if (document.getElementById(fieldname + '3').value.length != 4) {
        validphone = false;
    }

    if (validphone) {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}

function validatenumeric(fieldname) {
    document.getElementById(fieldname).value = document.getElementById(fieldname).value.replace(/[^\d]/, '');
    if (document.getElementById(fieldname).value != "" && document.getElementById(fieldname).value != "0") {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}

function validatedropdown(fieldname) {
    if (document.getElementById(fieldname).value != "0") {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}

function validatelicenseplate(fieldname) {

    document.getElementById(fieldname).value = document.getElementById(fieldname).value.toUpperCase();

    if (document.getElementById(fieldname).value.length >= 5) {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}


function validatezipcode(fieldname) {
    var zip = document.getElementById(fieldname).value;
    document.getElementById(fieldname).value = zip.replace(/[^\d]/, '');
    if (zip.length > 5) {
        document.getElementById(fieldname).value = zip.substring(0, 5);
    }

    if (zip.length == 5) {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}

function validateyear(fieldname) {
    var year = document.getElementById(fieldname).value;
    document.getElementById(fieldname).value = year.replace(/[^\d]/, '');
    if (year.length > 4) {
        document.getElementById(fieldname).value = year.substring(0, 4);
    }

    if (year.length == 4) {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}

function validaterequireddropdown(fieldname) {
    if (document.getElementById(fieldname).value != "0") {
        document.getElementById(fieldname + "_Valid").style.display = "inline";
        document.getElementById(fieldname + "_Invalid").style.display = "none";
        return true;
    }
    else {
        document.getElementById(fieldname + "_Valid").style.display = "none";
        document.getElementById(fieldname + "_Invalid").style.display = "inline";
        return false;
    }
}


function showloading() {
    document.getElementById("loading").style.removeProperty("display");
}