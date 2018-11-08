function EnsembleContainer(props) {
    // The container that holds all the members inside of it.

    const membersList = props.members;
    const memberItems = membersList.map((m) => <Member info={m} />);
    return (
        <div id="ensembleContainer" className="container-fluid">
            {memberItems}
        </div>
    );
}

function Member(props) {
    // The base component for a member.
    console.log(props);
    const link = "/Profile/id=" + props.info.profileID.toString();
    return (
        <a class="ensembleLink" href={link}>
            <img src={props.info.avatarURL} title={props.info.name} />
        </a>
    );
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