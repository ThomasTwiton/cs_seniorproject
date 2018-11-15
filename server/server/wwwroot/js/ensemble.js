/* ================================================== */
/* ================ React Components ================ */
/* ================================================== */

function EnsembleContainer(props) {
    // The container that holds all the members inside of it.

    const membersList = props.members;
    const memberItems = membersList.map((m) => <Member info={m} />);
    return (
        <div id="ensembleContainer" className="container">
            {memberItems}
            <ExtraOption />
        </div>
    );
}

function MemberRow(props) {
    // The rows in the EnsembleContainer that hold the Members.
}

function Member(props) {
    // The base component for a member.
    //console.log(props);
    const link = "ViewEnsemble/" + props.info.id.toString();
    return (
        <a className="ensembleLink" href={link}>
            <img src={props.info.avatarURL} title={props.info.name} />
        </a>
    );
}

function ExtraOption(props) {
    // An additional square that is used to create an audition, 
    // create an ensemble or other such functionality via modal.

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

function pageLoaded() {
    /* The way that the ensembles data is being passed into
     * the view requires the javascript to find all of the 
     * member tags and assemble the appropriate props to be
     * passed to the EnsembleContainer. */

    let eList = document.getElementsByTagName("Member");
    //console.log(eList);
    let propList = [];
    for (let m of eList) {
        let props = {}
        props.id = m.dataset.id;
        props.avatarURL = m.dataset.avatarURL;
        props.name = m.dataset.name;

        propList.push(props)
    }
    //console.log(propList);
    return propList;
}

const propList = pageLoaded();
console.log(propList);
ReactDOM.render(
    <EnsembleContainer members={propList} />,
    document.getElementById('ensembles')
);