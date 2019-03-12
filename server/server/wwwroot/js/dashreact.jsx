function DetailContainer(props) {
    return (
        <div className="detCont">
            {props.res.map((r,i) => <DetailRow key={i} profile={r.profile} collapseID={r.collapseID} />)}
        </div>
    );
}

function DetailRow(props) {
    /* Props parameters:
     *      collapseID  -> Name of associated collapse div
     *      profile     -> Object containing info about the profile/ensemble
     *          type    -> String indicating what type of entity should be displayed
     *          id      -> Id of the profile/ensemble
     *          name    -> Name to be displayed
     *          pic     -> Picture URL
     *          loc     -> Location
     *          inst    -> Array containing strings of instrument names
     *          bio     -> String to be displayed for bio section
     */

    return (
        <div className="collection">
            <div className="row collapsed" data-toggle="collapse" data-target={'#' + props.collapseID} aria-expanded="false" aria-controls={props.collapseID}>
                <div className="col-xs-10">
                    {props.profile.name}
                </div>

                <div className="col-xs-1">
                    <span className="glyphicon glyphicon-ok pointer" title="Accept Applicant" onClick={() => rejectProfile(props.profile.id)}></span>
                </div>

                <div className="col-xs-1">
                    <span className="glyphicon glyphicon-remove pointer" title="Reject Applicant" onClick={() => rejectProfile(props.profile.id)}></span>
                </div>
            </div>

            <div className="collapse" id={props.collapseID}>
                <div className="card card-body row">
                    <div className="col-xs-3">
                        <img className="mediumPic" src={props.profile.pic} />
                    </div>

                    <div className="col-xs-4">
                        <div className="row">
                            <span className="glyphicon glyphicon-map-marker"></span> {props.profile.loc}
                        </div>
                        <div className="row instruments">
                            <b>Instruments:</b> {props.profile.inst.join(", ")}
                        </div>
                    </div>

                    <div className="col-xs-5">
                        <b>Bio: </b> {props.profile.bio}
                     </div>
                </div>
            </div>
        </div>
    );
}

const p0 = {
    collapseID: "collapse0",
    profile: {
        type: "profile",
        id: 0,
        name: "Tyler Conzett",
        pic: "../../images/uploads/default.png",
        loc: "Decorah, IA",
        inst: ["Trumpet", "Trombone", "Voice", "Drums"],
        bio: "Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident."
    }
};

const p1 = {
    collapseID: "collapse1",
    profile: {
        type: "profile",
        id: 1,
        name: "Tyler Conzett",
        pic: "../../images/uploads/default.png",
        loc: "Decorah, IA",
        inst: ["Trumpet1", "Trombone1", "Voice1", "Drums1"],
        bio: "Yo"
    }
};

const p2 = {
    collapseID: "collapse2",
    profile: {
        type: "profile",
        id: 2,
        name: "Tyler Conzett",
        pic: "../../images/uploads/default.png",
        loc: "Decorah, IA",
        inst: ["Trumpet2", "Trombone2", "Voice2", "Drums2"],
        bio: "Yo"
    }
};

const rList = [p0, p1, p2];

ReactDOM.render(
    <DetailContainer res={rList} />,
    document.getElementById('profiles')
);