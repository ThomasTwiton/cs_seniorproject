function editAud(id) {
    console.log("Editing Audition:", id);

    callAPI("auditions/" + id, "GET", id, displayModal);

}

function delAud(id) {
    console.log("Deleting Audition:", id);
    alert("Are you sure you would like to close this audition?\n\nThe audition can be reinstated in the 'Previous Auditions' tab.")
    console.log(id, _EnsembleId);
}

function delMem(id) {
    console.log("Deleting Member:", id);
}

function displayModal(data) {
    console.log(data);

    document.getElementById("audID").value = data.auditionId;
    document.getElementById("audOpen").value = data.open_Date.split("T")[0];
    document.getElementById("audClose").value = data.closed_Date.split("T")[0];
    document.getElementById("audLoc").value = data.audition_location;
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