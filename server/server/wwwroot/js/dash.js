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
        callAPI("closeAud/" + id.toString(), "GET", "", location.reload);
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
    callAPI("applicants/" + audID.toString(), "GET", "", popApplicants);
}

function acceptProfile(pID) {
    console.log("Accepting Profile:", pID);
    let data = {
        AuditionId: _AudId,
        ProfileId: pID,
        EnsembleId: _EnsembleId
    }

    callAPI("acceptApplicant", "POST", data, () => console.log("DONE"));
}

function rejectProfile(pID) {
    console.log("Rejecting Profile:", pID);
    let data = {
        AuditionId: _AudId,
        ProfileId: pID
    }

    callAPI("remApplicant", "POST", data, () => getProfiles(_AudId));
}

function getClosedAuds() {
    console.log("Retrieving Previous Auditions for Ensemble:", _EnsembleId);

}


/* ================================================== */
/* ============ Manage Members Functions ============ */
/* ================================================== */

function delMem(id) {

    if (confirm("Are you sure you would like to remove this member?\n\nThe page will be refreshed.")) {
        console.log("Removing Member:", id);

        callAPI("remProfile", "POST", { ProfileId: id, EnsembleId: _EnsembleId }, window.location.reload);
    }
}

function transOwner() {
    let i = document.getElementById("transInput");

    if (confirm("Are you sure you would like to transfer ownership of this ensemble to '" + i.value + "'?\n\nYou're account will lose all permissions with regards to this ensemble.")) {
        console.log("Transfering Ownership to:", i.value);

        data = { name: i.value, EnsembleId: _EnsembleId };
        callAPI("transOwner", "POST", data, (a) => console.log(a));
    }

    i.value = "";
}

function addMember() {
    let i = document.getElementById("addInput");

    if (confirm("Are you sure you would like to add '" + i.value + "' as a member of this ensemble?")) {
        console.log("Adding Member:", i.value, "To EnsembleId:",_EnsembleId);

        data = { name: i.value, EnsembleId: _EnsembleId };
        callAPI("addProfile", "POST", data, (a) => console.log(a));
    }

    i.value = "";
}

function getMembers() {
    console.log("Getting Members for Ensemble:", _EnsembleId);
    //callAPI("members", "POST", { id: 21 }, popMembers);
    callAPI("members/21", "GET", null, popMembers);
}

function popMembers(data) {
    console.log(data);
}

