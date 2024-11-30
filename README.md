Project Intake Web Application
Overview
The Project Intake Web Application is a capstone project designed for Temple University to streamline the process of submitting, reviewing, and approving capstone project proposals. It provides a centralized platform for students, faculty, and reviewers to collaborate effectively, ensuring transparency and efficiency in the project lifecycle.

Features
User Authentication: Role-based access control for clients, capstone coordinators, and reviewers using secure login mechanisms.
Project Submission: A user-friendly form for clients to submit project proposals.
Approval Workflow: Reviewers can assess submissions, provide feedback, and approve or deny proposals.
Dashboards:
Admin Dashboard: Manage user profiles, track project submissions, and review statistics.
User Dashboard: Track submission status and resubmit proposals if necessary.
Dynamic Notifications: Real-time updates for users on the status of their submissions.
Scalable Architecture: Designed to handle an increasing number of project submissions as the platform grows.
Technologies Used
Frontend: HTML, CSS (Bootstrap), JavaScript (React)
Backend: ASP.NET Core MVC
Database: Microsoft SQL Server
APIs: RESTful APIs for secure and efficient data exchange
Tools: Git for version control, Figma for UI mockups
Architecture
Role-Based Access Control: Implements middleware to ensure appropriate permissions for clients, reviewers, and administrators.
Secure Data Management: Uses SQL stored procedures for data encryption and authentication.
Responsive Design: Ensures usability across various devices using Bootstrap.
Installation
Clone the repository:
bash
Copy code
git clone https://github.com/Abhijayshekhawat/project-intake-app.git
Navigate to the project directory:
bash
Copy code
cd project-intake-app
Install required dependencies:
bash
Copy code
npm install (for frontend)
Set up the database:
Import the provided SQL schema into Microsoft SQL Server.
Update the appsettings.json file with your database connection string.
Run the application:
bash
Copy code
dotnet run
Usage
Admin Login:
Manage user roles and profiles.
Review and approve/deny submitted projects.
Client:
Submit project proposals.
Track the status of submitted proposals.
Reviewer:
Access assigned projects for review.
Provide feedback and approve/deny submissions.

Future Enhancements
Enhanced Analytics: Provide detailed statistics and insights on submission trends.
Messaging System: Enable in-app communication between users.
Third-Party Integrations: Integrate with project management tools like Trello or Jira.
Contributors
Abhijay Shekhawat

Role: Compiler, Developer
Email: tuh18229@temple.edu
[Full list of team members as per project documentation]

License
This project is licensed under the MIT License. See LICENSE for details.
