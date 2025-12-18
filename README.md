# 🎯 Recruitment Platform

A full-stack web application for automating technical candidate screening and recruitment processes. Built with .NET 9 Web API, Angular 19 (Standalone), and ng-bootstrap.

## ✨ Features

### For Candidates
- **Browse & Apply**: View open positions for .NET, Angular, React, and QA roles
- **PDF Resume Upload**: Secure file upload with validation
- **Online Technical Interviews**: 
  - 3 sections: Basic/General, Technical, Practical
  - Timed questions (60 seconds per question)
  - Multiple-choice format
- **Instant Results**: Automated scoring with pass/fail determination
- **Email Notifications**: Receive interview credentials and results via email
- **PIN-based Access**: Secure interview start with unique credentials

### For Administrators
- **Position Management**: Create, edit, and deactivate job openings
- **Question Bank**: Full CRUD for interview questions across all sections
- **Interview Settings**: Configure pass thresholds, timers, and notifications
- **Analytics Dashboard**: Overview of applications and performance metrics
- **Role-based Access**: Secure admin panel with JWT authentication

## 🛠️ Tech Stack

- **Backend**: ASP.NET 9 Web API, Entity Framework Core, SQL Server
- **Frontend**: Angular 19 (Standalone Components), TypeScript, ng-bootstrap
- **Authentication**: JWT Bearer Tokens with Role-based Authorization
- **Database**: Microsoft SQL Server (with Docker support)
- **Email**: SMTP with MailKit (SendGrid ready)
- **File Storage**: Local filesystem (configurable for Azure/AWS)
- **Styling**: Bootstrap 5.x with custom SCSS

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [SQL Server 2022](https://www.microsoft.com/sql-server) or Docker
- [Git](https://git-scm.com/)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/GhulamShahbaz490/RecruitmentPlatform.git
cd RecruitmentPlatform
```
### 2. Database Setup
#### Option A: Using Docker
```bash
Copy
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 -d --name recruitment-db \
  mcr.microsoft.com/mssql/server:2022-latest
```
#### Option B: Using Local SQL Server
Create a database named RecruitmentPlatform and update connection string.

#### 3. Backend Setup
```bash
Copy
cd RecruitmentPlatform.Server
dotnet restore
dotnet ef database update
dotnet run
```
The API will be available at https://localhost:7277 and Swagger at https://localhost:7277/swagger.
#### 4. Frontend Setup
```bash
Copy
cd recruitmentplatform.client
npm install
ng serve
```
The Angular app will be available at http://localhost:4200.
#### ⚙️ Configuration
Backend (appsettings.json)
JSON
Copy
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=RecruitmentPlatform;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyForJwtTokenGenerationThatIsAtLeast32CharactersLong",
    "Issuer": "RecruitmentPlatform",
    "Audience": "RecruitmentPlatformUsers"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "FromEmail": "your-email@gmail.com",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
Frontend (src/environments/environment.ts)
TypeScript
Copy
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7277/api'
};
#### 🎯 Usage
Candidate Flow
Guest View: Browse available positions at http://localhost:4200
Apply: Click "Apply Now" and fill the application form
Check Email: Receive interview credentials (Interview Number & PIN)
Login: Access /login with email and PIN
Interview: Complete all three sections
Results: View immediate results and receive email confirmation
Admin Access
Login: Navigate to http://localhost:4200/admin/login
Default Admin Credentials:
Email: admin@recruit.com
Password: Admin@123
Dashboard: View analytics and manage content
Positions: Add/edit job openings
Questions: Manage interview questions
Settings: Configure interview parameters
#### 📁 Project Structure
Copy
RecruitmentPlatform/
├── RecruitmentPlatform.Server/          # ASP.NET 9 Web API
│   ├── Controllers/
│   │   ├── PositionsController.cs
│   │   ├── ApplicationsController.cs
│   │   ├── InterviewController.cs
│   │   └── Admin/                      # Admin controllers
│   ├── Models/
│   ├── DTOs/
│   ├── Services/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   └── Program.cs
│
├── recruitmentplatform.client/          # Angular 19 Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── home/
│   │   │   │   ├── apply/
│   │   │   │   ├── login/
│   │   │   │   ├── interview/
│   │   │   │   └── admin/              # Admin components
│   │   │   ├── services/
│   │   │   ├── guards/
│   │   │   └── models/
│   │   └── assets/
│   └── angular.json
│
├── docker-compose.yml
├── README.md
└── .gitignore
#### 🔒 Security Notes
For Production:
Replace default admin credentials immediately
Implement proper password hashing with ASP.NET Identity
Store JWT keys in Azure Key Vault or AWS Secrets Manager
Use environment-specific configuration files
Enable HTTPS only
Add rate limiting and request throttling
#### 🧪 Testing
```bash
Copy
# Run backend tests
cd RecruitmentPlatform.Server
dotnet test

# Run frontend tests
cd recruitmentplatform.client
ng test
```
#### 🐛 Common Issues
Database Connection Failed
Ensure SQL Server is running and accessible
Verify connection string in appsettings.Development.json
Check firewall settings for port 1433
CORS Errors
API runs on https://localhost:7277
Frontend runs on http://localhost:4200
CORS is configured in Program.cs
Email Not Sending
Use Gmail App Password, not regular password
For testing, use Mailtrap or similar
#### 🤝 Contributing
Fork the repository
Create a feature branch (git checkout -b feature/amazing-feature)
Commit changes (git commit -m 'Add amazing feature')
Push to branch (git push origin feature/amazing-feature)
Open a Pull Request
#### 📄 License
This project is licensed under the GNU General Public License v3.0 - see the LICENSE.txt file for details.
📞 Support
For support, please open an issue in the GitHub repository.
Built with ❤️ using .NET 9 and Angular 19
Copy

---

## Next Steps

1. **Copy this content** into your `README.md` file
2. **Customize** the description, URLs, and configuration examples
3. **Add screenshots** to the `assets` folder and embed them
4. **Update** the admin credentials section after implementing proper authentication

Once you've pushed this to GitHub, share the repository link and I'll help fix the three issues you mentioned (UI consistency, interview redirect, and button functionality).
