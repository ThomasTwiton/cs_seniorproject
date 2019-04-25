/* ================================================== */
/* ================ General Functions =============== */
/* ================================================== */

function refreshActiveView() {
    let activeTabs = document.getElementsByClassName("react-tabs__tab--selected");

    if (activeTabs.length <= 0) {
        // If we do not have an active tab
        // -- This should not occur -- 
        location.reload();
    }

    let activeTab = activeTabs[0];
    let refreshFunction;

    switch (activeTab.id) {
        case "react-tabs-0":
            refreshFunction = getClosedAuds;
            break;
        case "react-tabs-2":
            refreshFunction = getActiveAuds;
            break;
        case "react-tabs-4":
            refreshFunction = getMembers;
            break;
        default:
            refreshFunction = function () { console.log("View Redraw Not Specified") };
    }

    console.log("Refreshing Page View");
    refreshFunction();
}



/* ================================================== */
/* ================= Modal Functions ================ */
/* ================================================== */

function editAud(id) {
    console.log("Editing Audition:", id);

    callAPI("auditions/" + id, "GET", id, displayModal);

}

function delAud(id) {
    if (confirm("Are you sure you would like to close this audition?\n\nThe audition can be reinstated in the 'Previous Auditions' tab.")) {
        console.log("Deleting Audition:", id);
        callAPI("closeAud/" + id.toString(), "GET", null, refreshActiveView);
    }
}

function displayModal(data) {
    console.log(data);

    document.getElementById("audID").value = data.auditionId;
    document.getElementById("audOpen").value = data.open_Date.split("T")[0];
    document.getElementById("audClose").value = data.closed_Date.split("T")[0];
    document.getElementById("audLoc").value = data.audition_Location;
    document.getElementById("audIns").value = data.instrument_Name;
    document.getElementById("audID").value = data.auditionId;
    document.getElementById("desc").value = data.audition_Description;

    showModal();
}

function updateAud() {
    let aid = document.getElementById("audID").value;
    console.log("Updating Audition:", aid);

    let audObject = {
        auditionId: aid,
        audition_Description: document.getElementById("desc").value,
        audition_Location: document.getElementById("audLoc").value,
        closed_Date: document.getElementById("audClose").value,
        open_Date: document.getElementById("audOpen").value,
        instrument_Name: document.getElementById("audIns").value,
    };

    callAPI("auditions/" + aid, "POST", audObject, hideModal);

    console.log(audObject);

}



/* ================================================== */
/* ============== Auditions Functions =============== */
/* ================================================== */

function getProfiles(audID) {
    console.log("Getting Profiles for Audition:", audID);
    _AudId = audID;
    callAPI("applicants/" + audID.toString(), "GET", null, popApplicants);
}

function acceptProfile(pID) {
    console.log("Accepting Profile:", pID);
    let data = {
        AuditionId: _AudId,
        ProfileId: pID,
        EnsembleId: _EnsembleId
    }

    callAPI("acceptApplicant", "POST", data, callbackClosure(_AudId, closeAud));
}

function rejectProfile(pID) {
    console.log("Rejecting Profile:", pID);
    let data = {
        AuditionId: _AudId,
        ProfileId: pID
    }

    callAPI("remApplicant", "POST", data, callbackClosure(_AudId,getProfiles));
}

function closeAud(aID) {
    console.log("Closing Audition:", aID);

    callAPI("closeAud/" + aID.toString(), "GET", null, refreshActiveView);
}

function openAud(aID) {
    console.log("Re-opening Audition:", aID);

    callAPI("openAud/" + aID.toString(), "GET", null, refreshActiveView);
}

function getClosedAuds() {
    console.log("Retrieving Previous Auditions for Ensemble:", _EnsembleId);
    callAPI("getClosedAuditions/"+_EnsembleId.toString(), "GET", null, callbackClosure("closed",popAuditions));
}

function getActiveAuds() {
    console.log("Getting Active Auditions for Ensemble:", _EnsembleId);

    callAPI("getOpenAuditions/"+_EnsembleId.toString(), "GET", null, callbackClosure("pending", popAuditions));
}

function popAuditions(loc, res) {
    console.log("Populating Auditions for View:", loc);

    let table = document.getElementById(loc);
    table.getElementsByTagName("tbody")[0].innerHTML = table.rows[0].innerHTML;

    for (let aud of res) {
        let row = document.createElement("tr");

        for (let prop of ["instrument_Name", "open_Date", "closed_Date"]) {
            let c = document.createElement("td");

            if (prop.toLowerCase().includes("date")) {
                let d = new Date(aud[prop]);
                c.innerHTML = d.toLocaleDateString();

            } else {
                c.innerHTML = aud[prop];

            }
            row.appendChild(c);
        }

        // Create View column
        let v = document.createElement("td");
        let vs = document.createElement("span");

        vs.title = "View Applicants";
        vs.classList.add("glyphicon");
        vs.classList.add("glyphicon-eye-open");
        vs.classList.add("pointer");
        vs.onclick = () => getProfiles(aud["auditionId"]);

        v.appendChild(vs);
        row.appendChild(v);

        // Create Edit column
        let e = document.createElement("td");
        let es = document.createElement("span");

        es.title = "Edit Audition";
        es.classList.add("glyphicon");
        es.classList.add("glyphicon-pencil");
        es.classList.add("pointer");
        es.onclick = () => editAud(aud["auditionId"]);

        e.appendChild(es);
        row.appendChild(e);


        // Create final column
        let f = document.createElement("td");
        let fs = document.createElement("span");

        fs.title = "Reinstate Audition";
        fs.classList.add("glyphicon");
        fs.classList.add("glyphicon-remove");
        fs.classList.add("pointer");
        fs.onclick = () => delAud(aud["auditionId"]);
        
        f.appendChild(fs);
        row.appendChild(f);

        table.getElementsByTagName("tbody")[0].appendChild(row);

    }

    popApplicants(null);
}


/* ================================================== */
/* ============ Manage Members Functions ============ */
/* ================================================== */

function delMem(id) {

    if (confirm("Are you sure you would like to remove this member?\n\nThe page will be refreshed.")) {
        console.log("Removing Member:", id);

        callAPI("remProfile", "POST", { ProfileId: id, EnsembleId: _EnsembleId }, refreshActiveView);
    }
}

function transOwner() {
    let i = document.getElementById("transInput");

    if (confirm("Are you sure you would like to transfer ownership of this ensemble to '" + i.value + "'?\n\nYou're account will lose all permissions with regards to this ensemble.")) {
        console.log("Transfering Ownership to:", i.value);

        data = { Email: i.value, EnsembleId: _EnsembleId };
        callAPI("transferOwner", "POST", data, checkTransfer);
    }
}

function checkTransfer(res) {
    console.log(res);
    if (res["transferred"] == true) {
        alert("Ownership of this ensemble has been transferred to " + res["email"] + ".");
        location.reload(true);

    } else {
        alert("An error occured and we were unable to transfer ownership to " + res["email"] + ".\n\nMake sure the specified user has an account and that their email is spelled correctly.");

    }
}

function addMember() {
    let i = document.getElementById("addInput");

    if (confirm("Are you sure you would like to add '" + i.value + "' as a member of this ensemble?")) {
        console.log("Adding Member:", i.value, "To EnsembleId:",_EnsembleId);

        data = { name: i.value, EnsembleId: _EnsembleId };
        callAPI("addProfile", "POST", data, refreshActiveView);
    }

    i.value = "";
}

function getMembers() {
    console.log("Getting Members for Ensemble:", _EnsembleId);

    callAPI("members/"+_EnsembleId, "GET", null, popMembers);
}

function popMembers(res) {
    console.log(res);

    let table = document.getElementById("memTable");
    table.getElementsByTagName("tbody")[0].innerHTML = table.rows[0].innerHTML;

    for (let p of res) {
        console.log(p);
        let row = document.createElement("tr");

        let n = document.createElement("td");
        n.innerHTML = p["profile"]["first_Name"] + " " + p["profile"]["last_Name"];
        row.appendChild(n);

        let s = document.createElement("td");
        let d = new Date(p["start_Date"]);
        s.innerHTML = d.toLocaleDateString();
        row.appendChild(s);

        let r = document.createElement("td");
        let sp = document.createElement("span");
        sp.title = "Remove Member";
        sp.classList.add("glyphicon");
        sp.classList.add("glyphicon-remove");
        sp.classList.add("pointer");
        sp.onclick = () => delMem(p["profile"]["profileId"]);
        r.appendChild(sp);

        row.appendChild(r);

        table.getElementsByTagName("tbody")[0].appendChild(row);

    }

}