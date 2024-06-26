# Badminton Court Booking Platform

## Getting Started

To get started with this project, follow these steps:

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Git](https://git-scm.com/)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/votrongluan/badminton-booking-razor-page
   cd badminton-booking-razor-page
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

### Setting Up the Database

1. Update the `appsettings.json` file in folder `RazorWebApp` with your database connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnectionString": "YourConnectionStringHere"
     }
   }
   ```

   **Please do not commit any changes in `appsettings.json`**

2. Run the `DatabaseScript.sql` in `Others` folder

## Usage

### Running the Application

1. Run the application in `RazorWebApp` folder:

   ```bash
   dotnet run
   ```

2. Navigate to `https://localhost:5072` in your web browser to see the application in action.

### Examples

Here are some example usages of the application:

- View all clubs
- Book a court
- View your book history

## Collaborate with Your Team

To collaborate within the team without forking, follow these steps:

### Branching Strategy

We use a Git branching strategy with three main branches:

- `main`: Contains the stable version of the code. Direct commits to this branch are restricted.
- `dev`: Contains the latest development changes. This is the main branch for ongoing development.
- `test`: Contains code that is under testing before being merged into `dev`.

### Working on a Feature

1. Create a new branch from `dev` for your feature:

   ```bash
   git checkout dev
   git pull origin dev
   git checkout -b feature/your-feature-name
   ```

2. Make your changes and commit them:

   ```bash
   git add .
   git commit -m 'Add some feature'
   ```

3. Push your branch to the repository:

   ```bash
   git push origin feature/your-feature-name
   ```

4. Create a pull request (PR) from your feature branch to `test` for testing:

   - Ensure all tests pass before requesting a merge.
   - Team members review and approve the PR.
   - Once approved, the PR is merged into `test`.

5. After thorough testing, create a pull request from `test` to `dev`.

6. For releases, create a pull request from `dev` to `master`.

### Inviting Collaborators

- Go to your repository on GitHub.
- Click on `Settings`.
- Select `Manage Access`.
- Click `Invite a collaborator` and add the GitHub usernames of your team members.

## Test and Deploy

Use built-in continuous integration and continuous deployment (CI/CD) pipelines in GitHub Actions.

- Get started with GitHub Actions.
- Analyze your code for vulnerabilities with Static Application Security Testing (SAST).
- Deploy to your preferred cloud service.

## Screenshots

## Support

If you need help, you can reach out via:

- [Issue Tracker](https://github.com/votrongluan/badminton-booking-razor-page/issues)
- [Email](mailto:support@support.support)

## Contributing

We welcome contributions! To contribute:

1. Fork the repository.
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Make your changes and commit them: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a pull request.

## Authors and Acknowledgment

Thanks to all the contributors who have helped develop this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Project Status

This project is actively maintained. If you would like to contribute, please feel free to fork the repository and submit a pull request.
