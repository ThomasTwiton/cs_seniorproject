const result_ids = {
    "audquery" : "navaud",
    "gigquery" : "navgig",
    "profquery": "navprof",
    "ensquery" : "navens",
    "venquery": "navven"
};

function show(id) {

    for (resKey of Object.keys(result_ids)) {
        let results = document.getElementById(resKey);
        let navEl = document.getElementById(result_ids[resKey]);

        if (id == resKey) {
            results.style.display = "";
            navEl.classList.add("selected");

        } else {
            results.style.display = "none"
            navEl.classList.remove("selected");
        }
    }
}
