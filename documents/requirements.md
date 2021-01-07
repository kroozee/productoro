## Introduction and Scope

The purpose of this document is to present a detailed description of Productoro, its requirements, and the features it will offer. This document is intended to be a reference both for the developer as he implements the system and for the reviewer who wishes to know more about what the project intends to do.

Productoro will be a time-tracking application intended to enhance the productivity and focus of its users by tracking and reporting the time spent on projects and tasks with minimal user interaction. The system will be designed to track time spent on work simultaneously as the work is being performed by the user.

This system will organize works into projects made up of smaller tasks and facilitate simple project and task management. Users will be able to track the they time spend working by selecting projects and tasks to work on in real time. The system will also allow the users to view their tracked time per day and to make adjustments, allowing for an accurate depiction of how their tracked time was spent on a daily basis.

Productoro will also provide integration with the `digital.ai` (existing) time tracking system, by allowing the user to import `digital.ai` projects and tasks with their credentials, and sync tracking time and task status to `digital.ai`. 

## Tech Stack

* CSS Framework: `Bulma`
* UI Library: `React-Redux`
* Client-side Language: `Typescript`
* Server-side Language: `C#`
* Web Hosting Framework: `ASP.NET Core`
* Database: `SQLite`
* Server OS: `Windows 10`

## Use Cases

### Add a Project

**Description:** The user sets up a new project.

There will be two ways to set up a project: manual creation and importing from `digital.ai`.

**Steps (Manual Creation):**

1. The user navigates to the Projects page.
2. The user clicks the plus (+) button, bringing them to the New Project page.
3. The user enters in the name of their new project.
4. The user clicks the Add button, bringing them to the Projects page.
5. The user's new project is listed at the top of the Projects page.

**Steps (Importing from `digital.ai`):**

1. The user navigates to the Projects page.
2. The user clicks the plus (+) button, bringing them to the New Project page.
3. The user clicks the Import from `digital.ai` button, bringing them to the import page.
4. The user selects the project they wish to import from `digital.ai`, bringing them to the Projects page.
5. The newly imported project is listed with all pre-existing tasks at the top of the Projects page.

### Add a Task

**Description:** The user adds a task to a project.

Tasks will be required for time tracking.

**Steps:**

1. The user navigates to the Projects page.
2. The user finds the project to which they wish to add a task.
3. The user clicks the Add a task... text field.
4. The user types the name of the task they wish to add.
5. The user clicks the plus (+) button to the right of the task name they entered.

### Archive a Project

**Description:** The user archives a project they no longer want to see.

**Steps:**

1. The user navigates to the Projects page.
2. The user finds the project they wish to archive.
3. The user clicks the archive button on the project.
4. The project disappears from the Projects list.
5. The user navigates to the Archive page.
6. The archived project is listed at the top of the Archive page.

### Unarchive a Project

**Description:** The user unarchives a project they want to see again.

**Steps:**

1. The user navigates to the Archive page.
2. The user finds the project they wish to unarchive.
3. The user clicks the unarchive button on the project.
4. The project disappears from the Archive list.
5. The user navigates to the Projects page.
6. The unarchived project is listed at the top of the Projects page.

### Track Time Spent Working

**Description:** The user tracks the time they spend working on a task.

**Steps:**

1. The user navigates to the Projects page.
2. The user finds the project and task they wish to track time working on.
3. The user clicks the play button next to the task, bringing them to the Tracking page.
4. The user works on their task as the timer on the Tracking page increments.
5. When user is finished working, or when they wish to work on something else, the user clicks the Work on something else button, bringing them back to the Projects page.
6. The timer stops, and the new total amount worked is listed next to the worked task on the Projects page.

### Complete a Task

**Description:** The user marks a task as completed.

There will be two ways to mark a task as completed: on the Projects page and on the Tracking page.

**Steps (Projects Page):**

1. The user navigates to the Projects page.
2. The user finds the task they wish to mark as completed.
3. The user toggles the checkbox next to the task.

**Steps (Tracking Page):**

1. The user begins tracking time to a task on the Tracking page.
2. The user toggles the checkbox next to the task listed on the Tracking page, stopping the timer.

### View Time Tracked for a Day

**Description:** The user views the time tracked to their projects and tasks for a particular day.

**Steps:**

1. The user navigates to the Timesheet page.
2. The user uses the left arrow (<-) and right arrow (->) buttons to navigate to the day they wish to see.
3. A breakdown of time tracked to each project and task is listed.

### Make Adjustments

**Description:** The user makes an adjustment to the time tracked to a task for the day.

**Steps:**

1. The user navigates to the Timesheet page.
2. The user uses the left arrow (<-) and right arrow (->) buttons to navigate to the day they wish to adjust.
3. The user finds the task they wish to adjust the time tracked.
4. The user clicks the time listed next to the task.
5. The user enters a new timespan representing the actual time they spent on the task that day.