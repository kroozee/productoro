import React, { PropsWithChildren } from 'react';
import './Layout.css';
import NavigationBar from './NavigationBar';

export default (props: PropsWithChildren<{}>) => {
    return (
        <React.Fragment>
            <div className="body container">
                {props.children}
            </div>
            <NavigationBar />
        </React.Fragment>
    );
};
