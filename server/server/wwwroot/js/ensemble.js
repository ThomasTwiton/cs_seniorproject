function EnsembleContainer(props) {
    // The container that holds all the members inside of it.

    const membersList = props.members;
    const memberItems = membersList.map((m) => <Member info={m} />);
    return (
        <div id="ensembleContainer" className="container">
            {memberItems}
        </div>
    );
}

function MemberRow(props) {
    // The rows in the EnsembleContainer that hold the Members.
}

function Member(props) {
    // The base component for a member.
    return (
        <div)
}

const e = {
    name: 'The Beatles',
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: 1,
}

const n = {
    name: 'Solo',
    avatarURL: 'https://placekitten.com/g/64/64',
    profileID: null,
}

const eList = [e, n];
console.log(eList);

ReactDOM.render(
    <EnsembleContainer members={eList} />,
    document.getElementById('ensembles')
);