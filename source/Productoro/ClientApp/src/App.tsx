import * as React from 'react';
import './App.sass';
import { Route } from 'react-router';
import Archive from './components/Archive';
import Layout from './components/Layout';
import NewProject from './components/NewProject';
import Projects from './components/Projects';
import Timesheet from './components/Timesheet';
import Tracking from './components/Tracking';
import routes from './routes';

export default () => (
    <Layout>
        <Route exact path={routes.Projects} component={Projects} />
        <Route path={routes.Archive} component={Archive} />
        <Route path={routes.NewProject} component={NewProject} />
        <Route path={routes.Timesheet} component={Timesheet} />
        <Route path={routes.Tracking} component={Tracking} />
    </Layout>
);
