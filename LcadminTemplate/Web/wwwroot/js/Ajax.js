if (document.getElementById('form') != null) {
    document.getElementById('form').onsubmit = function (evt) {
        debugger;
        var blockpostback = false;
        if (document.getElementById('BlockPostBack') != null) {
            if (document.getElementById('BlockPostBack').value == 'true') {
                blockpostback = true;
            }
        }

        if (blockpostback) {
            evt.preventDefault();
        }

        if (document.getElementById('AjaxUpdate') != null) {
            if (document.getElementById('AjaxUpdate').value == 'true') {
                evt.preventDefault();

                var performaction = true;
                if (document.getElementById('Action') != null)
                    if (document.getElementById('Action').value == 'No Action')
                        performaction = false;

                if (performaction) {
                    $.ajax({
                        type: 'POST',
                        url: evt.target.action,
                        data: new FormData(evt.target),
                        contentType: false,
                        processData: false,
                        success: function (res) {
                            if (document.getElementById('DivToUpdate') != null) {
                                if (document.getElementById('DivToUpdate').value != "") {
                                    var divtoupdate = document.getElementById('DivToUpdate').value;
                                    document.getElementById('DivToUpdate').value = "";
                                    $(divtoupdate).html(res.html)
                                }
                                else {
                                    $('#partialview').html(res.html)
                                }
                            }
                            else {
                                $('#partialview').html(res.html)
                            }

                            if (document.getElementById('popupbackground') != null) {
                                document.getElementById('popupbackground').style.display = 'none';
                            }
                        },
                        error: function (err) {
                            alert('An Error occurred while making updates');
                        }
                    })
                }
            };
        }
    }
}