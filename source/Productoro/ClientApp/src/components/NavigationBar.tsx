import React from 'react';
import './NavigationBar.css';
import { Link } from 'react-router-dom';
import routes from '../routes';

export default () => {
    return (
        <nav className="navbar is-fixed-bottom" role="navigation">
            <div className="navbar-menu is-active">
                <div className="navbar-start">
                    <Link to={routes.Archive} className='navbar-item'>
                        Archive
                    </Link>

                    <Link to={routes.Projects} className='navbar-item'>
                        Projects
                    </Link>

                    <Link to={routes.Timesheet} className='navbar-item'>
                        Timesheet
                    </Link>
                </div>
            </div>
        </nav>
    )
}
