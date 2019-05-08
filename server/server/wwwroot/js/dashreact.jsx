function NullDetails(props) {
    return (
        <div className="detCont">
            Select an audition above to display applicants.
        </div>
    );
}

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
                    <span className="glyphicon glyphicon-ok pointer" title="Accept Applicant" onClick={() => acceptProfile(props.profile.id)}></span>
                </div>

                <div className="col-xs-1">
                    <span className="glyphicon glyphicon-remove pointer" title="Reject Applicant" onClick={() => rejectProfile(props.profile.id)}></span>
                </div>
            </div>

            <div className="collapse" id={props.collapseID}>
                <div className="card card-body row">
                    <div className="col-xs-3">
                        <a href={"../../Home/Profile/"+props.profile.id.toString()}><img className="mediumPic" src={props.profile.pic} /></a>
                    </div>

                    <div className="col-xs-4">
                        <div className="row">
                            <span className="glyphicon glyphicon-map-marker"></span> {props.profile.loc}
                        </div>
                        {/*<div className="row instruments">
                            <b>Instruments:</b> 
                        </div>*/}
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



/* ================================================== */
/* ================== jsx Functions ================= */
/* ================================================== */

function popApplicants(data) {

    if (data == null) {
        ReactDOM.render(
            <NullDetails />,
            document.getElementById('profiles')
        );
    } else {

        let res = [];
        let colID = 0;
        for (let a of data) {
            console.log(a);
            res.push({
                collapseID: "collapse" + colID.toString(),
                profile: {
                    type: "profile",
                    id: a.profileId,
                    name: a.first_Name + " " + a.last_Name,
                    pic: a.pic_Url,
                    inst: a.plays_Instrument,
                    bio: a.bio,
                    loc: a.city + ", " + a.state
                }
            });
            colID++;
        }

        ReactDOM.render(
            <DetailContainer res={res} />,
            document.getElementById('profiles')
        );
    }
}