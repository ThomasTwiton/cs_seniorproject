const { Tabs, TabList, Tab, TabPanel } = ReactTabs;

function TabContainer(props) {
    // The container that holds all posts inside of it.

    let tabList = [];
    for (let i = 0; i < props.tabs.length; i++) {
        tabList.push(<Tab>{props.title}</Tab>);
    }

    return (
        <Tabs>
            <TabList>
                {tabList}
            </TabList>

            <TabPanel>
                <h2>Any content 1</h2>
            </TabPanel>
            <TabPanel>
                <h2>Any content 2</h2>
            </TabPanel>
        </Tabs>
    );
}

const t1 = { title: "Previous" };

const t2 = { title: "Pending" };

const t3 = { title: "Manage" };

const tabList = [t1, t2, t3];

ReactDOM.render(
    <TabContainer tabs={tabList} />,
    document.getElementById('tabCC')
);