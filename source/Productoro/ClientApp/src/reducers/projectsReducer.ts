import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import * as duration from 'dayjs/plugin/duration';

export type TaskId = string;
export type TaskName = string;

export class Task {
    constructor(
        readonly id: TaskId,
        readonly name: TaskName,
        readonly duration: duration.Duration,
        readonly isCompleted: boolean
    ) {
    }
}

export class NewTask {
    constructor(
        readonly name: TaskName
    ) {
    }
}

export type ProjectId = string;
export type ProjectName = string;

export class Project {
    constructor(
        readonly id: ProjectId,
        readonly name: ProjectName,
        readonly tasks: Task[]
    ) {
    }
}

interface ProjectsState {
    projects: Project[]
}

const initialState = { projects: [] } as ProjectsState;

const projectsSlice = createSlice({
    name: 'projects',
    initialState,
    reducers: {
        archiveProject: (state, action: PayloadAction<ProjectId>) => {
            
        },
        beginSession: (state, action: PayloadAction<{ project: ProjectId, task: TaskId }>) => {

        },
        completeTask: (state, action: PayloadAction<{ project: ProjectId, task: TaskId }>) => {

        },
        unCompleteTask: (state, action: PayloadAction<{ project: ProjectId, task: TaskId }>) => {

        },
        addTask: (state, action: PayloadAction<{ project: ProjectId, newTask: NewTask }>) => {

        },
        removeTask: (state, action: PayloadAction<{ project: ProjectId, task: TaskId }>) => {

        }
    }
});

export const fetchProjectsAsync = createAsyncThunk(
    'aoi/projects',
    async (thunkApi) => {

    }
);


export interface ArchiveProject {
    type: 'ARCHIVE_PROJECT';
    project: ProjectId;
}

export interface BeginSession {
    type: 'BEGIN_SESSION';
    project: ProjectId;
    task: TaskId;
}

export interface CompleteTask {
    type: 'COMPLETE_TASK';
    project: ProjectId;
    task: TaskId;
}

export interface UnCompleteTask {
    type: 'UNCOMPLETE_TASK';
    project: ProjectId,
    task: TaskId;
}

export interface AddTask {
    type: 'ADD_TASK';
    project: ProjectId;
    newTask: NewTask;
}

export interface RemoveTask {
    type: 'REMOVE_TASK';
    project: ProjectId;
    task: TaskId;
}
