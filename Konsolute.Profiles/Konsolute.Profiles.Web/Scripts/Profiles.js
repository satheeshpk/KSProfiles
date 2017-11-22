var depts = [];
$(document).ready(function () {
    $("#srch-button").click(function () {
        var k = $("#srch-term").val();
        var url = "/Profile/Search?keyword=" + k;
        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: url,
            success: function (data, textStatus) {
                $("#plist").empty();
                var res = data;
                var content = '';
                if (res.length > 0) {
                    for (var i = 0; i < res.length; i++) {
                        content += '<tr>';
                        content += '<td id="td' + i + '">' + res[i].DisplayName + '</td>';
                        content += '<td id="tf' + i + '">' + res[i].FirstName + '</td>';
                        content += '<td id="tl' + i + '">' + res[i].LastName + '</td>';
                        content += '<td id="te' + i + '">' + res[i].EmailAddress + '</td>';
                        content += '<td id="tdp' + i + '">' + res[i].Department + '</td>';

                        var eb = "<input type='button' value='Edit' class='btn btn-primary' onclick='openEdit(&quot;" + res[i].UserId + "&quot;,&quot;" + i + "&quot;)' />";
                        content += "<td id='bt" + i + "'>" + eb + "</td>";
                        content += '</tr>';
                    }
                }
                else {
                    content += '<tr><td colspan="6" class="text- info text- center">No profiles to display for search.</td></tr>';
                }

                $("#plist").html(content);
            },
            error: function (xhr, textStatus, errorThrown) {
                alert('request failed');
            }
        });
    });

    loadDepts();
});

function openEdit(oid, i) {
    var tdv = $("#td" + i).text();
    var tfv = $("#tf" + i).text();
    var tlv = $("#tl" + i).text();
    var tev = $("#te" + i).text();
    var tpdv = $("#tdp" + i).text();

    $("#td" + i).html(getInputC(tdv));
    $("#tf" + i).html(getInputC(tfv));
    $("#tl" + i).html(getInputC(tlv));
    $("#tdp" + i).html(createDeptC(tpdv));

    var eb = "<input type='button' class='btn btn-primary' value='Update' onclick='updateP(&quot;" + oid + "&quot;,&quot;" + i + "&quot;)' />";
    eb += "<input type='button' class='btn btn-primary' value='Cancel' onclick='cancelP(&quot;" + oid + "&quot;,&quot;" + i + "&quot;)' />";

    $("#bt" + i).html(eb);
}

function getInputC(value) {
    return "<input value='" + value + "' type='text'/>";
}

function createDeptC(val) {
    var sel = $('<select />');
    for (var i = 0; i < depts.length; i++) {
        sel.append($("<option />").attr('value', depts[i].Value).text(depts[i].Value));
    }

    sel.val(val);

    return sel;
}

var ci = -1;
function updateP(oid, i) {

    var tdv = $("#td" + i + " input").val();
    var tfv = $("#tf" + i + " input").val();
    var tlv = $("#tl" + i + " input").val();
    var tev = $("#te" + i).text();
    var tpdv = $("#tdp" + i + " select").find(":selected").text();
    ci = i;
    var updateData = {
        "UserId": oid,
        "DisplayName": tdv,
        "FirstName": tfv,
        "LastName": tlv,
        "EmailAddress": tev,
        "Department": tpdv
    };

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Profile/Update',
        data: JSON.stringify(updateData),
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            cancelP(oid, ci);
            ci = -1;
        },
        error: function (response) {
            ci = -1;
        }
    });
}

function cancelP(oid, i) {
    var tdv = $("#td" + i + " input").val();
    var tfv = $("#tf" + i + " input").val();
    var tlv = $("#tl" + i + " input").val();
    var tpdv = $("#tdp" + i + " select").find(":selected").text();

    $("#td" + i).html(tdv);
    $("#tf" + i).html(tfv);
    $("#tl" + i).html(tlv);
    $("#tdp" + i).html(tpdv);
    var eb = "<input type='button' class='btn btn-primary' value='Edit' onclick='openEdit(&quot;" + oid + "&quot;,&quot;" + i + "&quot;)' />";
    $("#bt" + i).html(eb);
}

function loadDepts() {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: '/profile/departments',
        success: function (response) {
            depts = response;
        },
        error: function (response) {
            console.log(response);
        }
    });
}