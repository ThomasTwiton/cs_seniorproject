/* ================================================== */
/* ================ React Components ================ */
/* ================================================== */

function EnsembleContainer(props) {
    // The container that holds all the members inside of it.

    const membersList = props.members;
    const memberItems = membersList.map((m) => <Member info={m} />);
    return (
        <div id="ensembleContainer" className="container-fluid">
            {memberItems}
            <ExtraOption />
        </div>
    );
}

function Member(props) {
    // The base component for a member.

    const link = "/Profile/id=" + props.info.profileID.toString();
    return (
        <a className="ensembleLink" href={link}>
            <img src={props.info.avatarURL} title={props.info.name} />
        </a>
    );
}

function ExtraOption(props) {
    // An additional square that is used to create an audition, 
    // create an ensemble or other such functionality.

    let modal = document.getElementById('createModal');
    if (modal == null) {
        return (null)
    }

    let message;
    console.log(profileType);
    if (profileType == "ensemble") {
        message = "Hold Auditions!";
    } else {
        message = "Form a Group!";
    }

    return (
        <div title={message} className="extraBit" onClick={() => { showModal() }}>
            <span className="glyphicon glyphicon-plus"></span>
        </div>
    );
}

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
window.onclick = function (event) {
    let modal = document.getElementById('createModal');
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

const e = {
    name: 'The Beatles',
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 1,
}
const n = {
    name: 'Solo',
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 2,
}
const s = {
    name: "Jeremy's Band",
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 3,
}
const m = {
    name: 'Luther College Jazz Orchestra',
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 4,
}
const b = {
    name: "Peter's Jazz Combo",
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 5,
}
const l = {
    name: 'Gandolf the Hobbits',
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 6,
}

const eList = [e,n,s,m,b,l];
console.log(eList);

ReactDOM.render(
    <EnsembleContainer members={eList} />,
    document.getElementById('ensembles')
);