// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
//  is less clutter in the console).s
if (document.getElementById('createModal')) {
    window.onclick = function (event) {
        let modal = document.getElementById('createModal');
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}
