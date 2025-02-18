# Intake NSYNC Web Application

The Intake NSYNC Web Application is a capstone project developed for Temple University's Information Science and Technology program. It addresses the challenges in managing project proposals by streamlining submission, review, and approval workflows—all in a user-friendly, role-based web application.  
:contentReference[oaicite:0]{index=0}

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture and Technologies](#architecture-and-technologies)
- [Sprint Plans and Development](#sprint-plans-and-development)
- [Executed Tests](#executed-tests)
- [User Manual](#user-manual)
- [Outstanding Work](#outstanding-work)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Intake NSYNC Web Application was created to solve inefficiencies in Temple University’s capstone proposal process. Previously, project ideas were submitted without a centralized system, causing difficulties in tracking, reviewing, and approving proposals. This application implements a structured three-step workflow:

- **Submission:** Clients submit their project ideas.
- **Review:** Designated reviewers and coordinators evaluate submissions.
- **Approval/Denial:** Faculty members provide feedback, approve valid proposals, or offer resubmission opportunities.

The system is designed to be scalable, transparent, and secure, ensuring a smoother project intake process.  
:contentReference[oaicite:1]{index=1}

## Features

- **Role-Based Access Control:**  
  - **Clients:** Can submit and view their project proposals.
  - **Capstone Coordinators (Admins):** Have full access for reviewing and managing proposals.
  - **Reviewers:** Responsible for assessing project submissions.

- **User Authentication & Profile Management:**  
  Secure sign-up/login functionality with validations (e.g., professional email domains like .edu, .org).

- **Project Dashboard:**  
  Detailed dashboards for each user role, displaying project status, statistics, and history.

- **Notification System:**  
  Users receive updates on submission statuses and changes via notifications.

- **Workflow Transparency:**  
  Every step—from submission to review—includes feedback to ensure clarity and fairness in decision-making.

## Architecture and Technologies

- **Frameworks & Languages:**  
  Built using the MVC pattern and .NET Core Razor Pages, with integrations for SOAP services.

- **Database & Version Control:**  
  Designed with a structured database schema and managed through GitHub trunk branching for streamlined version control.

- **Front-End & Back-End Integration:**  
  Progressive development through multiple sprints has ensured a robust connection between the user interface and the back-end systems.

## Sprint Plans and Development

The project followed an iterative development process with detailed sprint plans:

- **Sprint 1-3:**  
  Focused on setting up the project structure, database connectivity, and developing the MVP for user authentication and basic front-end pages.

- **Sprint 4-6:**  
  Expanded functionalities including full dashboard creation, advanced user management, and complete integration of back-end logic with front-end interfaces.

Each sprint incorporated testing phases to validate functionality and ensure the system met its design goals.  
:contentReference[oaicite:2]{index=2}

## Executed Tests

Multiple test plans were executed to ensure quality and functionality:

- **Environment Setup & Database Integration:**  
  Verified that all team members could connect and interact with the database.

- **User Authentication Testing:**  
  Confirmed the sign-up and login processes work as intended, including validations for professional email domains.

- **Front-End & Back-End Functionality:**  
  Tested dashboard connectivity, UI responsiveness, and proper data manipulation across all user roles (Client, Reviewer, and Administrator).

The detailed test plans and their outcomes are documented within the project files.  
:contentReference[oaicite:3]{index=3}

## User Manual

A comprehensive user manual is provided in the project documentation. It covers:

- **Client Dashboard:**  
  Instructions for submitting new projects, viewing statuses, and managing profiles.

- **Reviewer Dashboard:**  
  Guidelines for accessing and reviewing assigned projects, including filtering and commenting.

- **Administrator Dashboard:**  
  Steps for managing user profiles, approving or denying project proposals, and updating project statuses.

For complete details on each page and feature, please refer to the user manual section in the documentation.

## Outstanding Work

While the system is fully functional, a few enhancements remain:

- **Dynamic Content Loading:**  
  Conversion of AJAX-based pages to fully integrated C# code for improved performance.

- **UI Refresh & Pagination:**  
  Implementing real-time table refreshes and pagination to display large datasets.

- **Enhanced Account Features:**  
  Adding robust email functionality for password resets and complete “Change Password” features.

These improvements are planned for future iterations to further enhance user experience.

## Contributing

Contributions to this project are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Submit a pull request with a detailed description of your changes.

For any issues or feature requests, please open an issue in the repository.


