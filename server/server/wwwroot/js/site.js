﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


/* ================================================== */
/* ================= Modal Functions ================ */
/* ================================================== */

function showModal() {
    // Get the modal
    let modal = document.getElementById('createModal');
    modal.style.display = "block";
}

function hideModal() {
    let modal = document.getElementById('createModal');
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
// (Only set this if the modal actually exists-- this way there
//  is less clutter in the console).
if (document.getElementById('createModal')) {
    window.onclick = function (event) {
        let modal = document.getElementById('createModal');
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}


/* ================================================== */
/* ============ Create Profile Functions ============ */
/* ================================================== */

// The following function changes the color of the label
//  to indicate whether the given checkbox has been selected.
function changeCheck(cb) {
    if (cb.checked == true) {
        // parentNode is the label containing the checkbox.
        cb.parentNode.parentNode.classList.add("selected")
    } else {
        cb.parentNode.parentNode.classList.remove("selected");
    }
}

// The following function changes which creation view is being
//  displayed to the user.
function switchView(id) {
    let infoRow = document.getElementById("infoRow");
    let forms = infoRow.getElementsByTagName("form");

    // Hide all forms
    for (let f of forms) {
        f.style.display = "none";
    }

    // Remove active tag.
    document.getElementById("active").id = "notActive";

    switch (id) {
        case "profile":
            document.getElementById("profileForm").style.display = "block";
            document.getElementsByClassName("p")[0].id = "active";
            break;
        case "ensemble":
            document.getElementById("ensembleForm").style.display = "block";
            document.getElementsByClassName("e")[0].id = "active";
            break;
        case "venue":
            document.getElementById("venueForm").style.display = "block";
            document.getElementsByClassName("v")[0].id = "active";
            break;
    }
}

// The following function figures out which form should be
//  submitted and submits that form.
function submitForm() {
    // whichever has the 'active' id is considered the one
    //  to be submitted.
    let activeView = document.getElementById("active");
    
    if ("p" == activeView.classList[1]) {
        document.getElementById("profileForm").submit();

    } else if ("e" == activeView.classList[1]) {
        document.getElementById("ensembleForm").submit();

    } else if ("v" == activeView.classList[1]) {
        document.getElementById("venueForm").submit();

    }
    console.log("Done");
}

/* ================================================== */
/* ============= Base API Call Function ============= */
/* ================================================== */

function callAPI(apiStr, method, data, sucFun) {
    console.log("Calling API:", method, apiStr);

    let dfd = new $.Deferred();
    // creating a deferred object allows functions that
    //  call this function not to hang and wait for a 
    //  server response.

    if (data == null) {
        $.ajax({
            url: "../../api/PluggedAPI/" + apiStr,
            method: method,
            contentType: "application/json;charset=utf-8",
            success: (resData) => sucFun(resData),
            error: (error) => console.log("AJAX Error:", error)

        }).done(function () {
            dfd.resolve();  // Resolve the deferred object

        });

    } else {

        $.ajax({
            url: "../../api/PluggedAPI/" + apiStr,
            method: method,
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: (resData) => sucFun(resData),
            error: (error) => console.log("AJAX Error:", error)

        }).done(function () {
            dfd.resolve();  // Resolve the deferred object

        });
    }

    return dfd;
}

function callbackClosure(i, callback) {
    return function (data) {
        if (data == null) {
            return callback(i);
        }

        return callback(i,data);
    }
}