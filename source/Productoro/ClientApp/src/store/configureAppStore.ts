import { combineReducers, configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import { connectRouter, routerMiddleware } from "connected-react-router";
import { History } from 'history';
import { ApplicationState, reducers } from ".";

export default function configureAppStore(history: History, preloadedState?: ApplicationState) {
    const middleware = [
        routerMiddleware(history)
    ];

    const rootReducer = combineReducers({
        ...reducers,
        router: connectRouter(history)
    });

    const store = configureStore({
        reducer: rootReducer,
        middleware: [...getDefaultMiddleware()],
        preloadedState
    })
  
    return store;
}